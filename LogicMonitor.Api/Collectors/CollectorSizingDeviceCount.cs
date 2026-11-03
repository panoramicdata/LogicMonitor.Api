namespace LogicMonitor.Api.Collectors;

/// <summary>
/// A count of devices and their instances
/// </summary>
[DataContract]
public class CollectorSizingDeviceCount
{
	/// <summary>
	/// The count
	/// </summary>
	[DataMember(Name = "count")]
	public int Count { get; set; }

	/// <summary>
	/// The instances
	/// </summary>
	[DataMember(Name = "instances")]
	public int Instances { get; set; }
}
