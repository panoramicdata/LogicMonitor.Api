using System.Text.Json.Serialization;

namespace LogicMonitor.Api.Websites;

/// <summary>
/// Test location parameters for internal ping/web checks
/// </summary>
public class TestLocationParameters
{
	/// <summary>
	/// Use all collectors in the group
	/// </summary>
	[JsonPropertyName("all")]
	public bool All { get; set; }

	/// <summary>
	/// Collector IDs to use for the check
	/// </summary>
	[JsonPropertyName("collectorIds")]
	public int[] CollectorIds { get; set; } = [];

	/// <summary>
	/// Site Monitor Group IDs
	/// </summary>
	[JsonPropertyName("smgIds")]
	public int[] SmgIds { get; set; } = [];
}
