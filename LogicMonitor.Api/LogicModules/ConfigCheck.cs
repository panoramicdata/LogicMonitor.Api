namespace LogicMonitor.Api.LogicModules;

/// <summary>
/// A config check
/// </summary>
[DataContract]
public class ConfigCheck
{
	/// <summary>
	/// The count that the alert must exist for these many poll cycles before it will be triggered
	/// </summary>
	[DataMember(Name = "alertTransitionInterval")]
	public int AlertTransitionInterval { get; set; }

	/// <summary>
	/// The ConfigCheck type. fetch|ignore|missing|value|groovy
	/// </summary>
	[DataMember(Name = "type")]
	public string Type { get; set; } = string.Empty;

	/// <summary>
	/// The ConfigCheck id
	/// </summary>
	[DataMember(Name = "id")]
	public int Id { get; set; }

	/// <summary>
	/// The ConfigCheck name
	/// </summary>
	[DataMember(Name = "name")]
	public string Name { get; set; } = string.Empty;

	/// <summary>
	/// The ConfigCheck id
	/// </summary>
	[DataMember(Name = "description")]
	public string Description { get; set; } = string.Empty;

	/// <summary>
	/// The triggered alert level if config check failure. 1-4 (1: no alert, 2: warn alert, 3: error alert, 4: critical alert)
	/// </summary>
	[DataMember(Name = "alertLevel")]
	public int AlertLevel { get; set; }

	/// <summary>
	/// Clear alert after ACKED or not
	/// </summary>
	[DataMember(Name = "ackClearAlert")]
	public bool AckClearAlert { get; set; }

	/// <summary>
	/// Alert effective interval
	/// </summary>
	[DataMember(Name = "alertEffectiveIval")]
	public int AlertEffectiveIval { get; set; }

	/// <summary>
	/// The ConfigSource id
	/// </summary>
	[DataMember(Name = "configSourceId")]
	public int ConfigSourceId { get; set; }

	/// <summary>
	/// portable id for origin tracking
	/// </summary>
	[DataMember(Name = "originId")]
	public string OriginId { get; set; } = string.Empty;

	/// <summary>
	/// The ConfigCheck script
	/// </summary>
	[DataMember(Name = "script")]
	public ConfigCheckScript Script { get; set; } = new();
}
