namespace LogicMonitor.Api.Alerts;

/// <summary>
/// An alert note DTO
/// </summary>
[DataContract]
public class AlertNote
{
	/// <summary>
	/// Constructor
	/// </summary>
	/// <param name="alertIds"></param>
	/// <param name="note"></param>
	public AlertNote(IList<string> alertIds, string note)
	{
		AlertIds = alertIds;
		Note = note;
	}

	/// <summary>
	/// The alert ids
	/// </summary>
	[DataMember(Name = "alertIds")]
	IList<string> AlertIds { get; set; }

	/// <summary>
	/// The note
	/// </summary>
	[DataMember(Name = "note")]
	public string Note { get; set; }
}
