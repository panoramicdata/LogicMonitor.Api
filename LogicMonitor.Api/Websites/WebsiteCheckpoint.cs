namespace LogicMonitor.Api.Websites;

/// <summary>
/// Website checkpoint
/// </summary>
[DataContract]
public class WebsiteCheckpoint : IdentifiedItem
{
	/// <summary>
	/// Website authentication credentials
	/// </summary>
	[DataMember(Name = "geoInfo")]
	public string GeographicInformation { get; set; }

	/// <summary>
	/// Website MG Id
	/// </summary>
	[DataMember(Name = "smgId")]
	public int SmgId { get; set; }
}
