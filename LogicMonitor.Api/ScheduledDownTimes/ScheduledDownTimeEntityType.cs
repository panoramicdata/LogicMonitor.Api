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
	Device = 1,

	/// <summary>
	/// DeviceGroup
	/// </summary>
	DeviceGroup = 2,

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
