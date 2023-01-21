namespace LogicMonitor.Api.Dashboards;

/// <summary>
/// WidgetTokenInheritance
/// </summary>
[DataContract]
public class WidgetTokenInheritance
{
	/// <summary>
	/// The fullpath for the widget token
	/// </summary>
	[DataMember(Name = "fullpath")]
	public string Fullpath { get; set; }

	/// <summary>
	/// The property value for the group
	/// </summary>
	[DataMember(Name = "value")]
	public string Value { get; set; }
}
