using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System.Runtime.Serialization;

namespace LogicMonitor.Api.Netscans;

/// <summary>
///    A Netscan Policy Schedule Type
/// </summary>
[DataContract(Name = "type")]
[JsonConverter(typeof(StringEnumConverter))]
public enum NetscanScheduleType
{
	/// <summary>
	/// Unknown
	/// </summary>
	[EnumMember(Value = "unknown")]
	Unknown = 0,

	/// <summary>
	///    Manual
	/// </summary>
	[EnumMember(Value = "manual")]
	Manual,

	/// <summary>
	///    Daily
	/// </summary>
	[EnumMember(Value = "daily")]
	Daily,

	/// <summary>
	///    Weekly
	/// </summary>
	[EnumMember(Value = "weekly")]
	Weekly,

	/// <summary>
	///    Weekly
	/// </summary>
	[EnumMember(Value = "monthly")]
	Monthly,

	/// <summary>
	///    Hourly
	/// </summary>
	[EnumMember(Value = "hourly")]
	Hourly
}
