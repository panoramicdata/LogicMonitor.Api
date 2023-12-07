namespace LogicMonitor.Api.Dashboards;

/// <summary>
/// A Table widget row
/// </summary>
[DataContract]
public class TableWidgetRow
{
	/// <summary>
	///     The label
	/// </summary>
	[DataMember(Name = "label")]
	public string Label { get; set; } = string.Empty;

	/// <summary>
	///     The groupId
	/// </summary>
	[DataMember(Name = "groupId")]
	public int GroupId { get; set; }

	/// <summary>
	///     The groupFullPath
	/// </summary>
	[DataMember(Name = "groupFullPath")]
	public string GroupFullPath { get; set; } = string.Empty;

	/// <summary>
	///     The deviceId
	/// </summary>
	[DataMember(Name = "deviceId")]
	public int DeviceId { get; set; }

	/// <summary>
	///     The deviceDisplayName
	/// </summary>
	[DataMember(Name = "deviceDisplayName")]
	public string DeviceDisplayName { get; set; } = string.Empty;

	/// <summary>
	///     The DataSourceInstances
	/// </summary>
	[DataMember(Name = "instances")]
	public List<TableWidgetRowDataSourceInstance> DataSourceInstance { get; set; } = [];
}
