using Microsoft.AspNetCore.Mvc;

namespace WebScheduleGenerator.Api.Errors
{
	/// <summary>
	/// Use this to return custom error bodies if we short circuit a request using our filters
	/// </summary>
	public class ApiErrorResult<T>(T error) : IActionResult where T : ApiError
	{
		private readonly int _statusCode = error.StatusCode;

		public async Task ExecuteResultAsync(ActionContext context)
		{
			var jsonResult = new JsonResult(error)
			{
				StatusCode = _statusCode
			};
			await jsonResult.ExecuteResultAsync(context);
		}
	}
}
