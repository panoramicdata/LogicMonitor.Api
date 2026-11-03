namespace LogicMonitor.Api.Collectors;

/// <summary>
/// The log level of a collector agent component
/// </summary>
[DataContract]
public class CollectorAgentLogLevel
{
	/// <summary>
	/// The component
	/// </summary>
	[DataMember(Name = "component")]
	public string Component { get; set; } = string.Empty;

	/// <summary>
	/// The log level
	/// </summary>
	[DataMember(Name = "logLevel")]
	public string LogLevel { get; set; } = string.Empty;
}
