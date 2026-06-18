namespace LogicMonitor.Api.Resources.Uptime;

/// <summary>
/// Creation DTO for an Uptime ping check.
/// </summary>
public class PingCheckResourceCreationDto : UptimeResourceCreationDto<PingCheckResource>, IPingCheckDefinition
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
