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
	public string GreaterThan { get; set; } = string.Empty;

	/// <summary>
	///    GreaterThanOrEqualTo
	/// </summary>
	[DataMember(Name = "ge")]
	public string GreaterThanOrEqualTo { get; set; } = string.Empty;

	/// <summary>
	///    LessThan
	/// </summary>
	[DataMember(Name = "lt")]
	public string LessThan { get; set; } = string.Empty;

	/// <summary>
	///    LessThanOrEqual
	/// </summary>
	[DataMember(Name = "lte")]
	public string LessThanOrEqualTo2 { get; set; } = string.Empty;

	/// <summary>
	///    LessThanOrEqualTo
	/// </summary>
	[DataMember(Name = "le")]
	public string LessThanOrEqualTo { get; set; } = string.Empty;

	/// <summary>
	///    Equals
	/// </summary>
	[DataMember(Name = "eq")]
	public new string Equals { get; set; } = string.Empty;

	/// <summary>
	///    Not equals
	/// </summary>
	[DataMember(Name = "ne")]
	public string NotEquals { get; set; } = string.Empty;
}
