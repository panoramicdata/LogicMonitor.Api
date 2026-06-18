using LogicMonitor.Api.Resources.Uptime.Serialization;

namespace LogicMonitor.Api.Resources.Uptime;

/// <summary>
/// The base creation DTO for an Uptime check. Exposes the same strongly-typed surface as
/// <see cref="UptimeResource"/>; the wire flattening is performed by
/// <see cref="UptimeResourceCreationDtoJsonConverter"/>.
/// </summary>
/// <typeparam name="T">The Uptime resource type created by this DTO.</typeparam>
[JsonConverter(typeof(UptimeResourceCreationDtoJsonConverter))]
public abstract class UptimeResourceCreationDto<T> : CreationDto<T>, IUptimeCheckDefinition where T : UptimeResource
{
	/// <summary>
	/// The name of the check. Required.
	/// </summary>
	public required string Name { get; set; }

	/// <summary>
	/// The display name of the check. Defaults to <see cref="Name"/> when left blank.
	/// </summary>
	public string DisplayName { get; set; } = string.Empty;

	/// <summary>
	/// The description of the check.
	/// </summary>
	public string Description { get; set; } = string.Empty;

	/// <summary>
	/// The host name or IP address (web checks may also use a domain) being checked. Required.
	/// </summary>
	public required string HostName { get; set; }

	/// <inheritdoc />
	public string ResourceGroupIds { get; set; } = string.Empty;

	/// <inheritdoc />
	public int PreferredCollectorId { get; set; }

	/// <inheritdoc />
	public List<int> SyntheticsCollectorIds { get; set; } = [];

	/// <inheritdoc />
	public int PollingIntervalMinutes { get; set; } = 5;

	/// <inheritdoc />
	public bool IsInternal { get; set; } = true;

	/// <inheritdoc />
	public bool DisableAlerting { get; set; }

	/// <inheritdoc />
	public UptimeTestLocation TestLocation { get; set; } = new();

	/// <inheritdoc />
	public bool UseDefaultLocationSetting { get; set; }

	/// <inheritdoc />
	public bool UseDefaultAlertSetting { get; set; }

	/// <inheritdoc />
	public UptimeAlertSettings Alerting { get; set; } = new();

	/// <inheritdoc />
	public abstract ResourceType ResourceType { get; }
}

