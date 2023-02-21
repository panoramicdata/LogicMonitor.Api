namespace LogicMonitor.Api.Settings;

/// <summary>
/// IntegrationAuditLogPaginationResponse
/// </summary>

[DataContract]
public class IntegrationAuditLogPaginationResponse
{
	/// <summary>
	/// Items
	/// </summary>
	[DataMember(Name = "items", IsRequired = false)]
	public IntegrationAuditLog[]? Items { get; set; }
}
