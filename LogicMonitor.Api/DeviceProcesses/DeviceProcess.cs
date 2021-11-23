namespace LogicMonitor.Api.DeviceProcesses;

/// <summary>
///     A process running on a device
/// </summary>
[DataContract]
public class DeviceProcess : NamedStringIdentifiedItem
{
	/// <summary>
	///     Whether alerting is enabled
	/// </summary>
	[DataMember(Name = "alert")]
	public bool IsAlertingEnabled { get; set; }

	/// <summary>
	///     Whether monitoring is enabled
	/// </summary>
	[DataMember(Name = "monitor")]
	public bool IsMonitoringEnabled { get; set; }

	/// <summary>
	///     Comment
	/// </summary>
	[DataMember(Name = "comment")]
	public string Comment { get; set; }

	/// <summary>
	///     Flag
	/// </summary>
	[DataMember(Name = "flag")]
	public int Flag { get; set; }

	/// <summary>
	///     Param
	/// </summary>
	[DataMember(Name = "param")]
	public string Parameters { get; set; }
}
