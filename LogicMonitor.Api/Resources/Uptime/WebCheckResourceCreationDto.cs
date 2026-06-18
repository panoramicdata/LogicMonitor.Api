namespace LogicMonitor.Api.Resources.Uptime;

/// <summary>
/// Creation DTO for an Uptime web check.
/// </summary>
public class WebCheckResourceCreationDto : UptimeResourceCreationDto<WebCheckResource>, IWebCheckDefinition
{
	/// <inheritdoc />
	public override ResourceType ResourceType => ResourceType.Web;

	/// <inheritdoc />
	public UptimeHttpScheme Scheme { get; set; } = UptimeHttpScheme.Https;

	/// <inheritdoc />
	public string Domain { get; set; } = string.Empty;

	/// <inheritdoc />
	public bool IgnoreSsl { get; set; }

	/// <inheritdoc />
	public int PageLoadAlertTimeMs { get; set; } = 30000;

	/// <inheritdoc />
	public bool TriggerSslStatusAlerts { get; set; }

	/// <inheritdoc />
	public bool TriggerSslExpirationAlerts { get; set; }

	/// <inheritdoc />
	public string AlertExpression { get; set; } = string.Empty;

	/// <inheritdoc />
	public List<UptimeWebCheckStep> Steps { get; set; } = [];
}

