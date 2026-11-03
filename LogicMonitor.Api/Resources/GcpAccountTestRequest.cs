namespace LogicMonitor.Api.Resources;

/// <summary>
/// A request to test GCP account credentials
/// </summary>
[DataContract]
public class GcpAccountTestRequest
{
	/// <summary>
	/// The checked services
	/// </summary>
	[DataMember(Name = "checkedServices")]
	public string CheckedServices { get; set; } = string.Empty;

	/// <summary>
	/// The group id
	/// </summary>
	[DataMember(Name = "groupId")]
	public int GroupId { get; set; }

	/// <summary>
	/// The project id
	/// </summary>
	[DataMember(Name = "projectId")]
	public string ProjectId { get; set; } = string.Empty;

	/// <summary>
	/// The service account key
	/// </summary>
	[DataMember(Name = "serviceAccountKey")]
	public string ServiceAccountKey { get; set; } = string.Empty;
}
