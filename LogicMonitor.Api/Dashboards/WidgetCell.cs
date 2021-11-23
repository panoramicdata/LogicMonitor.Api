using System.Collections.Generic;
using System.Runtime.Serialization;

namespace LogicMonitor.Api.Dashboards;

/// <summary>
/// A widget cell
/// </summary>
[DataContract]
public class WidgetCell
{
	/// <summary>
	/// Device Display Name
	/// </summary>
	[DataMember(Name = "deviceDisplayName")]
	public string DeviceDisplayName { get; set; }

	/// <summary>
	/// Value
	/// </summary>
	[DataMember(Name = "value")]
	public float Value { get; set; }

	/// <summary>
	/// Instance Id
	/// </summary>
	[DataMember(Name = "instanceId")]
	public int InstanceId { get; set; }

	/// <summary>
	/// Instance name
	/// </summary>
	[DataMember(Name = "instanceName")]
	public string InstanceName { get; set; }

	/// <summary>
	/// Forecast day
	/// </summary>
	[DataMember(Name = "forecastDay")]
	public int ForecastDay { get; set; }

	/// <summary>
	/// Alert status
	/// </summary>
	[DataMember(Name = "alertStatus")]
	public object AlertStatus { get; set; }

	/// <summary>
	/// Alert severity
	/// </summary>
	[DataMember(Name = "alertSeverity")]
	public string AlertSeverity { get; set; }

	/// <summary>
	/// Days until Alert list
	/// </summary>
	[DataMember(Name = "daysUntilAlertList")]
	public List<object> DaysUntilAlertList { get; set; }

	/// <summary>
	/// Color Level
	/// </summary>
	[DataMember(Name = "colorLevel")]
	public int ColorLevel { get; set; }

	/// <summary>
	/// Raw value
	/// </summary>
	[DataMember(Name = "rawValue")]
	public float RawValue { get; set; }
}
