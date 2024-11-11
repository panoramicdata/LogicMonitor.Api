namespace LogicMonitor.Api.OpsNotes;

/// <summary>
/// A device ops note source
/// </summary>
[DataContract]
public class ResourceOpsNoteScope : OpsNoteScope
{
	/// <summary>
	/// The Resource id
	/// </summary>
	[DataMember(Name = "deviceId")]
	public int ResourceId { get; set; }

	/// <summary>
	/// The Resource name
	/// </summary>
	[DataMember(Name = "deviceName")]
	public string ResourceName { get; set; } = string.Empty;
}
