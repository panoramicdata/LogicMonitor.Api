namespace LogicMonitor.Api.Websites;

/// <summary>
///     A LogicMonitor Collector
/// </summary>
[DataContract]
public class WebsiteCollectorInfo : DescribedItem
{
	/// <summary>
	///     Hostname
	/// </summary>
	[DataMember(Name = "hostname")]
	public string HostName { get; set; }

	/// <summary>
	///     The collector group id
	/// </summary>
	[DataMember(Name = "collectorGroupId")]
	public int CollectorGroupId { get; set; }

	/// <summary>
	///     The collector group name
	/// </summary>
	[DataMember(Name = "collectorGroupName")]
	public string CollectorGroupName { get; set; }

	/// <summary>
	///     The local time of user change
	/// </summary>
	[DataMember(Name = "status")]
	public string Status { get; set; }
}
