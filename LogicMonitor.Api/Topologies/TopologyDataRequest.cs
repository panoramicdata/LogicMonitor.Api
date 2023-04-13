namespace LogicMonitor.Api.Topologies;

/// <summary>
/// A Topology Data request
/// </summary>
public class TopologyDataRequest
{
	/// <summary>
	/// The resource id
	/// </summary>
	public int? ResourceId { get; set; }

	/// <summary>
	/// The algorithm
	/// </summary>
	public TopologyAlgorithm Algorithm { get; set; }

	/// <summary>
	/// Get the query string
	/// </summary>
	/// <returns></returns>
	public string GetQueryString()
	{
		// Get the attribute from the Algorithm
		var memberInfo = typeof(TopologyAlgorithm).GetField(Algorithm.ToString());
		var attribute = (EnumMemberAttribute)Attribute.GetCustomAttribute(memberInfo, typeof(EnumMemberAttribute));
		var algorithmString = attribute.Value;
		return $"resource=device.{ResourceId}&algorithm={algorithmString}";
	}
}
