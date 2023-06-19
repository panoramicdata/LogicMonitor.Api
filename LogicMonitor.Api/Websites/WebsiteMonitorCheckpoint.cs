namespace LogicMonitor.Api.Websites;

/// <summary>
/// Website monitor checkpoint
/// </summary>
[DataContract]
public class WebsiteMonitorCheckpoint : NamedItem, IHasEndpoint
{
	/// <summary>
	/// The geographical information (location) of the SiteMonitor Checkpoint
	/// </summary>
	[DataMember(Name = "geoinfo")]
	public string? GeographicInformation { get; set; }

	/// <summary>
	/// The display priority of the SiteMonitor Checkpoint in your LogicMonitor portal
	/// </summary>
	[DataMember(Name = "displayPrio")]
	public int DisplayPriority { get; set; }

	/// <summary>
	/// Checks if sitemonitor enabled in root service group
	/// </summary>
	[DataMember(Name = "isEnabledInRoot")]
	public bool IsEnabledInRoot { get; set; }

	/// <inheritdoc />
	public string Endpoint() => "website/smcheckpoints";
}
