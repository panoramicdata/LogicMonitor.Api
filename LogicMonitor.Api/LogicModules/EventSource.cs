namespace LogicMonitor.Api.LogicModules;

/// <summary>
/// An EventSource
/// </summary>
[DataContract]
public class EventSource : LogicModule, IHasEndpoint
{
	/// <summary>
	/// Whether or not duplicate alerts have to be suppressed
	/// </summary>
	[DataMember(Name = "suppressDuplicatesES", IsRequired = false)]
	public bool SuppressDuplicates { get; set; }

	/// <summary>
	/// The alert message subject for the EventSource
	/// </summary>
	[DataMember(Name = "alertSubjectTemplate", IsRequired = false)]
	public string AlertBodyTemplate { get; set; } = string.Empty;

	/// <summary>
	/// The default alert level: warn | error | critical | doMapping
	/// </summary>
	[DataMember(Name = "alertLevel", IsRequired = false)]
	public string AlertLevel { get; set; } = string.Empty;

	/// <summary>
	/// The Applies To for the LMModule
	/// </summary>
	[DataMember(Name = "appliesTo", IsRequired = false)]
	public string AppliesTo { get; set; } = string.Empty;

	/// <summary>
	/// The Technical Notes for the LMModule
	/// </summary>
	[DataMember(Name = "technology", IsRequired = false)]
	public string Technology { get; set; } = string.Empty;

	/// <summary>
	/// The filters for the EventSource
	/// </summary>
	[DataMember(Name = "filters", IsRequired = false)]
	public List<RestEventSourceFilter> Filters { get; set; } = new();

	/// <summary>
	/// The epoch time of the last update to the EventSource
	/// </summary>
	[DataMember(Name = "version", IsRequired = false)]
	public long Version { get; set; }

	/// <summary>
	/// The EventSource collector type: wineventlog | syslog | snmptrap | echo | logfile | scriptevent | awsrss | azurerss | azureadvisor | gcpatom | awsrdspievent | azureresourcehealthevent | azureemergingissue | azureloganalyticsworkspacesevent | awstrustedadvisor | awshealth | ipmievent
	/// </summary>
	[DataMember(Name = "collector", IsRequired = false)]
	public string Collector { get; set; } = string.Empty;

	/// <summary>
	/// The Tags for the LMModule
	/// </summary>
	[DataMember(Name = "tags", IsRequired = false)]
	public string Tags { get; set; } = string.Empty;

	/// <summary>
	/// The auditVersion of the EventSource
	/// </summary>
	[DataMember(Name = "auditVersion", IsRequired = false)]
	public long AuditVersion { get; set; }

	/// <summary>
	/// The alert message body for the EventSource
	/// </summary>
	[DataMember(Name = "alertBodyTemplate", IsRequired = false)]
	public string AlertSubjectTemplate { get; set; } = string.Empty;

	/// <summary>
	/// Whether or not the alert should clear after acknowledgement
	/// </summary>
	[DataMember(Name = "clearAfterAck", IsRequired = false)]
	public bool ClearAfterAcknowledgement { get; set; }

	/// <summary>
	/// The time in minutes after which the alert should clear
	/// </summary>
	[DataMember(Name = "alertEffectiveIval", IsRequired = true)]
	public int AlertEffectiveIntervalMinutes { get; set; }

	/// <summary>
	/// The LMModule dimension
	/// </summary>
	[DataMember(Name = "dimension", IsRequired = false)]
	public string? Dimension { get; set; } = string.Empty;

	/// <summary>
	/// The Groovy script
	/// </summary>
	[DataMember(Name = "groovyScript", IsRequired = false)]
	public string? GroovyScript { get; set; } = string.Empty;

	/// <summary>
	/// The group the LMModule is in
	/// </summary>
	[DataMember(Name = "group", IsRequired = false)]
	public string Group { get; set; } = string.Empty;

	/// <summary>
	/// The Linux command line
	/// </summary>
	[DataMember(Name = "linuxCmdLine", IsRequired = false)]
	public string? LinuxCommandLine { get; set; }

	/// <summary>
	/// The Linux script
	/// </summary>
	[DataMember(Name = "linuxScript", IsRequired = false)]
	public string? LinuxScript { get; set; } = string.Empty;

	/// <summary>
	/// The Log files
	/// </summary>
	[DataMember(Name = "logFiles", IsRequired = false)]
	public List<LogFile>? LogFiles { get; set; }

	/// <summary>
	/// The num(???)
	/// </summary>
	[DataMember(Name = "num", IsRequired = false)]
	public int? Number { get; set; }

	/// <summary>
	/// The EventSource schedule
	/// </summary>
	[DataMember(Name = "schedule", IsRequired = false)]
	public int? ScheduleMinutes { get; set; }

	/// <summary>
	/// The script type
	/// </summary>
	[DataMember(Name = "scriptType", IsRequired = false)]
	public EventSourceScriptType? ScriptType { get; set; }

	/// <summary>
	/// The Windows command line
	/// </summary>
	[DataMember(Name = "windowsCmdLine", IsRequired = false)]
	public string? WindowsCommandLine { get; set; }

	/// <summary>
	/// The Windows script
	/// </summary>
	[DataMember(Name = "windowsScript", IsRequired = false)]
	public string? WindowsScript { get; set; }

	/// <summary>
	/// ToString override
	/// </summary>
	/// <returns>'Id : Name - DisplayedAs'</returns>
	public override string ToString() => $"{Id} : {Name}";

	/// <summary>
	///    The endpoint
	/// </summary>
	public string Endpoint() => "setting/eventsources";
}
