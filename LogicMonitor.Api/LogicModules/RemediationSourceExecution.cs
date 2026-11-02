namespace LogicMonitor.Api.LogicModules;

/// <summary>
/// A manual execution of a RemediationSource against a resource
/// </summary>
[DataContract]
public class RemediationSourceExecution
{
	/// <summary>
	/// The alert id
	/// </summary>
	[DataMember(Name = "alertId")]
	public string AlertId { get; set; } = string.Empty;

	/// <summary>
	/// Execution Id
	/// </summary>
	[SantabaReadOnly]
	[DataMember(Name = "executionId")]
	public string ExecutionId { get; set; } = string.Empty;

	/// <summary>
	/// The remediation module id
	/// </summary>
	[SantabaReadOnly]
	[DataMember(Name = "executionStatus")]
	public string ExecutionStatus { get; set; } = string.Empty;

	/// <summary>
	/// The host id
	/// </summary>
	[DataMember(Name = "hostId")]
	public int HostId { get; set; }

	/// <summary>
	/// The remediation module id
	/// </summary>
	[DataMember(Name = "hostRemediationSourceId")]
	public long HostRemediationSourceId { get; set; }

	/// <summary>
	/// When true, the API waits for execution to finish and returns script output in the response
	/// </summary>
	[DataMember(Name = "includeOutput")]
	public bool IncludeOutput { get; set; }

	/// <summary>
	/// Script output when includeOutput is true and execution has completed
	/// </summary>
	[SantabaReadOnly]
	[DataMember(Name = "output")]
	public string Output { get; set; } = string.Empty;

	/// <summary>
	/// The remediation module id
	/// </summary>
	[DataMember(Name = "remediationId")]
	public int RemediationId { get; set; }

	/// <summary>
	/// Type of trigger
	/// </summary>
	[SantabaReadOnly]
	[DataMember(Name = "triggerType")]
	public string TriggerType { get; set; } = string.Empty;
}
