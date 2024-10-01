namespace LogicMonitor.Api.Dashboards;

/// <summary>
///     A Resource NOC widget
/// </summary>
[DataContract]
public class ResourceNocWidget : NocWidget, IWidget
{
	/// <summary>
	///     The Items
	/// </summary>
	[DataMember(Name = "items")]
	public new List<ResourceNocWidgetItem> Items { get; set; } = [];
}
