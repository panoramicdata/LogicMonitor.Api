namespace LogicMonitor.Api.Alerts;

/// <summary>
/// Display settings
/// </summary>
[DataContract]
public class DisplaySettings
{
	/// <summary>
	/// What to display as
	/// </summary>
	[DataMember(Name = "displayAs")]
	public string DisplayAs { get; set; } = string.Empty;

	/// <summary>
	/// The map style
	/// </summary>
	[DataMember(Name = "mapStyle")]
	public string MapStyle { get; set; } = string.Empty;

	/// <summary>
	/// The Sort direction
	/// </summary>
	[DataMember(Name = "sortDirection")]
	public string SortDirection { get; set; } = string.Empty;

	/// <summary>
	/// Whether to show the filter
	/// </summary>
	[DataMember(Name = "showFilter")]
	public bool ShowFilter { get; set; }

	/// <summary>
	/// The columns specifications
	/// </summary>
	[DataMember(Name = "columns")]
	public object Columns { get; set; } = new();

	/// <summary>
	/// The play sound
	/// </summary>
	[DataMember(Name = "playSound")]
	public PlaySound PlaySound { get; set; } = new();

	/// <summary>
	/// The font size
	/// </summary>
	[DataMember(Name = "fontsize")]
	public FontSize FontSize { get; set; }

	/// <summary>
	/// The column to sort by
	/// </summary>
	[DataMember(Name = "sort")]
	public string SortBy { get; set; } = string.Empty;

	/// <summary>
	/// The filters
	/// </summary>
	[DataMember(Name = "filters")]
	public object Filters { get; set; } = new();

	/// <summary>
	/// Whether to show all
	/// </summary>
	[DataMember(Name = "isShowAll")]
	public bool IsShowAll { get; set; }

	/// <summary>
	/// Whether to show the type icon
	/// </summary>
	[DataMember(Name = "showTypeIcon")]
	public bool ShowTypeIcon { get; set; }

	/// <summary>
	/// The page size
	/// </summary>
	[DataMember(Name = "pageSize")]
	public int PageSize { get; set; }

	/// <summary>
	/// Property columns - Too complex a structure to deserialize
	/// </summary>
	[DataMember(Name = "propertyColumns")]
	public object? PropertyColumns { get; set; }

	/// <summary>
	/// Columns v4
	/// </summary>
	[DataMember(Name = "columnsV4")]
	public List<ColumnV4> ColumnsV4 { get; set; } = [];
}
