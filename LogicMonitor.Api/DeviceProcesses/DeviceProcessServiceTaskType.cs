namespace LogicMonitor.Api.DeviceProcesses;

/// <summary>
///     The process service task type
/// </summary>
[DataContract]
[JsonConverter(typeof(StringEnumConverter))]
public enum DeviceProcessServiceTaskType
{
	/// <summary>
	/// Unknown
	/// </summary>
	[EnumMember(Value = "unknown")]
	Unknown = 0,

	/// <summary>
	///     Linux process
	/// </summary>
	[EnumMember(Value = "linuxprocess")]
	LinuxProcess,

	/// <summary>
	///     Windows process
	/// </summary>
	[EnumMember(Value = "winprocess")]
	WindowsProcess,

	/// <summary>
	///     Windows service
	/// </summary>
	[EnumMember(Value = "winservice")]
	WindowsService
}
