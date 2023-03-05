namespace LogicMonitor.Api.Dashboards;

/// <summary>
/// A column header
/// </summary>
public class ColumnHeader
{
	/// <summary>
	/// The name
	/// </summary>
	[DataMember(Name = "name")]
	public string Name { get; set; } = string.Empty;

	/// <summary>
	/// Whether a forecast is needed
	/// </summary>
	[DataMember(Name = "needForecast")]
	public bool NeedForecast { get; set; }

	/// <summary>
	/// The display type
	/// </summary>
	[DataMember(Name = "displayType")]
	public string DisplayType { get; set; } = string.Empty;
}
