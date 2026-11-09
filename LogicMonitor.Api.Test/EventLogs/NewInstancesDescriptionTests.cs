using System.Globalization;

namespace LogicMonitor.Api.Test.EventLogs;

/// <summary>
/// Tests for the non-regex parsing of Active Discovery "Found new instance(s)" descriptions,
/// which <see cref="LogItemExtensions"/> uses when a description is longer than
/// <see cref="LogItemExtensions.MaxRegexDescriptionLength"/>. The result must be indistinguishable
/// from what regex 8 (or 9, for the combined new-and-disappeared form) produces.
/// </summary>
/// <remarks>
/// These parse generated strings and touch no portal, so the class deliberately takes no fixture.
/// </remarks>
public class NewInstancesDescriptionTests
{
	private const string ResourceName = "example-host-01.example.com";
	private const int ResourceId = 4242;
	private const int LogicModuleId = 987654;

	/// <summary>
	/// A DataSource whose name ends in '-', so its instances read "ExampleDiskIO--{instance}".
	/// </summary>
	private const string DoubledDashDataSourceName = "ExampleDiskIO-";

	private static LogItem CreateLogItem(string description) => new()
	{
		Id = "new-instances-description-test",
		PerformedByUsername = "System:ActiveDiscovery",
		IpAddress = "127.0.0.1",
		Description = description
	};

	private static string CreateDescription(
		IReadOnlyList<string> newInstanceNames,
		IReadOnlyList<int> newInstanceIds,
		IReadOnlyList<string>? deletedInstanceNames = null,
		IReadOnlyList<int>? deletedInstanceIds = null,
		string action = "Update",
		int collectorId = 17)
	{
		var builder = new StringBuilder()
			.Append(CultureInfo.InvariantCulture, $@"""Action={action}""; ""Type=Instance""; ""Device=NA""; ""InstanceName=NA""; ""Description=Found new instance(s) for: {ResourceName} (CollectorID={collectorId}) [")
			.AppendJoin(',', newInstanceNames)
			.Append(']');

		if (deletedInstanceNames is not null)
		{
			builder
				.Append(CultureInfo.InvariantCulture, $"; Instance(s) disappeared from: {ResourceName} (CollectorID={collectorId}) [")
				.AppendJoin(',', deletedInstanceNames)
				.Append(']');
		}

		builder
			.Append(CultureInfo.InvariantCulture, $"; New_InstanceIds=[deviceId={ResourceId},dataSourceId={LogicModuleId},dataSourceNewInstanceId(s)=")
			.AppendJoin(',', newInstanceIds);

		if (deletedInstanceIds is not null)
		{
			builder
				.Append(",dataSourceDeletedInstanceId(s)=")
				.AppendJoin(',', deletedInstanceIds);
		}

		return builder.Append("];\"").ToString();
	}

	private static List<string> CreateInstanceNames(int count, int paddingLength = 0, string dataSourceName = DoubledDashDataSourceName)
		=> [.. Enumerable.Range(0, count).Select(i => $"{dataSourceName}-sd{i:D5}{new string('x', paddingLength)}")];

	private static List<int> CreateInstanceIds(int count, int firstId = 500_000_000)
		=> [.. Enumerable.Range(firstId, count)];

	/// <summary>
	/// Runs the description through the fast path directly, even though it is short enough for
	/// ToAuditEvent to use the regular expressions.
	/// </summary>
	private static AuditEvent ParseWithFastPath(LogItem logItem)
	{
		var auditEvent = LogItemExtensions.CreateAuditEvent(logItem);
		LogItemExtensions.TryHandleNewInstancesDescription(logItem.Description, auditEvent).Should().BeTrue();
		return auditEvent;
	}

	private static void AssertNewInstancesEvent(
		AuditEvent auditEvent,
		int expectedRegexId,
		List<string> expectedNewNames,
		List<int> expectedNewIds,
		List<string>? expectedDeletedNames = null,
		List<int>? expectedDeletedIds = null)
	{
		auditEvent.MatchedRegExId.Should().Be(expectedRegexId);
		auditEvent.EntityType.Should().Be(AuditEventEntityType.ResourceDataSourceInstance);
		auditEvent.ActionType.Should().Be(AuditEventActionType.Update);
		auditEvent.OutcomeType.Should().Be(AuditEventOutcomeType.Success);
		auditEvent.Description.Should().BeNull();
		auditEvent.InstanceName.Should().Be("NA");
		auditEvent.CollectorId.Should().Be(17);
		auditEvent.LogicModuleId.Should().Be(LogicModuleId);
		auditEvent.ResourceIds.Should().Equal([ResourceId]);
		auditEvent.ResourceNames.Should().Equal([ResourceName]);
		auditEvent.DataSourceNewInstanceNames.Should().Equal(expectedNewNames);
		auditEvent.DataSourceNewInstanceIds.Should().Equal(expectedNewIds);

		if (expectedDeletedNames is null)
		{
			auditEvent.DataSourceDeletedInstanceNames.Should().BeNull();
			auditEvent.DataSourceDeletedInstanceIds.Should().BeNull();
		}
		else
		{
			auditEvent.DataSourceDeletedInstanceNames.Should().Equal(expectedDeletedNames);
			auditEvent.DataSourceDeletedInstanceIds.Should().Equal(expectedDeletedIds);
		}
	}

	public static TheoryData<string, int> ShortDescriptions => new()
	{
		// Single instance
		{ CreateDescription(["Example Service-alpha"], [101]), 8 },
		// Several instances, a DataSource name ending in '-' and a negative collector id
		{ CreateDescription(CreateInstanceNames(5), CreateInstanceIds(5), collectorId: -2), 8 },
		// Instance names containing brackets, parentheses and a "]" followed by a space
		{ CreateDescription(["Example Volume-disk[0] (primary)", "Example Volume-disk] 1"], [7, 8]), 8 },
		// A comma inside an instance name is split exactly as the regex path splits it
		{ CreateDescription(["Example Port-eth0, uplink", "Example Port-eth1"], [1, 2]), 8 },
		// Each action the regex accepts
		{ CreateDescription(["Example Service-alpha"], [101], action: "Add"), 8 },
		{ CreateDescription(["Example Service-alpha"], [101], action: "Fetch"), 8 },
		// Combined new-and-disappeared form
		{ CreateDescription(CreateInstanceNames(3), CreateInstanceIds(3), CreateInstanceNames(2, 1), CreateInstanceIds(2, 400_000_000)), 9 },
		{ CreateDescription(["Example Service-alpha"], [101], ["Example Service-beta"], [102], collectorId: -2), 9 },
	};

	[Theory]
	[MemberData(nameof(ShortDescriptions))]
	public void FastPath_ShortDescription_MatchesRegexPath(string description, int expectedRegexId)
	{
		description.Length.Should().BeLessThanOrEqualTo(LogItemExtensions.MaxRegexDescriptionLength);
		var logItem = CreateLogItem(description);

		var viaRegex = logItem.ToAuditEvent();
		var viaFastPath = ParseWithFastPath(logItem);

		viaRegex.MatchedRegExId.Should().Be(expectedRegexId);
		viaFastPath.Should().BeEquivalentTo(viaRegex, options => options.WithStrictOrdering());
	}

	[Fact]
	public void ToAuditEvent_NewInstancesJustOverLimit_IsParsed()
	{
		// Grow the instance list until the description is just over the limit
		var count = 1;
		while (CreateDescription(CreateInstanceNames(count), CreateInstanceIds(count)).Length <= LogItemExtensions.MaxRegexDescriptionLength)
		{
			count++;
		}

		var names = CreateInstanceNames(count);
		var ids = CreateInstanceIds(count);
		var description = CreateDescription(names, ids);
		description.Length.Should().BeInRange(LogItemExtensions.MaxRegexDescriptionLength + 1, LogItemExtensions.MaxRegexDescriptionLength + 100);

		var auditEvent = CreateLogItem(description).ToAuditEvent();

		AssertNewInstancesEvent(auditEvent, 8, names, ids);
	}

	[Fact]
	public void ToAuditEvent_NewInstancesOverOneMegabyte_IsParsedQuickly()
	{
		// Warm up so that JIT compilation is not timed
		_ = CreateLogItem(CreateDescription(CreateInstanceNames(2_000), CreateInstanceIds(2_000))).ToAuditEvent();

		const int instanceCount = 5_000;
		var names = CreateInstanceNames(instanceCount, paddingLength: 180);
		var ids = CreateInstanceIds(instanceCount);
		var logItem = CreateLogItem(CreateDescription(names, ids));
		logItem.Description.Length.Should().BeGreaterThan(1_000_000);

		var stopwatch = Stopwatch.StartNew();
		var auditEvent = logItem.ToAuditEvent();
		stopwatch.Stop();

		AssertNewInstancesEvent(auditEvent, 8, names, ids);
		stopwatch.Elapsed.Should().BeLessThan(TimeSpan.FromSeconds(1));
	}

	[Fact]
	public void ToAuditEvent_NewAndDisappearedInstancesOverLimit_IsParsed()
	{
		var newNames = CreateInstanceNames(1_500, paddingLength: 20);
		var newIds = CreateInstanceIds(1_500);
		var deletedNames = CreateInstanceNames(400, paddingLength: 10, dataSourceName: "Example Interfaces");
		var deletedIds = CreateInstanceIds(400, 400_000_000);
		var description = CreateDescription(newNames, newIds, deletedNames, deletedIds);
		description.Length.Should().BeGreaterThan(LogItemExtensions.MaxRegexDescriptionLength);

		var auditEvent = CreateLogItem(description).ToAuditEvent();

		AssertNewInstancesEvent(auditEvent, 9, newNames, newIds, deletedNames, deletedIds);
	}

	public static TheoryData<string> MalformedOversizedDescriptions
	{
		get
		{
			var valid = CreateDescription(CreateInstanceNames(2_000), CreateInstanceIds(2_000));
			return new()
			{
				// No New_InstanceIds block, which regexes 8 and 9 require (a short one falls through
				// to the generic regex 77, which lists no instances)
				valid[..valid.IndexOf("; New_InstanceIds=", StringComparison.Ordinal)] + "\"",
				// Missing closing quote
				valid[..^1],
				// Unexpected text after the ids block
				valid + " ",
				// An empty id in the list
				valid.Replace("dataSourceNewInstanceId(s)=", "dataSourceNewInstanceId(s)=,", StringComparison.Ordinal),
				// A device other than NA
				valid.Replace("\"Device=NA\"", "\"Device=example\"", StringComparison.Ordinal),
				// An action the regex does not accept
				valid.Replace("\"Action=Update\"", "\"Action=Delete\"", StringComparison.Ordinal),
				// No collector id
				valid.Replace(" (CollectorID=17)", string.Empty, StringComparison.Ordinal),
			};
		}
	}

	[Theory]
	[MemberData(nameof(MalformedOversizedDescriptions))]
	public void ToAuditEvent_MalformedNewInstancesOverLimit_IsSkippedWithExplanation(string description)
	{
		description.Length.Should().BeGreaterThan(LogItemExtensions.MaxRegexDescriptionLength);

		var auditEvent = CreateLogItem(description).ToAuditEvent();

		auditEvent.MatchedRegExId.Should().Be(0);
		auditEvent.EntityType.Should().Be(AuditEventEntityType.None);
		auditEvent.DataSourceNewInstanceNames.Should().BeNull();
		auditEvent.Description.Should().StartWith($"Message not handled because description length ({description.Length}) exceeded max regex length ({LogItemExtensions.MaxRegexDescriptionLength}).");
	}

	[Fact]
	public void ToAuditEvent_OtherOversizedDescription_IsStillSkippedWithExplanation()
	{
		var description = $@"""Action=Update""; ""Type=Device""; ""Device=example-host (123)""; ""Description={new string('x', 40 * 1024)}""";

		var auditEvent = CreateLogItem(description).ToAuditEvent();

		auditEvent.MatchedRegExId.Should().Be(0);
		auditEvent.EntityType.Should().Be(AuditEventEntityType.None);
		auditEvent.Description.Should().StartWith($"Message not handled because description length ({description.Length}) exceeded max regex length ({LogItemExtensions.MaxRegexDescriptionLength}).");
	}
}
