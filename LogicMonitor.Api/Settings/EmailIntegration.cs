using System.Runtime.Serialization;

namespace LogicMonitor.Api.Settings
{
	/// <summary>
	///     An email integration
	/// </summary>
	[DataContract]
	public class EmailIntegration : Integration
	{
		/// <summary>
		///     The sender
		/// </summary>
		[DataMember(Name = "sender")]
		public string Sender { get; set; }

		/// <summary>
		///     The receivers
		/// </summary>
		[DataMember(Name = "receivers")]
		public string Receivers { get; set; }

		/// <summary>
		///     The subject
		/// </summary>
		[DataMember(Name = "subject")]
		public string Subject { get; set; }

		/// <summary>
		///     The body
		/// </summary>
		[DataMember(Name = "body")]
		public string Body { get; set; }
	}
}