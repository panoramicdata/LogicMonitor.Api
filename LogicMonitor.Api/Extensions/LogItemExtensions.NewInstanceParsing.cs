namespace LogicMonitor.Api.Extensions;

public static partial class LogItemExtensions
{
	private static readonly (string Header, AuditEventActionType ActionType)[] _newInstancesHeaders =
	[
		("\"Action=Add" + NewInstancesHeaderSuffix, AuditEventActionType.Create),
		("\"Action=Fetch" + NewInstancesHeaderSuffix, AuditEventActionType.Read),
		("\"Action=Update" + NewInstancesHeaderSuffix, AuditEventActionType.Update),
	];

	private static bool TryReadNewInstancesHeader(ReadOnlySpan<char> text, out AuditEventActionType actionType, out int position)
	{
		foreach (var (header, headerActionType) in _newInstancesHeaders)
		{
			if (text.StartsWith(header, StringComparison.Ordinal))
			{
				actionType = headerActionType;
				position = header.Length;
				return true;
			}
		}

		actionType = AuditEventActionType.None;
		position = 0;
		return false;
	}

	/// <summary>
	/// Reads "{resourceName} (CollectorID={collectorId}) [" starting at <paramref name="position"/>,
	/// which is left just after the opening bracket. The resource name must be non-empty.
	/// </summary>
	private static bool TryReadResourceAndCollector(
		ReadOnlySpan<char> text,
		ref int position,
		int limit,
		out int resourceNameEnd,
		out int collectorId)
	{
		collectorId = 0;
		resourceNameEnd = IndexOf(text, CollectorIdMarker, position + 1, limit);
		if (resourceNameEnd < 0)
		{
			return false;
		}

		var collectorIdStart = resourceNameEnd + CollectorIdMarker.Length;
		var collectorIdEnd = collectorIdStart;
		while (collectorIdEnd < limit && (char.IsAsciiDigit(text[collectorIdEnd]) || text[collectorIdEnd] == '-'))
		{
			collectorIdEnd++;
		}

		if (!int.TryParse(text[collectorIdStart..collectorIdEnd], NumberStyles.AllowLeadingSign, CultureInfo.InvariantCulture, out collectorId)
			|| !text[collectorIdEnd..limit].StartsWith(InstanceListStart, StringComparison.Ordinal))
		{
			return false;
		}

		position = collectorIdEnd + InstanceListStart.Length;
		return true;
	}

	/// <summary>
	/// Reads the trailing ids block, which runs from <paramref name="idsMarkerIndex"/> to the end of
	/// <paramref name="text"/>. Deleted instance ids are only present (non-null) in the combined form.
	/// </summary>
	private static bool TryReadInstanceIds(ReadOnlySpan<char> text, int idsMarkerIndex, out NewInstancesIds ids)
	{
		ids = default;
		var position = idsMarkerIndex + InstanceIdsMarker.Length;

		if (!TryReadNumber(text, ref position, out var resourceId)
			|| !TryReadLiteral(text, ref position, DataSourceIdMarker)
			|| !TryReadNumber(text, ref position, out var logicModuleId)
			|| !TryReadLiteral(text, ref position, NewInstanceIdsMarker)
			|| !TryReadIdLists(text, position, out var newInstanceIds, out var deletedInstanceIds))
		{
			return false;
		}

		ids = new(resourceId, logicModuleId, newInstanceIds, deletedInstanceIds);
		return true;
	}

	/// <summary>
	/// Reads "{newIds}];\"" or "{newIds},dataSourceDeletedInstanceId(s)={deletedIds}];\"", which must
	/// run to the end of <paramref name="text"/>.
	/// </summary>
	private static bool TryReadIdLists(
		ReadOnlySpan<char> text,
		int idsStart,
		[NotNullWhen(true)] out List<int>? newInstanceIds,
		out List<int>? deletedInstanceIds)
	{
		newInstanceIds = null;
		deletedInstanceIds = null;
		var idsEnd = SkipIdListCharacters(text, idsStart);

		if (text[idsEnd..].SequenceEqual(InstanceIdsEnd))
		{
			// Single form: ...,dataSourceNewInstanceId(s)={ids}];"
			return TryParseIdList(text[idsStart..idsEnd], out newInstanceIds);
		}

		// Combined form: ...,dataSourceNewInstanceId(s)={ids},dataSourceDeletedInstanceId(s)={ids}];"
		// The id-list scan will have consumed the comma that begins the deleted-ids marker.
		if (idsEnd <= idsStart || !text[(idsEnd - 1)..].StartsWith(DeletedInstanceIdsMarker, StringComparison.Ordinal))
		{
			return false;
		}

		var deletedIdsStart = idsEnd - 1 + DeletedInstanceIdsMarker.Length;
		var deletedIdsEnd = SkipIdListCharacters(text, deletedIdsStart);
		return text[deletedIdsEnd..].SequenceEqual(InstanceIdsEnd)
			&& TryParseIdList(text[idsStart..(idsEnd - 1)], out newInstanceIds)
			&& TryParseIdList(text[deletedIdsStart..deletedIdsEnd], out deletedInstanceIds);
	}
}
