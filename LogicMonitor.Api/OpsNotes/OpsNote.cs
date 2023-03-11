namespace LogicMonitor.Api.OpsNotes;

/// <summary>
///    An operational note
/// </summary>
[DataContract]
public class OpsNote : StringIdentifiedItem, IHasEndpoint
{
	/// <summary>
	///    The OpsNote note
	/// </summary>
	[DataMember(Name = "note")]
	public string Note { get; set; } = string.Empty;

	/// <summary>
	///    The creator
	/// </summary>
	[DataMember(Name = "createdBy")]
	public string CreatedBy { get; set; } = string.Empty;

	/// <summary>
	///    The timestamp of the OpsNote
	/// </summary>
	[DataMember(Name = "happenOnInSec")]
	public int HappenOnSeconds { get; set; }

	/// <summary>
	///    The scopes
	/// </summary>
	[DataMember(Name = "scopes")]
	public List<OpsNoteScope> Scopes { get; set; } = new();

	/// <summary>
	///    The tags
	/// </summary>
	[DataMember(Name = "tags")]
	public List<OpsNoteTag> Tags { get; set; } = new();

	/// <summary>
	///    The DateTime version of HappenOnTimeStampUtc
	/// </summary>
	[IgnoreDataMember]
	public DateTime HappenOnUtc => HappenOnSeconds.ToDateTimeUtc();

	/// <inheritdoc />
	public string Endpoint() => "setting/opsnotes";
}
