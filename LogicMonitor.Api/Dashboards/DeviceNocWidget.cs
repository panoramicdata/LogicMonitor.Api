namespace LogicMonitor.Api.Dashboards;

/// <summary>
///     A Device NOC widget
/// </summary>
[DataContract]
public class DeviceNocWidget : NocWidget
{
	/// <summary>
	///     The Items
	/// </summary>
	[DataMember(Name = "items")]
	public new List<DeviceNocWidgetItem> Items { get; set; }
}
