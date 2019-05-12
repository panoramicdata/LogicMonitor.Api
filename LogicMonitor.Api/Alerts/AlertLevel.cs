using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System.Runtime.Serialization;

namespace LogicMonitor.Api.Alerts
{
	/// <summary>
	///    Alert level
	/// </summary>
	[DataContract]
	[JsonConverter(typeof(StringEnumConverter))]
	public enum AlertLevel : byte
	{
		/// <summary>
		///    All level (same as warning and above)
		/// </summary>
		[EnumMember(Value = "")]
		Unknown = 0,

		/// <summary>
		///    All level (same as warning and above)
		/// </summary>
		[EnumMember(Value = "all")]
		All = 1,

		/// <summary>
		///    Warning level
		/// </summary>
		[EnumMember(Value = "warn")]
		Warning = 2,

		/// <summary>
		///    Error level
		/// </summary>
		[EnumMember(Value = "error")]
		Error = 3,

		/// <summary>
		///    Critical level
		/// </summary>
		[EnumMember(Value = "critical")]
		Critical = 4,

		/// <summary>
		///    Critical level
		/// </summary>
		[EnumMember(Value = "doMapping")]
		DoMapping = 5,

		/// <summary>
		///    Any level (same as warning and above)
		/// </summary>
		[EnumMember(Value = "any")]
		Any = 6,
	}
}