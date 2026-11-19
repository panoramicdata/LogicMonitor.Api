namespace LogicMonitor.Api.Extensions;

public static partial class LogItemExtensions
{
	/// <summary>
	/// Parses an Active Discovery "Found new instance(s)" description, including the combined
	/// "...; Instance(s) disappeared from: ..." form, without regular expressions. On success it
	/// populates exactly the fields that regex 8 (or 9, for the combined form) would, sets
	/// MatchedRegExId to that id and returns true. If the description does not have that shape it
	/// returns false and leaves <paramref name="auditEvent"/> untouched.
	/// </summary>
	/// <remarks>
	/// Each delimiter is located with a single forward or backward search, so the cost is linear
	/// in the description length however many instances are listed. Delimiters are taken at the
	/// same occurrence the regex's lazy groups would choose. The one difference is that a value
	/// which itself contains a delimiter (a resource name containing " (CollectorID=", say) is not
	/// retried further along: such a description is rejected, rather than parsed differently.
	/// </remarks>
	internal static bool TryHandleNewInstancesDescription(string description, AuditEvent auditEvent)
	{
		// The regex's trailing "$" also matches just before a final newline
		var length = description.EndsWith('\n') ? description.Length - 1 : description.Length;
		var text = description.AsSpan(0, length);

		// "Action=Update"; "Type=Instance"; "Device=NA"; "InstanceName={instanceName}"; "Description=Found new instance(s) for: {resourceName} (CollectorID={collectorId}) [
		if (!TryReadNewInstancesHeader(text, out var actionType, out var position)
			|| !TryReadInstanceAndResource(description, text, ref position, out var header))
		{
			return false;
		}

		// ...]; New_InstanceIds=[deviceId={resourceId},dataSourceId={logicModuleId},dataSourceNewInstanceId(s)={ids}...];"
		if (!TryReadInstanceIds(text, header.IdsMarkerIndex, out var ids))
		{
			return false;
		}

		// {newNames}] or, in the combined form, {newNames}]; Instance(s) disappeared from: ... [{deletedNames}]
		if (!TryReadInstanceNames(description, text, position, header.IdsMarkerIndex, ids.DeletedInstanceIds is not null, out var newInstanceNames, out var deletedInstanceNames))
		{
			return false;
		}

		auditEvent.MatchedRegExId = ids.DeletedInstanceIds is null ? 8 : 9;
		auditEvent.EntityType = AuditEventEntityType.ResourceDataSourceInstance;
		auditEvent.ActionType = actionType;
		auditEvent.OutcomeType = AuditEventOutcomeType.Success;
		auditEvent.InstanceName = header.InstanceName;
		auditEvent.CollectorId = header.CollectorId;
		auditEvent.LogicModuleId = ids.LogicModuleId;
		auditEvent.ResourceIds = [ids.ResourceId];
		auditEvent.ResourceNames = [header.ResourceName];
		auditEvent.DataSourceNewInstanceNames = newInstanceNames;
		auditEvent.DataSourceNewInstanceIds = ids.NewInstanceIds;
		auditEvent.DataSourceDeletedInstanceNames = deletedInstanceNames;
		auditEvent.DataSourceDeletedInstanceIds = ids.DeletedInstanceIds;
		ApplyRegexIdMetadata(auditEvent);
		return true;
	}

	private readonly record struct NewInstancesHeader(string InstanceName, string ResourceName, int CollectorId, int IdsMarkerIndex);

	private readonly record struct NewInstancesIds(int ResourceId, int LogicModuleId, List<int> NewInstanceIds, List<int>? DeletedInstanceIds);

	/// <summary>
	/// Reads "{instanceName}"; "Description=Found new instance(s) for: {resourceName} (CollectorID={collectorId}) ["
	/// and locates the trailing ids block. <paramref name="position"/> is left at the start of the new instance names.
	/// </summary>
	private static bool TryReadInstanceAndResource(string description, ReadOnlySpan<char> text, ref int position, out NewInstancesHeader header)
	{
		header = default;
		var instanceNameEnd = IndexOf(text, NewInstancesDescriptionMarker, position + 1, text.Length);
		if (instanceNameEnd < 0)
		{
			return false;
		}

		var instanceName = description[position..instanceNameEnd];
		var resourceNameStart = instanceNameEnd + NewInstancesDescriptionMarker.Length;

		// The ids block is fixed-format and cannot contain its own marker, so it starts at the last one
		var idsMarkerIndex = text.LastIndexOf(InstanceIdsMarker, StringComparison.Ordinal);
		if (idsMarkerIndex < resourceNameStart)
		{
			return false;
		}

		var namesStart = resourceNameStart;
		if (!TryReadResourceAndCollector(text, ref namesStart, idsMarkerIndex, out var resourceNameEnd, out var collectorId))
		{
			return false;
		}

		position = namesStart;
		header = new(instanceName, description[resourceNameStart..resourceNameEnd], collectorId, idsMarkerIndex);
		return true;
	}

	/// <summary>
	/// Reads the new instance names, and in the combined form the deleted instance names, which lie
	/// between <paramref name="namesStart"/> and the ids block. Each list must be non-empty.
	/// </summary>
	private static bool TryReadInstanceNames(
		string description,
		ReadOnlySpan<char> text,
		int namesStart,
		int idsMarkerIndex,
		bool isCombinedForm,
		[NotNullWhen(true)] out List<string>? newInstanceNames,
		out List<string>? deletedInstanceNames)
	{
		newInstanceNames = null;
		deletedInstanceNames = null;
		if (!isCombinedForm)
		{
			if (namesStart >= idsMarkerIndex)
			{
				return false;
			}

			newInstanceNames = SplitOnCommas(description, namesStart, idsMarkerIndex);
			return true;
		}

		// {newNames}]; Instance(s) disappeared from: {resourceName2} (CollectorID={collectorId2}) [{deletedNames}
		var newNamesEnd = IndexOf(text, DisappearedMarker, namesStart + 1, idsMarkerIndex);
		if (newNamesEnd < 0)
		{
			return false;
		}

		var deletedNamesStart = newNamesEnd + DisappearedMarker.Length;
		if (!TryReadResourceAndCollector(text, ref deletedNamesStart, idsMarkerIndex, out _, out _)
			|| deletedNamesStart >= idsMarkerIndex)
		{
			return false;
		}

		newInstanceNames = SplitOnCommas(description, namesStart, newNamesEnd);
		deletedInstanceNames = SplitOnCommas(description, deletedNamesStart, idsMarkerIndex);
		return true;
	}

	private const string NewInstancesHeaderSuffix = "\"; \"Type=Instance\"; \"Device=NA\"; \"InstanceName=";
	private const string NewInstancesDescriptionMarker = "\"; \"Description=Found new instance(s) for: ";
	private const string CollectorIdMarker = " (CollectorID=";
	private const string InstanceListStart = ") [";
	private const string DisappearedMarker = "]; Instance(s) disappeared from: ";
	private const string InstanceIdsMarker = "]; New_InstanceIds=[deviceId=";
	private const string DataSourceIdMarker = ",dataSourceId=";
	private const string NewInstanceIdsMarker = ",dataSourceNewInstanceId(s)=";
	private const string DeletedInstanceIdsMarker = ",dataSourceDeletedInstanceId(s)=";
	private const string InstanceIdsEnd = "];\"";
}
