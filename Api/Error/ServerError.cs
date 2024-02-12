using System.Net;

namespace WebScheduleGenerator.Api.Error;

public class ServerError : ApiError
{
	private readonly HttpStatusCode _statusCode;
	private string _message = string.Empty;
	public override string Message { get => _message; set => _message = value; }
	public override int StatusCode => (int)_statusCode;

	public ServerError() { }
	public ServerError(HttpStatusCode statusCode, string message)
	{
		_statusCode = statusCode;
		_message = message;
	}
}
