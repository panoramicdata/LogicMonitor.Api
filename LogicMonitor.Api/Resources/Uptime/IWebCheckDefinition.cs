namespace LogicMonitor.Api.Resources.Uptime;

/// <summary>
/// The web-specific surface used by the wire mapper.
/// </summary>
internal interface IWebCheckDefinition : IUptimeCheckDefinition
{
	UptimeHttpScheme Scheme { get; set; }

	string Domain { get; set; }

	bool IgnoreSsl { get; set; }

	int PageLoadAlertTimeMs { get; set; }

	bool TriggerSslStatusAlerts { get; set; }

	bool TriggerSslExpirationAlerts { get; set; }

	string AlertExpression { get; set; }

	List<UptimeWebCheckStep> Steps { get; set; }
}
