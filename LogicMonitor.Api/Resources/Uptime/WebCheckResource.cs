namespace LogicMonitor.Api.Resources.Uptime;

/// <summary>
/// A strongly-typed LogicMonitor Uptime web check.
/// </summary>
public class WebCheckResource : UptimeResource, IWebCheckDefinition, IHasCreationEndpoint
{
	/// <inheritdoc />
	public override ResourceType ResourceType => ResourceType.Web;

	/// <inheritdoc />
	public string CreationEndpoint() => "device/devices?type=uptimewebcheck";

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

