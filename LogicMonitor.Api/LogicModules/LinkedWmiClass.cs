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
	[DataMember(Name = "anchorClassWMIProperty")]
	public string AnchorClassWMIProperty { get; set; } 

	/// <summary>
	/// linkedWmiClass
	/// </summary>
	[DataMember(Name = "linkedWmiClass")]
	public string LinkedWmi { get; set; } 

	/// <summary>
	/// match
	/// </summary>
	[DataMember(Name = "match")]
	public PropertyMatchRule? Match { get; set; }

	/// <summary>
	/// myLinkWMIProperty
	/// </summary>
	[DataMember(Name = "myLinkWMIProperty")]
	public string? MyLinkWMIProperty { get; set; }

	/// <summary>
	/// ILP
	/// </summary>
	[DataMember(Name = "ILP")]
	public InstanceLevelProperty? ILP { get; set; }
}
