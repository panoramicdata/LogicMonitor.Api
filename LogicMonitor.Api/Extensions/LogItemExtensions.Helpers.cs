namespace LogicMonitor.Api.Extensions;

public static partial class LogItemExtensions
{
	private static bool TryHandleScheduledDebugCommand(string description, AuditEvent auditEvent)
	{
		if (!description.StartsWith("\"Action=Schedule debug command\"", StringComparison.Ordinal))
		{
			return false;
		}

		auditEvent.MatchedRegExId = 95;
		auditEvent.EntityType = AuditEventEntityType.Collector;
		auditEvent.ActionType = AuditEventActionType.Run;
		auditEvent.OutcomeType = AuditEventOutcomeType.Success;
		auditEvent.Command = ExtractValueBetween(description, "\"Command=", "\"");

		var collectorIdValue = ExtractValueBetween(description, "\"AgentId=", "\"");
		if (int.TryParse(collectorIdValue, NumberStyles.None, CultureInfo.InvariantCulture, out var collectorId))
		{
			auditEvent.CollectorId = collectorId;
		}

		return true;
	}

	private static bool TryHandleDeleteAzureAccount(string description, AuditEvent auditEvent)
	{
		const string prefix = "Delete Azure account ";

		if (!description.StartsWith(prefix, StringComparison.Ordinal))
		{
			return false;
		}

		auditEvent.MatchedRegExId = 0;
		auditEvent.EntityType = AuditEventEntityType.AzureAccount;
		auditEvent.ActionType = AuditEventActionType.Delete;
		auditEvent.OutcomeType = AuditEventOutcomeType.Success;

		var resourceName = ExtractValueBetween(description, prefix, ",");
		if (!string.IsNullOrEmpty(resourceName))
		{
			auditEvent.ResourceNames = [resourceName!];
		}

		return true;
	}

	private static string? ExtractNameBetweenSingleQuotes(string description, string prefix)
	{
		var start = prefix.Length;
		if (description.Length <= start)
		{
			return null;
		}

		var end = description.IndexOf('\'', start);
		if (end <= start)
		{
			return null;
		}

		return description.Substring(start, end - start);
	}

	private static string? ExtractValueBetween(string source, string startMarker, string endMarker)
	{
		var start = source.IndexOf(startMarker, StringComparison.Ordinal);
		if (start < 0)
		{
			return null;
		}

		start += startMarker.Length;
		var end = source.IndexOf(endMarker, start, StringComparison.Ordinal);
		if (end <= start)
		{
			return null;
		}

		return source.Substring(start, end - start);
	}

	private static AuditEventActionType GetAction(Match value)
		=> value.Groups["scheduledHealthCheck"].Success
			? AuditEventActionType.ScheduledHealthCheckScript
			: value.Groups["login"].Success
			? AuditEventActionType.Login
			: value.Groups["logout"].Success
			? AuditEventActionType.Logout
			: value.Groups["discardedEventAlert"].Success
			? AuditEventActionType.DiscardedEventAlert
			: value.Groups["alertId"].Success
			? AuditEventActionType.Update
			: value.Groups["action"].Value.ToUpperInvariant() switch
			{
				"ADD" or "CREATE" => AuditEventActionType.Create,
				"COLLECT NOW" => AuditEventActionType.CollectNow,
				"STARTED" => AuditEventActionType.Start,
				"TERMINATED" => AuditEventActionType.End,
				"FETCH" or "VIEW" => AuditEventActionType.Read,
				"LOGIN" => AuditEventActionType.Login,
				"UPDAT" or "UPDATE" or "EDIT" => AuditEventActionType.Update,
				"DELET" or "DELETE" => AuditEventActionType.Delete,
				"ENABLE" => AuditEventActionType.Enable,
				"DOWNLOAD" => AuditEventActionType.Download,
				"DISABLE" => AuditEventActionType.Disable,
				"REQUEST REMOTE" => AuditEventActionType.RequestRemoteSession,
				"RE-BALANCED" => AuditEventActionType.Rebalance,
				"FORGOT PASSWORD" => AuditEventActionType.RequestPasswordReset,
				"RUN" => AuditEventActionType.Run,
				"AUTO-DISCOVER POLL REQUEST" => AuditEventActionType.AutoDiscoveryPollRequest,
				"REGULAR DEVICE TOTAL MONTHLY METRICS" => AuditEventActionType.RegularResourceTotalMonthlyMetrics,
				"SCHEDULE" => AuditEventActionType.ScheduleActiveDiscovery,
				_ => AuditEventActionType.None
			};

	private class LogItemRegex(int id, AuditEventEntityType entityType, Regex regex)
	{
		public int Id { get; set; } = id;

		public AuditEventEntityType EntityType { get; set; } = entityType;

		public Regex Regex { get; set; } = WithTimeout(regex);
	}
}
