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
	[DataMember(Name = "zoomRoomIssueType", IsRequired = true)]
	public string ZoomRoomIssueType { get; set; } = null!;
}
