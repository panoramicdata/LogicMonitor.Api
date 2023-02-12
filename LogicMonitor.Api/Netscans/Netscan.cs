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
	[DataMember(Name = "creator", IsRequired = false)]
	public string? CreatorName { get; set; }

	/// <summary>
	/// The name of the group of the Collector associated with this Netscan
	/// </summary>
	[DataMember(Name = "collectorGroupName", IsRequired = false)]
	public string? CollectorGroupName { get; set; }

	/// <summary>
	/// The method that should be used to discover devices. Options are nmap (ICMP Ping), nec2 (EC2), enhancedScript and script
	/// </summary>
	[DataMember(Name = "method", IsRequired = true)]
	public NetscanMethod Method { get; set; }

	/// <summary>
	/// The ID of the group of the Collector associated with this Netscan
	/// </summary>
	[DataMember(Name = "collectorGroup", IsRequired = false)]
	public int CollectorGroupId { get; set; }

	/// <summary>
	/// The date and time of the next start time of the scan - displayed as manual if the scan does not run on a schedule
	/// </summary>
	[DataMember(Name = "nextStart", IsRequired = false)]
	public string? NextStart { get; set; }

	/// <summary>
	/// Information that determines how duplicate discovered devices should be handled
	/// </summary>
	[DataMember(Name = "duplicate", IsRequired = true)]
	public ExcludeDuplicateIps DuplicatesStrategy { get; set; } = null!;

	/// <summary>
	/// The Id of the device
	/// </summary>
	[DataMember(Name = "version", IsRequired = false)]
	public int Version { get; set; }

	/// <summary>
	/// The ID of the Collector associated with this Netscan
	/// </summary>
	[DataMember(Name = "collector", IsRequired = false)]
	public int CollectorId { get; set; }

	/// <summary>
	/// Ignore system.ips when checking for duplicate resources
	/// </summary>
	[DataMember(Name = "ignoreSystemIPsDuplicates", IsRequired = false)]
	public bool IgnoreSystemIpsDuplicates { get; set; }

	/// <summary>
	/// Information related to the recurring execution schedule for the Netscan Policy
	/// </summary>
	[DataMember(Name = "schedule", IsRequired = true)]
	public RestSchedule Schedule { get; set; } = null!;

	/// <summary>
	/// The description of the Collector associated with this Netscan
	/// </summary>
	[DataMember(Name = "collectorDescription", IsRequired = false)]
	public string? CollectorDescription { get; set; }

	/// <summary>
	/// The epoch of the next start time of the scan - displayed as 0 if the scan does not run on a schedule
	/// </summary>
	[DataMember(Name = "nextStartEpoch", IsRequired = false)]
	public long NextStartUtcSeconds { get; set; }

	/// <summary>
	/// The ID of the group the policy belongs to
	/// </summary>
	[DataMember(Name = "nsgId", IsRequired = false)]
	public int GroupId { get; set; }

	/// <summary>
	/// The group the Netscan policy should belong to
	/// </summary>
	[DataMember(Name = "group", IsRequired = false)]
	public string? GroupName { get; set; }

	/// <summary>
	/// The Discovered device rules
	/// </summary>
	[DataMember(Name = "ddr", IsRequired = false)]
	public Ec2DDR? Ddr { get; set; }

	/// <summary>
	/// The credentials to be used for the scan
	/// </summary>
	[DataMember(Name = "credentials", IsRequired = false)]
	public EC2NetscanPolicyCredential Credentials { get; set; } = null!;

	/// <summary>
	/// Which IP the EC2 instance should be monitored with for nec2 scans: private or public
	/// </summary>
	[DataMember(Name = "accessibility", IsRequired = false)]
	public string Accessibility { get; set; } = null!;

	/// <summary>
	/// How dead EC2 instances should be handled for nec2 scans. Must be Manually
	/// </summary>
	[DataMember(Name = "deadOperation", IsRequired = false)]
	public string? DeadOperation { get; set; }

	/// <summary>
	/// The ports
	/// </summary>
	[DataMember(Name = "ports", IsRequired = false)]
	public RestNetscanPorts? Ports { get; set; }

	/// <summary>
	/// The subnet to exclude from scanning from nmap scans
	/// </summary>
	[DataMember(Name = "exclude", IsRequired = false)]
	public string? Exclude { get; set; }

	/// <summary>
	/// Include Network \u0026 Broadcast Address for CIDR based netscan
	/// </summary>
	[DataMember(Name = "includeNetworkAndBroadcast", IsRequired = false)]
	public bool IncludeNetworkAndBroadcast { get; set; }

	/// <summary>
	/// The subnet to scan for nmap scans
	/// </summary>
	[DataMember(Name = "subnet", IsRequired = false)]
	public string SubnetScanRange { get; set; } = null!;

	/// <summary>
	/// The script path for an external script
	/// </summary>
	[DataMember(Name = "scriptPath", IsRequired = false)]
	public string? ScriptPath { get; set; }

	/// <summary>
	/// The full path of the default group to add discovered devices to
	/// </summary>
	[DataMember(Name = "defaultGroupFullPath", IsRequired = false)]
	public string? DefaultGroupFullPath { get; set; }

	/// <summary>
	/// The ID of the default group to add discovered devices to
	/// </summary>
	[DataMember(Name = "defaultGroup", IsRequired = false)]
	public int DefaultGroupName { get; set; }

	/// <summary>
	/// For embedded script scans, the groovy script contents
	/// </summary>
	[DataMember(Name = "groovyScript", IsRequired = false)]
	public string? GroovyScript { get; set; }

	/// <summary>
	/// The Linux script parameters
	/// </summary>
	[DataMember(Name = "linuxScriptParams", IsRequired = false)]
	public string? LinuxScriptParameters { get; set; }

	/// <summary>
	/// For script scans, the type of script. Options are embeded and external
	/// </summary>
	[DataMember(Name = "scriptType", IsRequired = false)]
	public NetscanScriptType ScriptType { get; set; }

	/// <summary>
	/// For embedded script scans, the groovy script parameters
	/// </summary>
	[DataMember(Name = "groovyScriptParams", IsRequired = false)]
	public string? GroovyScriptParameters { get; set; }

	/// <summary>
	/// The Windows script
	/// </summary>
	[DataMember(Name = "windowsScript", IsRequired = false)]
	public string? WindowsScript { get; set; }

	/// <summary>
	/// The Windows script parameters
	/// </summary>
	[DataMember(Name = "windowsScriptParams", IsRequired = false)]
	public string? WindowsScriptParameters { get; set; }

	/// <summary>
	/// The parameters for an external script
	/// </summary>
	[DataMember(Name = "scriptParams", IsRequired = false)]
	public string? ScriptParameters { get; set; }

	/// <summary>
	/// The Linux script
	/// </summary>
	[DataMember(Name = "linuxScript", IsRequired = false)]
	public string? LinuxScript { get; set; }

	/// <summary>
	///    The endpoint
	/// </summary>
	public string Endpoint() => "setting/netscans";
}
