namespace LogicMonitor.Api.OpsNotes;

/// <summary>
/// An Ops note tag
/// </summary>
[DataContract]
public class OpsNoteTag : StringIdentifiedItem, IHasEndpoint
{
	/// <summary>
	/// Name
	/// </summary>
	[DataMember(Name = "name")]
	public string Name { get; set; }

	/// <summary>
	/// Created on UTC seconds
	/// </summary>
	[DataMember(Name = "createdOnInSec")]
	public long CreatedOnSeconds { get; set; }

	/// <summary>
	/// Last modified UTC seconds
	/// </summary>
	[DataMember(Name = "updateOnInSec")]
	public long UpdatedOnSeconds { get; set; }

	/// <summary>
	/// The number of Ops Notes that refer to this tag
	/// </summary>
	[DataMember(Name = "referOpsNoteCount")]
	public int OpsNoteCount { get; set; }

	/// <inheritdoc />
	public string Endpoint() => "setting/opsnotetags";
}
