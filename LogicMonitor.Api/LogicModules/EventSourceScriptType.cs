using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System.Runtime.Serialization;

namespace LogicMonitor.Api.LogicModules
{
	/// <summary>
	///    EventSourceScriptType
	/// </summary>
	[DataContract]
	[JsonConverter(typeof(StringEnumConverter))]
	public enum EventSourceScriptType : byte
	{
		/// <summary>
		/// Unknown
		/// </summary>
		[EnumMember(Value = "unknown")]
		Unknown = 0,

		/// <summary>
		///    Embed
		/// </summary>
		[EnumMember(Value = "embed")]
		Embed = 1,
	}
}