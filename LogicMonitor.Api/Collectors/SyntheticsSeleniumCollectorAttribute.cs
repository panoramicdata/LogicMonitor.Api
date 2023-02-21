namespace LogicMonitor.Api.Collectors;

/// <summary>
/// SyntheticsSeleniumCollectorAttribute
/// </summary>

[DataContract]
public class SyntheticsSeleniumCollectorAttribute : CollectorAttribute
{
	/// <summary>
	/// Checktype
	/// </summary>
	[DataMember(Name = "checkType")]
	public string CheckType { get; set; }

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
