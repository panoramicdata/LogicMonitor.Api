namespace LogicMonitor.Api.ResourceProcesses;

/// <summary>
///     A process service task running on a Resource
/// </summary>
[DataContract]
public class ResourceProcessServiceTask
{
	/// <summary>
	///     The type
	/// </summary>
	[DataMember(Name = "type")]
	public ResourceProcessServiceTaskType Type { get; set; }

	/// <summary>
	///     The task id
	/// </summary>
	[DataMember(Name = "taskId")]
	public int TaskId { get; set; }
}
