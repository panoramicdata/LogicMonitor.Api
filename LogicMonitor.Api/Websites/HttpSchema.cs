using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System.Runtime.Serialization;

namespace LogicMonitor.Api.Websites
{
	/// <summary>
	/// The Post data edit type
	/// </summary>
	[DataContract]
	[JsonConverter(typeof(StringEnumConverter))]
	public enum HttpSchema
	{
		/// <summary>
		/// Unknown
		/// </summary>
		[EnumMember(Value = "unknown")]
		Unknown = 0,

		/// <summary>
		/// HTTP
		/// </summary>
		[EnumMember(Value = "http")]
		Http = 1,

		/// <summary>
		/// HTTPS
		/// </summary>
		[EnumMember(Value = "https")]
		Https = 2,
	}
}