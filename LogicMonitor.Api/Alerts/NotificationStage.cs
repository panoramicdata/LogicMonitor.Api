using System.Runtime.Serialization;

namespace LogicMonitor.Api.Alerts
{
	/// <summary>
	/// A notification stage
	/// </summary>
	[DataContract]
	public class NotificationStage
	{
		/// <summary>
		/// The destination type
		/// </summary>
		[DataMember(Name = "type")]
		public NotificationStageType NotificationStageType { get; set; }

		/// <summary>
		/// The destination address
		/// </summary>
		[DataMember(Name = "addr")]
		public string Address { get; set; }

		/// <summary>
		/// The comment
		/// </summary>
		[DataMember(Name = "comment")]
		public string Comment { get; set; }

		/// <summary>
		/// The method
		/// </summary>
		[DataMember(Name = "method")]
		public string Method { get; set; }

		/// <summary>
		/// The contact details
		/// </summary>
		[DataMember(Name = "contact")]
		public string Contact { get; set; }
	}
}