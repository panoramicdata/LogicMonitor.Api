namespace LogicMonitor.Api.LogicModules;

/// <summary>
/// The result of a diagnostic or remediation execution
/// </summary>
[DataContract]
public class DiagnosticRemediationExecutionResult
{
	/// <summary>
	/// Alert id when the execution was tied to an alert (may be empty).
	/// </summary>
	[DataMember(Name = "alertId")]
	public string AlertId { get; set; } = string.Empty;

	/// <summary>
	/// Diagnostic Source module id when sourceType is diagnosticSource
	/// </summary>
	[DataMember(Name = "diagnosticSourceId")]
	public int DiagnosticSourceId { get; set; }

	/// <summary>
	/// Execution end or log timestamp in epoch seconds (UTC), when available
	/// </summary>
	[DataMember(Name = "endTime")]
	public long EndTime { get; set; }

	/// <summary>
	/// User or principal that triggered the execution (when available)
	/// </summary>
	[DataMember(Name = "executedBy")]
	public string ExecutedBy { get; set; } = string.Empty;

	/// <summary>
	/// LogicModule execution id
	/// </summary>
	[DataMember(Name = "executionId")]
	public long ExecutionId { get; set; }

	/// <summary>
	/// Execution status name (enum string from collector pipeline)
	/// </summary>
	[DataMember(Name = "executionStatus")]
	public string ExecutionStatus { get; set; } = string.Empty;

	/// <summary>
	/// Device (resource) id
	/// </summary>
	[DataMember(Name = "hostId")]
	public int HostId { get; set; }

	/// <summary>
	/// Logic module display name
	/// </summary>
	[DataMember(Name = "moduleName")]
	public string ModuleName { get; set; } = string.Empty;

	/// <summary>
	/// Script output / message captured for this execution
	/// </summary>
	[DataMember(Name = "output")]
	public string Output { get; set; } = string.Empty;

	/// <summary>
	/// Remediation Source module id when sourceType is remediationSource
	/// </summary>
	[DataMember(Name = "remediationSourceId")]
	public int RemediationSourceId { get; set; }

	/// <summary>
	/// Row kind: diagnosticSource or remediationSource (exact values as in the JSON response).
	/// </summary>
	[DataMember(Name = "sourceType")]
	public string SourceType { get; set; } = string.Empty;

	/// <summary>
	/// Execution start time in epoch seconds (UTC), when available
	/// </summary>
	[DataMember(Name = "startTime")]
	public long StartTime { get; set; }

	/// <summary>
	/// How the execution was triggered (e.g.
	/// </summary>
	[DataMember(Name = "triggeredType")]
	public string TriggeredType { get; set; } = string.Empty;
}
