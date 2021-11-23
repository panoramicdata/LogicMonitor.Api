namespace LogicMonitor.Api;

/// <summary>
/// A deserialization exception
/// </summary>
public class DeserializationException : LogicMonitorApiException
{
	/// <summary>
	/// Constructor
	/// </summary>
	/// <param name="responseBody">The responseBody</param>
	/// <param name="exception">The inner exception</param>
	public DeserializationException(string responseBody, Exception exception) : base(responseBody, exception)
	{
	}

	/// <summary>
	/// Constructor
	/// </summary>
	/// <param name="method"></param>
	/// <param name="subUrl"></param>
	/// <param name="httpStatusCode"></param>
	/// <param name="responseBody"></param>
	public DeserializationException(HttpMethod method, string subUrl, System.Net.HttpStatusCode httpStatusCode, string responseBody) : base(method, subUrl, httpStatusCode, responseBody)
	{
	}

	/// <summary>
	/// Constructor
	/// </summary>
	/// <param name="info"></param>
	/// <param name="context"></param>
	protected DeserializationException(SerializationInfo info, StreamingContext context) : base(info, context)
	{
	}

	/// <summary>
	/// Constructor
	/// </summary>
	/// <param name="httpResponseMessage"></param>
	public DeserializationException(HttpResponseMessage httpResponseMessage) : base(httpResponseMessage)
	{
	}

	/// <summary>
	///  Constructor
	/// </summary>
	public DeserializationException()
	{
	}

	/// <summary>
	/// Constructor
	/// </summary>
	/// <param name="method"></param>
	/// <param name="subUrl"></param>
	/// <param name="httpStatusCode"></param>
	/// <param name="responseBody"></param>
	/// <param name="message"></param>
	public DeserializationException(HttpMethod method, string subUrl, System.Net.HttpStatusCode httpStatusCode, string responseBody, string message = null) : base(method, subUrl, httpStatusCode, responseBody, message)
	{
	}
}
