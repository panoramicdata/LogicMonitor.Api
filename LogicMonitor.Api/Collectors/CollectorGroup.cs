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
	public List<Property> CustomProperties { get; set; }

	/// <summary>
	/// The permission level of the user that made the API request
	/// </summary>
	[DataMember(Name = "userPermission")]
	public string UserPermission { get; set; }

	/// <summary>
	///    The number of seconds after 1970-01-01 that the collector group was created.
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
	///    Autobalance strategy
	/// </summary>
	[DataMember(Name = "autoBalanceStrategy")]
	public AutoBalanceStrategy AutoBalanceStrategy { get; set; }

	/// <summary>
	///    Autobalance device count threshold
	/// </summary>
	[DataMember(Name = "autoBalanceDeviceCountThreshold")]
	public int AutoBalanceDeviceCountThrehsold { get; set; }

	/// <summary>
	/// threshold for instance count strategy to check if a collector has high load
	/// </summary>
	[DataMember(Name = "autoBalanceInstanceCountThreshold")]
	public int AutoBalanceInstanceCountThrehsold { get; set; }

	/// <summary>
	///    Whether the version mismatches
	/// </summary>
	[DataMember(Name = "mismatchVerison")]
	public bool MismatchVerison { get; set; }

	/// <summary>
	/// the platform limitation
	/// </summary>
	[DataMember(Name = "platform")]
	public string Platform { get; set; }

	/// <summary>
	///    The instance count
	/// </summary>
	[DataMember(Name = "numOfInstances")]
	public int InstanceCount { get; set; }

	/// <summary>
	/// The number of hosts that belong to the group
	/// </summary>
	[DataMember(Name = "numOfHosts")]
	public long DeviceCount { get; set; }

	/// <summary>
	///    Whether versions mismatch
	/// </summary>
	[DataMember(Name = "mismatchVersion")]
	public bool MismatchVersion { get; set; }

	/// <summary>
	/// The status of the highest priority sub collector
	/// </summary>
	[DataMember(Name = "highestPriorityCollectorStatus")]
	public RestHighestPriorityCollectorStatus HighestPriorityCollectorStatus { get; set; }

	/// <summary>
	///    The subUrl for setting by id
	/// </summary>
	public string Endpoint() => "setting/collector/groups";
}
