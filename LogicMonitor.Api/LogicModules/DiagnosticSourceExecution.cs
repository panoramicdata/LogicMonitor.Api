namespace LogicMonitor.Api.LogicModules;

/// <summary>
/// Represents a manual or scheduled diagnostic source execution.
/// </summary>
[DataContract]
public class DiagnosticSourceExecution
{
	/// <summary>
	/// Execution id.
	/// </summary>
	[DataMember(Name = "executionId")]
	public string ExecutionId { get; set; } = string.Empty;

	/// <summary>
	/// Current execution status.
	/// </summary>
	[DataMember(Name = "executionStatus")]
	public string ExecutionStatus { get; set; } = string.Empty;

	/// <summary>
	/// Diagnostic source module id.
	/// </summary>
	[DataMember(Name = "diagnosticId")]
	public int DiagnosticId { get; set; }

	/// <summary>
	/// Host id.
	/// </summary>
	[DataMember(Name = "hostId")]
	public int HostId { get; set; }

	/// <summary>
	/// Host-diagnostic-source relation id.
	/// </summary>
	[DataMember(Name = "hostDiagnosticSourceId")]
	public int HostDiagnosticSourceId { get; set; }

	/// <summary>
	/// Trigger type.
	/// </summary>
	[DataMember(Name = "triggerType")]
	public string TriggerType { get; set; } = string.Empty;

	/// <summary>
	/// Related alert id.
	/// </summary>
	[DataMember(Name = "alertId")]
	public string AlertId { get; set; } = string.Empty;
}