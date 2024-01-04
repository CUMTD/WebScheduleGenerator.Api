namespace WebScheduleGenerator.Api.Errors
{
	/// <inheritdoc />
	/// <summary>
	/// This error indicates a problem with your request.
	/// </summary>
	public sealed class NotFoundError : ApiError
	{

		/// <inheritdoc />
		/// <example>400</example>
		public override int StatusCode => StatusCodes.Status400BadRequest;


		private string _message = "XML was not able to be parsed.";

		/// <inheritdoc />
		/// <example>There was a problem with your request.</example>
		public override string Message { get => _message; set => _message = value; }

	}
}
