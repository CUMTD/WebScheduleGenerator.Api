namespace WebScheduleGenerator.Api.Error;

/// <inheritdoc />
/// <summary>
/// This error is returned if you are unauthorized to access the API.
/// </summary>
public sealed class UnauthorizedError : ApiError
{
	/// <inheritdoc />
	/// <example>401</example>
	public override int StatusCode => StatusCodes.Status401Unauthorized;


	/// <inheritdoc />
	/// <example>You must provide an API key.</example>
	public override string Message { get; set; } = default!;
}
