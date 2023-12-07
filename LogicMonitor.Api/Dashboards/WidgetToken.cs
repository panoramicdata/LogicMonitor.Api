namespace LogicMonitor.Api.LogicModules;

/// <summary>
/// WidgetToken
/// </summary>
[DataContract]
[DebuggerDisplay("{Name}={Value}")]
public class WidgetToken
{
	/// <summary>
	/// This is the name of the parent group of devices, if there is one established
	/// </summary>
	[DataMember(Name = "name")]
	public string Name { get; set; } = string.Empty;

	/// <summary>
	/// owned | inherit\nSpecifies the type of the widget
	/// </summary>
	[DataMember(Name = "type")]
	public string Type { get; set; } = string.Empty;

	/// <summary>
	/// This is the name of the child group of devices, if there is one
	/// </summary>
	[DataMember(Name = "value")]
	public string Value { get; set; } = string.Empty;

	/// <summary>
	/// The widget token inherit list
	/// </summary>
	[DataMember(Name = "inheritList")]
	public List<WidgetTokenInheritance> InheritList { get; set; } = [];
}
