namespace LogicMonitor.Api.Dashboards;

/// <summary>
/// A DynamicTableWidgetRow
/// </summary>
[DataContract]
public class DynamicTableWidgetRow
{
	/// <summary>
	///     The label
	/// </summary>
	[DataMember(Name = "label")]
	public string Label { get; set; } = string.Empty;

	/// <summary>
	///     The label
	/// </summary>
	[DataMember(Name = "instanceName")]
	public string InstanceName { get; set; } = string.Empty;

	/// <summary>
	///     The group FullPath
	/// </summary>
	[DataMember(Name = "groupFullPath")]
	public string GroupFullPath { get; set; } = string.Empty;

	/// <summary>
	///     The Device display name
	/// </summary>
	[DataMember(Name = "deviceDisplayName")]
	public string DeviceDisplayName { get; set; } = string.Empty;
}
