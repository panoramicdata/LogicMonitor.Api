﻿namespace LogicMonitor.Api.Data;

/// <summary>
/// A poll now response
/// </summary>
[DataContract]
public class PollNowResponse
{
	/// <summary>
	/// The request status
	/// </summary>
	[DataMember(Name = "requestStatus")]
	public PollNowStatus RequestStatus { get; set; } = new();

	/// <summary>
	/// The result
	/// </summary>
	[DataMember(Name = "result")]
	public PollNowResult Result { get; set; } = new();
}
