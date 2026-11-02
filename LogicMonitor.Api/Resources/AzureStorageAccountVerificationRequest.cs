namespace LogicMonitor.Api.Resources;

/// <summary>
/// A request to verify Azure storage account permissions
/// </summary>
[DataContract]
public class AzureStorageAccountVerificationRequest
{
	/// <summary>
	/// The china account
	/// </summary>
	[DataMember(Name = "chinaAccount")]
	public bool ChinaAccount { get; set; }

	/// <summary>
	/// The client id
	/// </summary>
	[DataMember(Name = "clientId")]
	public string ClientId { get; set; } = string.Empty;

	/// <summary>
	/// The export name
	/// </summary>
	[DataMember(Name = "exportName")]
	public string ExportName { get; set; } = string.Empty;

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
	/// The storage account container directory
	/// </summary>
	[DataMember(Name = "storageAccountContainerDirectory")]
	public string StorageAccountContainerDirectory { get; set; } = string.Empty;

	/// <summary>
	/// The storage account container name
	/// </summary>
	[DataMember(Name = "storageAccountContainerName")]
	public string StorageAccountContainerName { get; set; } = string.Empty;

	/// <summary>
	/// The storage account name
	/// </summary>
	[DataMember(Name = "storageAccountName")]
	public string StorageAccountName { get; set; } = string.Empty;

	/// <summary>
	/// The tenant id
	/// </summary>
	[DataMember(Name = "tenantId")]
	public string TenantId { get; set; } = string.Empty;
}
