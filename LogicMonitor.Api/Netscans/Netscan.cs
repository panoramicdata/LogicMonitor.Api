namespace LogicMonitor.Api.Netscans;

/// <summary>
///    A Netscan
/// </summary>
[DataContract]
public class Netscan : NamedItem, IExecutable
{
	/// <summary>
	/// The user that created the policy
	/// </summary>
	[DataMember(Name = "creator")]
	public string CreatorName { get; set; } = string.Empty;

	/// <summary>
	/// The name of the group of the Collector associated with this Netscan
	/// </summary>
	[DataMember(Name = "collectorGroupName")]
	public string CollectorGroupName { get; set; } = string.Empty;

	/// <summary>
	/// The method that should be used to discover devices. Options are nmap (ICMP Ping), nec2 (EC2), enhancedScript and script
	/// </summary>
	[DataMember(Name = "method")]
	public NetscanMethod Method { get; set; }

	/// <summary>
	/// The ID of the group of the Collector associated with this Netscan
	/// </summary>
	[DataMember(Name = "collectorGroup")]
	public int CollectorGroupId { get; set; }

	/// <summary>
	/// The date and time of the next start time of the scan - displayed as manual if the scan does not run on a schedule
	/// </summary>
	[DataMember(Name = "nextStart")]
	public string NextStart { get; set; } = string.Empty;

	/// <summary>
	/// Information that determines how duplicate discovered devices should be handled
	/// </summary>
	[DataMember(Name = "duplicate")]
	public ExcludeDuplicateIps DuplicatesStrategy { get; set; } = new(); 

	/// <summary>
	/// The Id of the device
	/// </summary>
	[DataMember(Name = "version")]
	public int Version { get; set; }

	/// <summary>
	/// The ID of the Collector associated with this Netscan
	/// </summary>
	[DataMember(Name = "collector")]
	public int CollectorId { get; set; }

	/// <summary>
	/// Ignore system.ips when checking for duplicate resources
	/// </summary>
	[DataMember(Name = "ignoreSystemIPsDuplicates")]
	public bool IgnoreSystemIpsDuplicates { get; set; }

	/// <summary>
	/// Information related to the recurring execution schedule for the Netscan Policy
	/// </summary>
	[DataMember(Name = "schedule")]
	public NetscanSchedule Schedule { get; set; } = new();

	/// <summary>
	/// The description of the Collector associated with this Netscan
	/// </summary>
	[DataMember(Name = "collectorDescription")]
	public string CollectorDescription { get; set; } = string.Empty;

	/// <summary>
	/// The epoch of the next start time of the scan - displayed as 0 if the scan does not run on a schedule
	/// </summary>
	[DataMember(Name = "nextStartEpoch")]
	public long NextStartUtcSeconds { get; set; }

	/// <summary>
	/// The ID of the group the policy belongs to
	/// </summary>
	[DataMember(Name = "nsgId")]
	public int GroupId { get; set; }

	/// <summary>
	/// The group the Netscan policy should belong to
	/// </summary>
	[DataMember(Name = "group")]
	public string GroupName { get; set; } = string.Empty;

	/// <summary>
	/// The Discovered device rules
	/// </summary>
	[DataMember(Name = "ddr")]
	public Ec2DDR? Ddr { get; set; }

	/// <summary>
	/// The credentials to be used for the scan
	/// </summary>
	[DataMember(Name = "credentials")]
	public EC2NetscanPolicyCredential Credentials { get; set; } = new();

	/// <summary>
	/// Which IP the EC2 instance should be monitored with for nec2 scans: private or public
	/// </summary>
	[DataMember(Name = "accessibility")]
	public string Accessibility { get; set; } = string.Empty;

	/// <summary>
	/// How dead EC2 instances should be handled for nec2 scans. Must be Manually
	/// </summary>
	[DataMember(Name = "deadOperation")]
	public string DeadOperation { get; set; } = string.Empty;

	/// <summary>
	/// The ports
	/// </summary>
	[DataMember(Name = "ports")]
	public RestNetscanPorts? Ports { get; set; }

	/// <summary>
	/// The subnet to exclude from scanning from nmap scans
	/// </summary>
	[DataMember(Name = "exclude")]
	public string Exclude { get; set; } = string.Empty;

	/// <summary>
	/// Include Network \u0026 Broadcast Address for CIDR based netscan
	/// </summary>
	[DataMember(Name = "includeNetworkAndBroadcast")]
	public bool IncludeNetworkAndBroadcast { get; set; }

	/// <summary>
	/// The subnet to scan for nmap scans
	/// </summary>
	[DataMember(Name = "subnet")]
	public string SubnetScanRange { get; set; } = string.Empty;

	/// <summary>
	/// The script path for an external script
	/// </summary>
	[DataMember(Name = "scriptPath")]
	public string ScriptPath { get; set; } = string.Empty;

	/// <summary>
	/// The full path of the default group to add discovered devices to
	/// </summary>
	[DataMember(Name = "defaultGroupFullPath")]
	public string DefaultGroupFullPath { get; set; } = string.Empty;

	/// <summary>
	/// The ID of the default group to add discovered devices to
	/// </summary>
	[DataMember(Name = "defaultGroup")]
	public int DefaultGroupName { get; set; }

	/// <summary>
	/// For embedded script scans, the groovy script contents
	/// </summary>
	[DataMember(Name = "groovyScript")]
	public string GroovyScript { get; set; } = string.Empty;

	/// <summary>
	/// The Linux script parameters
	/// </summary>
	[DataMember(Name = "linuxScriptParams")]
	public string LinuxScriptParameters { get; set; } = string.Empty;

	/// <summary>
	/// For script scans, the type of script. Options are embeded and external
	/// </summary>
	[DataMember(Name = "scriptType")]
	public NetscanScriptType ScriptType { get; set; }

	/// <summary>
	/// For embedded script scans, the groovy script parameters
	/// </summary>
	[DataMember(Name = "groovyScriptParams")]
	public string GroovyScriptParameters { get; set; } = string.Empty;

	/// <summary>
	/// The Windows script
	/// </summary>
	[DataMember(Name = "windowsScript")]
	public string WindowsScript { get; set; } = string.Empty;

	/// <summary>
	/// The Windows script parameters
	/// </summary>
	[DataMember(Name = "windowsScriptParams")]
	public string WindowsScriptParameters { get; set; } = string.Empty;

	/// <summary>
	/// The parameters for an external script
	/// </summary>
	[DataMember(Name = "scriptParams")]
	public string ScriptParameters { get; set; } = string.Empty;

	/// <summary>
	/// The Linux script
	/// </summary>
	[DataMember(Name = "linuxScript")]
	public string LinuxScript { get; set; } = string.Empty;

	/// <summary>
	///    The endpoint
	/// </summary>
	public string Endpoint() => "setting/netscans";
}
