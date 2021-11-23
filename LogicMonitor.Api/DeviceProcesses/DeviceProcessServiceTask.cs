namespace LogicMonitor.Api.DeviceProcesses;

/// <summary>
///     A process running on a device
/// </summary>
[DataContract]
public class DeviceProcessServiceTask
{
	/// <summary>
	///     The type
	/// </summary>
	[DataMember(Name = "type")]
	public DeviceProcessServiceTaskType Type { get; set; }

	/// <summary>
	///     The task id
	/// </summary>
	[DataMember(Name = "taskId")]
	public int TaskId { get; set; }
}
