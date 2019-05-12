using System.Runtime.Serialization;

namespace LogicMonitor.Api.Flows
{
	/// <summary>
	/// A Flow data field
	/// </summary>
	public enum FlowField
	{
		/// <summary>
		/// Unknown
		/// </summary>
		[EnumMember(Value = "unknown")]
		Unknown = 0,

		/// <summary>
		/// Usage
		/// </summary>
		Usage = 1,
	}
}