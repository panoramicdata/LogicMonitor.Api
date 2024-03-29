﻿namespace LogicMonitor.Api.Dashboards;

/// <summary>
/// The WebsiteSlaWidgetMetric
/// </summary>
[DataContract]
public class WebsiteSlaWidgetMetric
{
	/// <summary>
	/// Whether to exclude SDT
	/// </summary>
	[DataMember(Name = "excludeSDT")]
	public bool ExcludeSdt { get; set; }

	/// <summary>
	/// The Website group name
	/// </summary>
	[DataMember(Name = "websiteGroup")]
	public string WebsiteGroupName { get; set; } = string.Empty;

	/// <summary>
	/// The Website name
	/// </summary>
	[DataMember(Name = "website")]
	public string WebsiteName { get; set; } = string.Empty;
}
