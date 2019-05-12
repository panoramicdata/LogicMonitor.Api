using System.Collections.Generic;
using System.Runtime.Serialization;

namespace LogicMonitor.Api.Settings
{
	/// <summary>
	///     An Slack integration
	/// </summary>
	[DataContract]
	public class SlackIntegration : Integration
	{
		/// <summary>
		///     The method
		/// </summary>
		[DataMember(Name = "method")]
		public string Method { get; set; }

		/// <summary>
		///     The updateMethod
		/// </summary>
		[DataMember(Name = "updateMethod")]
		public string UpdateMethod { get; set; }

		/// <summary>
		///     The update UserName
		/// </summary>
		[DataMember(Name = "updateUsername")]
		public string UpdateUserName { get; set; }

		/// <summary>
		///     The clearMethod
		/// </summary>
		[DataMember(Name = "clearMethod")]
		public string ClearMethod { get; set; }

		/// <summary>
		///     The ackMethod
		/// </summary>
		[DataMember(Name = "ackMethod")]
		public string AckMethod { get; set; }

		/// <summary>
		///     The url
		/// </summary>
		[DataMember(Name = "url")]
		public string Url { get; set; }

		/// <summary>
		///     The updateUrl
		/// </summary>
		[DataMember(Name = "updateUrl")]
		public string UpdateUrl { get; set; }

		/// <summary>
		///     The clearUrl
		/// </summary>
		[DataMember(Name = "clearUrl")]
		public string ClearUrl { get; set; }

		/// <summary>
		///     The ackUrl
		/// </summary>
		[DataMember(Name = "ackUrl")]
		public string AckUrl { get; set; }

		/// <summary>
		///     The username
		/// </summary>
		[DataMember(Name = "username")]
		public string Username { get; set; }

		/// <summary>
		///     The updateUername
		/// </summary>
		[DataMember(Name = "updateUername")]
		public string UpdateUername { get; set; }

		/// <summary>
		///     The clearUsername
		/// </summary>
		[DataMember(Name = "clearUsername")]
		public string ClearUsername { get; set; }

		/// <summary>
		///     The ackUsername
		/// </summary>
		[DataMember(Name = "ackUsername")]
		public string AckUsername { get; set; }

		/// <summary>
		///     The password
		/// </summary>
		[DataMember(Name = "password")]
		public string Password { get; set; }

		/// <summary>
		///     The updatePassword
		/// </summary>
		[DataMember(Name = "updatePassword")]
		public string UpdatePassword { get; set; }

		/// <summary>
		///     The clearPassword
		/// </summary>
		[DataMember(Name = "clearPassword")]
		public string ClearPassword { get; set; }

		/// <summary>
		///     The ackPassword
		/// </summary>
		[DataMember(Name = "ackPassword")]
		public string AckPassword { get; set; }

		/// <summary>
		///     The payload
		/// </summary>
		[DataMember(Name = "payload")]
		public string Payload { get; set; }

		/// <summary>
		///     The clearPayload
		/// </summary>
		[DataMember(Name = "clearPayload")]
		public string ClearPayload { get; set; }

		/// <summary>
		///     The updatePayload
		/// </summary>
		[DataMember(Name = "updatePayload")]
		public string UpdatePayload { get; set; }

		/// <summary>
		///     The ackPayload
		/// </summary>
		[DataMember(Name = "ackPayload")]
		public string AckPayload { get; set; }

		/// <summary>
		///     The payloadFormat
		/// </summary>
		[DataMember(Name = "payloadFormat")]
		public string PayloadFormat { get; set; }

		/// <summary>
		///     The updatePayloadFormat
		/// </summary>
		[DataMember(Name = "updatePayloadFormat")]
		public string UpdatePayloadFormat { get; set; }

		/// <summary>
		///     The clearPayloadFormat
		/// </summary>
		[DataMember(Name = "clearPayloadFormat")]
		public string ClearPayloadFormat { get; set; }

		/// <summary>
		///     The ackPayloadFormat
		/// </summary>
		[DataMember(Name = "ackPayloadFormat")]
		public string AckPayloadFormat { get; set; }

		/// <summary>
		///     The headers
		/// </summary>
		[DataMember(Name = "headers")]
		public List<object> Headers { get; set; }

		/// <summary>
		///     The updateHeaders
		/// </summary>
		[DataMember(Name = "updateHeaders")]
		public List<object> UpdateHeaders { get; set; }

		/// <summary>
		///     The clearHeaders
		/// </summary>
		[DataMember(Name = "clearHeaders")]
		public List<object> ClearHeaders { get; set; }

		/// <summary>
		///     The ackHeaders
		/// </summary>
		[DataMember(Name = "ackHeaders")]
		public List<object> AckHeaders { get; set; }

		/// <summary>
		///     The parseMethod
		/// </summary>
		[DataMember(Name = "parseMethod")]
		public string ParseMethod { get; set; }

		/// <summary>
		///     The parseExpression
		/// </summary>
		[DataMember(Name = "parseExpression")]
		public List<object> ParseExpression { get; set; }

		/// <summary>
		///     The enabledStatus
		/// </summary>
		[DataMember(Name = "enabledStatus")]
		public List<string> EnabledStatus { get; set; }

		/// <summary>
		///     The incomingWebhookUrl
		/// </summary>
		[DataMember(Name = "incomingWebhookUrl")]
		public string IncomingWebhookUrl { get; set; }
	}
}