using System.Runtime.Serialization;

namespace LogicMonitor.Api.Devices;

/// <summary>
/// A device type
/// </summary>
[DataContract]
public enum DeviceType
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
