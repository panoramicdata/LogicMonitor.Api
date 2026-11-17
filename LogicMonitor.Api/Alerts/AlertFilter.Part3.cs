namespace LogicMonitor.Api.Alerts;

/// <summary>
///    AlertFilters are used when performing Alert queries.
/// </summary>
public partial class AlertFilter
{

	/// <summary>
	/// Rejects the combinations of properties that cannot be expressed as a single query.
	/// </summary>
	private void Validate()
	{
		ValidateClearedCombination();

		// Either AlertType or AlertTypes may be set, or neither, but not both
		if (AlertType is not null && AlertTypes is not null)
		{
			throw new InvalidOperationException("Either AlertType or AlertTypes may be set, or neither, but not both.");
		}
	}

	private void ValidateClearedCombination()
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
	}

	private void ApplyPagingAndOrdering(Filter<Alert> filter)
	{
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
	}

	private void AppendFieldFilterItems(Filter<Alert> filter)
	{
		filter.AppendFilterItemIfNotNull(nameof(Alert.Id), Id);
		filter.AppendFilterItemIfNotNull(nameof(Alert.AlertType), GetAlertTypeQueryStrings());
		filter.AppendFilterItemIfNotNull(nameof(Alert.InternalId), InternalId);
		filter.AppendFilterItemIfNotNull(nameof(Alert.StartOnSeconds), StartEpochIsAfter, ">");
		filter.AppendFilterItemIfNotNull(nameof(Alert.StartOnSeconds), StartEpochIsBefore, "<");
		filter.AppendFilterItemIfNotNull(nameof(Alert.EndOnSeconds), EndEpochIsAfter, ">");
		filter.AppendFilterItemIfNotNull(nameof(Alert.EndOnSeconds), EndEpochIsBefore, "<");
		filter.AppendFilterItemIfNotNull(nameof(Alert.Acked), GetAckedValue());
		filter.AppendFilterItemIfNotNull(nameof(Alert.AckedBy), AckedBy);
		filter.AppendFilterItemIfNotNull(nameof(Alert.AlertRuleName), AlertRuleName);
		filter.AppendFilterItemIfNotNull(nameof(Alert.AlertRuleId), AlertRuleId);
		filter.AppendFilterItemIfNotNull(nameof(Alert.AlertEscalationChainName), EscalationChainName);
		filter.AppendFilterItemIfNotNull(nameof(Alert.AlertEscalationChainId), EscalationChainId);
		filter.AppendFilterItemIfNotNull(nameof(Alert.NextRecipient), NextRecipient);
		filter.AppendFilterItemIfNotNull(nameof(Alert.Severity), GetSeverityValues());
		filter.AppendFilterItemIfNotNull(nameof(Alert.InScheduledDownTime), GetInScheduledDownTimeValue());
		filter.AppendFilterItemIfNotNull(nameof(Alert.MonitorObjectGroups), MonitorObjectGroupFullPaths);
		filter.AppendFilterItemIfNotNull(nameof(Alert.MonitorObjectName), MonitorObjectName);
		filter.AppendFilterItemIfNotNull(nameof(Alert.MonitorObjectId), MonitorObjectId);
		filter.AppendFilterItemIfNotNull(nameof(Alert.DataPointName), DataPointName);
		filter.AppendFilterItemIfNotNull(nameof(Alert.DataPointId), DataPointId);
		filter.AppendFilterItemIfNotNull(nameof(Alert.ResourceTemplateName), EscapeBackslashes(ResourceTemplateName));
		filter.AppendFilterItemIfNotNull(nameof(Alert.ResourceTemplateId), ResourceTemplateId);
		filter.AppendFilterItemIfNotNull(nameof(Alert.InstanceName), EscapeBackslashes(InstanceName));
		filter.AppendFilterItemIfNotNull(nameof(Alert.InstanceId), InstanceId);
	}

	private List<string>? GetAlertTypeQueryStrings()
		=> GetAlertTypes()?.Select(alertType => alertType.GetQueryString()).ToList();

	private List<string>? GetSeverityValues()
		=> Levels?.OrderByDescending(l => l).Select(l => ((int)l).ToString(CultureInfo.InvariantCulture)).ToList();

	/// <summary>
	/// Null (meaning "do not filter on it at all") when the filter is All.
	/// </summary>
	private string? GetAckedValue()
		=> AckFilter == AckFilter.All ? null : (AckFilter == AckFilter.Acked).ToString().ToLowerInvariant();

	/// <summary>
	/// Null (meaning "do not filter on it at all") when the filter is All.
	/// </summary>
	private string? GetInScheduledDownTimeValue()
		=> SdtFilter == SdtFilter.All ? null : (SdtFilter == SdtFilter.Sdt).ToString().ToLowerInvariant();

	private static string? EscapeBackslashes(string? value) => value?.Replace("\\", "\\\\");
}
