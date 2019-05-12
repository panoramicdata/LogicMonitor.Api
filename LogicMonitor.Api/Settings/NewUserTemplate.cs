using System.Runtime.Serialization;

namespace LogicMonitor.Api.Settings
{
	/// <summary>
	///     New user message template
	/// </summary>
	[DataContract]
	public class NewUserMessageTemplate : IHasSingletonEndpoint
	{
		/// <summary>
		///     messageSubject
		/// </summary>
		[DataMember(Name = "messageSubject")]
		public string Subject { get; set; }

		/// <summary>
		///     messageBody
		/// </summary>
		[DataMember(Name = "messageBody")]
		public string Body { get; set; }

		/// <inheritdoc />
		public string Endpoint() => "setting/messagetemplate";
	}
}