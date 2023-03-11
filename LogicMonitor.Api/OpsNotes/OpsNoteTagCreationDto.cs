namespace LogicMonitor.Api.OpsNotes;

/// <summary>
/// Scopes
/// </summary>
[DataContract]
public class OpsNoteTagCreationDto
{
	/// <summary>
	/// The tag id (leave null if there is no existing Tag)
	/// </summary>
	[DataMember(Name = "id")]
	public string Id { get; set; } = string.Empty;

	/// <summary>
	/// The tag name
	/// </summary>
	[DataMember(Name = "name")]
	public string Name { get; set; } = string.Empty;
}
