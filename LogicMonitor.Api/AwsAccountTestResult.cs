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
	[DataMember(Name = "noPermissionServices", IsRequired = false)]
	public List<string>? NoPermissionServices { get; set; }

	/// <summary>
	/// detailLink
	/// </summary>
	[DataMember(Name = "detailLink", IsRequired = false)]
	public string DetailLink { get; set; }

	/// <summary>
	/// nonPermissionErrors
	/// </summary>
	[DataMember(Name = "nonPermissionErrors", IsRequired = false)]
	public List<string>? NonPermissionErrors { get; set; }
}
