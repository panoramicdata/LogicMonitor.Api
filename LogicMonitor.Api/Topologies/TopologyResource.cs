namespace LogicMonitor.Api.Topologies;

/// <summary>
/// Topology Data
/// </summary>
[DataContract]
public class TopologyResource : IdentifiedItem
{
	/// <summary>
	/// Resource Type
	/// </summary>
	[DataMember(Name = "type")]
	public TopologyResourceType Type { get; set; }

	/// <summary>
	/// name
	/// </summary>
	[DataMember(Name = "name")]
	public string Name { get; set; } = string.Empty;

	/// <summary>
	/// status
	/// </summary>
	[DataMember(Name = "status")]
	public string Status { get; set; } = string.Empty;
}
