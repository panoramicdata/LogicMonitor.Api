namespace LogicMonitor.Api.Resources;

/// <summary>
/// NbarApplicationNames
/// </summary>
[DataContract]

public class NbarApplicationNames
{
	/// <summary>
	/// operator
	/// </summary>
	[DataMember(Name = "operator")]
	public string Operator { get; set; } = string.Empty;

	/// <summary>
	/// applicationName
	/// </summary>
	[DataMember(Name = "applicationName")]
	public string ApplicationName { get; set; } = string.Empty;
}
