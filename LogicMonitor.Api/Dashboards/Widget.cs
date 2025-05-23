namespace LogicMonitor.Api.Dashboards;

/// <summary>
///     A Dashboard widget
/// </summary>
[DataContract]
[DebuggerDisplay("{Type}:{Name}")]
[JsonConverter(typeof(WidgetConverter))]
public class Widget : NamedItem, IHasEndpoint, IWidget
{
	/// <summary>
	/// alert | batchjob | flash | gmap | ngraph | ograph | cgraph | sgraph | netflowgraph | groupNetflowGraph | netflow | groupNetflow | html | bigNumber | gauge | pieChart | table | dynamicTable | deviceSLA | text | statsd | deviceStatus | serviceAlert | noc | websiteOverview | websiteOverallStatus | websiteIndividualStatus | websiteSLA | savedMap | devicestatus
	/// </summary>
	[DataMember(Name = "type")]
	public string Type { get; set; } = string.Empty;

	/// <summary>
	///     When the widget was last updated
	/// </summary>
	[DataMember(Name = "lastUpdatedOn")]
	public int LastUpdatedOnSeconds { get; set; }

	/// <summary>
	///     The ordinal
	/// </summary>
	[DataMember(Name = "lastUpdatedBy")]
	public string LastUpdatedBy { get; set; } = string.Empty;

	/// <summary>
	///     The dashboard id
	/// </summary>
	[DataMember(Name = "dashboardId")]
	public int DashboardId { get; set; }

	/// <summary>
	///     The column index
	/// </summary>
	[DataMember(Name = "columnIdx")]
	public int ColumnIndex { get; set; }

	/// <summary>
	///     The ordinal
	/// </summary>
	[DataMember(Name = "order")]
	public int Order { get; set; }

	/// <summary>
	///     The theme
	/// </summary>
	[DataMember(Name = "theme")]
	public string Theme { get; set; } = string.Empty;

	/// <summary>
	///     The column span
	/// </summary>
	[DataMember(Name = "colSpan")]
	public int ColumnSpan { get; set; }

	/// <summary>
	///     The row span
	/// </summary>
	[DataMember(Name = "rowSpan")]
	public int RowSpan { get; set; }

	/// <summary>
	///     The user permission values
	/// </summary>
	[DataMember(Name = "userPermission")]
	public UserPermission UserPermission { get; set; }

	/// <summary>
	///     The update interval
	/// </summary>
	[DataMember(Name = "interval")]
	public int UpdateIntervalMinutes { get; set; }

	/// <summary>
	///     The timescale
	/// </summary>
	[DataMember(Name = "timescale")]
	public string Timescale { get; set; } = string.Empty;

	/// <summary>
	///     SortBy
	/// </summary>
	[DataMember(Name = "sortBy")]
	public string SortBy { get; set; } = string.Empty;

	/// <summary>
	///     Display Column
	/// </summary>
	[DataMember(Name = "displayColumn")]
	public int DisplayColumn { get; set; }

	/// <summary>
	///     Whether to display a Warning Alert
	/// </summary>
	[DataMember(Name = "displayWarnAlert")]
	public bool DisplayWarningAlert { get; set; }

	/// <summary>
	///     Whether to display an Error Alert
	/// </summary>
	[DataMember(Name = "displayErrorAlert")]
	public bool DisplayErrorAlert { get; set; }

	/// <summary>
	///     Whether to display a Critical Alert
	/// </summary>
	[DataMember(Name = "displayCriticalAlert")]
	public bool DisplayCriticalAlert { get; set; }

	/// <summary>
	///     Whether ack Checked
	/// </summary>
	[DataMember(Name = "ackChecked")]
	public bool AckChecked { get; set; }

	/// <summary>
	///     Whether SDT Checked
	/// </summary>
	[DataMember(Name = "sdtChecked")]
	public bool SdtChecked { get; set; }

	/// <summary>
	///     The widget parameters
	/// </summary>
	[DataMember(Name = "params")]
	public List<WidgetParameter> WidgetParameters { get; set; } = [];

	/// <summary>
	///    Time Zone
	/// </summary>
	[DataMember(Name = "timezone")]
	public string TimeZone { get; set; } = string.Empty;

	/// <summary>
	///     The handled parameters
	/// </summary>
	protected virtual List<string> HandledParameters => ["interval", "timescale"];

	/// <summary>
	///     Whether this is a support custom property
	/// </summary>
	[DataMember(Name = "isSupportCustomProperty")]
	public bool IsSupportCustomProperty { get; set; }

	/// <summary>
	///     Whether this is a support custom property (again?)
	/// </summary>
	[DataMember(Name = "supportCustomProperty")]
	public bool SupportCustomProperty { get; set; }

	/// <summary>
	///     The endpoint
	/// </summary>
	public string Endpoint() => "dashboard/widgets";
}
