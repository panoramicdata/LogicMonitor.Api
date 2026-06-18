namespace LogicMonitor.Api.Resources.Uptime;

/// <summary>
/// The internal wire representation of the <c>website.private.serviceParameters</c> JSON blob used to
/// configure an Uptime ping check. This is an implementation detail of the wire mapping and is not part
/// of the public, strongly-typed surface (which is exposed via <see cref="PingCheckResource"/>).
///
/// All values are strings because that is the shape LogicMonitor expects inside this nested JSON document.
/// </summary>
internal sealed class UptimePingServiceParameters
{
	[JsonProperty("percentPktsNotReceiveInTime")]
	public string PercentPktsNotReceiveInTime { get; set; } = string.Empty;

	[JsonProperty("testLocation")]
	public string TestLocation { get; set; } = string.Empty;

	[JsonProperty("overallAlertLevel")]
	public string OverallAlertLevel { get; set; } = string.Empty;

	[JsonProperty("pollingInterval")]
	public string PollingInterval { get; set; } = string.Empty;

	[JsonProperty("dns")]
	public string Dns { get; set; } = string.Empty;

	[JsonProperty("count")]
	public string Count { get; set; } = string.Empty;

	[JsonProperty("individualSmAlertEnable")]
	public string IndividualSmAlertEnable { get; set; } = string.Empty;

	[JsonProperty("timeoutInMSPktsNotReceive")]
	public string TimeoutInMSPktsNotReceive { get; set; } = string.Empty;

	[JsonProperty("transition")]
	public string Transition { get; set; } = string.Empty;

	[JsonProperty("globalSmAlertCond")]
	public string GlobalSmAlertCond { get; set; } = string.Empty;

	[JsonProperty("isInternal")]
	public string IsInternal { get; set; } = string.Empty;

	[JsonProperty("individualAlertLevel")]
	public string IndividualAlertLevel { get; set; } = string.Empty;
}

