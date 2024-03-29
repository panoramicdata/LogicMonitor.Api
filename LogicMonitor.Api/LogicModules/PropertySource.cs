namespace LogicMonitor.Api.LogicModules;

/// <summary>
/// A PropertySource
/// </summary>
[DataContract]
public class PropertySource : LogicModule, IHasEndpoint
{
	/// <summary>
	/// What this applies to
	/// </summary>
	[DataMember(Name = "appliesTo")]
	public string AppliesTo { get; set; } = string.Empty;

	/// <summary>
	/// The audit version
	/// </summary>
	[DataMember(Name = "auditVersion")]
	public int? AuditVersion { get; set; }

	/// <summary>
	/// The audit version
	/// </summary>
	[DataMember(Name = "dataType")]
	public PropertySourceDataType DataType { get; set; }

	/// <summary>
	/// GroovyScript
	/// </summary>
	[DataMember(Name = "groovyScript")]
	public string GroovyScript { get; set; } = string.Empty;

	/// <summary>
	/// The Group name
	/// </summary>
	[DataMember(Name = "group")]
	public string Group { get; set; } = string.Empty;

	/// <summary>
	/// The interval
	/// </summary>
	[DataMember(Name = "interval")]
	public int Interval { get; set; }

	/// <summary>
	/// Linux command line
	/// </summary>
	[DataMember(Name = "linuxCmdline")]
	public string LinuxCommandLine { get; set; } = string.Empty;

	/// <summary>
	/// Linux script
	/// </summary>
	[DataMember(Name = "linuxScript")]
	public string LinuxScript { get; set; } = string.Empty;

	/// <summary>
	/// Published
	/// </summary>
	[DataMember(Name = "published")]
	public int Published { get; set; }

	/// <summary>
	/// The schedule option
	/// </summary>
	[DataMember(Name = "scheduleOption")]
	public string Schedule { get; set; } = string.Empty;

	/// <summary>
	/// Script type
	/// </summary>
	[DataMember(Name = "scriptType")]
	public string ScriptType { get; set; } = string.Empty;

	/// <summary>
	/// Tags
	/// </summary>
	[DataMember(Name = "tags")]
	public string Tags { get; set; } = string.Empty;

	/// <summary>
	/// Technology
	/// </summary>
	[DataMember(Name = "technology")]
	public string Technology { get; set; } = string.Empty;

	/// <summary>
	/// The version
	/// </summary>
	[DataMember(Name = "version")]
	public int? Version { get; set; }

	/// <summary>
	/// Windows command line
	/// </summary>
	[DataMember(Name = "windowsCmdline")]
	public string WindowsCommandLine { get; set; } = string.Empty;

	/// <summary>
	/// Windows script
	/// </summary>
	[DataMember(Name = "windowsScript")]
	public string WindowsScript { get; set; } = string.Empty;

	/// <summary>
	/// ToString override
	/// </summary>
	/// <returns>'Id : Name'</returns>
	public override string ToString() => $"{Id} : {Name}";

	/// <summary>
	///    The endpoint
	/// </summary>
	public string Endpoint() => "setting/propertyrules";
}
