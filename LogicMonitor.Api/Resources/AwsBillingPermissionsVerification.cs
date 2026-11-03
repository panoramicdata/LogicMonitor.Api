namespace LogicMonitor.Api.Resources;

/// <summary>
/// The result of verifying AWS billing permissions
/// </summary>
[DataContract]
public class AwsBillingPermissionsVerification
{
	/// <summary>
	/// The is valid
	/// </summary>
	[DataMember(Name = "isValid")]
	public bool IsValid { get; set; }

	/// <summary>
	/// The non permission errors
	/// </summary>
	[DataMember(Name = "nonPermissionErrors")]
	public List<string> NonPermissionErrors { get; set; } = [];

	/// <summary>
	/// The opt in warnings
	/// </summary>
	[DataMember(Name = "optInWarnings")]
	public List<string> OptInWarnings { get; set; } = [];

	/// <summary>
	/// The permission errors
	/// </summary>
	[DataMember(Name = "permissionErrors")]
	public List<string> PermissionErrors { get; set; } = [];
}
