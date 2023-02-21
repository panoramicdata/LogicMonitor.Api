namespace LogicMonitor.Api.LogicModules;

/// <summary>
/// LinkedWmiClass
/// </summary>

[DataContract]
public class LinkedWmiClass
{
	/// <summary>
	/// anchorClassWMIProperty
	/// </summary>
	[DataMember(Name = "anchorClassWMIProperty", IsRequired = true)]
	public string AnchorClassWMIProperty { get; set; } = null!;

	/// <summary>
	/// linkedWmiClass
	/// </summary>
	[DataMember(Name = "linkedWmiClass", IsRequired = true)]
	public string LinkedWmi { get; set; } = null!;

	/// <summary>
	/// match
	/// </summary>
	[DataMember(Name = "match", IsRequired = false)]
	public PropertyMatchRule? Match { get; set; }

	/// <summary>
	/// myLinkWMIProperty
	/// </summary>
	[DataMember(Name = "myLinkWMIProperty", IsRequired = false)]
	public string? MyLinkWMIProperty { get; set; }

	/// <summary>
	/// ILP
	/// </summary>
	[DataMember(Name = "ILP", IsRequired = false)]
	public InstanceLevelProperty? ILP { get; set; }
}
