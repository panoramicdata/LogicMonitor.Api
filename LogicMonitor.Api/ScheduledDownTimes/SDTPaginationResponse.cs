using System;
using System.Collections.Generic;
using System.Text;

namespace LogicMonitor.Api.ScheduledDownTimes;

/// <summary>
/// SDTPaginationResponse
/// </summary>

[DataContract]
public class SDTPaginationResponse
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
	public ScheduledDownTime[]? Items { get; set; }
}
