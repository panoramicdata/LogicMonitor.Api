namespace LogicMonitor.Api;

/// <summary>
///     Push Metrics
/// </summary>
public partial class LogicMonitorClient
{
	/// <summary>
	///     Pushes a metric direct to the Time series database
	/// </summary>
	/// <param name="pushMetric">A push metric</param>
	/// <param name="cancellationToken"></param>
	/// <returns>The response</returns>
	public Task<PushMetricResponse> PushMetricAsync(
		PushMetric pushMetric,
		CancellationToken cancellationToken = default)
		=> PostAsync<PushMetric, PushMetricResponse>(pushMetric, "metric/ingest", cancellationToken);
}
