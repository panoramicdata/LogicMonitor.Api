using System;
using System.Collections.Generic;
using System.Text;

namespace LogicMonitor.Api.Dashboards;

/// <summary>
/// AlertWidgetData
/// </summary>

[DataContract]
public class AlertWidgetData : WidgetData
{
	/// <summary>
	/// Total
	/// </summary>
	[DataMember(Name = "total")]
	public int Total { get; set; }

	/// <summary>
	/// Search id
	/// </summary>
	[DataMember(Name = "searchId")]
	public string SearchId { get; set; } = string.Empty;

	/// <summary>
	/// Items
	/// </summary>
	[DataMember(Name = "items")]
	public List<Alert> Items { get; set; } = new();
}
