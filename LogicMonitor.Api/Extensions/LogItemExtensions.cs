namespace LogicMonitor.Api.Extensions;

/// <summary>
/// Log item extensions
/// </summary>
public static partial class LogItemExtensions
{
	/// <summary>
	/// The maximum LogItem description length, in characters, that ToAuditEvent will process
	/// with regular expressions. Longer descriptions are deliberately skipped to protect
	/// processing time (DataSource update messages can be enormous), returning an AuditEvent
	/// with MatchedRegExId 0, EntityType None and an explanatory Description. The exceptions are
	/// resource group updates containing "getExtra: update value=", and Active Discovery
	/// "Found new instance(s)" messages, which are parsed without running the full regex set.
	/// Public so that consumers can report the limit when logging size-skipped messages.
	/// A property rather than a const so the value is not compile-time inlined into consumers.
	/// </summary>
	public static int MaxRegexDescriptionLength { get; } = 32 * 1024;
	private const int OversizedDescriptionPreviewLength = 512;

	/// <summary>
	/// "Add new &lt;module type&gt; '" prefixes, each of which names the module between single quotes.
	/// </summary>
	private static readonly (string Prefix, AuditEventEntityType EntityType)[] _logicModuleAdditionPrefixes =
	[
		("Add new DataSource '", AuditEventEntityType.DataSource),
		("Add new PropertySource '", AuditEventEntityType.PropertySource),
		("Add new TopologySource '", AuditEventEntityType.TopologySource),
	];

	private static readonly (string Prefix, AuditEventActionType ActionType)[] _websitePrefixes =
	[
		("Add website ", AuditEventActionType.Create),
		("Update website ", AuditEventActionType.Update),
	];

	/// <summary>
	/// Action and outcome types implied by the matched pattern id alone. A null entry means
	/// "leave whatever the description itself yielded".
	/// </summary>
	private static readonly Dictionary<int, (AuditEventActionType? ActionType, AuditEventOutcomeType? OutcomeType)> _regexIdMetadata = new()
	{
		[22] = (AuditEventActionType.GeneralApi, AuditEventOutcomeType.Failure),
		[27] = (AuditEventActionType.Update, null),
		[36] = (AuditEventActionType.GeneralApi, null),
		[37] = (AuditEventActionType.GeneralApi, AuditEventOutcomeType.Failure),
		[41] = (AuditEventActionType.GeneralApi, null),
		[69] = (null, AuditEventOutcomeType.Failure),
		[70] = (AuditEventActionType.Update, null),
		[71] = (AuditEventActionType.Update, null),
		[72] = (AuditEventActionType.Update, null),
		[94] = (AuditEventActionType.Update, null),
		[95] = (AuditEventActionType.Run, null),
		[98] = (AuditEventActionType.Run, AuditEventOutcomeType.Failure),
		[105] = (AuditEventActionType.Update, null),
		[109] = (AuditEventActionType.Update, null),
	};

	internal static void ValidateRegexes()
	{
		// Check that no two Regex have the same id
		var regexIds = new HashSet<int>();
		foreach (var regex in _regexs)
		{
			if (!regexIds.Add(regex.Id))
			{
				throw new InvalidDataException($"Duplicate regex id {regex.Id} found");
			}
		}
	}

	/// <summary>
	/// Converts a logItem to an AuditItem
	/// </summary>
	/// <param name="logItem"></param>
	public static AuditEvent ToAuditEvent(this LogItem logItem)
	{
		ArgumentNullException.ThrowIfNull(logItem);

		var auditEvent = CreateAuditEvent(logItem);

		// Some messages carry very large payloads (imported module definitions, script bodies) or
		// are otherwise not worth running the full regex set over. Those are recognised purely by
		// their prefix and returned immediately.
		if (TryHandleByPrefix(logItem.Description, auditEvent))
		{
			return auditEvent;
		}

		// Active Discovery "Found new instance(s)" messages list every new instance on a device and
		// can run to over a megabyte. Too long for the regex set, they are parsed by plain scanning
		// into exactly the fields regex 8 or 9 would have produced.
		if (logItem.Description.Length > MaxRegexDescriptionLength
			&& TryHandleNewInstancesDescription(logItem.Description, auditEvent))
		{
			return auditEvent;
		}

		// Interpret the description field
		if (!TryGetEntityTypeMatch(logItem.Description, auditEvent, out var logItemRegex, out var match))
		{
			return auditEvent;
		}

		auditEvent.MatchedRegExId = logItemRegex.Id;
		// Have we determined the EntityType already?
		auditEvent.EntityType = logItemRegex.EntityType;

		auditEvent.ActionType = GetAction(match);
		auditEvent.OutcomeType = match.Groups["failed"].Success ? AuditEventOutcomeType.Failure : AuditEventOutcomeType.Success;

		// Add metadata that can't be extracted from the description using the regex
		ApplyRegexIdMetadata(auditEvent);

		PopulateFromMatchGroups(auditEvent, match);
		PopulateResources(auditEvent, match);
		PopulateAffectedInstances(auditEvent, match);

		return auditEvent;
	}

	internal static AuditEvent CreateAuditEvent(LogItem logItem)
		=> new()
		{
			Id = logItem.Id,
			DateTime = logItem.HappenedOnUtc,
			PerformedByUsername = logItem.PerformedByUsername,
			Host = logItem.IpAddress,
			OriginalDescription = logItem.Description,
			SessionId = logItem.SessionId,
			OriginatorType =
				logItem.PerformedByUsername.StartsWith("System:", StringComparison.Ordinal) ? AuditEventOriginatorType.System :
				logItem.PerformedByUsername == "k8smonitoring" ? AuditEventOriginatorType.CollectorKubernetes :
				logItem.PerformedByUsername == "lmsupport" ? AuditEventOriginatorType.CollectorOther :
				AuditEventOriginatorType.User,
		};

	/// <summary>
	/// Chooses the regular expression that describes <paramref name="description"/>. Returns false
	/// if the description is unrecognised, or is too long to run the regex set over - in the latter
	/// case <paramref name="auditEvent"/> is populated with an explanatory Description. Oversized
	/// "Found new instance(s)" messages never reach here: ToAuditEvent parses them beforehand.
	/// </summary>
	private static bool TryGetEntityTypeMatch(
		string description,
		AuditEvent auditEvent,
		[NotNullWhen(true)] out LogItemRegex? logItemRegex,
		[NotNullWhen(true)] out Match? match)
	{
		if (description.Length > MaxRegexDescriptionLength)
		{
			if (TryGetGroupUpdateWithGetExtraMatch(description, out var getExtraMatch))
			{
				logItemRegex = _groupActionLogItemRegex;
				match = getExtraMatch;
				return true;
			}

			auditEvent.MatchedRegExId = 0;
			auditEvent.EntityType = AuditEventEntityType.None;
			auditEvent.Description =
				$"Message not handled because description length ({description.Length}) exceeded max regex length ({MaxRegexDescriptionLength}). Preview: {GetPreview(description, OversizedDescriptionPreviewLength)}";
			logItemRegex = null;
			match = null;
			return false;
		}

		// Not recognised leaves both out parameters null
		(logItemRegex, match) = GetMatchFromDescription(description);
		return logItemRegex is not null && match is not null;
	}

	/// <summary>
	/// Applies the action and outcome types that are implied by the matched pattern id rather than
	/// by anything captured from the description itself.
	/// </summary>
	private static void ApplyRegexIdMetadata(AuditEvent auditEvent)
	{
		if (!_regexIdMetadata.TryGetValue(auditEvent.MatchedRegExId, out var metadata))
		{
			// Most patterns do not imply an action or outcome type; those are left as found.
			return;
		}

		if (metadata.ActionType is not null)
		{
			auditEvent.ActionType = metadata.ActionType.Value;
		}

		if (metadata.OutcomeType is not null)
		{
			auditEvent.OutcomeType = metadata.OutcomeType.Value;
		}
	}
}
