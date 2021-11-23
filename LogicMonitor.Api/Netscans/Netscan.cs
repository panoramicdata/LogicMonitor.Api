namespace LogicMonitor.Api.Netscans;

/// <summary>
///    A Netscan
/// </summary>
[DataContract]
public class Netscan : NamedItem, IExecutable
{
	/// <summary>
	///    The accessibility
	/// </summary>
	[DataMember(Name = "accessibility")]
	public string Accessibility { get; set; }

	/// <summary>
	///    The operation when a device is dead
	/// </summary>
	[DataMember(Name = "deadOperation")]
	public string DeadOperation { get; set; }

	/// <summary>
	///    The method
	/// </summary>
	[DataMember(Name = "method")]
	public NetscanMethod Method { get; set; }

	/// <summary>
	///    The default group name
	/// </summary>
	[DataMember(Name = "defaultGroup")]
	public string DefaultGroupName { get; set; }

	/// <summary>
	///    The default group name
	/// </summary>
	[DataMember(Name = "defaultGroupFullPath")]
	public string DefaultGroupFullPath { get; set; }

	/// <summary>
	///    The group name
	/// </summary>
	[DataMember(Name = "group")]
	public string GroupName { get; set; }

	/// <summary>
	///    The netscan group id
	/// </summary>
	[DataMember(Name = "nsgId")]
	public int GroupId { get; set; }

	/// <summary>
	///    The schedule
	/// </summary>
	[DataMember(Name = "schedule")]
	public NetscanSchedule Schedule { get; set; }

	/// <summary>
	///    The creator name
	/// </summary>
	[DataMember(Name = "creator")]
	public string CreatorName { get; set; }

	/// <summary>
	///    The collector group id
	/// </summary>
	[DataMember(Name = "collectorGroup")]
	public int CollectorGroupId { get; set; }

	/// <summary>
	///    The collector group name
	/// </summary>
	[DataMember(Name = "collectorGroupName")]
	public object CollectorGroupName { get; set; }

	/// <summary>
	///    The collector id
	/// </summary>
	[DataMember(Name = "collector")]
	public int CollectorId { get; set; }

	/// <summary>
	///    The collector description
	/// </summary>
	[DataMember(Name = "collectorDescription")]
	public string CollectorDescription { get; set; }

	/// <summary>
	///    The next start
	/// </summary>
	[DataMember(Name = "nextStart")]
	public string NextStart { get; set; }

	/// <summary>
	///    The duplicate
	/// </summary>
	[DataMember(Name = "duplicate")]
	public NetscanDuplicatesStrategy DuplicatesStrategy { get; set; }

	/// <summary>
	///    The subnet scan range
	/// </summary>
	[DataMember(Name = "subnet")]
	public string SubnetScanRange { get; set; }

	/// <summary>
	///    The excluded IP addresses
	/// </summary>
	[DataMember(Name = "exclude")]
	public string ExcludedIpAddresses { get; set; }

	/// <summary>
	///    The credentials
	/// </summary>
	[DataMember(Name = "credentials")]
	public NetscanCredentials Credentials { get; set; }

	/// <summary>
	///    The Discovered device rules
	/// </summary>
	[DataMember(Name = "ddr")]
	public NetscanDdr Ddr { get; set; }

	/// <summary>
	///    The Script type (if any)
	/// </summary>
	[DataMember(Name = "scriptType")]
	public NetscanScriptType? ScriptType { get; set; }

	/// <summary>
	///    The groovy script
	/// </summary>
	[DataMember(Name = "groovyScript")]
	public string GroovyScript { get; set; }

	/// <summary>
	///    The groovy script parameters
	/// </summary>
	[DataMember(Name = "groovyScriptParams")]
	public string GroovyScriptParameters { get; set; }

	/// <summary>
	///    The Linux script
	/// </summary>
	[DataMember(Name = "linuxScript")]
	public string LinuxScript { get; set; }

	/// <summary>
	///    The Linux script parameters
	/// </summary>
	[DataMember(Name = "linuxScriptParams")]
	public string LinuxScriptParameters { get; set; }

	/// <summary>
	///    The Windows script
	/// </summary>
	[DataMember(Name = "windowsScript")]
	public string WindowsScript { get; set; }

	/// <summary>
	///    The Windows script parameters
	/// </summary>
	[DataMember(Name = "windowsScriptParams")]
	public string WindowsScriptParameters { get; set; }

	/// <summary>
	///    The script path
	/// </summary>
	[DataMember(Name = "scriptPath")]
	public string ScriptPath { get; set; }

	/// <summary>
	///    The script parameters
	/// </summary>
	[DataMember(Name = "scriptParams")]
	public string ScriptParameters { get; set; }

	/// <summary>
	///    The next start time in seconds since the Epoch
	/// </summary>
	[DataMember(Name = "nextStartEpoch")]
	public long NextStartUtcSeconds { get; set; }

	/// <summary>
	///    The endpoint
	/// </summary>
	public string Endpoint() => "setting/netscans";

	/// <summary>
	///    The version
	/// </summary>
	[DataMember(Name = "version")]
	public int Version { get; set; }

	/// <summary>
	/// The ports
	/// </summary>
	[DataMember(Name = "ports")]
	public NetscanPorts Ports { get; set; }

	/// <summary>
	/// Whether to ignore duplicates of the system.ips property
	/// /// </summary>
	[DataMember(Name = "ignoreSystemIPsDuplicates")]
	public bool IgnoreSystemIpsDuplicates { get; set; }
}
