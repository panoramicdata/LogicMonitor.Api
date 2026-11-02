namespace LogicMonitor.Api.Resources;

/// <summary>
/// A request to test SaaS account credentials
/// </summary>
[DataContract]
public class SaasAccountTestRequest
{
	/// <summary>
	/// The account id
	/// </summary>
	[DataMember(Name = "accountId")]
	public string AccountId { get; set; } = string.Empty;

	/// <summary>
	/// The account type
	/// </summary>
	[DataMember(Name = "accountType")]
	public string AccountType { get; set; } = string.Empty;

	/// <summary>
	/// The client id
	/// </summary>
	[DataMember(Name = "clientId")]
	public string ClientId { get; set; } = string.Empty;

	/// <summary>
	/// The private key
	/// </summary>
	[DataMember(Name = "privateKey")]
	public string PrivateKey { get; set; } = string.Empty;

	/// <summary>
	/// The secret key
	/// </summary>
	[DataMember(Name = "secretKey")]
	public string SecretKey { get; set; } = string.Empty;

	/// <summary>
	/// The service name
	/// </summary>
	[DataMember(Name = "serviceName")]
	public string ServiceName { get; set; } = string.Empty;

	/// <summary>
	/// The status type
	/// </summary>
	[DataMember(Name = "statusType")]
	public string StatusType { get; set; } = string.Empty;

	/// <summary>
	/// The status urls
	/// </summary>
	[DataMember(Name = "statusUrls")]
	public string StatusUrls { get; set; } = string.Empty;

	/// <summary>
	/// The tenant id
	/// </summary>
	[DataMember(Name = "tenantId")]
	public string TenantId { get; set; } = string.Empty;

	/// <summary>
	/// The user id
	/// </summary>
	[DataMember(Name = "userId")]
	public string UserId { get; set; } = string.Empty;
}
