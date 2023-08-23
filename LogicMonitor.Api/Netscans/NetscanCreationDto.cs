namespace LogicMonitor.Api.Netscans;

/// <summary>
///    A class for crating NetscanPolicies
/// </summary>
[DataContract]
public class NetscanCreationDto : CreationDto<Netscan>
{
	/// <summary>
	///    Constructor
	/// </summary>
	public NetscanCreationDto()
	{
	}

	/// <summary>
	///    The creator
	/// </summary>
	[DataMember(Name = "creator")]
	public string Creator { get; set; } = string.Empty;

	/// <summary>
	///    The method
	/// </summary>
	[DataMember(Name = "method")]
	public NetscanMethod Method { get; set; }

	/// <summary>
	///    Schedule on which to run this policy
	/// </summary>
	[DataMember(Name = "schedule")]
	public NetscanSchedule Schedule { get; set; } = new();

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
	///    The Netscan policy group name
	/// </summary>
	[DataMember(Name = "nsgId")]
	public string GroupId { get; set; } = string.Empty;

	/// <summary>
	///    The collector id as a string
	/// </summary>
	/// <example>"18"</example>
	[DataMember(Name = "collector")]
	public string CollectorId { get; set; } = string.Empty;

	/// <summary>
	///    Netscan credentials
	/// </summary>
	[DataMember(Name = "credentials")]
	public NetscanCredentials Credentials { get; set; } = new();

	/// <summary>
	///    The subnet scan range
	/// </summary>
	[DataMember(Name = "subnet")]
	public string SubnetScanRange { get; set; } = string.Empty;

	/// <summary>
	///    IP addresses to exclude
	/// </summary>
	[DataMember(Name = "exclude")]
	public string ExcludedIpAddresses { get; set; } = string.Empty;

	/// <summary>
	///    Discovered device rule
	/// </summary>
	[DataMember(Name = "ddr")]
	public DiscoveredDeviceRule DiscoveredDeviceRule { get; set; } = new();

	/// <summary>
	///    IP addresses to exclude
	/// </summary>
	[DataMember(Name = "duplicate")]
	public NetscanDuplicatesStrategy DuplicatesStrategy { get; set; } = new();

	/// <summary>
	///    The script path (if required)
	/// </summary>
	[DataMember(Name = "scriptPath")]
	public string ScriptPath { get; set; } = string.Empty;

	/// <summary>
	///    The script parameters
	/// </summary>
	[DataMember(Name = "scriptParams")]
	public string ScriptParameters { get; set; } = string.Empty;

	/// <summary>
	///    Script type
	/// </summary>
	[DataMember(Name = "scriptType")]
	public NetscanScriptType? ScriptType { get; set; }

	/// <summary>
	///    Groovyscript
	/// </summary>
	[DataMember(Name = "groovyScript")]
	public string GroovyScript { get; set; } = string.Empty;

	/// <summary>
	///    The groovy script parameters
	/// </summary>
	[DataMember(Name = "groovyScriptParams")]
	public string GroovyScriptParameters { get; set; } = string.Empty;

	/// <summary>
	///    The ports
	/// </summary>
	[DataMember(Name = "ports")]
	public NetscanPorts Ports { get; set; } = new();
}
