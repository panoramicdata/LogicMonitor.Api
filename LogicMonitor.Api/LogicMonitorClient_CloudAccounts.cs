namespace LogicMonitor.Api;

/// <summary>
///     Cloud account credential tests, used before adding an AWS, Azure, GCP or SaaS resource group
/// </summary>
public partial class LogicMonitorClient
{
	/// <summary>
	///     Get the LogicMonitor AWS account ID to trust in an IAM role
	/// </summary>
	/// <param name="cancellationToken">The cancellation token</param>
	public Task<AwsAccountId> GetAwsAccountIdAsync(CancellationToken cancellationToken)
		=> GetBySubUrlAsync<AwsAccountId>("aws/accountId", cancellationToken);

	/// <summary>
	///     Test AWS account credentials
	/// </summary>
	/// <param name="request">The credentials and services to check</param>
	/// <param name="cancellationToken">The cancellation token</param>
	/// <returns>The permissions found, keyed by service</returns>
	public Task<JObject> TestAwsAccountAsync(AwsAccountTestRequest request, CancellationToken cancellationToken)
		=> PostAsync<AwsAccountTestRequest, JObject>(request, "aws/functions/testAccount", cancellationToken);

	/// <summary>
	///     Verify that AWS credentials can read the billing bucket
	/// </summary>
	/// <param name="request">The credentials and billing bucket</param>
	/// <param name="cancellationToken">The cancellation token</param>
	public Task<AwsBillingPermissionsVerification> VerifyAwsBillingPermissionsAsync(AwsAccountTestRequest request, CancellationToken cancellationToken)
		=> PostAsync<AwsAccountTestRequest, AwsBillingPermissionsVerification>(request, "aws/functions/verifyBillingPermissions", cancellationToken);

	/// <summary>
	///     Test Azure account credentials
	/// </summary>
	/// <param name="request">The credentials and services to check</param>
	/// <param name="cancellationToken">The cancellation token</param>
	/// <returns>The permissions found, keyed by service</returns>
	public Task<JObject> TestAzureAccountAsync(AzureAccountTestRequest request, CancellationToken cancellationToken)
		=> PostAsync<AzureAccountTestRequest, JObject>(request, "azure/functions/testAccount", cancellationToken);

	/// <summary>
	///     List the Azure subscriptions that credentials can see
	/// </summary>
	/// <param name="request">The credentials</param>
	/// <param name="cancellationToken">The cancellation token</param>
	public Task<Page<AzureSubscription>> DiscoverAzureSubscriptionsAsync(AzureSubscriptionsDiscoveryRequest request, CancellationToken cancellationToken)
		=> PostAsync<AzureSubscriptionsDiscoveryRequest, Page<AzureSubscription>>(request, "azure/functions/discoverSubscriptions", cancellationToken);

	/// <summary>
	///     Verify that Azure credentials can read a billing export storage account
	/// </summary>
	/// <param name="request">The credentials and storage account</param>
	/// <param name="cancellationToken">The cancellation token</param>
	public Task<JObject> VerifyAzureStorageAccountPermissionsAsync(AzureStorageAccountVerificationRequest request, CancellationToken cancellationToken)
		=> PostAsync<AzureStorageAccountVerificationRequest, JObject>(request, "azure/functions/verifyStorageAccountsPermissions", cancellationToken);

	/// <summary>
	///     Test GCP account credentials
	/// </summary>
	/// <param name="request">The service account key and services to check</param>
	/// <param name="cancellationToken">The cancellation token</param>
	/// <returns>The permissions found, keyed by service</returns>
	public Task<JObject> TestGcpAccountAsync(GcpAccountTestRequest request, CancellationToken cancellationToken)
		=> PostAsync<GcpAccountTestRequest, JObject>(request, "gcp/functions/testAccount", cancellationToken);

	/// <summary>
	///     Test SaaS account credentials
	/// </summary>
	/// <param name="request">The SaaS service and credentials</param>
	/// <param name="cancellationToken">The cancellation token</param>
	/// <returns>The permissions found, keyed by service</returns>
	public Task<JObject> TestSaasAccountAsync(SaasAccountTestRequest request, CancellationToken cancellationToken)
		=> PostAsync<SaasAccountTestRequest, JObject>(request, "saas/functions/testAccount", cancellationToken);
}
