using Microsoft.AspNetCore.Mvc;
using WebScheduleGenerator.Api.Errors;
using WebScheduleGenerator.Core;
using WebScheduleGenerator.Core.Entities.Schedule;
using WebScheduleGenerator.Init.Serialization;

namespace WebScheduleGenerator.Api.Controllers
{
	[Route("api/web-schedule/generate")]
	[ApiController]
	public class WebScheduleController(IScheduleConverter<InitTimetable> scheduleConverter, ILogger<WebScheduleController> logger) : ControllerBase
	{
		[HttpPost, HttpOptions, HttpHead, Consumes("multipart/form-data"), Route("file")]
		[ProducesResponseType<ProcessingResult>(StatusCodes.Status200OK, "application/json")]
		[ProducesResponseType<UnauthorizedError>(StatusCodes.Status401Unauthorized, "application/json")]
		[ProducesResponseType<BadRequestError>(StatusCodes.Status400BadRequest, "application/json")]
		[ProducesResponseType<ServerError>(StatusCodes.Status500InternalServerError, "application/json")]
		public async Task<IActionResult> ProcessXml(IFormFile file, CancellationToken cancellationToken)
		{
			if (file == null || file.Length == 0)
			{
				return new ApiErrorResult<BadRequestError>(new BadRequestError());
			}

			ProcessingResult result;
			try
			{
				using var stream = file.OpenReadStream();
				result = await scheduleConverter.ConvertScheduleAsync(stream, cancellationToken).ConfigureAwait(false);
			}
			catch (Exception ex)
			{
				logger.LogError(ex, "failed to convert XML");
				return new ApiErrorResult<ServerError>(new ServerError(System.Net.HttpStatusCode.InternalServerError, "Failed to convert XML"));
			}

			return Ok(result);
		}

		[HttpPost, HttpOptions, HttpHead, Consumes("application/xml"), Route("xml")]
		[ProducesResponseType(typeof(ProcessingResult), StatusCodes.Status200OK, contentType: "application/json")]
		[ProducesResponseType(typeof(UnauthorizedError), StatusCodes.Status401Unauthorized, contentType: "application/json")]
		[ProducesResponseType(typeof(BadRequestError), StatusCodes.Status400BadRequest, contentType: "application/json")]
		[ProducesResponseType(typeof(ServerError), StatusCodes.Status500InternalServerError, contentType: "application/json")]
		public async Task<IActionResult> ProcessXml([FromBody] InitTimetable xml, CancellationToken cancellationToken)
		{
			if (xml == null)
			{
				return new ApiErrorResult<BadRequestError>(new BadRequestError());
			}

			ProcessingResult result;
			try
			{
				result = await scheduleConverter.ConvertScheduleAsync(xml, cancellationToken).ConfigureAwait(false);
			}
			catch (Exception ex)
			{
				logger.LogError(ex, "failed to convert XML");
				return new ApiErrorResult<ServerError>(new ServerError(System.Net.HttpStatusCode.InternalServerError, "Failed to convert XML"));
			}

			return Ok(result);
		}
	}
}
