using System.Runtime.Serialization;

namespace LogicMonitor.Api.Data;

/// <summary>
/// Graph data scope
/// </summary>
[DataContract]
public class GraphDataScope
{
	/// <summary>
	///    The id
	/// </summary>
	[DataMember(Name = "id")]
	public int Id { get; set; }

	/// <summary>
	///    The type
	/// </summary>
	[DataMember(Name = "type")]
	public string Type { get; set; }
}
