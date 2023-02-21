namespace LogicMonitor.Api.Collectors;

/// <summary>
/// SyntheticsSeleniumCollectorAttribute
/// </summary>

[DataContract]
public class SyntheticsSeleniumCollectorAttribute : CollectorAttribute
{
	/// <summary>
	/// Check type
	/// </summary>
	[DataMember(Name = "checkType")]
	public CheckType CheckType { get; set; }

	/// <summary>
	/// Configs
	/// </summary>
	[DataMember(Name = "configs")]
	public string Configs { get; set; }

	/// <summary>
	/// syntheticScript
	/// </summary>
	[DataMember(Name = "syntheticScript")]
	public string SyntheticScript { get; set; }
}
