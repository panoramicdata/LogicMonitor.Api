namespace LogicMonitor.Api.LogicModules;

/// <summary>
/// A Value check range
/// </summary>
[DataContract]
public class ValueCheckRange
{
	/// <summary>
	///    GreaterThan
	/// </summary>
	[DataMember(Name = "gt")]
	public string GreaterThan { get; set; }

	/// <summary>
	///    GreaterThanOrEqualTo
	/// </summary>
	[DataMember(Name = "ge")]
	public string GreaterThanOrEqualTo { get; set; }

	/// <summary>
	///    LessThan
	/// </summary>
	[DataMember(Name = "lt")]
	public string LessThan { get; set; }

	/// <summary>
	///    LessThanOrEqual
	/// </summary>
	[DataMember(Name = "lte")]
	public string LessThanOrEqualTo2 { get; set; }

	/// <summary>
	///    LessThanOrEqualTo
	/// </summary>
	[DataMember(Name = "le")]
	public string LessThanOrEqualTo { get; set; }

	/// <summary>
	///    Equals
	/// </summary>
	[DataMember(Name = "eq")]
	public new string Equals { get; set; }

	/// <summary>
	///    Not equals
	/// </summary>
	[DataMember(Name = "ne")]
	public string NotEquals { get; set; }
}
