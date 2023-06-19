namespace LogicMonitor.Api.Netscans;

/// <summary>
/// The Netscan policy duplication strategy
/// </summary>
[DataContract(Name = "duplicate")]
public class ExcludeDuplicateIps
{
	/// <summary>
	/// What types of duplicate IPs should be excluded. Options are 1 (matching any monitored devices), 2 (matching devices already discovered by this scan), 3 (matching devices in these groups), and 4 (matching devices assigned to these collectors)
	/// </summary>
	[DataMember(Name = "type")]
	public NetscanExcludeDuplicatesStrategy Type { get; set; }

	/// <summary>
	/// The groups for which devices should be used to identify and exclude duplicate IPs, if duplicate type is 3
	/// </summary>
	[DataMember(Name = "groups")]
	public List<string>? Groups { get; set; }

	/// <summary>
	/// The collectors for which monitored devices should be used to identify and exclude duplicate IPs, if duplicate type is 4
	/// </summary>
	[DataMember(Name = "collectors")]
	public List<string>? Collectors { get; set; }
}
