using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Options;
using WebScheduleGenerator.Api.Config;
using WebScheduleGenerator.Api.Errors;

namespace WebScheduleGenerator.Api.Filters
{
	internal class ApiKeyFilter : IAuthorizationFilter
	{
		private readonly string[] _keys;
		private readonly ILogger<ApiKeyFilter> _logger;

		public ApiKeyFilter(IOptions<Security> securityConfig, ILogger<ApiKeyFilter> logger)
		{
			if (securityConfig.Value is null)
			{
				throw new ArgumentNullException(nameof(securityConfig));
			}

			_keys = securityConfig.Value.ApiKeys;
			_logger = logger ?? throw new ArgumentNullException(nameof(logger));
		}

		public void OnAuthorization(AuthorizationFilterContext context)
		{
			_logger.LogTrace("Executing {filterName}", nameof(ApiKeyFilter));

			var keyFromRequest = string.Empty;
			if (context.HttpContext.Request.Headers.TryGetValue("X-ApiKey", out var headerKey))
			{
				_logger.LogDebug("Got key from 'X-ApiKey' header.");
				keyFromRequest = headerKey;
			}
			else if (context.HttpContext.Request.Query.TryGetValue("key", out var queryKey))
			{
				_logger.LogDebug("Got key from 'key' query string param");
				keyFromRequest = queryKey;
			}
			else
			{
				_logger.LogInformation("No API key provided.");
			}

			if (_keys.Any(k => string.Equals(k, keyFromRequest, StringComparison.OrdinalIgnoreCase)))
			{
				_logger.LogTrace("Good API key.");
				return;
			}
			else
			{
				_logger.LogWarning("API Key provided but no matches. {key}", keyFromRequest);
			}

			context.Result = new ApiErrorResult<UnauthorizedError>(new UnauthorizedError
			{
				Message = "You are not authorized to use this API."
			});
		}
	}
}
