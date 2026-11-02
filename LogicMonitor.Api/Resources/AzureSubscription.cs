namespace LogicMonitor.Api.Resources;

/// <summary>
/// An Azure subscription
/// </summary>
[DataContract]
public class AzureSubscription
{
	/// <summary>
	/// The id
	/// </summary>
	[DataMember(Name = "id")]
	public string Id { get; set; } = string.Empty;

	/// <summary>
	/// The authorization source
	/// </summary>
	[DataMember(Name = "authorizationSource")]
	public string AuthorizationSource { get; set; } = string.Empty;

	/// <summary>
	/// The location placement id
	/// </summary>
	[DataMember(Name = "locationPlacementId")]
	public string LocationPlacementId { get; set; } = string.Empty;

	/// <summary>
	/// The name
	/// </summary>
	[DataMember(Name = "name")]
	public string Name { get; set; } = string.Empty;

	/// <summary>
	/// The quota id
	/// </summary>
	[DataMember(Name = "quotaId")]
	public string QuotaId { get; set; } = string.Empty;

	/// <summary>
	/// The spending limit
	/// </summary>
	[DataMember(Name = "spendingLimit")]
	public string SpendingLimit { get; set; } = string.Empty;

	/// <summary>
	/// The state
	/// </summary>
	[DataMember(Name = "state")]
	public string State { get; set; } = string.Empty;

	/// <summary>
	/// The tenant id
	/// </summary>
	[DataMember(Name = "tenantId")]
	public string TenantId { get; set; } = string.Empty;
}
