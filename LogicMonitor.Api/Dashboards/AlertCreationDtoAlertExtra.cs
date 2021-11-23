namespace LogicMonitor.Api.Dashboards;

/// <summary>
/// An AlertCreationDtoAlertExtra
/// </summary>
[DataContract]
public class AlertCreationDtoAlertExtra
{
	/// <summary>
	/// The columns
	/// </summary>
	[DataMember(Name = "columns")]
	public List<AlertCreationDtoColumn> Columns { get; set; }

	/// <summary>
	/// Whether to show the filter
	/// </summary>
	[DataMember(Name = "showFilter")]
	public bool ShowFilter { get; set; }

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
	/// The sort direction
	/// </summary>
	[DataMember(Name = "sortDirection")]
	public string SortDirection { get; set; }

	/// <summary>
	/// The play sound
	/// </summary>
	[DataMember(Name = "playSound")]
	public AlertCreationDtoPlaysound PlaySound { get; set; }
}
