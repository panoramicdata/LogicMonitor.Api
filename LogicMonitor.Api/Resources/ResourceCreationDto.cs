namespace LogicMonitor.Api.Resources;

/// <summary>
///    A Resource Creation DTO
/// </summary>
[DataContract]
public class ResourceCreationDto : CreationDto<Resource>, IHasName, IHasDescription
{
	/// <summary>
	/// The ResourceGroup ids
	/// </summary>
	[DataMember(Name = "hostGroupIds")]
	public string ResourceGroupIds { get; set; } = string.Empty;

	/// <summary>
	///    The device name
	/// </summary>
	[DataMember(Name = "name")]
	public string Name { get; set; } = string.Empty;

	/// <summary>
	///    The display name
	/// </summary>
	[DataMember(Name = "displayName")]
	public string DisplayName { get; set; } = string.Empty;

	/// <summary>
	///    The device name
	/// </summary>
	[DataMember(Name = "description")]
	public string Description { get; set; } = string.Empty;

	/// <summary>
	///    The device link
	/// </summary>
	[DataMember(Name = "link")]
	public string Link { get; set; } = string.Empty;

	/// <summary>
	///    Whether to disable alerting
	/// </summary>
	[DataMember(Name = "disableAlerting")]
	public bool DisableAlerting { get; set; }

	/// <summary>
	///    Whether Netflow is enabled
	/// </summary>
	[DataMember(Name = "enableNetflow")]
	public bool EnableNetflow { get; set; }

	/// <summary>
	///    The Netflow Collector Id as a string
	/// </summary>
	[DataMember(Name = "netflowCollectorId")]
	public string NetflowCollectorId { get; set; } = string.Empty;

	/// <summary>
	///    The Preferred Collector's Id
	/// </summary>
	[DataMember(Name = "preferredCollectorId")]
	public int PreferredCollectorId { get; set; }

	/// <summary>
	///    The device type as an integer (e.g. 19 for Ping checks). Omitted when null, defaulting to 0 (Regular) on the server.
	/// </summary>
	[DataMember(Name = "deviceType", EmitDefaultValue = false)]
	public ResourceType? ResourceType { get; set; }

	/// <summary>
	///    The test location (collectors that perform the check). Required for ICMP resources.
	/// </summary>
	[DataMember(Name = "testLocation", EmitDefaultValue = false)]
	public WebsiteLocation? TestLocation { get; set; }

	/// <summary>
	///    The synthetics collector IDs (for internal ping/web checks)
	/// </summary>
	[DataMember(Name = "syntheticsCollectorIds", EmitDefaultValue = false)]
	public List<int>? SyntheticsCollectorIds { get; set; }

	/// <summary>
	///    Custom properties
	/// </summary>
	[DataMember(Name = "customProperties")]
	public List<EntityProperty> CustomProperties { get; set; } = [];

	/// <summary>
	///    ToString override
	/// </summary>
	public override string ToString() => !string.IsNullOrWhiteSpace(DisplayName) ? DisplayName : Name;
}
