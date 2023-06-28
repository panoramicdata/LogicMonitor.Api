namespace LogicMonitor.Api.OpsNotes;

/// <summary>
/// A collection of OpsNotes
/// </summary>
[DataContract]
public class OpsNotesCollection
{
	/// <summary>
	/// The number of OpsNotes
	/// </summary>
	[DataMember(Name = "total")]
	public int Count { get; set; }

	/// <summary>
	/// The OpsNotes
	/// </summary>
	[DataMember(Name = "opsnotes")]
	public List<OpsNote> OpsNotes { get; set; } = new();
}
