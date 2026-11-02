namespace LogicMonitor.Api.Settings;

/// <summary>
/// A summary of an action chain
/// </summary>
[DataContract]
public class ActionChainSummary
{
	/// <summary>
	/// The id
	/// </summary>
	[DataMember(Name = "id")]
	public int Id { get; set; }

	/// <summary>
	/// The name
	/// </summary>
	[DataMember(Name = "name")]
	public string Name { get; set; } = string.Empty;
}
