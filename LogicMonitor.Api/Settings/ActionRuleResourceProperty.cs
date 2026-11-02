namespace LogicMonitor.Api.Settings;

/// <summary>
/// A resource property that an action rule matches
/// </summary>
[DataContract]
public class ActionRuleResourceProperty
{
	/// <summary>
	/// The resource property name
	/// </summary>
	[DataMember(Name = "name")]
	public string Name { get; set; } = string.Empty;

	/// <summary>
	/// The resource property value
	/// </summary>
	[DataMember(Name = "value")]
	public string Value { get; set; } = string.Empty;
}
