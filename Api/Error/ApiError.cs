namespace WebScheduleGenerator.Api.Errors
{
	/// <summary>
	/// Indicates an API error.
	/// </summary>
	public abstract class ApiError
	{
		/// <summary>
		/// The HTTP status code for this response.
		/// </summary>
		/// <example>401</example>

		public abstract int StatusCode { get; }

		/// <summary>
		/// A message explaining the error.
		/// </summary>
		/// <example>You must provide an API key</example>
		public virtual string Message { get; set; } = default!;
	}
}
