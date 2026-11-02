namespace LogicMonitor.Api.Settings;

/// <summary>
/// An action chain: an ordered list of diagnostic and remediation stages
/// </summary>
[DataContract]
public class ActionChain : NamedItem, IHasEndpoint, IPatchable
{
	/// <summary>
	/// The list of stages in the action chain
	/// </summary>
	[DataMember(Name = "stages")]
	public List<ActionChainStage> Stages { get; set; } = [];

	/// <inheritdoc />
	public string Endpoint() => "setting/action/chains";
}
