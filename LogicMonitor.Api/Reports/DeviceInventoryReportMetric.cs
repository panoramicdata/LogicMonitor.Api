using System.Runtime.Serialization;

namespace LogicMonitor.Api.Reports;

/// <summary>
/// A HostInventoryReportMetric
/// </summary>
[DataContract]
public class DeviceInventoryReportMetric
{
	/// <summary>
	/// The item type
	/// </summary>
	[DataMember(Name = "itemType")]
	public string ItemType { get; set; }

	/// <summary>
	/// The item value
	/// </summary>
	[DataMember(Name = "itemVal")]
	public string ItemValue { get; set; }
}
