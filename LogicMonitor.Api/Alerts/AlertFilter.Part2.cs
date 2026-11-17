namespace LogicMonitor.Api.Alerts;

/// <summary>
///    AlertFilters are used when performing Alert queries.
/// </summary>
public partial class AlertFilter
{

	/// <summary>
	///    The alert Id.  AlertType must not be null.
	/// </summary>
	public string? Id { get; set; }

	/// <summary>
	///    The alert internal Id
	/// </summary>
	public string? InternalId { get; set; }

	/// <summary>
	///    Whether to include inactive alerts (i.e. cleared alerts)
	/// </summary>
	public bool? IncludeCleared { get; set; } = true;

	/// <summary>
	///    The Alert level
	/// </summary>
	public List<AlertLevel> Levels { get; set; } =
		[
			AlertLevel.Error,
			AlertLevel.Critical
		];

	/// <summary>
	///    The order by property, for example, nameof(Alert.StartOnSeconds) (the default)
	/// </summary>
	public string OrderByProperty { get; set; } = nameof(Alert.StartOnSeconds);

	/// <summary>
	///    The order direction
	/// </summary>
	public OrderDirection OrderDirection { get; set; } = OrderDirection.Desc;

	/// <summary>
	///    Whether to include alerts that occurred during SDT
	/// </summary>
	public SdtFilter SdtFilter { get; set; }

	/// <summary>
	///    The skip
	/// </summary>
	public int? Skip { get; set; }

	/// <summary>
	///    The start DateTime for the query in seconds since the Epoch
	/// </summary>
	public long? StartEpochIsAfter { get; set; }

	/// <summary>
	///    The start DateTime for the query in seconds since the Epoch
	/// </summary>
	public long? EndEpochIsAfter { get; set; }

	/// <summary>
	///    The start DateTime for the query in seconds since the Epoch
	/// </summary>
	public long? EndEpochIsBefore { get; set; }

	/// <summary>
	///    The take
	/// </summary>
	public int? Take { get; set; }

	/// <summary>
	///    The search id.  If present, all fields except skip and take are ignored when constructing the query string.
	/// </summary>
	public string? SearchId { get; set; }

	/// <summary>
	///    The next recipient
	/// </summary>
	public string? NextRecipient { get; set; }

	/// <summary>
	///    The user that acknowledged the alert
	/// </summary>
	public string? AckedBy { get; set; }

	/// <summary>
	///    The requested alert properties.
	///    By default (null), ALL properties are returned.
	///    If you wish to improve your query time, you can ask for only certain properties to be returned.
	/// </summary>
	public List<string>? Properties { get; set; }

	/// <summary>
	///    When null (default), has no effect
	///    When true, only cleared alerts are returned
	///    When false, only active alerts are returned
	/// </summary>
	public bool? IsCleared { get; set; }

	/// <summary>
	///    Reset the search
	/// </summary>
	public void ResetSearch()
	{
		SearchId = null;
		Skip = null;
		Take = null;
	}

	/// <summary>
	/// Remove any filter items that would specify a Resource etc.
	/// Required because some API calls (like /device/devices/{Resource ID}/alerts
	/// ALREADY have a device ID in the URL, and so it should NOT be set again!
	/// </summary>
	public void RemoveMonitorObjectReferences()
	{
		MonitorObjectId = null;
		MonitorObjectName = string.Empty;
		MonitorObjectGroupFullPaths?.Clear();
	}

	/// <summary>
	///    The query string for REST calls
	/// </summary>
	public Filter<Alert> GetFilter()
	{
		Validate();

		var filter = new Filter<Alert>();

		ApplyPagingAndOrdering(filter);
		AppendFieldFilterItems(filter);
		AppendClearedFilterItems(filter);

		return filter;
	}
}
