namespace LogicMonitor.Api.Alerts;

/// <summary>
///    AlertFilters are used when performing Alert queries.
/// </summary>
public partial class AlertFilter
{
	/// <summary>
	///    Whether to exclude acknowledged Alerts
	/// </summary>
	public AckFilter AckFilter { get; set; } = AckFilter.All;

	/// <summary>
	///    The AlertRule name
	/// </summary>
	public string? AlertRuleName { get; set; }

	/// <summary>
	///    The AlertRule Id
	/// </summary>
	public int? AlertRuleId { get; set; }

	/// <summary>
	///    The alertType used only with Id.
	///    If set, AlertTypes should not be (and vice versa)
	/// </summary>
	public AlertType? AlertType { get; set; }

	/// <summary>
	/// A list of alert types.
	/// If set, AlertType should not be (and vice versa)
	/// </summary>
	public List<AlertType>? AlertTypes { get; set; }

	/// <summary>
	///    The DataPoint
	/// </summary>
	public string? DataPointName { get; set; }

	/// <summary>
	///    The DataPoint Id
	/// </summary>
	public string? DataPointId { get; set; }

	/// <summary>
	///    The DataSource (etc.)
	/// </summary>
	public string? ResourceTemplateName { get; set; }

	/// <summary>
	///    The DataSource (etc.) Id
	/// </summary>
	public int? ResourceTemplateId { get; set; }

	/// <summary>
	///    The DataSourceInstance
	/// </summary>
	public string? InstanceName { get; set; }

	/// <summary>
	///    The DataSourceInstance Id
	/// </summary>
	public string? InstanceId { get; set; }

	/// <summary>
	///    The Resource or Website name
	/// </summary>
	public string? MonitorObjectName { get; set; }

	/// <summary>
	///    The Resource or Website ID
	/// </summary>
	public int? MonitorObjectId { get; set; }

	/// <summary>
	///    The Resource or Website Group
	/// </summary>
	public List<string>? MonitorObjectGroupFullPaths { get; set; }

	/// <summary>
	///    Whether the full message is needed
	/// </summary>
	public bool NeedMessage { get; set; } = true;

	/// <summary>
	///    The end DateTime for the query in seconds since the Epoch
	/// </summary>
	public long? StartEpochIsBefore { get; set; }

	/// <summary>
	///    Start DateTime in UTC is after
	/// </summary>
	public DateTime? StartUtcIsAfter
	{
		get => StartEpochIsAfter?.ToDateTimeUtc();
		set => StartEpochIsAfter = value?.SecondsSinceTheEpoch();
	}

	/// <summary>
	///    Start DateTime in UTC is Before
	/// </summary>
	public DateTime? StartUtcIsBefore
	{
		get => StartEpochIsBefore?.ToDateTimeUtc();
		set => StartEpochIsBefore = value?.SecondsSinceTheEpoch();
	}

	/// <summary>
	///    End DateTime in UTC is after
	/// </summary>
	public DateTime? EndUtcIsAfter
	{
		get => EndEpochIsAfter?.ToDateTimeUtc();
		set => EndEpochIsAfter = value?.SecondsSinceTheEpoch();
	}

	/// <summary>
	///    End DateTime in UTC is Before
	/// </summary>
	public DateTime? EndUtcIsBefore
	{
		get => EndEpochIsBefore?.ToDateTimeUtc();
		set => EndEpochIsBefore = value?.SecondsSinceTheEpoch();
	}

	/// <summary>
	///    The Escalation Chain
	/// </summary>
	public string? EscalationChainName { get; set; }

	/// <summary>
	///    The Escalation Chain Id
	/// </summary>
	public string? EscalationChainId { get; set; }
}
