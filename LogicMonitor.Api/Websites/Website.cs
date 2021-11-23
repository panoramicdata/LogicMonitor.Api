namespace LogicMonitor.Api.Websites;

/// <summary>
/// A Websites
/// </summary>
[DataContract]
public class Website : NamedItem, IPatchable, IHasCustomProperties
{
	/// <summary>
	/// Associated collectors
	/// </summary>
	[DataMember(Name = "collectors")]
	public List<WebsiteCollectorInfo> Collectors { get; set; }

	/// <summary>
	/// The count of packets to send
	/// </summary>
	[DataMember(Name = "count")]
	public int Count { get; set; }

	/// <summary>
	/// The domain
	/// </summary>
	[DataMember(Name = "domain")]
	public string Domain { get; set; }

	/// <summary>
	/// Authentication credentials
	/// </summary>
	[DataMember(Name = "host")]
	public string HostName { get; set; }

	/// <summary>
	/// Whether alerting is disabled
	/// </summary>
	[DataMember(Name = "alertDisableStatus")]
	public AlertDisableStatus AlertDisableStatus { get; set; }

	/// <summary>
	/// The alert expression
	/// </summary>
	[DataMember(Name = "alertExpr")]
	public string AlertExpression { get; set; }

	/// <summary>
	/// The alert status
	/// </summary>
	[DataMember(Name = "alertStatus")]
	public AlertStatus AlertStatus { get; set; }

	/// <summary>
	/// The alert status
	/// </summary>
	[DataMember(Name = "alertStatusPriority")]
	public int AlertStatusPriority { get; set; }

	/// <summary>
	/// Checkpoints
	/// </summary>
	[DataMember(Name = "checkpoints")]
	public List<WebsiteCheckpoint> Checkpoints { get; set; }

	/// <summary>
	/// Whether alerting is disabled
	/// </summary>
	[DataMember(Name = "disableAlerting")]
	public bool IsAlertingDisabled { get; set; }

	/// <summary>
	/// globalSmAlertCond
	/// </summary>
	[DataMember(Name = "globalSmAlertCond")]
	public int GlobalSmAlertCond { get; set; }

	/// <summary>
	/// Whether Individual SmAlerts are enabled
	/// </summary>
	[DataMember(Name = "individualSmAlertEnable")]
	public bool IndividualSmAlertEnable { get; set; }

	/// <summary>
	/// The individual alert level
	/// </summary>
	[DataMember(Name = "individualAlertLevel")]
	[JsonConverter(typeof(StringEnumConverter))]
	public Level IndividualAlertLevel { get; set; }

	/// <summary>
	/// Whether to ignore SSL
	/// </summary>
	[DataMember(Name = "ignoreSSL")]
	public bool IgnoreSsl { get; set; }

	/// <summary>
	/// Whether the check is internal
	/// </summary>
	[DataMember(Name = "isInternal")]
	public bool IsInternal { get; set; }

	/// <summary>
	/// The method
	/// </summary>
	[DataMember(Name = "method")]
	public WebsiteMethod WebsiteMethod { get; set; }

	/// <summary>
	/// The overall alert level
	/// </summary>
	[DataMember(Name = "overallAlertLevel")]
	[JsonConverter(typeof(StringEnumConverter))]
	public Level OverallAlertLevel { get; set; }

	/// <summary>
	/// pageLoadAlertTimeInMS
	/// </summary>
	[DataMember(Name = "pageLoadAlertTimeInMS")]
	public int PageLoadAlertTimeInMs { get; set; }

	/// <summary>
	/// The percentage of packets not received in time before an alert condition will exist.
	/// </summary>
	[DataMember(Name = "percentPktsNotReceiveInTime")]
	public int PercentPacketsNotReceiveInTime { get; set; }

	/// <summary>
	/// The timeout in ms before packets will be considered not to have been received.
	/// </summary>
	[DataMember(Name = "timeoutInMSPktsNotReceive")]
	public int PacketsNotReceivedTimeoutMs { get; set; }

	/// <summary>
	/// The polling interval in minutes
	/// </summary>
	[DataMember(Name = "pollingInterval")]
	public int PollingIntervalMinutes { get; set; }

	/// <summary>
	/// The HTTP schema
	/// </summary>
	[DataMember(Name = "schema")]
	public HttpSchema Schema { get; set; }

	/// <summary>
	/// The script
	/// </summary>
	[DataMember(Name = "script")]
	public string Script { get; set; }

	/// <summary>
	/// The SDT status
	/// </summary>
	[DataMember(Name = "sdtStatus")]
	public SdtStatus SdtStatus { get; set; }

	/// <summary>
	/// The website group id
	/// </summary>
	[DataMember(Name = "groupId")]
	public int WebsiteGroupId { get; set; }

	/// <summary>
	/// The steps
	/// </summary>
	[DataMember(Name = "steps")]
	public List<WebsiteStep> Steps { get; set; }

	/// <summary>
	/// Whether to stop Monitoring
	/// </summary>
	[DataMember(Name = "stopMonitoring")]
	public bool StopMonitoring { get; set; }

	/// <summary>
	/// Whether to stop Monitoring by group
	/// </summary>
	[DataMember(Name = "stopMonitoringByFolder")]
	public bool StopMonitoringByWebsiteGroup { get; set; }

	/// <summary>
	/// Template
	/// </summary>
	[DataMember(Name = "template")]
	public object Template { get; set; }

	/// <summary>
	/// Test Location
	/// </summary>
	[DataMember(Name = "testLocation")]
	public TestLocation TestLocation { get; set; }

	/// <summary>
	/// Whether to trigger SSL Status Alerts
	/// </summary>
	[DataMember(Name = "triggerSSLStatusAlert")]
	public bool TriggerSslStatusAlerts { get; set; }

	/// <summary>
	/// Whether to trigger SSL Expiration Alerts
	/// </summary>
	[DataMember(Name = "triggerSSLExpirationAlert")]
	public bool TriggerSslExpirationAlerts { get; set; }

	/// <summary>
	/// Transition
	/// </summary>
	[DataMember(Name = "transition")]
	public int Transition { get; set; }

	/// <summary>
	/// The type
	/// </summary>
	[DataMember(Name = "type")]
	public WebsiteType Type { get; set; }

	/// <summary>
	/// Whether to use the default alert setting
	/// </summary>
	[DataMember(Name = "useDefaultAlertSetting")]
	public bool UseDefaultAlertSetting { get; set; }

	/// <summary>
	/// Whether to use the default location setting
	/// </summary>
	[DataMember(Name = "useDefaultLocationSetting")]
	public bool UseDefaultLocationSetting { get; set; }

	/// <summary>
	/// The user permission
	/// </summary>
	[DataMember(Name = "userPermission")]
	public UserPermission UserPermissionString { get; set; }

	/// <summary>
	/// The properties
	/// </summary>
	[DataMember(Name = "properties")]
	public List<Property> CustomProperties { get; set; }

	/// <summary>
	/// The status
	/// </summary>
	[DataMember(Name = "status")]
	public WebsiteStatus Status { get; set; }

	/// <summary>
	///    The time it was updated
	/// </summary>
	[DataMember(Name = "lastUpdated")]
	public long UpdatedOnSeconds { get; set; }

	/// <summary>
	///    createOn
	/// </summary>
	[IgnoreDataMember]
	public DateTime UpdatedOnDateTimeUtc => UpdatedOnSeconds.ToDateTimeUtc();

	/// <summary>
	///    The endpoint
	/// </summary>
	public string Endpoint() => "website/websites";
}
