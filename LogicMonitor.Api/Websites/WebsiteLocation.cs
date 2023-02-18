namespace LogicMonitor.Api.Websites;

/// <summary>
/// A test location
/// </summary>
[DataContract]
public class WebsiteLocation
{
	/// <summary>
	/// This field only for the SiteMonitor Groups, does not include Internal Service Groups
	/// </summary>
	[DataMember(Name = "all", IsRequired = false)]
	public bool All { get; set; }

	/// <summary>
	/// The Internal Service Groups Ids
	/// </summary>
	[DataMember(Name = "collectorIds", IsRequired = false)]
	public int[]? CollectorIds { get; set; }

	/// <summary>
	/// The collector info of the services
	/// </summary>
	[DataMember(Name = "collectors")]
	public WebsiteCollectorInfo[]? Collectors { get; set; }

	/// <summary>
	/// The SiteMonitor Groups Ids
	/// </summary>
	[DataMember(Name = "smgIds")]
	public int[]? SmgIds { get; set; }






}
