using System;
using System.Collections.Generic;
using System.Text;

namespace LogicMonitor.Api.Netscans;

/// <summary>
/// NetscanPaginationResponse
/// </summary>

[DataContract]
public class NetscanPaginationResponse
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
	public List<Netscan>? Items { get; set; }
}
