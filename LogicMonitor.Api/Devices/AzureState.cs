namespace LogicMonitor.Api.Devices;

/// <summary>
/// Azure State
/// </summary>
[DataContract]
public enum AzureState
{
	/// <summary>
	/// Unknown
	/// </summary>
	Unknown = 0,

	/// <summary>
	/// Running
	/// </summary>
	Running = 1,

	/// <summary>
	/// Stopped
	/// </summary>
	Stopped = 2,

	/// <summary>
	/// Terminated
	/// </summary>
	Terminated = 3,
}
