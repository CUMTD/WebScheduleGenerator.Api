using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace WebScheduleGenerator.Api.Swagger.DocumentFilter
{


	internal class BodyParamFilter : IOperationFilter
	{

		public BodyParamFilter()
		{
		}

		public void Apply(OpenApiOperation operation, OperationFilterContext context)
		{
			if (context.ApiDescription.HttpMethod == "POST")
			{
				var matches = context
					.ApiDescription
					.CustomAttributes()
					.Where(ca => ca.GetType() == typeof(AddParamAttribute));

				if (matches != null)
				{
					foreach (var match in matches)
					{
						if (match is AddParamAttribute attribute)
						{
							operation.Parameters.Add(attribute.Parameter);
						}
					}
				}
			}
		}
	}
}
