namespace LogicMonitor.Api.Extensions;

public static partial class LogItemExtensions
{
	/// <summary>
	/// Handles the messages that are recognised by prefix alone. Returns true (and populates
	/// <paramref name="auditEvent"/>) if the description was handled, in which case no regular
	/// expression matching is needed. Order is significant and matches the original chain.
	/// </summary>
	private static bool TryHandleByPrefix(string description, AuditEvent auditEvent)
		=> TryHandleBulkPayloadPrefix(description, auditEvent)
			|| TryHandleLogicModuleAdditionPrefix(description, auditEvent)
			|| TryHandleWebsitePrefix(description, auditEvent)
			|| TryHandleDeleteAzureAccount(description, auditEvent)
			|| TryHandleScheduledDebugCommand(description, auditEvent);

	/// <summary>
	/// Messages whose payloads are enormous and which do not need detailed parsing.
	/// </summary>
	private static bool TryHandleBulkPayloadPrefix(string description, AuditEvent auditEvent)
	{
		// DataSource imports have a LOT of text.  Skip these for now.
		if (description.StartsWith("Import DataSource", StringComparison.Ordinal))
		{
			auditEvent.MatchedRegExId = 27;
			// This "none" denotes that we are not expecting to handle this type of message
			auditEvent.EntityType = AuditEventEntityType.None;
			auditEvent.OutcomeType = AuditEventOutcomeType.Success;
			return true;
		}

		// Change host collectors messages can be extremely large and don't need detailed parsing.
		if (description.StartsWith("Change host collectors", StringComparison.Ordinal))
		{
			auditEvent.MatchedRegExId = 100;
			auditEvent.EntityType = AuditEventEntityType.Collector;
			auditEvent.ActionType = AuditEventActionType.GeneralApi;
			auditEvent.OutcomeType = AuditEventOutcomeType.Success;
			return true;
		}

		// Test script scheduled messages contain full script bodies and are very long.
		if (description.StartsWith("\"Action=Test script scheduled\"", StringComparison.Ordinal))
		{
			auditEvent.MatchedRegExId = 0;
			auditEvent.EntityType = AuditEventEntityType.TestScriptScheduled;
			auditEvent.ActionType = AuditEventActionType.TestScriptScheduled;
			auditEvent.OutcomeType = AuditEventOutcomeType.Success;
			return true;
		}

		return false;
	}

	/// <summary>
	/// Imported/added LogicModule messages, which can embed full script or config payloads.
	/// </summary>
	private static bool TryHandleLogicModuleAdditionPrefix(string description, AuditEvent auditEvent)
	{
		foreach (var (prefix, entityType) in _logicModuleAdditionPrefixes)
		{
			if (!description.StartsWith(prefix, StringComparison.Ordinal))
			{
				continue;
			}

			auditEvent.MatchedRegExId = 0;
			auditEvent.EntityType = entityType;
			auditEvent.ActionType = AuditEventActionType.Create;
			auditEvent.OutcomeType = AuditEventOutcomeType.Success;
			auditEvent.LogicModuleName = ExtractNameBetweenSingleQuotes(description, prefix);
			return true;
		}

		if (description.StartsWith("Import EventSource from XML.", StringComparison.Ordinal))
		{
			auditEvent.MatchedRegExId = 0;
			auditEvent.EntityType = AuditEventEntityType.EventSource;
			auditEvent.ActionType = AuditEventActionType.Create;
			auditEvent.OutcomeType = AuditEventOutcomeType.Success;
			auditEvent.LogicModuleName = ExtractValueBetween(description, "\"LogicModuleName=", "\";");
			return true;
		}

		return false;
	}

	/// <summary>
	/// Website add/update messages raised via a URL check.
	/// </summary>
	private static bool TryHandleWebsitePrefix(string description, AuditEvent auditEvent)
	{
		if (!description.Contains(" via URL check", StringComparison.Ordinal))
		{
			return false;
		}

		foreach (var (prefix, actionType) in _websitePrefixes)
		{
			if (!description.StartsWith(prefix, StringComparison.Ordinal))
			{
				continue;
			}

			auditEvent.MatchedRegExId = 0;
			auditEvent.EntityType = AuditEventEntityType.Website;
			auditEvent.ActionType = actionType;
			auditEvent.OutcomeType = AuditEventOutcomeType.Success;
			var websiteName = ExtractValueBetween(description, prefix, " via URL check");
			if (!string.IsNullOrEmpty(websiteName))
			{
				auditEvent.WebsiteName = websiteName;
			}

			return true;
		}

		return false;
	}

	private static string GetPreview(string value, int maxLength)
	{
		var previewLength = Math.Min(value.Length, maxLength);
		return value
			.Substring(0, previewLength)
		 .Replace("\r", "\\r")
			.Replace("\n", "\\n");
	}
}
