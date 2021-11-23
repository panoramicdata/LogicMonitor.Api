using System.Runtime.Serialization;

namespace LogicMonitor.Api.Topologies;

/// <summary>
/// A Vertex
/// </summary>
[DataContract]
public class Vertex : StringIdentifiedItem
{
	/// <summary>
	///    X
	/// </summary>
	[DataMember(Name = "x")]
	public float X { get; set; }

	/// <summary>
	///    Y
	/// </summary>
	[DataMember(Name = "y")]
	public float Y { get; set; }
}
