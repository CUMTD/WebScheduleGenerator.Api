using Microsoft.OpenApi.Models;

namespace WebScheduleGenerator.Api.Swagger.DocumentFilter
{
	internal class AddParamAttribute : Attribute
	{
		public OpenApiParameter Parameter { get; }

		public AddParamAttribute(string name, string description, OpenApiParameter openApiParameter)
		{
			openApiParameter.Name = name;
			openApiParameter.Description = description;

			Parameter = openApiParameter;
		}

		public AddParamAttribute(string name, string description) : this(name, description, new OpenApiParameter
		{
			Schema  = new OpenApiSchema
			{
				Type = "string"
			}
		})
		{
		}
	}
}
