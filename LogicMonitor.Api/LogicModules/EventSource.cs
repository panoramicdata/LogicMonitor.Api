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
	[DataMember(Name = "suppressDuplicatesES")]
	public bool SuppressDuplicates { get; set; }

	/// <summary>
	/// The alert message subject for the EventSource
	/// </summary>
	[DataMember(Name = "alertSubjectTemplate")]
	public string AlertBodyTemplate { get; set; } = string.Empty;

	/// <summary>
	/// The default alert level: warn | error | critical | doMapping
	/// </summary>
	[DataMember(Name = "alertLevel")]
	public string AlertLevel { get; set; } = string.Empty;

	/// <summary>
	/// The Applies To for the LMModule
	/// </summary>
	[DataMember(Name = "appliesTo")]
	public string AppliesTo { get; set; } = string.Empty;

	/// <summary>
	/// The column instance name
	/// </summary>
	[DataMember(Name = "columnInstanceName")]
	public string? ColumnInstanceName { get; set; }

	/// <summary>
	/// The Technical Notes for the LMModule
	/// </summary>
	[DataMember(Name = "technology")]
	public string Technology { get; set; } = string.Empty;

	/// <summary>
	/// The filters for the EventSource
	/// </summary>
	[DataMember(Name = "filters")]
	public List<RestEventSourceFilter> Filters { get; set; } = [];

	/// <summary>
	/// The epoch time of the last update to the EventSource
	/// </summary>
	[DataMember(Name = "version")]
	public long Version { get; set; }

	/// <summary>
	/// The EventSource collector type
	/// </summary>
	[DataMember(Name = "collector")]
	public EventSourceType Type { get; set; }

	/// <summary>
	/// The Tags for the LMModule
	/// </summary>
	[DataMember(Name = "tags")]
	public string Tags { get; set; } = string.Empty;

	/// <summary>
	/// The auditVersion of the EventSource
	/// </summary>
	[DataMember(Name = "auditVersion")]
	public long AuditVersion { get; set; }

	/// <summary>
	/// The alert message body for the EventSource
	/// </summary>
	[DataMember(Name = "alertBodyTemplate")]
	public string AlertSubjectTemplate { get; set; } = string.Empty;

	/// <summary>
	/// Whether or not the alert should clear after acknowledgement
	/// </summary>
	[DataMember(Name = "clearAfterAck")]
	public bool ClearAfterAcknowledgement { get; set; }

	/// <summary>
	/// The time in minutes after which the alert should clear
	/// </summary>
	[DataMember(Name = "alertEffectiveIval")]
	public int AlertEffectiveIntervalMinutes { get; set; }

	/// <summary>
	/// The LMModule dimension
	/// </summary>
	[DataMember(Name = "dimension")]
	public string Dimension { get; set; } = string.Empty;

	/// <summary>
	/// The Groovy script
	/// </summary>
	[DataMember(Name = "groovyScript")]
	public string GroovyScript { get; set; } = string.Empty;

	/// <summary>
	/// The group the LMModule is in
	/// </summary>
	[DataMember(Name = "group")]
	public string Group { get; set; } = string.Empty;

	/// <summary>
	/// The Linux command line
	/// </summary>
	[DataMember(Name = "linuxCmdLine")]
	public string LinuxCommandLine { get; set; } = string.Empty;

	/// <summary>
	/// The Linux script
	/// </summary>
	[DataMember(Name = "linuxScript")]
	public string LinuxScript { get; set; } = string.Empty;

	/// <summary>
	/// The Log files
	/// </summary>
	[DataMember(Name = "logFiles")]
	public List<LogFile>? LogFiles { get; set; }

	/// <summary>
	/// The num(???)
	/// </summary>
	[DataMember(Name = "num")]
	public int? Number { get; set; }

	/// <summary>
	/// The query
	/// </summary>
	[DataMember(Name = "query")]
	public string? Query { get; set; }

	/// <summary>
	/// The EventSource schedule
	/// </summary>
	[DataMember(Name = "schedule")]
	public int? ScheduleMinutes { get; set; }

	/// <summary>
	/// The script type
	/// </summary>
	[DataMember(Name = "scriptType")]
	public EventSourceScriptType? ScriptType { get; set; }

	/// <summary>
	/// The Windows command line
	/// </summary>
	[DataMember(Name = "windowsCmdLine")]
	public string WindowsCommandLine { get; set; } = string.Empty;

	/// <summary>
	/// The Windows script
	/// </summary>
	[DataMember(Name = "windowsScript")]
	public string WindowsScript { get; set; } = string.Empty;

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
