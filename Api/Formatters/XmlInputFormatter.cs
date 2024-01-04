using System.Text;
using System.Xml.Serialization;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.Net.Http.Headers;

namespace WebScheduleGenerator.Api.Formatters
{
	public class XmlInputFormatter<T> : TextInputFormatter where T : class
	{
		private static readonly XmlSerializer Serializer = new(typeof(T));

		public XmlInputFormatter()
		{
			SupportedMediaTypes.Add(MediaTypeHeaderValue.Parse("application/xml"));
			SupportedMediaTypes.Add(MediaTypeHeaderValue.Parse("text/xml"));

			SupportedEncodings.Add(Encoding.Default);
			SupportedEncodings.Add(Encoding.UTF8);
			SupportedEncodings.Add(Encoding.Unicode);
			SupportedEncodings.Add(Encoding.ASCII);
		}

		public override bool CanRead(InputFormatterContext context) =>
			typeof(T).IsAssignableFrom(context.ModelType) && base.CanRead(context);

		public override async Task<InputFormatterResult> ReadRequestBodyAsync(InputFormatterContext context, Encoding encoding)
		{
			var httpContext = context.HttpContext;

			try
			{
				// Have to do this silly stream manipulation because ASP.NET will not let us do
				// syncronous stream reads, but the xml deserializer does not have an async method.
				using var stream = httpContext.Request.Body;
				using var streamReader = new StreamReader(stream);
				var xmlString = await streamReader.ReadToEndAsync().ConfigureAwait(false);
				using var stringReader = new StringReader(xmlString);

				var result = Serializer.Deserialize(stringReader) as T;
				return await InputFormatterResult.SuccessAsync(result).ConfigureAwait(false);
			}
			catch
			{
				return await InputFormatterResult.FailureAsync().ConfigureAwait(false);
			}
		}
	}
}
