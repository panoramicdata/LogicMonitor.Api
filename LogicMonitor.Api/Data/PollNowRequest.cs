using System.Runtime.Serialization;

namespace LogicMonitor.Api.Data
{
	/// <summary>
	/// A poll now request
	/// </summary>
	[DataContract]
	public class PollNowRequest
	{
		/// <summary>
		/// The collector id
		/// </summary>
		[DataMember(Name = "agentId")]
		public int CollectorId { get; set; }

		/// <summary>
		/// The request id
		/// </summary>
		[DataMember(Name = "requestId")]
		public int RequestId { get; set; }
	}
}