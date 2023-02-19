namespace LogicMonitor.Api.Websites;

/// <summary>
/// A LogicMonitor Collector
/// </summary>
[DataContract]
public class WebsiteCollectorInfo : DescribedItem
{
	/// <summary>
	/// The hostname of the collector
	/// </summary>
	[DataMember(Name = "hostname", IsRequired = false)]
	public string? HostName { get; set; }

	/// <summary>
	/// The group name of the group the collector is present in
	/// </summary>
	[DataMember(Name = "collectorGroupName")]
	public string? CollectorGroupName { get; set; }

	/// <summary>
	/// The group Id of the group the collector is present in
	/// </summary>
	[DataMember(Name = "collectorGroupId")]
	public int CollectorGroupId { get; set; }

	/// <summary>
	/// The status of the collector
	/// </summary>
	[DataMember(Name = "status")]
	public string? Status { get; set; }
}
