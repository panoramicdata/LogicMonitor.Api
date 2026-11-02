namespace LogicMonitor.Api.Settings;

/// <summary>
/// A request to create an action rule
/// </summary>
[DataContract]
public class ActionRuleCreationDto : CreationDto<ActionRule>
{
	/// <summary>
	/// The action chain id for the action rule
	/// </summary>
	[DataMember(Name = "actionChainId")]
	public int ActionChainId { get; set; }

	/// <summary>
	/// The datapoint for which the action rule is configured to match
	/// </summary>
	[DataMember(Name = "datapoint")]
	public string Datapoint { get; set; } = string.Empty;

	/// <summary>
	/// The datasource for which the action rule is configured to match
	/// </summary>
	[DataMember(Name = "datasource")]
	public string Datasource { get; set; } = string.Empty;

	/// <summary>
	/// The device groups for which the action rule is configured to match
	/// </summary>
	[DataMember(Name = "deviceGroups")]
	public List<string> DeviceGroups { get; set; } = [];

	/// <summary>
	/// The devices for which the action rule is configured to match
	/// </summary>
	[DataMember(Name = "devices")]
	public List<string> Devices { get; set; } = [];

	/// <summary>
	/// Whether action Rule is enabled or not
	/// </summary>
	[DataMember(Name = "enabled")]
	public bool Enabled { get; set; }

	/// <summary>
	/// The instance for which the action rule is configured to match
	/// </summary>
	[DataMember(Name = "instance")]
	public string Instance { get; set; } = string.Empty;

	/// <summary>
	/// The severity levels for which the action rule is configured to match.
	/// </summary>
	[DataMember(Name = "levelStr")]
	public string LevelStr { get; set; } = string.Empty;

	/// <summary>
	/// The name of the action rule
	/// </summary>
	[DataMember(Name = "name")]
	public string Name { get; set; } = string.Empty;

	/// <summary>
	/// The resource property filters list
	/// </summary>
	[DataMember(Name = "resourceProperties")]
	public List<ActionRuleResourceProperty> ResourceProperties { get; set; } = [];
}
