namespace LogicMonitor.Api.Resources;

/// <summary>
/// A request to discover Azure subscriptions
/// </summary>
[DataContract]
public class AzureSubscriptionsDiscoveryRequest
{
	/// <summary>
	/// The client id
	/// </summary>
	[DataMember(Name = "clientId")]
	public string ClientId { get; set; } = string.Empty;

	/// <summary>
	/// The group id
	/// </summary>
	[DataMember(Name = "groupId")]
	public int GroupId { get; set; }

	/// <summary>
	/// The is china account
	/// </summary>
	[DataMember(Name = "isChinaAccount")]
	public bool IsChinaAccount { get; set; }

	/// <summary>
	/// The secret key
	/// </summary>
	[DataMember(Name = "secretKey")]
	public string SecretKey { get; set; } = string.Empty;

	/// <summary>
	/// The tenant id
	/// </summary>
	[DataMember(Name = "tenantId")]
	public string TenantId { get; set; } = string.Empty;
}
