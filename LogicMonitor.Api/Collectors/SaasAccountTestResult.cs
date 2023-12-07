namespace LogicMonitor.Api.Collectors;

/// <summary>
/// SaasAccountTestResult
/// </summary>

[DataContract]
public class SaasAccountTestResult
{
	/// <summary>
	/// nonPermissionApisErrors
	/// </summary>
	[DataMember(Name = "nonPermissionApisErrors")]
	public List<string> NonPermissionApisErrors { get; set; } = [];

	/// <summary>
	/// invalidStatusUrls
	/// </summary>
	[DataMember(Name = "invalidStatusUrls")]
	public string InvalidStatusUrls { get; set; } = string.Empty;

	/// <summary>
	/// nonPermissionService
	/// </summary>
	[DataMember(Name = "nonPermissionService")]
	public string NonPermissionService { get; set; } = string.Empty;

	/// <summary>
	/// resultCode
	/// </summary>
	[DataMember(Name = "resultCode")]
	public int ResultCode { get; set; }

	/// <summary>
	/// detailLink
	/// </summary>
	[DataMember(Name = "detailLink")]
	public string DetailLink { get; set; } = string.Empty;

	/// <summary>
	/// nonPermissionApis
	/// </summary>
	[DataMember(Name = "nonPermissionApis")]
	public List<string> NonPermissionApis { get; set; } = [];
}
