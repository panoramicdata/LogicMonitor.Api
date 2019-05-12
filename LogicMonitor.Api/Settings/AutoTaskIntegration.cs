using System.Runtime.Serialization;

namespace LogicMonitor.Api.Settings
{
	/// <summary>
	///     An AutoTask integration
	/// </summary>
	[DataContract]
	public class AutoTaskIntegration : Integration
	{
		/// <summary>
		/// The method
		/// </summary>
		[DataMember(Name = "method")]
		public string Method { get; set; }

		/// <summary>
		/// The updateMethod
		/// </summary>
		[DataMember(Name = "updateMethod")]
		public string UpdateMethod { get; set; }

		/// <summary>
		/// The clearMethod
		/// </summary>
		[DataMember(Name = "clearMethod")]
		public string ClearMethod { get; set; }

		/// <summary>
		/// The ackMethod
		/// </summary>
		[DataMember(Name = "ackMethod")]
		public string AckMethod { get; set; }

		/// <summary>
		/// The url
		/// </summary>
		[DataMember(Name = "url")]
		public string Url { get; set; }

		/// <summary>
		/// The updateUrl
		/// </summary>
		[DataMember(Name = "updateUrl")]
		public string UpdateUrl { get; set; }

		/// <summary>
		/// The clearUrl
		/// </summary>
		[DataMember(Name = "clearUrl")]
		public string ClearUrl { get; set; }

		/// <summary>
		/// The ackUrl
		/// </summary>
		[DataMember(Name = "ackUrl")]
		public string AckUrl { get; set; }

		/// <summary>
		/// The username
		/// </summary>
		[DataMember(Name = "username")]
		public string Username { get; set; }

		/// <summary>
		/// The updateUsername
		/// </summary>
		[DataMember(Name = "updateUsername")]
		public string UpdateUsername { get; set; }

		/// <summary>
		/// The clearUsername
		/// </summary>
		[DataMember(Name = "clearUsername")]
		public string ClearUsername { get; set; }

		/// <summary>
		/// The ackUsername
		/// </summary>
		[DataMember(Name = "ackUsername")]
		public string AckUsername { get; set; }

		/// <summary>
		/// The password
		/// </summary>
		[DataMember(Name = "password")]
		public string Password { get; set; }

		/// <summary>
		/// The updatePassword
		/// </summary>
		[DataMember(Name = "updatePassword")]
		public string UpdatePassword { get; set; }

		/// <summary>
		/// The clearPassword
		/// </summary>
		[DataMember(Name = "clearPassword")]
		public string ClearPassword { get; set; }

		/// <summary>
		/// The ackPassword
		/// </summary>
		[DataMember(Name = "ackPassword")]
		public string AckPassword { get; set; }

		/// <summary>
		/// The payload
		/// </summary>
		[DataMember(Name = "payload")]
		public string Payload { get; set; }

		/// <summary>
		/// The clearPayload
		/// </summary>
		[DataMember(Name = "clearPayload")]
		public string ClearPayload { get; set; }

		/// <summary>
		/// The updatePayload
		/// </summary>
		[DataMember(Name = "updatePayload")]
		public string UpdatePayload { get; set; }

		/// <summary>
		/// The ackPayload
		/// </summary>
		[DataMember(Name = "ackPayload")]
		public string AckPayload { get; set; }

		/// <summary>
		/// The payloadFormat
		/// </summary>
		[DataMember(Name = "payloadFormat")]
		public string PayloadFormat { get; set; }

		/// <summary>
		/// The updatePayloadFormat
		/// </summary>
		[DataMember(Name = "updatePayloadFormat")]
		public string UpdatePayloadFormat { get; set; }

		/// <summary>
		/// The clearPayloadFormat
		/// </summary>
		[DataMember(Name = "clearPayloadFormat")]
		public string ClearPayloadFormat { get; set; }

		/// <summary>
		/// The ackPayloadFormat
		/// </summary>
		[DataMember(Name = "ackPayloadFormat")]
		public string AckPayloadFormat { get; set; }

		/// <summary>
		/// The headers
		/// </summary>
		[DataMember(Name = "headers")]
		public object Headers { get; set; }

		/// <summary>
		/// The updateHeaders
		/// </summary>
		[DataMember(Name = "updateHeaders")]
		public object UpdateHeaders { get; set; }

		/// <summary>
		/// The clearHeaders
		/// </summary>
		[DataMember(Name = "clearHeaders")]
		public object ClearHeaders { get; set; }

		/// <summary>
		/// The ackHeaders
		/// </summary>
		[DataMember(Name = "ackHeaders")]
		public object AckHeaders { get; set; }

		/// <summary>
		/// The parseMethod
		/// </summary>
		[DataMember(Name = "parseMethod")]
		public string ParseMethod { get; set; }

		/// <summary>
		/// The parseExpression
		/// </summary>
		[DataMember(Name = "parseExpression")]
		public string ParseExpression { get; set; }

		/// <summary>
		/// The enabledStatus
		/// </summary>
		[DataMember(Name = "enabledStatus")]
		public string[] EnabledStatus { get; set; }

		/// <summary>
		/// The zone
		/// </summary>
		[DataMember(Name = "zone")]
		public int Zone { get; set; }

		/// <summary>
		/// The accountId
		/// </summary>
		[DataMember(Name = "accountId")]
		public int AccountId { get; set; }

		/// <summary>
		/// The dueDateTime
		/// </summary>
		[DataMember(Name = "dueDateTime")]
		public string DueDateTime { get; set; }

		/// <summary>
		/// The warnPriority
		/// </summary>
		[DataMember(Name = "warnPriority")]
		public int WarnPriority { get; set; }

		/// <summary>
		/// The errorPriority
		/// </summary>
		[DataMember(Name = "errorPriority")]
		public int ErrorPriority { get; set; }

		/// <summary>
		/// The criticalPriority
		/// </summary>
		[DataMember(Name = "criticalPriority")]
		public int CriticalPriority { get; set; }

		/// <summary>
		/// The queueId
		/// </summary>
		[DataMember(Name = "queueId")]
		public int QueueId { get; set; }

		/// <summary>
		/// The statusNewTicket
		/// </summary>
		[DataMember(Name = "statusNewTicket")]
		public int StatusNewTicket { get; set; }

		/// <summary>
		/// The statusUpdateTicket
		/// </summary>
		[DataMember(Name = "statusUpdateTicket")]
		public int StatusUpdateTicket { get; set; }

		/// <summary>
		/// The statusCloseTicket
		/// </summary>
		[DataMember(Name = "statusCloseTicket")]
		public int StatusCloseTicket { get; set; }

		/// <summary>
		/// The statusAckTicket
		/// </summary>
		[DataMember(Name = "statusAckTicket")]
		public int StatusAckTicket { get; set; }
	}
}