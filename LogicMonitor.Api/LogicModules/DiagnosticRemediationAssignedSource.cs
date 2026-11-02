namespace LogicMonitor.Api.LogicModules;

/// <summary>
/// A diagnostic or remediation source assigned to a resource or alert
/// </summary>
[DataContract]
public class DiagnosticRemediationAssignedSource
{
	/// <summary>
	/// Exchange module definition id.
	/// </summary>
	[DataMember(Name = "id")]
	public int Id { get; set; }

	/// <summary>
	/// Applies-to script for the module.
	/// </summary>
	[DataMember(Name = "appliesTo")]
	public string AppliesTo { get; set; } = string.Empty;

	/// <summary>
	/// Module description.
	/// </summary>
	[DataMember(Name = "description")]
	public string Description { get; set; } = string.Empty;

	/// <summary>
	/// Module group.
	/// </summary>
	[DataMember(Name = "group")]
	public string Group { get; set; } = string.Empty;

	/// <summary>
	/// Module name.
	/// </summary>
	[DataMember(Name = "name")]
	public string Name { get; set; } = string.Empty;

	/// <summary>
	/// Module kind: diagnostic or remediation.
	/// </summary>
	[DataMember(Name = "type")]
	public string Type { get; set; } = string.Empty;
}
