namespace LogicMonitor.Api;

/// <summary>
/// AwsAccountTestResult
/// </summary>

[DataContract]
public class AwsAccountTestResult
{
	/// <summary>
	/// noPermissionServices
	/// </summary>
	[DataMember(Name = "noPermissionServices")]
	public List<string> NoPermissionServices { get; set; } = [];

	/// <summary>
	/// detailLink
	/// </summary>
	[DataMember(Name = "detailLink")]
	public string DetailLink { get; set; } = string.Empty;

	/// <summary>
	/// nonPermissionErrors
	/// </summary>
	[DataMember(Name = "nonPermissionErrors")]
	public List<string> NonPermissionErrors { get; set; } = [];
}
