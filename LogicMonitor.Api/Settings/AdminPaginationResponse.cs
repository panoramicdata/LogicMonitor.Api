using System;
using System.Collections.Generic;
using System.Text;

namespace LogicMonitor.Api.Settings;

/// <summary>
/// AdminPaginationResponse
/// </summary>

[DataContract]
public class AdminPaginationResponse
{
	/// <summary>
	/// Total
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
	public List<Admin>? Items { get; set; }
}
