namespace LogicMonitor.Api.Settings;

/// <summary>
/// A stage in an action chain
/// </summary>
[DataContract]
public class ActionChainStage
{
	/// <summary>
	/// The source ID
	/// </summary>
	[DataMember(Name = "id")]
	public int Id { get; set; }

	/// <summary>
	/// The action source name
	/// </summary>
	[SantabaReadOnly]
	[DataMember(Name = "name")]
	public string Name { get; set; } = string.Empty;

	/// <summary>
	/// The type of the action source
	/// </summary>
	[DataMember(Name = "type")]
	public string Type { get; set; } = string.Empty;
}
