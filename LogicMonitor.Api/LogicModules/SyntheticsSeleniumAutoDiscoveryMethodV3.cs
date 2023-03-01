namespace LogicMonitor.Api.LogicModules;

/// <summary>
/// SyntheticsSeleniumAutoDiscoveryMethodV3
/// </summary>

[DataContract]
public class SyntheticsSeleniumAutoDiscoveryMethodV3 : AutoDiscoveryMethod
{
	/// <summary>
	/// isInternal
	/// </summary>
	[DataMember(Name = "isInternal")]
	public bool IsInternal { get; set; }

	/// <summary>
	/// checkpoints
	/// </summary>
	[DataMember(Name = "checkpoints")]
	public string? Checkpoints { get; set; }
}
