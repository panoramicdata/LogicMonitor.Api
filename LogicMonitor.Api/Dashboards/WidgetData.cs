namespace LogicMonitor.Api.Dashboards;

/// <summary>
/// Widget data
/// </summary>
[DataContract]
public class WidgetData
{
	/// <summary>
	///     Type
	/// </summary>
	/// <remarks>
	/// This is used for discrimination during deserialization
	/// </remarks>
	[DataMember(Name = "type")]
	public string Type { get; set; } = string.Empty;

	/// <summary>
	///     Title
	/// </summary>
	[DataMember(Name = "title")]
	public string Title { get; set; } = string.Empty;

	/// <summary>
	///     Availability
	/// </summary>
	[DataMember(Name = "availability")]
	public double? Availability { get; set; }

	/// <summary>
	///     Color level
	/// </summary>
	[DataMember(Name = "colorLevel")]
	public int? ColorLevel { get; set; }

	/// <summary>
	///     Data
	/// </summary>
	[DataMember(Name = "data")]
	public List<WidgetDataItem> Data { get; set; } = new();

	/// <summary>
	///     Result list (used by SLA Multi widget)
	/// </summary>
	[DataMember(Name = "resultList")]
	public List<WidgetDataItem> ResultList { get; set; } = new();

	/// <summary>
	///     Column headers
	/// </summary>
	[DataMember(Name = "columnHeaders")]
	public List<WidgetColumnHeader> ColumnHeaders { get; set; } = new();

	/// <summary>
	///     Rows
	/// </summary>
	[DataMember(Name = "rows")]
	public List<WidgetRow> Rows { get; set; } = new();
}
