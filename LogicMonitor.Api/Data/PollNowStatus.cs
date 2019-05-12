using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System.Runtime.Serialization;

namespace LogicMonitor.Api.Data
{
	/// <summary>
	/// A poll now status
	/// </summary>
	[DataContract]
	[JsonConverter(typeof(StringEnumConverter))]
	public enum PollNowStatus
	{
		/// <summary>
		/// Unknown
		/// </summary>
		[EnumMember(Value = "unknown")]
		Unknown = 0,

		/// <summary>
		/// Waiting
		/// </summary>
		[EnumMember(Value = "waiting")]
		Waiting,

		/// <summary>
		/// Running
		/// </summary>
		[EnumMember(Value = "running")]
		Running,

		/// <summary>
		/// Done
		/// </summary>
		[EnumMember(Value = "done")]
		Done,
	}
}