namespace LogicMonitor.Api.Alerts;

/// <summary>
/// An alert note DTO
/// </summary>
[DataContract]
public class AlertNote
{
	public AlertNote(IList<string> alertIds, string note)
	{
		AlertIds = alertIds;
		Note = note;
	}

	[DataMember(Name = "alertIds")]
	IList<string> AlertIds { get; set; }

	/// <summary>
	/// The note
	/// </summary>
	[DataMember(Name = "note")]
	public string Note { get; set; }
}
