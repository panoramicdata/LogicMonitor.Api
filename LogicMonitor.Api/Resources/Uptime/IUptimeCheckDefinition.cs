namespace LogicMonitor.Api.Resources.Uptime;

/// <summary>
/// The common, strongly-typed surface shared by an Uptime resource and its creation DTO.
/// The wire mapper reads from this interface so the create and round-trip paths share one definition.
/// </summary>
internal interface IUptimeCheckDefinition
{
	string Name { get; set; }

	string DisplayName { get; set; }

	string Description { get; set; }

	/// <summary>
	/// The host name or IP address being checked (the <c>uptime.hostname</c> property).
	/// </summary>
	string HostName { get; set; }

	/// <summary>
	/// The comma-separated resource group ids (the <c>hostGroupIds</c> field).
	/// </summary>
	string ResourceGroupIds { get; set; }

	int PreferredCollectorId { get; set; }

	List<int> SyntheticsCollectorIds { get; set; }

	/// <summary>
	/// The polling interval in minutes (1-10).
	/// </summary>
	int PollingIntervalMinutes { get; set; }

	bool IsInternal { get; set; }

	bool DisableAlerting { get; set; }

	UptimeTestLocation TestLocation { get; set; }

	bool UseDefaultLocationSetting { get; set; }

	bool UseDefaultAlertSetting { get; set; }

	UptimeAlertSettings Alerting { get; set; }

	/// <summary>
	/// The device type discriminator (Ping = 19, Web = 18).
	/// </summary>
	ResourceType ResourceType { get; }
}

