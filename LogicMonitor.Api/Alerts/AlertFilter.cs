namespace LogicMonitor.Api.Alerts;

/// <summary>
///    AlertFilters are used when performing Alert queries.
/// </summary>
public class AlertFilter
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
	///    The Device or Website name
	/// </summary>
	public string? MonitorObjectName { get; set; }

	/// <summary>
	///    The Device or Website name
	/// </summary>
	public int? MonitorObjectId { get; set; }

	/// <summary>
	///    The Device or Website Group
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
	/// Remove any filter items that would specify a Device etc.
	/// Required because some API calls (like /device/devices/{Device ID}/alerts
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
		// Either Cleared or IncludeCleared can be specified, but not both
		if (IsCleared is not null && IncludeCleared is not null)
		{
			throw new InvalidOperationException("Either IsCleared or IncludeCleared can be specified, but not both.  Try setting IncludeCleared to null.");
		}

		// If IsCleared is false, it is not valid to set either EndDateTimes
		if (IsCleared == false && (EndEpochIsAfter is not null || EndEpochIsBefore is not null))
		{
			throw new InvalidOperationException("Either IsCleared set to false OR and EndTime filter can be set, but not both.");
		}

		// Either AlertType or AlertTypes may be set, or neither, but not both
		if (AlertType is not null && AlertTypes is not null)
		{
			throw new InvalidOperationException("Either AlertType or AlertTypes may be set, or neither, but not both.");
		}

		// Create the filter
		var filter = new Filter<Alert>();

		if (OrderByProperty is not null)
		{
			filter.Order = new Order<Alert> { Direction = OrderDirection, Property = OrderByProperty };
		}

		if (Properties is not null)
		{
			filter.Properties = Properties;
			if (!filter.Properties.Any(p => p == nameof(Alert.Id)))
			{
				filter.Properties.Add(nameof(Alert.Id));
			}
		}

		if (Skip is not null)
		{
			filter.Skip = Skip.Value;
		}

		if (Take is not null)
		{
			filter.Take = Take.Value;
		}

		filter.AppendFilterItemIfNotNull(nameof(Alert.Id), Id);
		filter.AppendFilterItemIfNotNull(nameof(Alert.AlertType), GetAlertTypes()?.Select(alertType => alertType.GetQueryString()).ToList());
		filter.AppendFilterItemIfNotNull(nameof(Alert.InternalId), InternalId);
		filter.AppendFilterItemIfNotNull(nameof(Alert.StartOnSeconds), StartEpochIsAfter, ">");
		filter.AppendFilterItemIfNotNull(nameof(Alert.StartOnSeconds), StartEpochIsBefore, "<");
		filter.AppendFilterItemIfNotNull(nameof(Alert.EndOnSeconds), EndEpochIsAfter, ">");
		filter.AppendFilterItemIfNotNull(nameof(Alert.EndOnSeconds), EndEpochIsBefore, "<");
		filter.AppendFilterItemIfNotNull(nameof(Alert.Acked), AckFilter == AckFilter.All ? null : (AckFilter == AckFilter.Acked).ToString().ToLowerInvariant());
		filter.AppendFilterItemIfNotNull(nameof(Alert.AckedBy), AckedBy);
		filter.AppendFilterItemIfNotNull(nameof(Alert.AlertRuleName), AlertRuleName);
		filter.AppendFilterItemIfNotNull(nameof(Alert.AlertRuleId), AlertRuleId);
		filter.AppendFilterItemIfNotNull(nameof(Alert.AlertEscalationChainName), EscalationChainName);
		filter.AppendFilterItemIfNotNull(nameof(Alert.AlertEscalationChainId), EscalationChainId);
		filter.AppendFilterItemIfNotNull(nameof(Alert.NextRecipient), NextRecipient);
		filter.AppendFilterItemIfNotNull(nameof(Alert.Severity), Levels?.OrderByDescending(l => l).Select(l => ((int)l).ToString(CultureInfo.InvariantCulture)).ToList());
		filter.AppendFilterItemIfNotNull(nameof(Alert.InScheduledDownTime), SdtFilter == SdtFilter.All ? null : (SdtFilter == SdtFilter.Sdt).ToString().ToLowerInvariant());
		filter.AppendFilterItemIfNotNull(nameof(Alert.MonitorObjectGroups), MonitorObjectGroupFullPaths);
		filter.AppendFilterItemIfNotNull(nameof(Alert.MonitorObjectName), MonitorObjectName);
		filter.AppendFilterItemIfNotNull(nameof(Alert.MonitorObjectId), MonitorObjectId);
		filter.AppendFilterItemIfNotNull(nameof(Alert.DataPointName), DataPointName);
		filter.AppendFilterItemIfNotNull(nameof(Alert.DataPointId), DataPointId);
		filter.AppendFilterItemIfNotNull(nameof(Alert.ResourceTemplateName), ResourceTemplateName?.Replace(@"\", @"\\"));
		filter.AppendFilterItemIfNotNull(nameof(Alert.ResourceTemplateId), ResourceTemplateId);
		filter.AppendFilterItemIfNotNull(nameof(Alert.InstanceName), InstanceName?.Replace(@"\", @"\\"));
		filter.AppendFilterItemIfNotNull(nameof(Alert.InstanceId), InstanceId);

		// The IncludeCleared approach
		if (IncludeCleared is not null)
		{
			filter.AppendFilterItemIfNotNull(nameof(Alert.IsCleared), IncludeCleared == true ? "*" : null);
		}
		else if (IsCleared == true)
		{
			filter.AppendFilterItemIfNotNull(nameof(Alert.EndOnSeconds), 0, ">");
		}
		else if (IsCleared == false)
		{
			filter.AppendFilterItemIfNotNull(nameof(Alert.EndOnSeconds), 0);
		}

		return filter;
	}

	internal List<AlertType>? GetAlertTypes()
	{
		var alertTypes = (AlertTypes ?? []).Union(AlertType.HasValue
			? [AlertType.Value]
			: []).ToList();
		return alertTypes.Count > 0 ? alertTypes : null;
	}

	/// <summary>
	///    Clones the object
	/// </summary>
	public AlertFilter Clone() => new()
	{
		AckFilter = AckFilter,
		AlertRuleName = AlertRuleName,
		AlertRuleId = AlertRuleId,
		AlertType = AlertType,
		AlertTypes = AlertTypes,
		DataPointName = DataPointName,
		ResourceTemplateName = ResourceTemplateName,
		InstanceName = InstanceName,
		MonitorObjectName = MonitorObjectName,
		MonitorObjectId = MonitorObjectId,
		MonitorObjectGroupFullPaths = MonitorObjectGroupFullPaths,
		NeedMessage = NeedMessage,
		EscalationChainName = EscalationChainName,
		EscalationChainId = EscalationChainId,
		Id = Id,
		InternalId = InternalId,
		IncludeCleared = IncludeCleared,
		Levels = Levels.ConvertAll(l => l), // Clone
		OrderByProperty = OrderByProperty,
		OrderDirection = OrderDirection,
		Properties = Properties,
		SdtFilter = SdtFilter,
		Skip = Skip,
		StartEpochIsBefore = StartEpochIsBefore,
		StartEpochIsAfter = StartEpochIsAfter,
		EndEpochIsBefore = EndEpochIsBefore,
		EndEpochIsAfter = EndEpochIsAfter,
		Take = Take,
		SearchId = SearchId,
		NextRecipient = NextRecipient,
		AckedBy = AckedBy,
		DataPointId = DataPointId,
		InstanceId = InstanceId,
		IsCleared = IsCleared,
		ResourceTemplateId = ResourceTemplateId,
	};

	/// <inheritdoc />
	public override string ToString() =>
		$"StartUtcIsAfter: {StartUtcIsAfter?.ToString("yyyy-MM-dd HH:mm", CultureInfo.InvariantCulture)}, " +
		$"StartUtcIsBefore: {StartUtcIsBefore?.ToString("yyyy-MM-dd HH:mm", CultureInfo.InvariantCulture)}";
}
