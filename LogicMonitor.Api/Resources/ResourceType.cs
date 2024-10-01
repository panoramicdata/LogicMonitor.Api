namespace LogicMonitor.Api.Resources;

/// <summary>
/// A device type
/// </summary>
[DataContract]
public enum ResourceType
{
	/// <summary>
	/// Regular device
	/// </summary>
	Regular = 0,

	/// <summary>
	/// Aws
	/// </summary>
	Aws = 2,

	/// <summary>
	/// Azure
	/// </summary>
	Azure = 4,

	/// <summary>
	/// Service
	/// </summary>
	Service = 6
}
