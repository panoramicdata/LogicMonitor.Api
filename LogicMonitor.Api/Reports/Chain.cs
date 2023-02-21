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
	[DataMember(Name = "period", IsRequired = false)]
	public Period? Period { get; set; }

	/// <summary>
	/// stages
	/// </summary>
	[DataMember(Name = "stages", IsRequired = true)]
	public List<Recipient> Stages { get; set; } = null!;

	/// <summary>
	/// period
	/// </summary>
	[DataMember(Name = "type", IsRequired = true)]
	public string Type { get; set; } = null!;
}
