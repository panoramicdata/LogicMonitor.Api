using System.Collections.Generic;
using System.Runtime.Serialization;

namespace LogicMonitor.Api.Dashboards;

/// <summary>
/// A Dynamic table widget
/// </summary>
[DataContract]
public class DynamicTableWidget : Widget
{
	/// <summary>
	///     The dataSourceId
	/// </summary>
	[DataMember(Name = "dataSourceId")]
	public string DataSourceId { get; set; }

	/// <summary>
	///     The dataSourceFullName
	/// </summary>
	[DataMember(Name = "dataSourceFullName")]
	public string DataSourceFullName { get; set; }

	/// <summary>
	///     The columns
	/// </summary>
	[DataMember(Name = "columns")]
	public List<DynamicTableWidgetColumn> Columns { get; set; }

	/// <summary>
	///     The columns
	/// </summary>
	[DataMember(Name = "rows")]
	public List<DynamicTableWidgetRow> Rows { get; set; }

	/// <summary>
	///     The forecast
	/// </summary>
	[DataMember(Name = "forecast")]
	public DynamicTableWidgetForecast Forecast { get; set; }

	/// <summary>
	///     The Top X
	/// </summary>
	[DataMember(Name = "topX")]
	public int TopX { get; set; }

	/// <summary>
	///     The Sort order
	/// </summary>
	[DataMember(Name = "sortOrder")]
	public string SortOrder { get; set; }

	/// <summary>
	///     The column headers
	/// </summary>
	[DataMember(Name = "columnHeaders")]
	public List<ColumnHeader> ColumnHeaders { get; set; }

	/// <summary>
	///     The Display Settings
	/// </summary>
	[DataMember(Name = "displaySettings")]
	public object DisplaySettings { get; set; }
}
