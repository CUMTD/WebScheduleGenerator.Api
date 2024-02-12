using Microsoft.Extensions.Options;
using WebScheduleGenerator.Api.Config;

namespace WebScheduleGenerator.Api.Extensions;

internal static class WebApplicationExtensions
{
	public static WebApplication StartApp(this WebApplication app)
	{
		if (app.Environment.IsProduction())
		{
			_ = app.UseHsts();
		}

		_ = app.UseHttpsRedirection();
		_ = app.UseStaticFiles();

		_ = app.UseSwagger();

		_ = app.UseSwaggerUI(options =>
		{
			options.RoutePrefix = string.Empty;
			options.DocExpansion(Swashbuckle.AspNetCore.SwaggerUI.DocExpansion.List);
			options.DocumentTitle = "Web Schedule Generator API";

			options.DisplayRequestDuration();

			options.InjectStylesheet("/css/swagger-ui.css");

			// This really slows down the swagger UI for big responses
			options.ConfigObject.AdditionalItems.Add("syntaxHighlight", false);

			options.SwaggerEndpoint("/swagger/v1.0/swagger.json", "Web Schedule Generator API".Trim());
		});

		_ = app.UseRouting();

		var cors = app.Services.GetRequiredService<IOptions<Cors>>().Value ?? throw new ArgumentException("No Cors options registred");
		_ = app.UseCors(cors.PolicyName);

		_ = app.MapHealthChecks("/healthz");

		_ = app.MapControllers();

		return app;
	}


}
