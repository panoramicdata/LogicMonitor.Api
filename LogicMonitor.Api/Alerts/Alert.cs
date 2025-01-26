namespace LogicMonitor.Api.Alerts;

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
	public string Id { get; set; } = string.Empty;

	/// <summary>
	/// The dependency role
	/// </summary>
	[DataMember(Name = "dependencyRole")]
	public string DependencyRole { get; set; } = string.Empty;

	/// <summary>
	/// The dependency routing state
	/// </summary>
	[DataMember(Name = "dependencyRoutingState")]
	public string DependencyRoutingState { get; set; } = string.Empty;

	/// <summary>
	/// Whether or not the alert is dynamic threshold based
	/// </summary>
	[DataMember(Name = "adAlert")]
	public bool IsActiveDiscoveryAlert { get; set; }

	/// <summary>
	/// The description for dynamic threshold based alert
	/// </summary>
	[DataMember(Name = "adAlertDesc")]
	public string ActiveDiscoveryAlertDescription { get; set; } = string.Empty;

	/// <summary>
	/// The alert group entity value
	/// </summary>
	[DataMember(Name = "alertGroupEntityValue")]
	public string AlertGroupEntityValue { get; set; } = string.Empty;

	/// <summary>
	/// The alert query
	/// </summary>
	[DataMember(Name = "alertQuery")]
	public object? AlertQuery { get; set; }

	/// <summary>
	///    The Alert type
	/// </summary>
	[DataMember(Name = "type")]
	public AlertType AlertType { get; set; }

	/// <summary>
	/// Indicates the anomaly alert, value can be true/false/null. If alert value lies within confidence band then false, otherwise true. If confidence band is not available then value will be null.
	/// </summary>
	[DataMember(Name = "anomaly")]
	public bool IsAnomaly { get; set; }

	/// <summary>
	/// Indicates dynamic threshold alert generation setting. expression is comma separated\n0 denotes OFF, 1 denotes ON, -1 denotes INVALID\n1,0,1 \u003d   warn : ON     error: OFF   critical: ON\nEmpty value on this parameter means : 0,0,0
	/// </summary>
	[DataMember(Name = "enableAnomalyAlertGeneration")]
	public string EnableAnomalyAlertGeneration { get; set; } = string.Empty;    // STRING not a bool

	/// <summary>
	/// Indicates anomaly detection alert suppression setting, expression is comma separated\n0 denotes OFF, 1 denotes ON, -1 denotes INVALID\n1,0,1 \u003d   warn : ON     error: OFF   critical: ON\nEmpty value on this parameter means : 0,0,0
	/// </summary>
	[DataMember(Name = "enableAnomalyAlertSuppression")]
	public string EnableAnomalyAlertSuppression { get; set; } = string.Empty;   // This is a string NOT a bool e.g. "enableAnomalyAlertSuppression": "0,0,0"

	/// <summary>
	///    The internal Id
	/// </summary>
	[DataMember(Name = "internalId")]
	public string InternalId { get; set; } = string.Empty;

	/// <summary>
	///    The log partition
	/// </summary>
	[DataMember(Name = "logPartition")]
	public string LogPartition { get; set; } = string.Empty;

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
	///    The number of seconds since the Epoch when the alert state started
	/// </summary>
	[DataMember(Name = "alertExternalTicketUrl")]
	public AlertExternalTicketUrl ExternalTicketUrl { get; set; } = null!;

	/// <summary>
	///    The log metadata
	/// </summary>
	[DataMember(Name = "logMetaData")]
	public object? LogMetadata { get; set; }

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
	public string AckedBy { get; set; } = string.Empty;

	/// <summary>
	///    The Acknowledgement comment
	/// </summary>
	[DataMember(Name = "ackComment")]
	public string AckComment { get; set; } = string.Empty;

	/// <summary>
	///    The alert rule name
	/// </summary>
	[DataMember(Name = "rule")]
	public string AlertRuleName { get; set; } = string.Empty;

	/// <summary>
	///    The alert rule Id
	/// </summary>
	[DataMember(Name = "ruleId")]
	public int AlertRuleId { get; set; }

	/// <summary>
	/// tenant to which this alert belongs to.
	/// </summary>
	[DataMember(Name = "tenant")]
	public string Tenant { get; set; } = string.Empty;

	/// <summary>
	///    The EscalationChain name
	/// </summary>
	[DataMember(Name = "chain")]
	public string AlertEscalationChainName { get; set; } = string.Empty;

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
	/// The recipients that have received the alert
	/// </summary>
	[DataMember(Name = "receivedList")]
	public string AlertRecipients { get; set; } = string.Empty;

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
	/// It specifies if the SDT is set for an active alert or not. However, the sdted is set to false for cleared alert as you cannot apply SDT to a cleared alert.
	/// </summary>
	[DataMember(Name = "sdted")]
	public bool InScheduledDownTime { get; set; }

	/// <summary>
	///    The specific SDT
	/// </summary>
	[DataMember(Name = "SDT")]
	public AlertSdt Sdt { get; set; } = new CollectorAlertSdt();

	/// <summary>
	///    The value that triggered the Alert
	/// </summary>
	[DataMember(Name = "alertValue")]
	public string Value { get; set; } = string.Empty;

	/// <summary>
	///    The threshold levels
	/// </summary>
	[DataMember(Name = "threshold")]
	public string Thresholds { get; set; } = string.Empty;

	/// <summary>
	///    The value at which the alert cleared
	/// </summary>
	[DataMember(Name = "clearValue")]
	public string ClearValue { get; set; } = string.Empty;

	/// <summary>
	///    The monitor object's name
	/// </summary>
	[DataMember(Name = "monitorObjectType")]
	public MonitoredObjectType MonitorObjectType { get; set; }

	/// <summary>
	///    The monitor object's Id
	/// </summary>
	[DataMember(Name = "monitorObjectId")]
	public string MonitorObjectId { get; set; } = string.Empty;

	/// <summary>
	///    The monitor object's name
	/// </summary>
	[DataMember(Name = "monitorObjectName")]
	public string MonitorObjectName { get; set; } = string.Empty;

	/// <summary>
	///    The monitor object's groups
	/// </summary>
	[DataMember(Name = "monitorObjectGroups")]
	public List<ResourceGroup> MonitorObjectGroups { get; set; } = [];

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
	public string ResourceTemplateType { get; set; } = string.Empty;

	/// <summary>
	/// The name of the datasource in alert
	/// </summary>
	[DataMember(Name = "resourceTemplateName")]
	public string ResourceTemplateName { get; set; } = string.Empty;

	/// <summary>
	///    The instance id
	/// </summary>
	[DataMember(Name = "instanceId")]
	public int InstanceId { get; set; }

	/// <summary>
	///    The instance name
	/// </summary>
	[DataMember(Name = "instanceName")]
	public string InstanceName { get; set; } = string.Empty;

	/// <summary>
	///    The instance name
	/// </summary>
	[DataMember(Name = "instanceDescription")]
	public string InstanceDescription { get; set; } = string.Empty;

	/// <summary>
	///    The dataPoint name
	/// </summary>
	[DataMember(Name = "dataPointName")]
	public string DataPointName { get; set; } = string.Empty;

	/// <summary>
	///    The dataPoint id
	/// </summary>
	[DataMember(Name = "dataPointId")]
	public int DataPointId { get; set; }

	/// <summary>
	///    The alert detail message
	/// </summary>
	[DataMember(Name = "detailMessage")]
	public AlertDetailMessage DetailMessage { get; set; } = new();

	/// <summary>
	///    The custom columns
	/// </summary>
	[DataMember(Name = "customColumns")]
	public object CustomColumns { get; set; } = new();

	/// <summary>
	/// The component (Ex SDT, HostClusterAlert) which suppressed the alert
	/// </summary>
	[DataMember(Name = "suppressor")]
	public string Suppressor { get; set; } = string.Empty;

	/// <summary>
	/// The description for suppressed alert
	/// </summary>
	[DataMember(Name = "suppressDesc")]
	public string SuppressedDescending { get; set; } = string.Empty;

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
	public string Endpoint() => "alert/alerts";

	/// <summary>
	///    ToString
	/// </summary>
	public override string ToString() => $"{Id} : {MonitorObjectName}/{AlertType} : {AlertLevel}/{DataPointName ?? "no datapoint"}";
}
