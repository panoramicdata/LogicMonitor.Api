namespace LogicMonitor.Api.Alerts;

/// <summary>
/// Display settings
/// </summary>
[DataContract]
public class DisplaySettings
{
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
	public AlertCreationDtoPlaysound PlaySound { get; set; }

	/// <summary>
	/// The font size
	/// </summary>
	[DataMember(Name = "fontsize")]
	public FontSize FontSize { get; set; }

	/// <summary>
	/// The column to sort by
	/// </summary>
	[DataMember(Name = "sortBy")]
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
}
