using LogicMonitor.Api.Devices;
using LogicMonitor.Api.Extensions;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace LogicMonitor.Api.Alerts
{
	/// <summary>
	///    A LogicMonitor alert
	/// </summary>
	[DataContract]
	public class Alert : IHasEndpoint
	{
		/// <summary>
		/// The Id
		/// </summary>
		[DataMember(Name = "id")]
		public string Id { get; set; }

		/// <summary>
		///    The Alert type
		/// </summary>
		[DataMember(Name = "type")]
		public AlertType AlertType { get; set; }

		/// <summary>
		/// Whether enableAnomalyAlertSuppression is enabled
		/// </summary>
		[DataMember(Name = "enableAnomalyAlertSuppression")]
		public bool? EnableAnomalyAlertSuppression { get; set; }

		/// <summary>
		///    The internal Id
		/// </summary>
		[DataMember(Name = "internalId")]
		public string InternalId { get; set; }

		/// <summary>
		///    The number of seconds since the Epoch when the alert state started
		/// </summary>
		[DataMember(Name = "startEpoch")]
		public int StartOnSeconds { get; set; }

		/// <summary>
		///    The number of seconds since the Epoch when the alert state started
		/// </summary>
		[DataMember(Name = "endEpoch")]
		public int EndOnSeconds { get; set; }

		/// <summary>
		///    The DateTime that the Alert state was acknowledged as text
		/// </summary>
		[DataMember(Name = "acked")]
		public bool Acked { get; set; }

		/// <summary>
		///    The number of seconds since the Epoch when the alert was acknowledged
		/// </summary>
		[DataMember(Name = "ackedEpoch")]
		public int AckedOnSeconds { get; set; }

		/// <summary>
		///    The user that acknowledged the Alert (if any)
		/// </summary>
		[DataMember(Name = "ackedBy")]
		public string AckedBy { get; set; }

		/// <summary>
		///    The Acknowledgement comment
		/// </summary>
		[DataMember(Name = "ackComment")]
		public string AckComment { get; set; }

		/// <summary>
		///    The alert rule name
		/// </summary>
		[DataMember(Name = "rule")]
		public string AlertRuleName { get; set; }

		/// <summary>
		///    The alert rule Id
		/// </summary>
		[DataMember(Name = "ruleId")]
		public int AlertRuleId { get; set; }

		/// <summary>
		///    The EscalationChain name
		/// </summary>
		[DataMember(Name = "chain")]
		public string AlertEscalationChainName { get; set; }

		/// <summary>
		///    The Id of the escalation chain
		/// </summary>
		[DataMember(Name = "chainId")]
		public int AlertEscalationChainId { get; set; }

		/// <summary>
		///    The EscalationSubChainId
		/// </summary>
		[DataMember(Name = "subChainId")]
		public int AlertEscalationSubChainId { get; set; }

		/// <summary>
		///    The next recipient
		/// </summary>
		[DataMember(Name = "nextRecipient")]
		public int NextRecipient { get; set; }

		/// <summary>
		///    The alert recipients
		/// </summary>
		[DataMember(Name = "receivedList")]
		public string AlertRecipients { get; set; }

		/// <summary>
		///    The alert severity
		/// </summary>
		[DataMember(Name = "severity")]
		public int Severity { get; set; }

		/// <summary>
		///    The alert severity
		/// </summary>
		[DataMember(Name = "cleared")]
		public bool IsCleared { get; set; }

		/// <summary>
		///    Whether the alert occurred during scheduled down time
		/// </summary>
		[DataMember(Name = "sdted")]
		public bool InScheduledDownTime { get; set; }

		/// <summary>
		///    The specific SDT
		/// </summary>
		[DataMember(Name = "SDT")]
		public AlertSdt Sdt { get; set; }

		/// <summary>
		///    The value that triggered the Alert
		/// </summary>
		[DataMember(Name = "alertValue")]
		public string Value { get; set; }

		/// <summary>
		///    The threshold levels
		/// </summary>
		[DataMember(Name = "threshold")]
		public string Thresholds { get; set; }

		/// <summary>
		///    The value at which the alert cleared
		/// </summary>
		[DataMember(Name = "clearValue")]
		public string ClearValue { get; set; }

		/// <summary>
		///    The monitor object's name
		/// </summary>
		[DataMember(Name = "monitorObjectType")]
		public MonitoredObjectType MonitorObjectType { get; set; }

		/// <summary>
		///    The monitor object's Id
		/// </summary>
		[DataMember(Name = "monitorObjectId")]
		public string MonitorObjectId { get; set; }

		/// <summary>
		///    The monitor object's name
		/// </summary>
		[DataMember(Name = "monitorObjectName")]
		public string MonitorObjectName { get; set; }

		/// <summary>
		///    The monitor object's groups
		/// </summary>
		[DataMember(Name = "monitorObjectGroups")]
		public List<DeviceGroup> MonitorObjectGroups { get; set; }

		/// <summary>
		///    The resource id
		/// </summary>
		[DataMember(Name = "resourceId")]
		public int? ResourceId { get; set; }

		/// <summary>
		///    The resource template id
		/// </summary>
		[DataMember(Name = "resourceTemplateId")]
		public int? ResourceTemplateId { get; set; }

		/// <summary>
		///    The resource template type
		/// </summary>
		[DataMember(Name = "resourceTemplateType")]
		public string ResourceTemplateType { get; set; }

		/// <summary>
		///    The resource template name
		/// </summary>
		[DataMember(Name = "resourceTemplateName")]
		public string ResourceTemplateName { get; set; }

		/// <summary>
		///    The instance id
		/// </summary>
		[DataMember(Name = "instanceId")]
		public int InstanceId { get; set; }

		/// <summary>
		///    The instance name
		/// </summary>
		[DataMember(Name = "instanceName")]
		public string InstanceName { get; set; }

		/// <summary>
		///    The instance name
		/// </summary>
		[DataMember(Name = "instanceDescription")]
		public string InstanceDescription { get; set; }

		/// <summary>
		///    The dataPoint name
		/// </summary>
		[DataMember(Name = "dataPointName")]
		public string DataPointName { get; set; }

		/// <summary>
		///    The dataPoint id
		/// </summary>
		[DataMember(Name = "dataPointId")]
		public int DataPointId { get; set; }

		/// <summary>
		///    The alert detail message
		/// </summary>
		[DataMember(Name = "detailMessage")]
		public AlertDetailMessage DetailMessage { get; set; }

		/// <summary>
		///    The custom columns
		/// </summary>
		[DataMember(Name = "customColumns")]
		public object CustomColumns { get; set; }

		///////////////////

		/// <summary>
		///    The Alert level
		/// </summary>
		[IgnoreDataMember]
		public AlertLevel AlertLevel => (AlertLevel)Severity;

		/// <summary>
		///    The DateTime that the Alert state started
		/// </summary>
		[IgnoreDataMember]
		public DateTime StartOnUtc => StartOnSeconds.ToDateTimeUtc();

		/// <summary>
		///    The DateTime that the Alert state was acknowledged
		/// </summary>
		[IgnoreDataMember]
		public DateTime? AckedOnUtc => AckedOnSeconds.ToNullableDateTimeUtc();

		/// <summary>
		///    The DateTime that the Alert state finished
		/// </summary>
		[IgnoreDataMember]
		public DateTime? EndOnUtc => EndOnSeconds.ToNullableDateTimeUtc();

		/// <summary>
		///    Whether the alert is active (not cleared)
		/// </summary>
		[IgnoreDataMember]
		public bool IsActive => !IsCleared;

		/// <summary>
		/// The alert endpoint
		/// </summary>
		/// <returns></returns>
		public string Endpoint() => "alert/alerts";

		/// <summary>
		///    ToString
		/// </summary>
		/// <returns></returns>
		public override string ToString() => $"{Id} : {MonitorObjectName}/{AlertType} : {AlertLevel}/{DataPointName ?? "no datapoint"}";
	}
}