namespace LogicMonitor.Api.Resources.Uptime;

/// <summary>
/// A strongly-typed LogicMonitor Uptime ping check.
/// </summary>
public class PingCheckResource : UptimeResource, IPingCheckDefinition
{
	/// <inheritdoc />
	public override ResourceType ResourceType => ResourceType.Ping;

	/// <inheritdoc />
	public int PacketCount { get; set; } = 5;

	/// <inheritdoc />
	public int TimeoutMs { get; set; } = 500;

	/// <inheritdoc />
	public int PercentPacketsNotReceivedInTime { get; set; } = 80;
}
