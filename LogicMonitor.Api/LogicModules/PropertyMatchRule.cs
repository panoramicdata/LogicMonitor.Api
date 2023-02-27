namespace LogicMonitor.Api.LogicModules;

/// <summary>
/// PropertyMatchRule
/// </summary>

[DataContract]
public class PropertyMatchRule
{
	/// <summary>
	/// underscore
	/// </summary>
	[DataMember(Name = "underscore", IsRequired=false)]
	public bool Underscore { get; set; }

	/// <summary>
	/// caseInsensitive
	/// </summary>
	[DataMember(Name = "caseInsensitive")]
	public bool CaseInsensitive { get; set; }
}
