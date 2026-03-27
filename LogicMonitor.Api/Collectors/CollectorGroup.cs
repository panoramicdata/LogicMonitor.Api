namespace LogicMonitor.Api.Collectors;

/// <summary>
///    A LogicMonitor Collector Group
/// </summary>
[DataContract]
public class CollectorGroup : NamedItem, IHasCustomProperties, IHasEndpoint
{
	/// <summary>
	/// The custom properties defined for the Collector group
	/// </summary>
	[DataMember(Name = "customProperties")]
	public List<EntityProperty> CustomProperties { get; set; } = [];

	/// <summary>
	/// The permission level of the user that made the API request
	/// </summary>
	[DataMember(Name = "userPermission")]
	public string UserPermission { get; set; } = string.Empty;

	/// <summary>
	/// The time at which the group was created, in epoch format
	/// </summary>
	[DataMember(Name = "createOn")]
	public long CreatedOnTimeStampSeconds { get; set; }

	/// <summary>
	/// The number of Collectors that belong to the group
	/// </summary>
	[DataMember(Name = "numOfCollectors")]
	public int CollectorCount { get; set; }

	/// <summary>
	///    The UTC DateTime that the CollectorGroup was created, if known
	/// </summary>
	[IgnoreDataMember]
	public DateTime? CreatedOnUtc => CreatedOnTimeStampSeconds.ToNullableDateTimeUtc();

	/// <summary>
	/// if the collector has autoBalance set as true or false
	/// </summary>
	[DataMember(Name = "autoBalance")]
	public bool AutoBalance { get; set; }

	/// <summary>
	/// the auto balance strategy
	/// </summary>
	[DataMember(Name = "autoBalanceStrategy")]
	public string AutoBalanceStrategy { get; set; } = string.Empty;

	/// <summary>
	///    Auto-balance device count threshold
	/// </summary>
	[DataMember(Name = "autoBalanceDeviceCountThreshold")]
	public int AutoBalanceResourceCountThreshold { get; set; }

	/// <summary>
	/// tThreshold for instance count strategy to check if a collector has high load
	/// </summary>
	[DataMember(Name = "autoBalanceInstanceCountThreshold")]
	public int AutoBalanceInstanceCountThreshold { get; set; }

	/// <summary>
	/// Threshold for CAL (what is that?)
	/// </summary>
	[DataMember(Name = "calThreshold")]
	public int CalThreshold { get; set; }

	/// <summary>
	///    Whether the versions mismatch
	/// </summary>
	[DataMember(Name = "mismatchVerison")]
	public bool VersionsMismatch { get; set; }

	/// <summary>
	/// the platform limitation
	/// </summary>
	[DataMember(Name = "platform")]
	public string Platform { get; set; } = string.Empty;

	/// <summary>
	/// The number of instances that belong to the group
	/// </summary>
	[DataMember(Name = "numOfInstances")]
	public long InstanceCount { get; set; }

	/// <summary>
	/// The number of hosts that belong to the group
	/// </summary>
	[DataMember(Name = "numOfHosts")]
	public long ResourceCount { get; set; }

	/// <summary>
	/// specifies if the version of all collectors in group is same
	/// </summary>
	[DataMember(Name = "mismatchVersion")]
	public bool MismatchVersion { get; set; }

	/// <summary>
	/// The status of the highest priority sub collector
	/// </summary>
	[DataMember(Name = "highestPriorityCollectorStatus")]
	public RestHighestPriorityCollectorStatus HighestPriorityCollectorStatus { get; set; } = new();

	/// <summary>
	/// The property for balancing, which is used to store the property name and value for auto-balancing. It is in the format of "propertyName:propertyValue". For example, if the property name is "location" and the property value is "us-west", then the property for balancing will be "location:us-west". This property is used to determine which collectors are eligible for auto-balancing based on their custom properties. If a collector has a custom property that matches the property for balancing, then it will be considered for auto-balancing.
	/// </summary>
	[DataMember(Name = "propertyForBalancing")]
	public string PropertyForBalancing { get; set; } = string.Empty;

	/// <summary>
	/// The time at which the property for balancing was last updated, in epoch format. This property is used to determine when the property for balancing was last changed, so that the system can decide whether to trigger auto-balancing or not. If the property for balancing was updated recently, then the system may delay auto-balancing to avoid unnecessary movements of collectors. If the property for balancing was updated a long time ago, then the system may trigger auto-balancing if there are any collectors that match the property for balancing and are eligible for auto-balancing.
	/// </summary>
	[DataMember(Name = "propertyForBalancingLastUpdatedOn")]
	public long PropertyForBalancingLastUpdatedOn { get; set; }

	/// <summary>
	/// The time up to which the property for balancing update is locked, in epoch format. This property is used to prevent multiple updates to the property for balancing within a short period of time, which can cause instability in the system. When the property for balancing is updated, the system will set this property to a future time (e.g., current time + 30 minutes) to indicate that any further updates to the property for balancing should be ignored until this time has passed. This allows the system to stabilize after an update to the property for balancing before allowing any more changes.
	/// </summary>
	[DataMember(Name = "propertyForBalancingUpdateLockedUptoMS")]
	public long PropertyForBalancingUpdateLockedUpToMilliseconds { get; set; }

	/// <summary>
	///    The subUrl for setting by id
	/// </summary>
	public string Endpoint() => "setting/collector/groups";
}
