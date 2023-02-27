namespace LogicMonitor.Api.Reports;

/// <summary>
/// Chain
/// </summary>

[DataContract]
public class Chain
{
	/// <summary>
	/// period
	/// </summary>
	[DataMember(Name = "period")]
	public Period? Period { get; set; }

	/// <summary>
	/// stages
	/// </summary>
	[DataMember(Name = "stages")]
	public List<Recipient> Stages { get; set; } = null!;

	/// <summary>
	/// period
	/// </summary>
	[DataMember(Name = "type")]
	public string Type { get; set; } = null!;
}
