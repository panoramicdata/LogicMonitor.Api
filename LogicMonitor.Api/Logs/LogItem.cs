using LogicMonitor.Api.Extensions;
using System;
using System.Runtime.Serialization;

namespace LogicMonitor.Api.Logs;

/// <summary>
///    User logged event
/// </summary>
[DataContract]
public class LogItem : StringIdentifiedItem, IHasEndpoint
{
	/// <summary>
	///    The user that performed the action
	/// </summary>
	[DataMember(Name = "username")]
	public string UserName { get; set; }

	/// <summary>
	///    The DateTime the event happened in seconds since the Epoch
	/// </summary>
	[DataMember(Name = "happenedOn")]
	public long HappenedOnTimeStampUtc { get; set; }

	/// <summary>
	///    The session ID
	/// </summary>
	[DataMember(Name = "sessionId")]
	public string SessionId { get; set; }

	/// <summary>
	///    Event description
	/// </summary>
	[DataMember(Name = "description")]
	public string Description { get; set; }

	/// <summary>
	///    Event time (local)
	/// </summary>
	[DataMember(Name = "happenedOnLocal")]
	public string HappenedOnLocalString { get; set; }

	/// <summary>
	///    IP Address
	/// </summary>
	[DataMember(Name = "ip")]
	public string IpAddress { get; set; }

	/// <summary>
	///    The DateTime the event happened UTC
	/// </summary>
	[IgnoreDataMember]
	public DateTime HappenedOnUtc => HappenedOnTimeStampUtc.ToDateTimeUtc();

	/// <summary>
	///    Returns a string that represents the current object.
	/// </summary>
	public override string ToString() => $"{HappenedOnUtc:yyyy-MM-dd}: {UserName} - {Description}";

	/// <inheritdoc />
	public string Endpoint() => "setting/accesslogs";
}
