using System.Collections.Generic;
using System.Runtime.Serialization;

namespace LogicMonitor.Api.Topologies;

/// <summary>
/// A Topology View
/// </summary>
[DataContract]
public class TopologyView
{
	/// <summary>
	///    The vertices
	/// </summary>
	[DataMember(Name = "vertices")]
	public List<string> Vertices { get; set; }

	/// <summary>
	///    The edge types
	/// </summary>
	[DataMember(Name = "edgeTypes")]
	public List<object> EdgeTypes { get; set; }

	/// <summary>
	///    The algorithm
	/// </summary>
	[DataMember(Name = "algorithm")]
	public string Algorithm { get; set; }
}
