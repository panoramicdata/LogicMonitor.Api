using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System.Runtime.Serialization;

namespace LogicMonitor.Api.Netscans
{
	/// <summary>
	///    A netscan policy inclusion type
	/// </summary>
	[DataContract]
	[JsonConverter(typeof(StringEnumConverter))]
	public enum NetscanInclusionType
	{
		/// <summary>
		/// Unknown
		/// </summary>
		[EnumMember(Value = "unknown")]
		Unknown = 0,

		/// <summary>
		///    All devices
		/// </summary>
		[EnumMember(Value = "Include")]
		Include = 1,

		/// <summary>
		///    Cisco devices
		/// </summary>
		[EnumMember(Value = "Exclude")]
		Exclude = 2
	}
}