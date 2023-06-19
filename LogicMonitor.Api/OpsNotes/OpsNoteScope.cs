namespace LogicMonitor.Api.OpsNotes;

/// <summary>
/// An ops note scope
/// </summary>
[DataContract]
[JsonConverter(typeof(OpsNoteScopeConverter))]
public abstract class OpsNoteScope
{
	/// <summary>
	/// The type
	/// </summary>
	[DataMember(Name = "type")]
	public string Type { get; set; } = string.Empty;

	/// <summary>
	/// The group id
	/// </summary>
	[DataMember(Name = "groupId")]
	public int GroupId { get; set; }

	/// <summary>
	/// The group full path
	/// </summary>
	[DataMember(Name = "fullPath")]
	public string GroupFullPath { get; set; } = string.Empty;
}
