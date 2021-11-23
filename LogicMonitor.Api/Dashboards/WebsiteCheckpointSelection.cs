using System.Runtime.Serialization;

namespace LogicMonitor.Api.Dashboards;

/// <summary>
/// Website checkpoint
/// </summary>
[DataContract]
public class WebsiteCheckpointSelection
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

	/// <summary>
	/// Whether the Website MG is selected
	/// </summary>
	[DataMember(Name = "selected")]
	public bool IsSelected { get; set; }
}
