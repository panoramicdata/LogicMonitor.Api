using LogicMonitor.Api.Resources.Uptime.Serialization;

namespace LogicMonitor.Api.Resources.Uptime;

/// <summary>
/// A strongly-typed LogicMonitor Uptime check (an internal/external ping or web "device").
///
/// All Uptime checks live at the <c>device/devices</c> endpoint and encode their configuration in a mix
/// of top-level device fields and custom properties. This type, together with
/// <see cref="UptimeResourceJsonConverter"/>, hides that wire detail behind a clean typed surface and
/// works directly with the generic client methods (<c>GetAsync</c>, <c>PutAsync</c>, <c>DeleteAsync</c>).
/// </summary>
/// <remarks>
/// See the LogicMonitor documentation:
/// <list type="bullet">
/// <item><description><see href="https://www.logicmonitor.com/support/adding-uptime-devices">Adding Uptime Devices</see></description></item>
/// <item><description><see href="https://www.logicmonitor.com/support/internal-ping-check-using-lm-uptime">Internal Ping Check using LM Uptime</see></description></item>
/// </list>
/// </remarks>
[JsonConverter(typeof(UptimeResourceJsonConverter))]
public abstract class UptimeResource : IdentifiedItem, IHasEndpoint, IHasName, IUptimeCheckDefinition
{
	/// <inheritdoc />
	public string Name { get; set; } = string.Empty;

	/// <summary>
	/// The display name of the check.
	/// </summary>
	public string DisplayName { get; set; } = string.Empty;

	/// <summary>
	/// The description of the check.
	/// </summary>
	public string Description { get; set; } = string.Empty;

	/// <inheritdoc />
	public string HostName { get; set; } = string.Empty;

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

	/// <summary>
	/// The device type discriminator for this check (Ping = 19, Web = 18).
	/// </summary>
	public abstract ResourceType ResourceType { get; }

	/// <inheritdoc />
	public string Endpoint() => "device/devices";
}

