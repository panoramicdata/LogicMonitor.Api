namespace LogicMonitor.Api.Reports;

/// <summary>
/// An Word Template report
/// </summary>
[DataContract]
public class WordTemplateReport : DateRangeReport
{
	/// <summary>
	/// The macros
	/// </summary>
	[DataMember(Name = "macros")]
	public List<object> Macros { get; set; } = new();
}
