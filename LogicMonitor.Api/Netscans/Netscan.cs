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
	///    The endpoint
	/// </summary>
	public string Endpoint() => "setting/netscans";
}
