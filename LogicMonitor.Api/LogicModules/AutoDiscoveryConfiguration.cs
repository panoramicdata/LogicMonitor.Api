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
	[DataMember(Name = "persistentInstance")]
	public bool PersistentInstance { get; set; }

	/// <summary>
	/// disable discovered instance
	/// </summary>
	[DataMember(Name = "disableInstance")]
	public bool DisableInstance { get; set; }

	/// <summary>
	/// delete inactive instance
	/// </summary>
	[DataMember(Name = "deleteInactiveInstance")]
	public bool DeleteInactiveInstance { get; set; }

	/// <summary>
	/// auto group method
	/// </summary>
	[DataMember(Name = "instanceAutoGroupMethod")]
	public string InstanceAutoGroupMethod { get; set; } = string.Empty;

	/// <summary>
	/// auto group method\u0027s parameters
	/// </summary>
	[DataMember(Name = "instanceAutoGroupMethodParams")]
	public string InstanceAutoGroupMethodParams { get; set; } = string.Empty;

	/// <summary>
	/// auto discovery schedule interval in minutes, 0 means host or data source changed, values can be 0|15|60|1440
	/// </summary>
	[DataMember(Name = "scheduleInterval")]
	public int ScheduleIntervalSeconds { get; set; }

	/// <summary>
	/// method used to do auto discovery instance
	/// </summary>
	[DataMember(Name = "method")]
	public AutoDiscoveryMethod AutoDiscoveryMethod { get; set; } = new();

	/// <summary>
	/// The version
	/// </summary>
	[DataMember(Name = "filters")]
	public List<AutoDiscoveryFilter> AutoDiscoveryFilters { get; set; } = new();
}
