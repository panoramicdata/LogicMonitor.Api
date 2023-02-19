using System;
using System.Collections.Generic;
using System.Text;

namespace LogicMonitor.Api.Websites;

/// <summary>
/// WebsiteGroupPaginationResponse
/// </summary>

[DataContract]
public class WebsiteGroupPaginationResponse
{
	/// <summary>
	/// total
	/// </summary>
	[DataMember(Name = "total", IsRequired = false)]
	public int Total { get; set; }

	/// <summary>
	/// searchId
	/// </summary>
	[DataMember(Name = "searchId", IsRequired = false)]
	public string? SearchId { get; set; }

	/// <summary>
	/// items
	/// </summary>
	[DataMember(Name = "items", IsRequired = false)]
	public WebsiteGroup[]? Items { get; set; }
}
