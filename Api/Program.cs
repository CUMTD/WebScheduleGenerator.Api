using Serilog;
using WebScheduleGenerator.Api.Extensions;

var app = WebApplication
	.CreateBuilder(args)
	.Configure()
	.Build()
	.StartApp();

var assembly = "WebScheduleGenerator.Api";

try
{
	assembly = System.Reflection.Assembly.GetExecutingAssembly().GetName().Name;
	Log.Information("The {assembly} is starting.", assembly);
	await app.RunAsync();
}
catch (Exception ex)
{
	Log.Fatal(ex, "The {assembly} failed to start correctly.", assembly);
}
finally
{
	Log.Information("The {assembly} has stopped.", assembly);
}
