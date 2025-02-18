namespace LogicMonitor.Api.Dashboards;

/// <summary>
///     Resource NOC Widget
/// </summary>
public class ResourceNocWidgetCreationDto : WidgetCreationDto<NocWidget>
{
	/// <summary>
	///     Whether to show warning alerts
	/// </summary>
	[DataMember(Name = "displayWarnAlert")]
	public bool DisplayWarningAlerts { get; set; } = true;

	/// <summary>
	///     Whether to show error alerts
	/// </summary>
	[DataMember(Name = "displayErrorAlert")]
	public bool DisplayErrorAlerts { get; set; } = true;

	/// <summary>
	///     Whether to show critical alerts
	/// </summary>
	[DataMember(Name = "displayCriticalAlert")]
	public bool DisplayCriticalAlerts { get; set; } = true;

	/// <summary>
	///     Whether to show acknowledged alerts
	/// </summary>
	[DataMember(Name = "ackChecked")]
	public bool AckChecked { get; set; } = true;

	/// <summary>
	///     Whether to show alerts in SDT
	/// </summary>
	[DataMember(Name = "sdtChecked")]
	public bool SdtChecked { get; set; } = true;

	/// <summary>
	///     What to sort by
	/// </summary>
	[DataMember(Name = "sortBy")]
	[JsonConverter(typeof(StringEnumConverter))]
	public NocWidgetSortBy SortBy { get; set; } = NocWidgetSortBy.Name;

	/// <summary>
	///     What to sort by
	/// </summary>
	[DataMember(Name = "displayColumn")]
	[JsonConverter(typeof(StringEnumConverter))]
	public NocWidgetDisplayColumnCount DisplayColumnCount { get; set; } = (NocWidgetDisplayColumnCount)1;

	/// <summary>
	///     The Resource NOC widget items
	/// </summary>
	[DataMember(Name = "items")]
	public List<ResourceNocWidgetItem> Items { get; set; } = [];

	/// <inheritdoc />
	public override string Type { get; } = "deviceNOC";
}
