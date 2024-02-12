namespace WebScheduleGenerator.Api.Error;

/// <inheritdoc />
/// <summary>
/// This error indicates a problem with your request.
/// </summary>
public sealed class BadRequestError : ApiError
{
	/// <inheritdoc />
	/// <example>404</example>
	public override int StatusCode => StatusCodes.Status404NotFound;


	private string _message = "This method was not found.";

	/// <inheritdoc />
	/// <example>There was a problem with your request.</example>
	public override string Message { get => _message; set => _message = value; }
}
