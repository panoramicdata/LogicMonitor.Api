namespace LogicMonitor.Api.Logs;

/// <summary>
/// An interpreted AuditItem
/// </summary>
public class AuditEvent
{
	/// <summary>
	/// When the event occurred
	/// </summary>
	public DateTimeOffset DateTime { get; internal set; }

	/// <summary>
	/// The user that initiated the event
	/// </summary>
	public string UserName { get; internal set; }

	/// <summary>
	/// The host that initiated the event
	/// </summary>
	public string Host { get; internal set; }

	/// <summary>
	/// The original description from the LogItem
	/// </summary>
	public string OriginalDescription { get; internal set; }

	/// <summary>
	/// The event's session Id
	/// </summary>
	public string SessionId { get; internal set; }

	/// <summary>
	/// The originator type
	/// </summary>
	public AuditEventOriginatorType OriginatorType { get; internal set; }

	/// <summary>
	/// The entity type
	/// </summary>
	public AuditEventEntityType EntityType { get; internal set; }

	/// <summary>
	/// The action type
	/// </summary>
	public AuditEventActionType ActionType { get; internal set; }

	/// <summary>
	/// Whether the interpretation was successful
	/// </summary>
	public bool IsInterpreted { get; internal set; }

	/// <summary>
	/// The Id of the entity
	/// </summary>
	public int? EntityId { get; internal set; }

	/// <summary>
	/// Entity notes
	/// </summary>
	public string EntityNotes { get; internal set; }

	/// <summary>
	/// The Id of the collector
	/// </summary>
	public int? CollectorId { get; internal set; }

	/// <summary>
	/// The Id of the accessToken
	/// </summary>
	public string AccessTokenId { get; internal set; }

	/// <summary>
	/// The collector name
	/// </summary>
	public string CollectorName { get; internal set; }

	/// <summary>
	/// Any other information
	/// </summary>
	public string AdditionalInformation { get; internal set; }

	/// <summary>
	/// The API Token Id
	/// </summary>
	public string ApiTokenId { get; internal set; }
}
