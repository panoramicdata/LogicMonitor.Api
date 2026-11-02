namespace LogicMonitor.Api.LogicModules;

/// <summary>
/// A page of diagnostic and remediation execution results
/// </summary>
[DataContract]
public class DiagnosticRemediationExecutionResultPage : Page<DiagnosticRemediationExecutionResult>
{
	/// <summary>
	/// The cursor for the next page of diagnostic results
	/// </summary>
	[DataMember(Name = "cursor")]
	public string? Cursor { get; set; }

	/// <summary>
	/// The cursor for the next page of remediation results
	/// </summary>
	[DataMember(Name = "remediationCursor")]
	public string? RemediationCursor { get; set; }

	/// <summary>
	/// The sort order applied
	/// </summary>
	[DataMember(Name = "sort")]
	public string? Sort { get; set; }
}
