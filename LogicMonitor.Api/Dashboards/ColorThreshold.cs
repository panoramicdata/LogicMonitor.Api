namespace LogicMonitor.Api.Dashboards;

/// <summary>
/// A color threshold
/// </summary>
[DataContract]
public class ColorThreshold
{
	/// <summary>
	/// The relation
	/// </summary>
	[DataMember(Name = "relation")]
	public string Relation { get; set; } = string.Empty;

	/// <summary>
	/// The level
	/// </summary>
	[DataMember(Name = "level")]
	public int Level { get; set; }

	/// <summary>
	/// The threshold
	/// </summary>
	[DataMember(Name = "threshold")]
	public string Threshold { get; set; } = string.Empty;
}
