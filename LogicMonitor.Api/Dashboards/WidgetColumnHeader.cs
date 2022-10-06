namespace LogicMonitor.Api.Dashboards;

/// <summary>
/// A widget column header
/// </summary>
[DataContract]
public class WidgetColumnHeader
{
	/// <summary>
	/// The name of the column
	/// </summary>
	[DataMember(Name = "name")]
	public string Name { get; set; } = string.Empty;

	/// <summary>
	/// Whether a forecast is needed
	/// </summary>
	[DataMember(Name = "needForecast")]
	public bool NeedForecast { get; set; }

	/// <summary>
	/// The display type for the column
	/// </summary>
	[DataMember(Name = "displayType")]
	public string DisplayType { get; set; } = string.Empty;

	/// <summary>
	/// The label for the units of this column
	/// </summary>
	[DataMember(Name = "unitLabel")]
	public string UnitLabel { get; set; } = string.Empty;
}
