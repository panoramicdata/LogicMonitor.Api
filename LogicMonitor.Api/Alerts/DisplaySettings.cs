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
	public string? DisplayAs { get; set; }

	/// <summary>
	/// The map style
	/// </summary>
	[DataMember(Name = "mapStyle")]
	public string? MapStyle { get; set; }

	/// <summary>
	/// The Sort direction
	/// </summary>
	[DataMember(Name = "sortDirection")]
	public string SortDirection { get; set; }

	/// <summary>
	/// Whether to show the filter
	/// </summary>
	[DataMember(Name = "showFilter")]
	public bool ShowFilter { get; set; }

	/// <summary>
	/// The columns specifications
	/// </summary>
	[DataMember(Name = "columns")]
	public object Columns { get; set; }

	/// <summary>
	/// The play sound
	/// </summary>
	[DataMember(Name = "playSound")]
	public PlaySound PlaySound { get; set; }

	/// <summary>
	/// The font size
	/// </summary>
	[DataMember(Name = "fontsize")]
	public FontSize FontSize { get; set; }

	/// <summary>
	/// The column to sort by
	/// </summary>
	[DataMember(Name = "sort")]
	public string SortBy { get; set; }

	/// <summary>
	/// The filters
	/// </summary>
	[DataMember(Name = "filters")]
	public object Filters { get; set; }

	/// <summary>
	/// Whether to show all
	/// </summary>
	[DataMember(Name = "isShowAll")]
	public bool IsShowAll { get; set; }

	/// <summary>
	/// Whether to show the type icon
	/// </summary>
	[DataMember(Name = "showTypeIcon")]
	public bool? ShowTypeIcon { get; set; }

	/// <summary>
	/// The page size
	/// </summary>
	[DataMember(Name = "pageSize")]
	public int PageSize { get; set; }
}
