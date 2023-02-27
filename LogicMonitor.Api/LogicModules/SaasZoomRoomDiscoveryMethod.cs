namespace LogicMonitor.Api.LogicModules;

/// <summary>
/// SaasZoomRoomDiscoveryMethod
/// </summary>
[DataContract]
public class SaasZoomRoomDiscoveryMethod : AutoDiscoveryMethod
{
	/// <summary>
	/// zoomRoomIssueType
	/// </summary>
	[DataMember(Name = "zoomRoomIssueType")]
	public string ZoomRoomIssueType { get; set; } = null!;
}
