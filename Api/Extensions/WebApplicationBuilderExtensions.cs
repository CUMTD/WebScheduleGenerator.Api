using Azure.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using Serilog;
using WebScheduleGenerator.Api.Config;
using WebScheduleGenerator.Api.Filters;
using WebScheduleGenerator.Api.Formatters;
using WebScheduleGenerator.Api.Swagger.DocumentFilter;
using WebScheduleGenerator.Core;
using WebScheduleGenerator.EF;
using WebScheduleGenerator.Init;
using WebScheduleGenerator.Init.Serialization;

namespace WebScheduleGenerator.Api.Extensions
{
	internal static class WebApplicationBuilderExtensions
	{
		public static WebApplicationBuilder Configure(this WebApplicationBuilder builder) => builder
				.AddConfiguration()
				.RegisterEF()
				.RegisterDependencyInjection()
				.ConfigureLogging()
				.ConfigureApi();

		private static WebApplicationBuilder AddConfiguration(this WebApplicationBuilder builder)
		{
			var configUrl = builder.Configuration.GetValue<string>("KeyVaultUrl") ?? throw new ArgumentException("KeyVaultUrl");
			var keyVaultUri = new Uri(configUrl);

			if (builder.Environment.IsDevelopment())
			{
				builder.Configuration.AddAzureKeyVault(keyVaultUri, new DefaultAzureCredential());
			}
			// Enable this to deploy in Azure
			//else
			//{
			//	builder.Configuration.AddAzureKeyVault(keyVaultUri, new ManagedIdentityCredential());
			//}

			var envPrefix = builder.Configuration.GetValue<string>("EnvPrefix") ?? throw new ArgumentException("EnvPrefix");
			builder.Configuration.AddEnvironmentVariables(envPrefix);

			builder.Services
				.AddOptions<Security>()
				.BindConfiguration("Security")
				.ValidateDataAnnotations()
				.ValidateOnStart();

			builder.Services
				.AddOptions<Cors>()
				.BindConfiguration("Cors")
				.ValidateDataAnnotations()
				.ValidateOnStart();

			return builder;
		}

		private static WebApplicationBuilder RegisterEF(this WebApplicationBuilder builder)
		{
			var connectionString = builder.Configuration.GetConnectionString("Stopwatch");

			_ = builder.Services.AddDbContextPool<StopwatchContext>(options => options
				.UseSqlServer(
					connectionString,
					sqlOptions =>
						sqlOptions.CommandTimeout((int)TimeSpan.FromSeconds(30).TotalSeconds)
						.EnableRetryOnFailure(1)
				)
			);

			return builder;
		}

		private static WebApplicationBuilder RegisterDependencyInjection(this WebApplicationBuilder builder)
		{
			_ = builder.Services.AddScoped<ApiKeyFilter>();
			_ = builder.Services.AddScoped<IScheduleConverter<InitTimetable>, InitScheduleConverter>();

			return builder;
		}
		private static WebApplicationBuilder ConfigureLogging(this WebApplicationBuilder builder)
		{
			builder
				.Host
				.UseSerilog((context, loggerConfig) => loggerConfig.ReadFrom.Configuration(context.Configuration));

			return builder;
		}

		private static WebApplicationBuilder ConfigureApi(this WebApplicationBuilder builder)
		{
			_ = builder.Services.AddHealthChecks();

			_ = builder
				.Services
				.AddControllers(options =>
				{
					options.Filters.Add<ApiKeyFilter>();
					options.InputFormatters.Add(new XmlInputFormatter<InitTimetable>());
				});

			_ = builder.Services.AddRouting(options =>
			{
				options.LowercaseUrls = true;
				options.AppendTrailingSlash = true;
			});

			if (builder.Environment.IsProduction())
			{
				_ = builder.Services.AddHsts(options =>
				{
					options.Preload = true;
					options.IncludeSubDomains = true;
					options.MaxAge = TimeSpan.FromDays(365 * 2);
				});
			}

			_ = builder.Services.AddSwaggerGen(options =>
			{
				options.SwaggerDoc($"v1.0", new OpenApiInfo
				{
					Title = $"Web Schedule Generator API",
					Description = "Web Schedule Generator API.",
					Contact = new OpenApiContact
					{
						Name = "MTD",
						Email = "developer@mtd.org"
					}
				});

				options.EnableAnnotations();

				if (builder.Environment.IsProduction())
				{
					options.DocumentFilter<HideVerbsFilter>();
				}

				var authMethodName = "API Key - Header";
				var securityScheme = new OpenApiSecurityScheme
				{
					Reference = new OpenApiReference
					{
						Type = ReferenceType.SecurityScheme,
						Id = authMethodName
					},
					Description = "Provide your API key in the header using X-ApiKey.",
					In = ParameterLocation.Header,
					Name = "X-ApiKey",
					Type = SecuritySchemeType.ApiKey,
					Scheme = "Bearer"
				};
				options.AddSecurityDefinition(authMethodName, securityScheme);
				options.AddSecurityRequirement(new OpenApiSecurityRequirement() {
					{ securityScheme, Array.Empty <string>() }
				  });

			});

			var provider = builder.Services.BuildServiceProvider();
			var corsPolicyName = provider.GetRequiredService<IOptions<Cors>>().Value?.PolicyName ?? throw new ArgumentException(nameof(Cors));
			_ = builder.Services.AddCors(options =>
			{
				options.AddPolicy(corsPolicyName, builder => _ = builder
					.AllowAnyOrigin()
					.AllowAnyMethod()
					.AllowAnyHeader()
				);
				options.DefaultPolicyName = corsPolicyName;
			});

			return builder;
		}

	}
}
