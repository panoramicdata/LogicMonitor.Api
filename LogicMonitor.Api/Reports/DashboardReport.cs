namespace LogicMonitor.Api.Reports;

/// <summary>
/// A dashboard report
/// </summary>
[DataContract]
public class DashboardReport : DateRangeReport
{
	/// <summary>
	/// The dashboardId
	/// </summary>
	[DataMember(Name = "dashboardId")]
	public int DashboardId { get; set; }

	/// <summary>
	/// The displayName
	/// </summary>
	[DataMember(Name = "displayName")]
	public string DisplayName { get; set; } = string.Empty;

	/// <summary>
	/// The dashboardName
	/// </summary>
	[DataMember(Name = "dashboardName")]
	public string DashboardName { get; set; } = string.Empty;

	/// <summary>
	/// The dashboardGroupFullPath
	/// </summary>
	[DataMember(Name = "dashboardGroupFullPath")]
	public string DashboardGroupFullPath { get; set; } = string.Empty;

	/// <summary>
	/// The widgetExtraData
	/// </summary>
	[DataMember(Name = "widgetExtraData")]
	public string WidgetExtraData { get; set; } = string.Empty;

	/// <summary>
	/// The fromSource
	/// </summary>
	[DataMember(Name = "fromSource")]
	public int FromSource { get; set; }

	/// <summary>
	/// The displayLink
	/// </summary>
	[DataMember(Name = "displayLink")]
	public bool DisplayLink { get; set; }
}
