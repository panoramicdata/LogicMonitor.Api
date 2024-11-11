namespace LogicMonitor.Api.Netscans;

/// <summary>
///    A Netscan policy exclude duplicates strategy
/// </summary>
[DataContract(Name = "type")]
public enum NetscanExcludeDuplicatesStrategy
{
	/// <summary>
	/// Unknown
	/// </summary>
	[EnumMember(Value = "unknown")]
	Unknown = 0,

	/// <summary>
	///    MatchingAnyMonitoredResources
	/// </summary>
	[EnumMember(Value = "1")]
	MatchingAnyMonitoredResources = 1,

	/// <summary>
	///    MatchingResourcesAlreadyDiscoveredByThisPolicy
	/// </summary>
	[EnumMember(Value = "2")]
	MatchingResourcesAlreadyDiscoveredByThisPolicy = 2,

	/// <summary>
	///    MatchingResourcesInSpecifiedResourceGroups
	/// </summary>
	[EnumMember(Value = "3")]
	MatchingResourcesInSpecifiedResourceGroups = 3,

	/// <summary>
	///    MatchingResourceCurrentlyAssignedToSpecifiedollectors
	/// </summary>
	[EnumMember(Value = "4")]
	MatchingResourcesCurrentlyAssignedToSpecifiedCollectors = 4,
}
