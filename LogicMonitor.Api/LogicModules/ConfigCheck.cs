namespace LogicMonitor.Api.LogicModules;

/// <summary>
/// A config check
/// </summary>
[DataContract]
public class ConfigCheck
{
	/// <summary>
	/// The type
	/// </summary>
	[DataMember(Name = "alertTransitionInterval")]
	public string AlertTransitionInterval { get; set; }

	/// <summary>
	/// The type
	/// </summary>
	[DataMember(Name = "type")]
	public string Type { get; set; }

	/// <summary>
	/// The id
	/// </summary>
	[DataMember(Name = "id")]
	public int Id { get; set; }

	/// <summary>
	/// The name
	/// </summary>
	[DataMember(Name = "name")]
	public string Name { get; set; }

	/// <summary>
	/// The description
	/// </summary>
	[DataMember(Name = "description")]
	public string Description { get; set; }

	/// <summary>
	/// The alertLevel
	/// </summary>
	[DataMember(Name = "alertLevel")]
	public AlertLevel AlertLevel { get; set; }

	/// <summary>
	/// The ackClearAlert
	/// </summary>
	[DataMember(Name = "ackClearAlert")]
	public string AckClearAlert { get; set; }

	/// <summary>
	/// The alertEffectiveIval
	/// </summary>
	[DataMember(Name = "alertEffectiveIval")]
	public string AlertEffectiveIval { get; set; }

	/// <summary>
	/// The configSourceId
	/// </summary>
	[DataMember(Name = "configSourceId")]
	public int ConfigSourceId { get; set; }

	/// <summary>
	/// Origin id
	/// </summary>
	[DataMember(Name = "originId")]
	public string OriginId { get; set; }

	/// <summary>
	/// The script
	/// </summary>
	[DataMember(Name = "script")]
	public ConfigCheckScript Script { get; set; }
}
