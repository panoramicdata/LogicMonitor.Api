using System;
using System.Collections.Generic;
using System.Text;

namespace LogicMonitor.Api.LogicModules;

/// <summary>
/// UpdateReason
/// </summary>

[DataContract]
public class DataSourceUpdateReason :IdentifiedItem
{
	/// <summary>
	/// update date epoch
	/// </summary>
	[DataMember(Name = "timeEpoch")]
	public int TimeEpoch { get; set; }

	/// <summary>
	/// Client IP from which this update has been made
	/// </summary>
	[DataMember(Name = "clientIp")]
	public string ClientIp { get; set; }

	/// <summary>
	/// update reason
	/// </summary>
	[DataMember(Name = "updateReason")]
	public string UpdateReason { get; set; }

	/// <summary>
	/// update date in form \u0027YYYY-MM-DD HH:MM:SS\u0027
	/// </summary>
	[DataMember(Name = "timeStr")]
	public string TimeStr { get; set; }

	/// <summary>
	/// user who made this update
	/// </summary>
	[DataMember(Name = "userName")]
	public string UserName { get; set; }
}
