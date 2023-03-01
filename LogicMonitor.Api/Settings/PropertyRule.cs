namespace LogicMonitor.Api.Settings;

/// <summary>
/// PropertyRule
/// </summary>

[DataContract]
public class PropertyRule : NamedItem
{
	/// <summary>
	/// property rule schedule option: onAP | onAPpropertyChanges
	/// </summary>
	[DataMember(Name = "scheduleOption")]
	public string? ScheduleOption { get; set; }

	/// <summary>
	/// groovy script
	/// </summary>
	[DataMember(Name = "groovyScript")]
	public string? GroovyScript { get; set; }

	/// <summary>
	/// external windows script name
	/// </summary>
	[DataMember(Name = "windowsScript")]
	public string? WindowsScript { get; set; }

	/// <summary>
	/// The data type of property source, default is 0\n\n0: property source\n\n1: raw ERI\n\n 
	/// </summary>
	[DataMember(Name = "dataType")]
	public int DataType { get; set; }

	/// <summary>
	/// property rule applies to
	/// </summary>
	[DataMember(Name = "appliesTo")]
	public string? AppliesTo { get; set; }

	/// <summary>
	/// technology notes
	/// </summary>
	[DataMember(Name = "technology")]
	public string? Technology { get; set; }

	/// <summary>
	/// external linux script args
	/// </summary>
	[DataMember(Name = "linuxCmdline")]
	public string? LinuxCmdline { get; set; }

	/// <summary>
	/// the property rule version
	/// </summary>
	[DataMember(Name = "version")]
	public long Version { get; set; }

	/// <summary>
	/// external windows script args
	/// </summary>
	[DataMember(Name = "windowsCmdline")]
	public string? WindowsCmdline { get; set; }

	/// <summary>
	/// LM module lineageId
	/// </summary>
	[DataMember(Name = "lineageId")]
	public string? LineageId { get; set; }

	/// <summary>
	/// property rule tags
	/// </summary>
	[DataMember(Name = "tags")]
	public string? Tags { get; set; }

	/// <summary>
	/// the property rule auditVersion
	/// </summary>
	[DataMember(Name = "auditVersion")]
	public long AuditVersion { get; set; }

	/// <summary>
	/// The local module\u0027s IntegrationMetadata, readable for troubleshooting purposes
	/// </summary>
	[DataMember(Name = "installationMetadata")]
	public IntegrationMetadata? InstallationMetadata { get; set; }

	/// <summary>
	/// script type: embed | powershell | external
	/// </summary>
	[DataMember(Name = "scriptType")]
	public string? ScriptType { get; set; }

	/// <summary>
	/// LM module checksum
	/// </summary>
	[DataMember(Name = "checksum")]
	public string? Checksum { get; set; }

	/// <summary>
	/// The collect interval of raw ERI
	/// </summary>
	[DataMember(Name = "interval")]
	public int Interval { get; set; }

	/// <summary>
	/// external linux script name
	/// </summary>
	[DataMember(Name = "linuxScript")]
	public string? LinuxScript { get; set; }

	/// <summary>
	/// property rule group name
	/// </summary>
	[DataMember(Name = "group")]
	public string? Group { get; set; }
}
