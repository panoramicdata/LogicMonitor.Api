using System.Runtime.Serialization;

namespace LogicMonitor.Api.Alerts
{
	/// <summary>
	/// The notification stage type
	/// </summary>
	[DataContract]
	public enum NotificationStageType
	{
		/// <summary>
		/// Unknown
		/// </summary>
		[EnumMember(Value = "unknown")]
		Unknown = 0,

		/// <summary>
		/// A group stage type
		/// </summary>
		[EnumMember(Value = "group")]
		Group = 1,

		/// <summary>
		/// A group stage type
		/// </summary>
		[EnumMember(Value = "ADMIN")]
		Admin = 2,

		/// <summary>
		/// A arbitrary stage type
		/// </summary>
		[EnumMember(Value = "ARBITRARY")]
		Arbitrary = 3,
	}
}