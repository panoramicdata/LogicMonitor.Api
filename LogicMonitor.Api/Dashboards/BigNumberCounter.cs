namespace LogicMonitor.Api.Dashboards;

/// <summary>
/// Big Number counter
/// </summary>
[DataContract]
public class BigNumberCounter
{
	/// <summary>
	/// The name
	/// </summary>
	[DataMember(Name = "name")]
	public string Name { get; set; } = string.Empty;

	/// <summary>
	/// The appliesTo
	/// </summary>
	[DataMember(Name = "appliesTo")]
	public string AppliesTo { get; set; } = string.Empty;
}
