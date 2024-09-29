namespace LogicMonitor.Api.ScheduledDownTimes;

/// <summary>
/// A scheduled down time entity type
/// </summary>
public enum ScheduledDownTimeEntityType
{
	/// <summary>
	/// Unknown
	/// </summary>
	Unknown = 0,

	/// <summary>
	/// Device
	/// </summary>
	Resource = 1,

	/// <summary>
	/// Obsolete
	/// </summary>
	[JsonIgnore]
	[Obsolete("Use Resource instead", true)]
	Device = Resource,

	/// <summary>
	/// ResourceGroup
	/// </summary>
	ResourceGroup = 2,

	/// <summary>
	/// Obsolete
	/// </summary>
	[JsonIgnore]
	[Obsolete("Use ResourceGroup instead", true)]
	DeviceGroup = ResourceGroup,

	/// <summary>
	/// DataSourceInstance
	/// </summary>
	Instance = 3,

	/// <summary>
	/// DataSource
	/// </summary>
	DataSource = 4,

	/// <summary>
	/// DataSourceInstanceGroup
	/// </summary>
	InstanceGroup = 5,

	/// <summary>
	/// Collector
	/// </summary>
	Agent = 6,

	/// <summary>
	/// Website
	/// </summary>
	Website = 7,
}
