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
	/// Aws IaaS
	/// </summary>
	Aws = 2,

	/// <summary>
	/// Azure IaaS
	/// </summary>
	Azure = 4,

	/// <summary>
	/// Service
	/// </summary>
	Service = 6,

	/// <summary>
	/// PaaS
	/// </summary>
	PaaS = 8,

	/// <summary>
	/// Service
	/// </summary>
	SaaS = 10
}
