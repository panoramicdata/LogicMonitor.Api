namespace LogicMonitor.Api.Settings;

/// <summary>
/// A request to create an action chain
/// </summary>
[DataContract]
public class ActionChainCreationDto : CreationDto<ActionChain>
{
	/// <summary>
	/// The action chain description
	/// </summary>
	[DataMember(Name = "description")]
	public string Description { get; set; } = string.Empty;

	/// <summary>
	/// The action chain name
	/// </summary>
	[DataMember(Name = "name")]
	public string Name { get; set; } = string.Empty;

	/// <summary>
	/// The list of stages in the action chain
	/// </summary>
	[DataMember(Name = "stages")]
	public List<ActionChainStage> Stages { get; set; } = [];
}
