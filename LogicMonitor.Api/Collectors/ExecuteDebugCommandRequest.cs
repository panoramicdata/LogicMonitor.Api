using System.Runtime.Serialization;

namespace LogicMonitor.Api.Collectors
{
	/// <summary>
	/// ExecuteDebugCommandRequest information
	/// </summary>
	[DataContract]
	public class ExecuteDebugCommandRequest
	{
		/// <summary>
		/// The request ID
		/// </summary>
		[DataMember(Name = "cmdline")]
		public string Command { get; set; }
	}
}