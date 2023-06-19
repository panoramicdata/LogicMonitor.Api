namespace LogicMonitor.Api.Websites;

/// <summary>
///    A Website creation DTO
/// </summary>
[DataContract]
public class WebsiteCreationDto : CreationDto<Website>
{
	/// <summary>
	///    The website group id
	/// </summary>
	[DataMember(Name = "groupId")]
	public string WebsiteGroupId { get; set; } = string.Empty;

	/// <summary>
	///    Whether monitoring is disabled
	/// </summary>
	[DataMember(Name = "disableAlerting")]
	public string IsAlertingDisabled { get; set; } = string.Empty;

	/// <summary>
	///    The polling interval in minutes
	/// </summary>
	[DataMember(Name = "pollingInterval")]
	public string PollingIntervalMinutes { get; set; } = string.Empty;

	/// <summary>
	///    Whether to use the default location setting
	/// </summary>
	[DataMember(Name = "useDefaultLocationSetting")]
	public bool UseDefaultLocationSetting { get; set; }

	/// <summary>
	///    Whether to use the default location setting
	/// </summary>
	[DataMember(Name = "useDefaultAlertSetting")]
	public bool UseDefaultAlertSetting { get; set; }

	/// <summary>
	///    Whether the testing is internal
	/// </summary>
	[DataMember(Name = "isInternal")]
	public bool IsInternal { get; set; }

	/// <summary>
	///    Whether to disable alerting
	/// </summary>
	[DataMember(Name = "type")]
	public WebsiteType Type { get; set; }

	/// <summary>
	///    The name
	/// </summary>
	[DataMember(Name = "name")]
	public string Name { get; set; } = string.Empty;

	/// <summary>
	///    The description
	/// </summary>
	[DataMember(Name = "description")]
	public string Description { get; set; } = string.Empty;

	/// <summary>
	///    The hostname / IP address
	/// </summary>
	[DataMember(Name = "host")]
	public string HostName { get; set; } = string.Empty;

	/// <summary>
	/// The HTTP schema
	/// </summary>
	[DataMember(Name = "schema")]
	public HttpSchema HttpSchema { get; set; }

	/// <summary>
	/// The domain
	/// </summary>
	[DataMember(Name = "domain")]
	public string Domain { get; set; } = string.Empty;

	/// <summary>
	/// The steps
	/// </summary>
	[DataMember(Name = "steps")]
	public List<WebCheckStep> Steps { get; set; } = new();

	/// <summary>
	///    The attempt count as a string
	/// </summary>
	[DataMember(Name = "count")]
	public string Count { get; set; } = string.Empty;

	/// <summary>
	///    The percentPktsNotReceiveInTime
	/// </summary>
	[DataMember(Name = "percentPktsNotReceiveInTime")]
	public string PercentPktsNotReceiveInTime { get; set; } = string.Empty;

	/// <summary>
	///    The timeoutInMSPktsNotReceive
	/// </summary>
	[DataMember(Name = "timeoutInMSPktsNotReceive")]
	public string TimeoutInMsPktsNotReceive { get; set; } = string.Empty;

	/// <summary>
	///    The transition
	/// </summary>
	[DataMember(Name = "transition")]
	public string Transition { get; set; } = string.Empty;

	/// <summary>
	///    The globalSmAlertCond
	/// </summary>
	[DataMember(Name = "globalSmAlertCond")]
	public string GlobalSmAlertCond { get; set; } = string.Empty;

	/// <summary>
	///    The overallAlertLevel
	/// </summary>
	[DataMember(Name = "overallAlertLevel")]
	public Level OverallAlertLevel { get; set; } = Level.Warning;

	/// <summary>
	///    The individualAlertLevel
	/// </summary>
	[DataMember(Name = "individualAlertLevel")]
	public Level IndividualAlertLevel { get; set; } = Level.Warning;

	/// <summary>
	///    The individualSmAlertEnable
	/// </summary>
	[DataMember(Name = "individualSmAlertEnable")]
	public bool IndividualSmAlertEnable { get; set; }

	/// <summary>
	///    The test location.
	/// </summary>
	[DataMember(Name = "testLocation")]
	public WebsiteLocation TestLocation { get; set; } = new();

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
	/// The website properties
	/// </summary>
	[DataMember(Name = "properties")]
	public List<EntityProperty> WebsiteProperties { get; set; } = new();

	/// <summary>
	/// The alert expression for SSL expiration alerts
	/// </summary>
	[DataMember(Name = "alertExpr")]
	public string AlertExpression { get; set; } = string.Empty;

	/// <summary>
	/// Whether to ignore SSL errors
	/// </summary>
	[DataMember(Name = "ignoreSSL")]
	public bool IgnoreSsl { get; set; }

	/// <summary>
	/// pageLoadAlertTimeInMS
	/// </summary>
	[DataMember(Name = "pageLoadAlertTimeInMS")]
	public int? PageLoadAlertTimeInMs { get; set; }
}
