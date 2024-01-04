using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace WebScheduleGenerator.Api.Swagger.DocumentFilter
{
	internal class HideVerbsFilter : IDocumentFilter
	{
		private readonly OperationType[] _operationTypesToRemove;

		public HideVerbsFilter()
		{
			_operationTypesToRemove = [OperationType.Options, OperationType.Head];
		}

		public void Apply(OpenApiDocument swaggerDoc, DocumentFilterContext context)
		{
			foreach (var path in swaggerDoc.Paths.Values)
			{
				foreach (var operationType in _operationTypesToRemove)
				{
					if (path.Operations.ContainsKey(operationType))
					{
						_ = path.Operations.Remove(operationType);
					}
				}
			}
		}
	}
}
