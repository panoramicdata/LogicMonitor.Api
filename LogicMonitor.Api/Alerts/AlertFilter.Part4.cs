namespace LogicMonitor.Api.Alerts;

/// <summary>
///    AlertFilters are used when performing Alert queries.
/// </summary>
public partial class AlertFilter
{

	/// <summary>
	/// IncludeCleared and IsCleared are mutually exclusive (enforced by Validate), and express
	/// the same intent in two different ways: IncludeCleared as a wildcard on IsCleared, and
	/// IsCleared as a constraint on the end time.
	/// </summary>
	private void AppendClearedFilterItems(Filter<Alert> filter)
	{
		// The IncludeCleared approach
		if (IncludeCleared is not null)
		{
			filter.AppendFilterItemIfNotNull(nameof(Alert.IsCleared), IncludeCleared == true ? "*" : null);
			return;
		}

		if (IsCleared == true)
		{
			filter.AppendFilterItemIfNotNull(nameof(Alert.EndOnSeconds), 0, ">");
			return;
		}

		if (IsCleared == false)
		{
			filter.AppendFilterItemIfNotNull(nameof(Alert.EndOnSeconds), 0);
		}
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
