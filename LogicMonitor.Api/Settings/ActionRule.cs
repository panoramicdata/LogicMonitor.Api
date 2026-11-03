namespace LogicMonitor.Api.Settings;

/// <summary>
/// An action rule: which alerts trigger an action chain
/// </summary>
[DataContract]
public class ActionRule : NamedItem, IHasEndpoint, IPatchable
{
	/// <summary>
	/// The action chain summary for the action rule
	/// </summary>
	[DataMember(Name = "actionChain")]
	public ActionChainSummary? ActionChain { get; set; }

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
	/// The resource property filters list
	/// </summary>
	[DataMember(Name = "resourceProperties")]
	public List<ActionRuleResourceProperty> ResourceProperties { get; set; } = [];

	/// <inheritdoc />
	public string Endpoint() => "setting/action/rules";
}
