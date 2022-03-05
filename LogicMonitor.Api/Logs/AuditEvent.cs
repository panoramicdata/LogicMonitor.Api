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
	public string? UserName { get; internal set; }

	/// <summary>
	/// The host that initiated the event
	/// </summary>
	public string? Host { get; internal set; }

	/// <summary>
	/// The original description from the LogItem
	/// </summary>
	public string OriginalDescription { get; internal set; } = string.Empty;

	/// <summary>
	/// The event's session Id
	/// </summary>
	public string? SessionId { get; internal set; }

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
	/// The outcome type
	/// </summary>
	public AuditEventOutcomeType OutcomeType { get; internal set; }

	/// <summary>
	/// The resource id
	/// </summary>
	public int? ResourceId { get; internal set; }

	/// <summary>
	/// The resource name
	/// </summary>
	public string? ResourceName { get; internal set; }

	/// <summary>
	/// The DataSource id
	/// </summary>
	public int? DataSourceId { get; internal set; }

	/// <summary>
	/// The DataSource name
	/// </summary>
	public string? DataSourceName { get; internal set; }

	/// <summary>
	/// The Instance id
	/// </summary>
	public int? InstanceId { get; internal set; }

	/// <summary>
	/// The Instance name
	/// </summary>
	public string? InstanceName { get; internal set; }

	/// <summary>
	/// The collector id
	/// </summary>
	public int? CollectorId { get; internal set; }

	/// <summary>
	/// The collector name
	/// </summary>
	public string? CollectorName { get; internal set; }

	/// <summary>
	/// Any other information
	/// </summary>
	public string? AdditionalInformation { get; internal set; }

	/// <summary>
	/// The API Token Id
	/// </summary>
	public string? ApiTokenId { get; internal set; }

	/// <summary>
	/// The DataSource new instance ids
	/// </summary>
	public ICollection<int?>? DataSourceNewInstanceIds { get; internal set; }

	/// <summary>
	/// The ResourceGroup name
	/// </summary>
	public string? ResourceGroupName { get; internal set; }

	/// <summary>
	/// The property name
	/// </summary>
	public string? PropertyName { get; internal set; }
}
