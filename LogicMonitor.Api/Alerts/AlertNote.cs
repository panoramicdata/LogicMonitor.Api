namespace LogicMonitor.Api.Alerts;

/// <summary>
/// An alert note DTO
/// </summary>
/// <remarks>
/// Constructor
/// </remarks>
/// <param name="alertIds"></param>
/// <param name="note"></param>
[DataContract]
public class AlertNote(IList<string> alertIds, string note)
{

	/// <summary>
	/// The alert ids
	/// </summary>
	[DataMember(Name = "alertIds")]
	public IList<string> AlertIds { get; set; } = alertIds;

	/// <summary>
	/// The note
	/// </summary>
	[DataMember(Name = "note")]
	public string Note { get; set; } = note;
}
