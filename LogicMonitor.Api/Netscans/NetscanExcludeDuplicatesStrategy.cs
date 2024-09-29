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
	///    MatchingAnyMonitoredDevices
	/// </summary>
	[EnumMember(Value = "1")]
	MatchingAnyMonitoredResources = 1,

	/// <summary>
	/// Obsolete
	/// </summary>
	[JsonIgnore]
	[Obsolete("Use MatchingAnyMonitoredResource instead", true)]
	MatchingAnyMonitoredDevices = MatchingAnyMonitoredResources,

	/// <summary>
	///    MatchingDevicesAlreadyDiscoveredByThisPolicy
	/// </summary>
	[EnumMember(Value = "2")]
	MatchingResourcesAlreadyDiscoveredByThisPolicy = 2,

	/// <summary>
	/// Obsolete
	/// </summary>
	[JsonIgnore]
	[Obsolete("Use MatchingResourcesInSpecifiedResourceGroups instead", true)]
	MatchingDevicesAlreadyDiscoveredByThisPolicy = MatchingResourcesAlreadyDiscoveredByThisPolicy,

	/// <summary>
	///    MatchingDevicesInSpecifiedDeviceGroups
	/// </summary>
	[EnumMember(Value = "3")]
	MatchingResourcesInSpecifiedResourceGroups = 3,

	/// <summary>
	/// Obsolete
	/// </summary>
	[JsonIgnore]
	[Obsolete("Use MatchingResourcesInSpecifiedResourceGroups instead", true)]
	MatchingDevicesInSpecifiedDeviceGroups = MatchingResourcesInSpecifiedResourceGroups,

	/// <summary>
	///    MatchingDevicesCurrentlyAssignedToSpecifiedollectors
	/// </summary>
	[EnumMember(Value = "4")]
	MatchingResourcesCurrentlyAssignedToSpecifiedCollectors = 4,

	/// <summary>
	/// Obsolete
	/// </summary>
	[JsonIgnore]
	[Obsolete("Use MatchingResourcesCurrentlyAssignedToSpecifiedCollectors instead", true)]
	MatchingDevicesCurrentlyAssignedToSpecifiedCollectors = MatchingResourcesCurrentlyAssignedToSpecifiedCollectors,
}
