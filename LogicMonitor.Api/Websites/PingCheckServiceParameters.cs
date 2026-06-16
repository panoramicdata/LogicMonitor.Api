using System.Text.Json.Serialization;

namespace LogicMonitor.Api.Websites;

/// <summary>
/// Service parameters for internal ping checks
/// </summary>
public class PingCheckServiceParameters
{
	/// <summary>
	/// Threshold percentage of packets not received in time
	/// </summary>
	[JsonPropertyName("percentPktsNotReceiveInTime")]
	public string PercentPktsNotReceiveInTime { get; set; } = string.Empty;

	/// <summary>
	/// Test location configuration as JSON string
	/// </summary>
	[JsonPropertyName("testLocation")]
	public string TestLocation { get; set; } = string.Empty;

	/// <summary>
	/// Overall alert level (e.g., "critical", "warn", "error")
	/// </summary>
	[JsonPropertyName("overallAlertLevel")]
	public string OverallAlertLevel { get; set; } = string.Empty;

	/// <summary>
	/// Polling interval in minutes
	/// </summary>
	[JsonPropertyName("pollingInterval")]
	public string PollingInterval { get; set; } = string.Empty;

	/// <summary>
	/// DNS/hostname to ping
	/// </summary>
	[JsonPropertyName("dns")]
	public string Dns { get; set; } = string.Empty;

	/// <summary>
	/// Number of pings to send
	/// </summary>
	[JsonPropertyName("count")]
	public string Count { get; set; } = string.Empty;

	/// <summary>
	/// Enable individual service monitor alerts
	/// </summary>
	[JsonPropertyName("individualSmAlertEnable")]
	public string IndividualSmAlertEnable { get; set; } = string.Empty;

	/// <summary>
	/// Timeout in milliseconds for packets not received
	/// </summary>
	[JsonPropertyName("timeoutInMSPktsNotReceive")]
	public string TimeoutInMSPktsNotReceive { get; set; } = string.Empty;

	/// <summary>
	/// Number of failed polls before alerting
	/// </summary>
	[JsonPropertyName("transition")]
	public string Transition { get; set; } = string.Empty;

	/// <summary>
	/// Global service monitor alert condition
	/// </summary>
	[JsonPropertyName("globalSmAlertCond")]
	public string GlobalSmAlertCond { get; set; } = string.Empty;

	/// <summary>
	/// Whether this is an internal check
	/// </summary>
	[JsonPropertyName("isInternal")]
	public string IsInternal { get; set; } = string.Empty;

	/// <summary>
	/// Number of successful polls before clearing alert
	/// </summary>
	[JsonPropertyName("clearTransition")]
	public string ClearTransition { get; set; } = string.Empty;

	/// <summary>
	/// Individual alert level for checkpoints (e.g., "warn", "error")
	/// </summary>
	[JsonPropertyName("individualAlertLevel")]
	public string IndividualAlertLevel { get; set; } = string.Empty;
}
