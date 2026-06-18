namespace LogicMonitor.Api.Resources.Uptime;

/// <summary>
/// The ping-specific surface used by the wire mapper.
/// </summary>
internal interface IPingCheckDefinition : IUptimeCheckDefinition
{
	/// <summary>
	/// The number of ping packets to send (1-50).
	/// </summary>
	int PacketCount { get; set; }

	/// <summary>
	/// The per-packet timeout in milliseconds.
	/// </summary>
	int TimeoutMs { get; set; }

	/// <summary>
	/// The threshold percentage of packets that may fail to return in time before the check is considered failed.
	/// </summary>
	int PercentPacketsNotReceivedInTime { get; set; }
}
