namespace LogicMonitor.Api.OpsNotes;

/// <summary>
/// A device ops note source
/// </summary>
[DataContract]
public class DeviceOpsNoteScope : OpsNoteScope
{
	/// <summary>
	/// The device id
	/// </summary>
	[DataMember(Name = "deviceId")]
	public int DeviceId { get; set; }

	/// <summary>
	/// The device name
	/// </summary>
	[DataMember(Name = "deviceName")]
	public string DeviceName { get; set; } = string.Empty;
}
