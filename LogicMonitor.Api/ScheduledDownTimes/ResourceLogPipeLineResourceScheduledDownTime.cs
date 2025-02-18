﻿namespace LogicMonitor.Api.ScheduledDownTimes;

/// <summary>
/// ResourceLogPipeLineResourceSDT
/// </summary>
[DataContract]
public class ResourceLogPipeLineResourceScheduledDownTime
{
	/// <summary>
	/// The id of the Resource logPipeLineResource that the SDT will be associated with
	/// </summary>
	[DataMember(Name = "deviceLogPipeLineResourceId")]
	public int ResourceLogPipeLineResourceId { get; set; }

	/// <summary>
	/// The name of the pipe line that the SDT will apply to
	/// </summary>
	[DataMember(Name = "logPipeLineName")]
	public string? LogPipeLineName { get; set; }

	/// <summary>
	/// The id of the device associated with the pipe line that the SDT will apply to
	/// </summary>
	[DataMember(Name = "deviceId")]
	public int ResourceId { get; set; }

	/// <summary>
	/// The display name of the device associated with the logPipeLine that the SDT will apply to
	/// </summary>
	[DataMember(Name = "deviceDisplayName")]
	public string? ResourceDisplayName { get; set; }
}
