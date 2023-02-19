namespace LogicMonitor.Api.Websites;

/// <summary>
/// Website checkpoint
/// </summary>
[DataContract]
public class WebsiteCheckpoint : IdentifiedItem
{
	/// <summary>
	/// The geographical information (location) of the SiteMonitor Checkpoint
	/// </summary>
	[DataMember(Name = "geoInfo", IsRequired = false)]
	public string? GeographicInformation { get; set; }

	/// <summary>
	/// The sitemonitor group id
	/// </summary>
	[DataMember(Name = "smgId", IsRequired = false)]
	public int SmgId { get; set; }
}
