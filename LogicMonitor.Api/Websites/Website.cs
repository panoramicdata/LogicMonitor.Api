namespace LogicMonitor.Api.Websites;

/// <summary>
/// A Websites
/// </summary>
[DataContract]
public class Website : NamedItem, IPatchable
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
	[DataMember(Name = "rolePrivileges", IsRequired = false)]
	public string[]? RolePrivileges { get; set; }

	/// <summary>
	/// The time (in epoch format) that the website was updated
	/// </summary>
	[DataMember(Name = "lastUpdated", IsRequired = false)]
	public long UpdatedOnSeconds { get; set; }

	/// <summary>
	/// true: monitoring is disabled for all services in the website\u0027s folder\nfalse: monitoring is not disabled for all services in website\u0027s folder
	/// </summary>
	[DataMember(Name = "stopMonitoringByFolder", IsRequired = false)]
	public bool StopMonitoringByFolder { get; set; }

	/// <summary>
	/// true: monitoring is disabled for the website\nfalse: monitoring is enabled for the website\nIf stopMonitoring\u003dtrue, then alerting will also by default be disabled for the website
	/// </summary>
	[DataMember(Name = "stopMonitoring", IsRequired = false)]
	public bool StopMonitoring { get; set; }

	/// <summary>
	/// write | read | ack. The permission level of the user that made the API request
	/// </summary>
	[DataMember(Name = "userPermission", IsRequired = false)]
	[JsonConverter(typeof(StringEnumConverter))]
	public UserPermission UserPermissionString { get; set; }

	/// <summary>
	/// true: an alert will be triggered if a check fails from an individual test location\nfalse: an alert will not be triggered if a check fails from an individual test location
	/// </summary>
	[DataMember(Name = "individualSmAlertEnable", IsRequired = false)]
	public bool IndividualSmAlertEnable { get; set; }

	/// <summary>
	/// The checkpoints from the which the website is monitored. This object should reference each location specified in testLocation in addition to an \u0027Overall\u0027 checkpoint
	/// </summary>
	[DataMember(Name = "checkpoints", IsRequired = false)]
	public WebsiteCheckpoint[]? Checkpoints { get; set; }

	/// <summary>
	/// Required for type\u003dwebcheck , An object comprising one or more steps, see the table below for the properties included in each step
	/// </summary>
	[DataMember(Name = "steps", IsRequired = false)]
	public WebsiteStep[]? Steps { get; set; }

	/// <summary>
	/// 1 | 2 | 3 | 4 | 5 | 6 | 7 | 8 | 9 | 10 | 30 | 60\nThe number of checks that must fail before an alert is triggered
	/// </summary>
	[DataMember(Name = "transition", IsRequired = false)]
	public int Transition { get; set; }

	/// <summary>
	/// The number of test locations that checks must fail at to trigger an alert, where the alert triggered will be consistent with the value of overallAlertLevel. Possible values and corresponding number of Site Monitor locations are\n0 : all\n1 : half\n2 : more than one\n3 : any
	/// </summary>
	[DataMember(Name = "globalSmAlertCond", IsRequired = false)]
	public int GlobalSmAlertCond { get; set; }

	/// <summary>
	/// Whether or not the website is internal
	/// </summary>
	[DataMember(Name = "isInternal", IsRequired = false)]
	public bool IsInternal { get; set; }

	/// <summary>
	/// The collectors that are monitoring the website, if the website is internal
	/// </summary>
	[DataMember(Name = "collectors", IsRequired = false)]
	public WebsiteCollectorInfo[]? Collectors { get; set; }

	/// <summary>
	/// Required for type\u003dwebcheck , The domain of the service. This is the base URL of the service
	/// </summary>
	[DataMember(Name = "domain", IsRequired = false)]
	public string? Domain { get; set; }

	/// <summary>
	/// true: The checkpoint locations configured in the website Default Settings will be used\nfalse: The checkpoint locations specified in the testLocation will be used
	/// </summary>
	[DataMember(Name = "useDefaultLocationSetting", IsRequired = false)]
	public bool UseDefaultLocationSetting { get; set; }

	/// <summary>
	/// true: The alert settings configured in the website Default Settings will be used\nfalse: Service Default Settings will not be used, and you will need to specify individualSMAlertEnable, individualAlertLevel, globalSmAlertConf, overallAlertLevel and pollingInterval
	/// </summary>
	[DataMember(Name = "useDefaultAlertSetting", IsRequired = false)]
	public bool UseDefaultAlertSetting { get; set; }

	/// <summary>
	/// warn | error | critical\nThe level of alert to trigger if the website fails a check from an individual test location
	/// </summary>
	[DataMember(Name = "individualAlertLevel", IsRequired = false)]
	[JsonConverter(typeof(StringEnumConverter))]
	public Level IndividualAlertLevel { get; set; }

	/// <summary>
	/// The properties associated with the website
	/// </summary>
	[DataMember(Name = "properties", IsRequired = false)]
	public EntityProperty[]? Properties { get; set; }

	/// <summary>
	/// Whether is the website dead (the collector is down) or not
	/// </summary>
	[DataMember(Name = "status", IsRequired = false)]
	[JsonConverter(typeof(StringEnumConverter))]
	public WebsiteStatus Status { get; set; }

	/// <summary>
	///    The endpoint
	/// </summary>
	public string Endpoint() => "website/websites";
}
