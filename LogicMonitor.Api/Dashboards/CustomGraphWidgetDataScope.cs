namespace LogicMonitor.Api.Dashboards;

/// <summary>
/// The scope for a custom graph
/// </summary>
[DataContract]
public class CustomGraphWidgetDataScope
{
	/// <summary>
	/// The Id of this scope
	/// </summary>
	[DataMember(Name = "id")]
	public int Id { get; set; }

	/// <summary>
	/// The type of the scope
	/// </summary>
	[DataMember(Name = "type")]
	public string Type { get; set; } = string.Empty;
}