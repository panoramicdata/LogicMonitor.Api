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
	public string AnchorClassWMIProperty { get; set; } = string.Empty;

	/// <summary>
	/// linkedWmiClass
	/// </summary>
	[DataMember(Name = "linkedWmiClass")]
	public string LinkedWmi { get; set; } = string.Empty;

	/// <summary>
	/// match
	/// </summary>
	[DataMember(Name = "match")]
	public PropertyMatchRule Match { get; set; } = new();

	/// <summary>
	/// myLinkWMIProperty
	/// </summary>
	[DataMember(Name = "myLinkWMIProperty")]
	public string MyLinkWMIProperty { get; set; } = string.Empty;

	/// <summary>
	/// ILP
	/// </summary>
	[DataMember(Name = "ILP")]
	public InstanceLevelProperty ILP { get; set; } = new();
}
