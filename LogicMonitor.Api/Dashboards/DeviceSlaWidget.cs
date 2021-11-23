using System.Collections.Generic;
using System.Runtime.Serialization;

namespace LogicMonitor.Api.Dashboards;

/// <summary>
///     A Device SLA widget
/// </summary>
[DataContract]
public class DeviceSlaWidget : Widget
{
	/// <summary>
	/// The bottom label
	/// </summary>
	[DataMember(Name = "bottomLabel")]
	public string BottomLabel { get; set; }

	/// <summary>
	/// The metrics
	/// </summary>
	[DataMember(Name = "metrics")]
	public List<DeviceSlaWidgetMetric> Metrics { get; set; }

	/// <summary>
	/// The daysInWeek
	/// </summary>
	[DataMember(Name = "daysInWeek")]
	public string DaysInWeek { get; set; }

	/// <summary>
	/// The periodInOneDay
	/// </summary>
	[DataMember(Name = "periodInOneDay")]
	public string PeriodInOneDay { get; set; }

	/// <summary>
	/// The unmonitoredTimeType
	/// </summary>
	[DataMember(Name = "unmonitoredTimeType")]
	public int UnmonitoredTimeType { get; set; }

	/// <summary>
	/// The displayType
	/// </summary>
	[DataMember(Name = "displayType")]
	public int DisplayType { get; set; }

	/// <summary>
	/// The unit label
	/// </summary>
	[DataMember(Name = "unitLabel")]
	public string UnitLabel { get; set; }

	/// <summary>
	/// The top X
	/// </summary>
	[DataMember(Name = "topX")]
	public int TopX { get; set; }

	/// <summary>
	/// The color thresholds
	/// </summary>
	[DataMember(Name = "colorThresholds")]
	public List<ColorThreshold> ColorThresholds { get; set; }

	/// <summary>
	///     The display settings
	/// </summary>
	[DataMember(Name = "displaySettings")]
	public object DisplaySettings { get; set; }
}
