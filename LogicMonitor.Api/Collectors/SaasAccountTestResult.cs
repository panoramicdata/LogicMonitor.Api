using System;
using System.Collections.Generic;
using System.Text;

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
	public string[] NonPermissionApisErrors { get; set; }

	/// <summary>
	/// invalidStatusUrls
	/// </summary>
	[DataMember(Name = "invalidStatusUrls")]
	public string InvalidStatusUrls { get; set; }

	/// <summary>
	/// nonPermissionService
	/// </summary>
	[DataMember(Name = "nonPermissionService")]
	public string NonPermissionService { get; set; }

	/// <summary>
	/// resultCode
	/// </summary>
	[DataMember(Name = "resultCode")]
	public int ResultCode { get; set; }

	/// <summary>
	/// detailLink
	/// </summary>
	[DataMember(Name = "detailLink")]
	public string DetailLink { get; set; }

	/// <summary>
	/// nonPermissionApis
	/// </summary>
	[DataMember(Name = "nonPermissionApis")]
	public string[] NonPermissionApis { get; set; }
}
