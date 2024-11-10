namespace LogicMonitor.Api.Dashboards;

/// <summary>
/// A widget row
/// </summary>
[DataContract]
public class WidgetRow
{
	/// <summary>
	/// Resource id
	/// </summary>
	[DataMember(Name = "deviceId")]
	public int ResourceId { get; set; }

	/// <summary>
	/// Resource Display Name
	/// </summary>
	[DataMember(Name = "deviceDisplayName")]
	public string ResourceDisplayName { get; set; } = string.Empty;

	/// <summary>
	/// Cells
	/// </summary>
	[DataMember(Name = "cells")]
	public List<WidgetCell> Cells { get; set; } = [];
}
