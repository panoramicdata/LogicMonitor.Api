namespace LogicMonitor.Api;

/// <summary>
///     Contract, usage, metrics summary and cloud cost optimization portal interaction
/// </summary>
/// <remarks>Cloud recommendations use the generic methods, for example GetAllAsync&lt;CloudRecommendation&gt;</remarks>
public partial class LogicMonitorClient
{
	/// <summary>
	///     Get the account's contract information, one entry per product
	/// </summary>
	/// <param name="cancellationToken">The cancellation token</param>
	public async Task<List<ContractInfo>> GetContractInfoAsync(CancellationToken cancellationToken)
		=> await SendRawJsonAsync<List<ContractInfo>>(HttpMethod.Get, "usage/contractInfo", null, cancellationToken).ConfigureAwait(false) ?? [];

	/// <summary>
	///     Get usage shaped as a FOCUS (FinOps Open Cost and Usage Specification) export
	/// </summary>
	/// <param name="query">The query string without the leading '?', for example "feature=...&amp;startTime=...&amp;endTime=..."</param>
	/// <param name="cancellationToken">The cancellation token</param>
	/// <returns>The response, which the specification leaves untyped</returns>
	public async Task<JToken?> GetFocusCostUsageAsync(string query, CancellationToken cancellationToken)
	{
		ArgumentNullException.ThrowIfNull(query);
		var responseBody = await SendRawAsync(
			HttpMethod.Get,
			$"usage/focusCostUsage{(query.Length == 0 ? string.Empty : "?" + query)}",
			null,
			"application/json",
			cancellationToken).ConfigureAwait(false);
		return string.IsNullOrWhiteSpace(responseBody) ? null : JToken.Parse(responseBody);
	}

	/// <summary>
	///     Get a summary of the portal's usage metrics
	/// </summary>
	/// <param name="cancellationToken">The cancellation token</param>
	public Task<CompanyMetricsSummary> GetMetricsSummaryAsync(CancellationToken cancellationToken)
		=> GetBySubUrlAsync<CompanyMetricsSummary>("metrics/summary", cancellationToken);

	/// <summary>
	///     Get the cloud cost optimization recommendation categories
	/// </summary>
	/// <param name="cancellationToken">The cancellation token</param>
	public Task<Page<CloudRecommendationCategory>> GetCloudRecommendationCategoriesAsync(CancellationToken cancellationToken)
		=> GetBySubUrlAsync<Page<CloudRecommendationCategory>>("cost-optimization/recommendations/categories", cancellationToken);
}
