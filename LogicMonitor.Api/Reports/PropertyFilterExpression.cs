namespace LogicMonitor.Api.Reports;

/// <summary>
/// An expression in a report property filter rule
/// </summary>
[DataContract]
public class PropertyFilterExpression
{
	/// <summary>
	/// The operator, for example EQ
	/// </summary>
	[DataMember(Name = "operator")]
	public string Operator { get; set; } = string.Empty;

	/// <summary>
	/// The value the property is compared with
	/// </summary>
	[DataMember(Name = "value")]
	public string Value { get; set; } = string.Empty;
}
