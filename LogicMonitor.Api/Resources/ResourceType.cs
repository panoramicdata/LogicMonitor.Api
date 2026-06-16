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
	/// Platform as a Service
	/// </summary>
	PaaS = 8,

	/// <summary>
	/// Software as a Service
	/// </summary>
	SaaS = 10,

	/// <summary>
	/// Web Check (Internal or External)
	/// </summary>
	Web = 18,

	/// <summary>
	/// Ping Check (Internal or External)
	/// </summary>
	Ping = 19,
}
