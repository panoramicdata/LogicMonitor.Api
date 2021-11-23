namespace LogicMonitor.Api.Dashboards;

/// <summary>
/// A widget row
/// </summary>
[DataContract]
public class WidgetRow
{
	/// <summary>
	/// Device id
	/// </summary>
	[DataMember(Name = "deviceId")]
	public int DeviceId { get; set; }

	/// <summary>
	/// Device Display Name
	/// </summary>
	[DataMember(Name = "deviceDisplayName")]
	public string DeviceDisplayName { get; set; }

	/// <summary>
	/// Cells
	/// </summary>
	[DataMember(Name = "cells")]
	public List<WidgetCell> Cells { get; set; }
}
