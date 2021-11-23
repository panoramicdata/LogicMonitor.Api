namespace LogicMonitor.Api.OpsNotes;

/// <summary>
/// An ops note scope creation DTO
/// </summary>
[DataContract]
public abstract class OpsNoteScopeCreationDto
{
	/// <summary>
	/// The tag name
	/// </summary>
	[DataMember(Name = "type")]
	public string Type { get; set; }
}
