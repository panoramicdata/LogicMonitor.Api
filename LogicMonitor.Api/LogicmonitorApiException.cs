namespace LogicMonitor.Api;

/// <summary>
/// A LogicMonitor API exception
/// </summary>
[Serializable]
public class LogicMonitorApiException : Exception
{
	/// <summary>
	/// Constructor
	/// </summary>
	/// <param name="method"></param>
	/// <param name="subUrl"></param>
	/// <param name="httpStatusCode"></param>
	/// <param name="responseBody"></param>
	public LogicMonitorApiException(HttpMethod method, string subUrl, HttpStatusCode httpStatusCode, string responseBody)
		: this(method, subUrl, httpStatusCode, responseBody, null)
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
	public LogicMonitorApiException(HttpMethod method, string subUrl, HttpStatusCode httpStatusCode, string responseBody, string? message)
		: base(message ?? $"Unsuccessful {method} to {subUrl} ({httpStatusCode} - {(int)httpStatusCode}).  Response Body:\n{responseBody}")
	{
		HttpStatusCode = httpStatusCode;
		if (message != null)
		{
			ErrorMessage = message;
		}
		else
		{
			ErrorMessage = string.Empty;
		}

		ResponseBody = responseBody;
	}

	internal LogicMonitorApiException(string message)
		: base(message)
	{
	}

	/// <summary>
	/// Constructor
	/// </summary>
	/// <param name="info"></param>
	/// <param name="context"></param>
	protected LogicMonitorApiException(SerializationInfo info, StreamingContext context)
		: base(info, context)
	{
	}

	/// <summary>
	/// Constructor
	/// </summary>
	/// <param name="httpResponseMessage"></param>
	public LogicMonitorApiException(HttpResponseMessage httpResponseMessage)
	{
		HttpStatusCode = httpResponseMessage.StatusCode;
		ErrorMessage = httpResponseMessage.ReasonPhrase;
		ResponseBody = httpResponseMessage.Content.ReadAsStringAsync().Result;
	}

	/// <summary>
	/// Constructor
	/// </summary>
	/// <param name="responseBody"></param>
	/// <param name="exception"></param>
	protected LogicMonitorApiException(string responseBody, Exception exception) : base(exception.Message, exception)
	{
		ErrorMessage = exception.Message;
		ResponseBody = responseBody;
	}

	/// <summary>
	/// Constructor
	/// </summary>
	public LogicMonitorApiException()
	{
	}

	/// <summary>
	/// The body of the response sent by LogicMonitor
	/// </summary>
	public string ResponseBody { get; protected set; } = string.Empty;

	/// <summary>
	/// The HTTP status code
	/// </summary>
	public HttpStatusCode HttpStatusCode { get; internal set; }

	/// <summary>
	/// The error message
	/// </summary>
	public string ErrorMessage { get; } = string.Empty;

	private bool Equals(LogicMonitorApiException other) => HttpStatusCode == other.HttpStatusCode && string.Equals(ErrorMessage, other.ErrorMessage, StringComparison.Ordinal);

	/// <inheritdoc />
	public override bool Equals(object obj)
	{
		if (obj is null)
		{
			return false;
		}

		if (ReferenceEquals(this, obj))
		{
			return true;
		}

		return obj.GetType() == GetType() && Equals((LogicMonitorApiException)obj);
	}

	/// <inheritdoc />
	public override int GetHashCode()
	{
		unchecked
		{
			return ((int)HttpStatusCode * 397) ^ (ErrorMessage?.GetHashCode() ?? 0);
		}
	}

	/// <inheritdoc />
	public override string ToString() => $"{HttpStatusCode}:{ErrorMessage}:{base.ToString()}";

	/// <inheritdoc />
	[SecurityPermission(SecurityAction.Demand, SerializationFormatter = true)]
	public override void GetObjectData(SerializationInfo info, StreamingContext context)
	{
		base.GetObjectData(info, context);

		info.AddValue("HttpStatusCode", HttpStatusCode);
		info.AddValue("ErrorMessage", ErrorMessage);
	}
}
