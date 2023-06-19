namespace LogicMonitor.Api.Topologies;

/// <summary>
/// The edge type
/// </summary>
[DataContract]
public class EdgeTypeAndDirection
{
	/// <summary>
	/// The type
	/// </summary>
	[DataMember(Name = "type")]
	public string Type { get; set; } = string.Empty;

	/// <summary>
	/// The type
	/// </summary>
	[DataMember(Name = "direction")]
	public EdgeDirection Direction { get; set; }
}