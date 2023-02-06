namespace LogicMonitor.Api.LogicModules;

/// <summary>
/// An autodiscovery configuration
/// </summary>
[DataContract]
public class AutoDiscoveryConfiguration
{
	/// <summary>
	/// persist discovered instance
	/// </summary>
	[DataMember(Name = "persistentInstance", IsRequired = false)]
	public bool PersistentInstance { get; set; }

	/// <summary>
	/// disable discovered instance
	/// </summary>
	[DataMember(Name = "disableInstance", IsRequired = false)]
	public bool DisableInstance { get; set; }

	/// <summary>
	/// delete inactive instance
	/// </summary>
	[DataMember(Name = "deleteInactiveInstance", IsRequired = false)]
	public bool DeleteInactiveInstance { get; set; }

	/// <summary>
	/// auto group method
	/// </summary>
	[DataMember(Name = "instanceAutoGroupMethod", IsRequired = false)]
	public string? InstanceAutoGroupMethod { get; set; }

	/// <summary>
	/// auto group method\u0027s parameters
	/// </summary>
	[DataMember(Name = "instanceAutoGroupMethodParams", IsRequired = false)]
	public string? InstanceAutoGroupMethodParams { get; set; }

	/// <summary>
	/// auto discovery schedule interval in minutes, 0 means host or data source changed, values can be 0|15|60|1440
	/// </summary>
	[DataMember(Name = "scheduleInterval", IsRequired = false)]
	public int ScheduleIntervalSeconds { get; set; }

	/// <summary>
	/// method used to do auto discovery instance
	/// </summary>
	[DataMember(Name = "method", IsRequired = true)]
	public AutoDiscoveryMethod AutoDiscoveryMethod { get; set; } = null!;

	/// <summary>
	/// The version
	/// </summary>
	[DataMember(Name = "filters", IsRequired = false)]
	public List<AutoDiscoveryFilter>? AutoDiscoveryFilters { get; set; }
}
