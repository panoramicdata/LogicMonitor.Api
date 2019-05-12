using System.Runtime.Serialization;

namespace LogicMonitor.Api.Settings
{
	/// <summary>
	///     An HTTP integration
	/// </summary>
	[DataContract]
	public class ServiceNowIntegration : HttpIntegration
	{
		/// <summary>
		/// The ServiceNow subdomain
		/// </summary>
		[DataMember(Name = "subDomain")]
		public string SubDomain { get; set; }

		/// <summary>
		/// The ServiceNow username
		/// </summary>
		[DataMember(Name = "servicenowUsername")]
		public string ServiceNowUsername { get; set; }

		/// <summary>
		/// The ServiceNow password
		/// </summary>
		[DataMember(Name = "servicenowPassword")]
		public string ServiceNowPassword { get; set; }

		/// <summary>
		/// The ServiceNow company
		/// </summary>
		[DataMember(Name = "servicenowCompany")]
		public string ServiceNowCompany { get; set; }

		/// <summary>
		/// The ServiceNow warn severity
		/// </summary>
		[DataMember(Name = "servicenowWarnSeverity")]
		public string ServiceNowWarnSeverity { get; set; }

		/// <summary>
		/// The ServiceNow error severity
		/// </summary>
		[DataMember(Name = "servicenowErrorSeverity")]
		public string ServiceNowErrorSeverity { get; set; }

		/// <summary>
		/// The ServiceNow critical severity
		/// </summary>
		[DataMember(Name = "servicenowCriticalSeverity")]
		public string ServiceNowCriticalSeverity { get; set; }

		/// <summary>
		/// The ServiceNow create incident state
		/// </summary>
		[DataMember(Name = "servicenowCreateIncidentState")]
		public string ServiceNowCreateIncidentState { get; set; }

		/// <summary>
		/// The ServiceNow update incident state
		/// </summary>
		[DataMember(Name = "servicenowUpdateIncidentState")]
		public string ServiceNowUpdateIncidentState { get; set; }

		/// <summary>
		/// The ServiceNow clear incident state
		/// </summary>
		[DataMember(Name = "servicenowClearIncidentState")]
		public string ServiceNowClearIncidentState { get; set; }

		/// <summary>
		/// The ServiceNow acknowledgement incident state
		/// </summary>
		[DataMember(Name = "servicenowAckIncidentState")]
		public string ServiceNowAckIncidentState { get; set; }

		/// <summary>
		/// The ServiceNow due Date/time
		/// </summary>
		[DataMember(Name = "servicenowDueDateTime")]
		public string ServiceNowDueDateTime { get; set; }
	}
}