namespace LogicMonitor.Api.Collectors;

/// <summary>
/// An automatic upgrade info
/// </summary>
[DataContract]
[JsonConverter(typeof(AutomaticUpgradeInfoConverter))]
public abstract class AutomaticUpgradeInfo
{
	/// <summary>
	/// The type
	/// </summary>
	[DataMember(Name = "type")]
	public string Type { get; set; }
}
