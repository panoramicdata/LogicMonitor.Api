using System.Runtime.Serialization;

namespace LogicMonitor.Api.PushMetrics;

/// <summary>
/// A Push Metric response's resource ids
/// </summary>
[DataContract]
public class PushMetricResponseResourceIds
{
	/// <summary>
	/// The device id
	/// </summary>
	[DataMember(Name = "system.deviceId")]
	public string DeviceId { get; set; }
}
