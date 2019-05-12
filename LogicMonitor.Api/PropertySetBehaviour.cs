using System.Runtime.Serialization;

namespace LogicMonitor.Api
{
	/// <summary>
	/// The property set behaviour
	/// </summary>
	public enum PropertySetBehaviour
	{
		/// <summary>
		/// Unknown
		/// </summary>
		[EnumMember(Value = "unknown")]
		Unknown = 0,

		/// <summary>
		/// The properties included in the payload will be added, but all existing properties will remain the same
		/// </summary>
		Add = 1,

		/// <summary>
		/// The properties included in the request payload will be added if they don't already exist, or updated if they do already exist, but all other existing properties will remain the same.
		/// </summary>
		Replace
	}
}