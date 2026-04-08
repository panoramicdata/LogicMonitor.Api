namespace LogicMonitor.Api;

/// <summary>
/// A LogicMonitor API parameter exception
/// </summary>
[Serializable]
public class LogicMonitorApiParameterException : Exception
{
	/// <summary>
	/// The parameter
	/// </summary>
	public string Parameter { get; } = string.Empty;

	/// <summary>
	/// Constructor
	/// </summary>
	/// <param name="parameter"></param>
	public LogicMonitorApiParameterException(string parameter)
	{
		Parameter = parameter;
	}

	/// <summary>
	///  Constructor
	/// </summary>
	public LogicMonitorApiParameterException()
	{
	}

	/// <summary>
	///  Constructor
	/// </summary>
	/// <param name="info"></param>
	/// <param name="context"></param>
#pragma warning disable SYSLIB0051
	protected LogicMonitorApiParameterException(SerializationInfo info, StreamingContext context) : base(info, context)
	{
	}
#pragma warning restore SYSLIB0051

	/// <summary>
	///  Constructor
	/// </summary>
	/// <param name="message"></param>
	/// <param name="innerException"></param>
	public LogicMonitorApiParameterException(string message, Exception innerException) : base(message, innerException)
	{
	}

	/// <inheritdoc />
#pragma warning disable CS0672, SYSLIB0003, SYSLIB0051
	[SecurityPermission(SecurityAction.Demand, SerializationFormatter = true)]
	public override void GetObjectData(SerializationInfo info, StreamingContext context)
	{
		base.GetObjectData(info, context);

		info.AddValue("Parameter", Parameter);
	}
#pragma warning restore CS0672, SYSLIB0003, SYSLIB0051
}
