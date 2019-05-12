using System.Runtime.Serialization;

namespace LogicMonitor.Api.Websites
{
	/// <summary>
	/// The mtachType
	/// </summary>
	public enum MatchType
	{
		/// <summary>
		/// Unknown
		/// </summary>
		[EnumMember(Value = "unknown")]
		Unknown = 0,

		/// <summary>
		/// Plain
		/// </summary>
		[DataMember(Name = "plain")]
		Plain = 1,

		/// <summary>
		/// JSON
		/// </summary>
		[DataMember(Name = "json")]
		Json = 2,

		/// <summary>
		/// Regular
		/// </summary>
		[DataMember(Name = "regular")]
		Regular = 3
	}
}