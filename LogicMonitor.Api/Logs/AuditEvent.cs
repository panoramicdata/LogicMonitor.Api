using System.Collections.ObjectModel;

namespace LogicMonitor.Api.Logs;

/// <summary>
/// An interpreted AuditItem
/// </summary>
public class AuditEvent
{
	/// <summary>
	/// The original LogItem ID
	/// </summary>
	public string Id { get; internal set; } = string.Empty;

	/// <summary>
	/// When the event occurred
	/// </summary>
	public DateTimeOffset DateTime { get; internal set; }

	/// <summary>
	/// The host that initiated the event
	/// </summary>
	public string? Host { get; internal set; }

	/// <summary>
	/// The original description from the LogItem
	/// </summary>
	public string? OriginalDescription { get; internal set; } = string.Empty;

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
	public Collection<int>? ResourceIds { get; internal set; }

	/// <summary>
	/// The resource name
	/// </summary>
	public Collection<string>? ResourceNames { get; internal set; }

	/// <summary>
	/// The LogicModule id
	/// </summary>
	public int? LogicModuleId { get; internal set; }

	/// <summary>
	/// The LogicModule name
	/// </summary>
	public string? LogicModuleName { get; internal set; }

	/// <summary>
	/// The LogicModule version
	/// </summary>
	public int? LogicModuleVersion { get; internal set; }

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
	/// The collector description
	/// </summary>
	public string? CollectorDescription { get; internal set; }

	/// <summary>
	/// Description
	/// </summary>
	public string? Description { get; internal set; }

	/// <summary>
	/// The API Token Id
	/// </summary>
	public string? ApiTokenId { get; internal set; }

	/// <summary>
	/// The API Path
	/// </summary>
	public string? ApiPath { get; internal set; }

	/// <summary>
	/// The API Method
	/// </summary>
	public string? ApiMethod { get; internal set; }

	/// <summary>
	/// The Dashboard name
	/// </summary>
	public string? DashboardName { get; internal set; }

	/// <summary>
	/// The Widget name
	/// </summary>
	public string? WidgetName { get; internal set; }

	/// <summary>
	/// The DataSource new instance ids
	/// </summary>
	public ICollection<int>? DataSourceNewInstanceIds { get; internal set; }

	/// <summary>
	/// The DataSource new instance names
	/// </summary>
	public ICollection<string>? DataSourceNewInstanceNames { get; internal set; }

	/// <summary>
	/// The DataSource deleted instance ids
	/// </summary>
	public ICollection<int>? DataSourceDeletedInstanceIds { get; internal set; }

	/// <summary>
	/// The DataSource deleted instance names
	/// </summary>
	public ICollection<string>? DataSourceDeletedInstanceNames { get; internal set; }

	/// <summary>
	/// The ResourceGroup name
	/// </summary>
	public string? ResourceGroupName { get; internal set; }

	/// <summary>
	/// The ResourceGroup id
	/// </summary>
	public int? ResourceGroupId { get; internal set; }

	/// <summary>
	/// The Property Name
	/// </summary>
	public string? PropertyName { get; internal set; }

	/// <summary>
	/// The ResourceDataSource Id
	/// </summary>
	public int? ResourceDataSourceId { get; internal set; }

	/// <summary>
	/// The Id of the regex that resulted in the translation from the LogItem
	/// -1 means a regex match was not made
	/// </summary>
	public int MatchedRegExId { get; internal set; } = -1;

	/// <summary>
	/// The Property Value
	/// </summary>
	public string? PropertyValue { get; internal set; }

	/// <summary>
	/// Time
	/// </summary>
	public string? Time { get; internal set; }

	/// <summary>
	/// The WildValue - available when a ResourceDataSourceInstance was added without Ids
	/// </summary>
	public string? WildValue { get; internal set; }

	/// <summary>
	/// The login name for a login event
	/// </summary>
	public string? PerformedByUsername { get; internal set; }

	/// <summary>
	/// The user name for an event
	/// </summary>
	public string? UserName { get; internal set; }

	/// <summary>
	/// The email address
	/// </summary>
	public string? UserEmail { get; internal set; }

	/// <summary>
	/// The user id
	/// </summary>
	public int? UserId { get; internal set; }

	/// <summary>
	/// The role name for role events
	/// </summary>
	public string? UserRole { get; internal set; }

	/// <summary>
	/// The alert id for alert update events
	/// </summary>
	public string? AlertId { get; internal set; }

	/// <summary>
	/// The alert note for alert update events
	/// </summary>
	public string? AlertNote { get; internal set; }

	/// <summary>
	/// The regular device monthly metrics
	/// </summary>
	public long? MonthlyMetrics { get; internal set; }

	/// <summary>
	/// The scheduled down time start time
	/// </summary>
	public string? StartDownTime { get; internal set; }

	/// <summary>
	/// The scheduled down time end time
	/// </summary>
	public string? EndDownTime { get; internal set; }

	/// <summary>
	/// The Collector command that was run
	/// </summary>
	public string? Command { get; internal set; }

	/// <summary>
	/// Resource host name
	/// </summary>
	public string? ResourceHostname { get; internal set; }

	/// <summary>
	/// Remote Session ID
	/// </summary>
	public long? RemoteSessionId { get; internal set; }

	/// <summary>
	/// Remote Session Type
	/// </summary>
	public string? RemoteSessionType { get; internal set; }

	/// <summary>
	/// Restrict SSO
	/// </summary>
	public bool? RestrictSso { get; internal set; }

	/// <summary>
	/// Collector Group Name
	/// </summary>
	public string? CollectorGroupName { get; internal set; }

	/// <summary>
	/// Collector Group Id
	/// </summary>
	public int? CollectorGroupId { get; internal set; }

	/// <summary>
	/// RequestId
	/// </summary>
	public long? RequestId { get; internal set; }
}
