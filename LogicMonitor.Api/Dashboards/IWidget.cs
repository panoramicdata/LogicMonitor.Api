using System;
using System.Collections.Generic;
using System.Text;

namespace LogicMonitor.Api.Dashboards;

/// <summary>
/// Widget
/// </summary>

public interface IWidget
{
	/// <summary>
	/// alert | batchjob | flash | gmap | ngraph | ograph | cgraph | sgraph | netflowgraph | groupNetflowGraph | netflow | groupNetflow | html | bigNumber | gauge | pieChart | table | dynamicTable | deviceSLA | text | statsd | deviceStatus | serviceAlert | noc | websiteOverview | websiteOverallStatus | websiteIndividualStatus | websiteSLA | savedMap
	/// </summary>
	public string Type { get; set; }

	/// <summary>
	///     When the widget was last updated
	/// </summary>
	public int LastUpdatedOnSeconds { get; set; }

	/// <summary>
	///     The ordinal
	/// </summary>
	public string LastUpdatedBy { get; set; }

	/// <summary>
	///     The dashboard id
	/// </summary>
	public int DashboardId { get; set; }

	/// <summary>
	///     The column index
	/// </summary>
	public int ColumnIndex { get; set; }

	/// <summary>
	///     The ordinal
	/// </summary>
	public int Order { get; set; }

	/// <summary>
	///     The theme
	/// </summary>
	public string Theme { get; set; }

	/// <summary>
	///     The column span
	/// </summary>
	public int ColumnSpan { get; set; }

	/// <summary>
	///     The row span
	/// </summary>
	public int RowSpan { get; set; }

	/// <summary>
	///     The user permission values
	/// </summary>
	public UserPermission UserPermission { get; set; }

	/// <summary>
	///     The update interval
	/// </summary>
	public int UpdateIntervalMinutes { get; set; }

	/// <summary>
	///     The timescale
	/// </summary>
	public string Timescale { get; set; }

	/// <summary>
	///     SortBy
	/// </summary>
	public string SortBy { get; set; }

	/// <summary>
	///     Display Column
	/// </summary>
	public int DisplayColumn { get; set; }

	/// <summary>
	///     Whether to display a Warning Alert
	/// </summary>
	public bool DisplayWarningAlert { get; set; }

	/// <summary>
	///     Whether to display an Error Alert
	/// </summary>
	public bool DisplayErrorAlert { get; set; }

	/// <summary>
	///     Whether to display a Critical Alert
	/// </summary>
	public bool DisplayCriticalAlert { get; set; }

	/// <summary>
	///     Whether ack Checked
	/// </summary>
	public bool AckChecked { get; set; }

	/// <summary>
	///     Whether SDT Checked
	/// </summary>
	public bool SdtChecked { get; set; }

	/// <summary>
	///     The widget parameters
	/// </summary>
	public List<WidgetParameter> WidgetParameters { get; set; }

	/// <summary>
	///    Time Zone
	/// </summary>
	public string TimeZone { get; set; }
}
