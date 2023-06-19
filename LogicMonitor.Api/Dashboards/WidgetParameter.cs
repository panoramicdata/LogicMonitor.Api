namespace LogicMonitor.Api.Dashboards;

/// <summary>
///  A widget parameter
/// </summary>
[DataContract]
public class WidgetParameter
{
	/// <summary>
	/// The name
	/// </summary>
	[DataMember(Name = "name")]
	public string Name { get; set; } = string.Empty;

	/// <summary>
	/// The value
	/// </summary>
	[DataMember(Name = "value")]
	public string Value { get; set; } = string.Empty;

	/// <summary>
	/// The ToString override
	/// </summary>
	public override string ToString() => $"Name={Name}, Value={Value}";
}
