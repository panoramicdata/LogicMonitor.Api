namespace LogicMonitor.Api.Resources;

/// <summary>
/// Netflow device info
/// </summary>

[DataContract]
public class NetflowResourceInfo : IdentifiedItem
{
	/// <summary>
	/// deleted
	/// </summary>
	[DataMember(Name = "deleted")]
	public bool Deleted { get; set; }

	/// <summary>
	/// displayName
	/// </summary>
	[DataMember(Name = "displayName")]
	public string DisplayName { get; set; } = string.Empty;
}
