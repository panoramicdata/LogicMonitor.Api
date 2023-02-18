namespace LogicMonitor.Api.Websites;

/// <summary>
/// A Websites
/// </summary>
[DataContract]
public class Website : NamedItem, IPatchable, IHasCustomProperties
{
	/// <summary>
	/// The website template
	/// </summary>
	[DataMember(Name = "template", IsRequired = false)]
	public object? Template { get; set; }

	/// <summary>
	/// The locations from which the website is monitored. If the website is internal, this field should include Collectors. If Non-Internal, possible test locations are:\n1 : US - LA\n2 : US - DC\n3 : US - SF\n4 : Europe - Dublin\n5 : Asia - Singapore\n6 : Australia - Sydney\ntestLocation:\"{all:true}\" indicates that the service will be monitored from all checkpoint locations\ntestLocation:\"{smgIds:[1,2,3]}\" indicates that the service will be monitored from checkpoint locations 1, 2 and 3\ntestLocation:\"{collectorIds:[85,90]}\" indicates that the service will be monitored by Collectors 85 and 90
	/// </summary>
	[DataMember(Name = "testLocation", IsRequired = true)]
	public WebsiteLocation TestLocation { get; set; } = null!;

	/// <summary>
	/// The id of the group the website is in
	/// </summary>
	[DataMember(Name = "groupId", IsRequired =  false)]
	public int GroupId { get; set; }

	/// <summary>
	/// warn | error | critical\nThe level of alert to trigger if the website fails the number of checks specified by transition from the test locations specified by globalSmAlertCond
	/// </summary>
	[DataMember(Name = "overallAlertLevel", IsRequired = false)]
	[JsonConverter(typeof(StringEnumConverter))]
	public Level OverallAlertLevel { get; set; }

	/// <summary>
	/// 1 | 2 | 3 | 4 | 5 | 6 | 7 | 8 | 9 | 10\nThe polling interval for the website, in units of minutes. This value indicates how often the website is checked. The minimum is 1 minute, and the maximum is 10 minutes
	/// </summary>
	[DataMember(Name = "pollingInterval", IsRequired = false)]
	public int PollingIntervalMinutes { get; set; }

	/// <summary>
	/// true: alerting is disabled for the website\nfalse: alerting is enabled for the website\nIf stopMonitoring\u003dtrue, then alerting will also by default be disabled for the website
	/// </summary>
	[DataMember(Name = "disableAlerting", IsRequired = false)]
	public bool DisableAlerting { get; set; }

	/// <summary>
	/// pingcheck | webcheck\nThe type of the service
	/// </summary>
	[DataMember(Name = "type", IsRequired = true)]
	public WebsiteType Type { get; set; }

	/// <summary>
	/// The role privilege operation(s) for this website that are granted to the user who made the API request
	/// </summary>
	[DataMember(Name = "rolePrivileges")]
	public string[]? RolePrivileges { get; set; }

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

x

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
	public List<EntityProperty> CustomProperties { get; set; }

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
