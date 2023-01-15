using System;
using System.Collections.Generic;
using System.Text;

namespace LogicMonitor.Api.Settings;

/// <summary>
/// The alert rule pagination response
/// </summary>
[DataContract]
public class AlertRulePaginationResponse
{
	/// <summary>
	/// The total
	/// </summary>
	[DataMember(Name = "total")]
	public int Total { get; set; }

	/// <summary>
	/// The searchId
	/// </summary>
	[DataMember(Name = "searchId")]
	public string SearchId { get; set; }

	/// <summary>
	/// The items
	/// </summary>
	[DataMember(Name = "items")]
	public List<AlertRule> Items { get; set; }
}
