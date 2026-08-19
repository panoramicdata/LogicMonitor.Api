namespace LogicMonitor.Api.Test.EventLogs;

/// <summary>
/// Tests for the maximum description length guard in <see cref="LogItemExtensions"/>.
/// Descriptions longer than the limit are deliberately skipped before regex processing
/// (DataSource update messages can be enormous and would stall parsing), returning a
/// blank AuditEvent with MatchedRegExId 0 and an explanatory Description.
/// </summary>
public class OversizedDescriptionTests
{
	private const int ExpectedMaxRegexDescriptionLength = 32 * 1024;

	private static LogItem CreateLogItem(string description) => new()
	{
		Id = "oversized-description-test",
		Description = description
	};

	private static string CreateDeviceUpdateDescription(int descriptionFieldLength)
		=> $@"""Action=Update""; ""Type=Device""; ""Device=TestDevice (123)""; ""Description={new string('x', descriptionFieldLength)}""";

	[Fact]
	public void ToAuditEvent_DescriptionOver16KButUnderLimit_IsParsedByRegex()
	{
		// 20K total: over the old 16K limit, under the 32K limit
		var description = CreateDeviceUpdateDescription(20 * 1024);

		var auditEvent = CreateLogItem(description).ToAuditEvent();

		auditEvent.MatchedRegExId.Should().Be(2);
		auditEvent.EntityType.Should().Be(AuditEventEntityType.Resource);
		auditEvent.ActionType.Should().Be(AuditEventActionType.Update);
		auditEvent.ResourceIds.Should().Equal([123]);
	}

	[Fact]
	public void ToAuditEvent_DescriptionOverLimit_IsSkippedWithExplanation()
	{
		var description = CreateDeviceUpdateDescription(33 * 1024);

		var auditEvent = CreateLogItem(description).ToAuditEvent();

		auditEvent.MatchedRegExId.Should().Be(0);
		auditEvent.EntityType.Should().Be(AuditEventEntityType.None);
		auditEvent.Description.Should().Contain($"exceeded max regex length ({ExpectedMaxRegexDescriptionLength})");
		auditEvent.Description.Should().Contain($"description length ({description.Length})");
	}

	[Fact]
	public void MaxRegexDescriptionLength_IsPubliclyVisibleWithExpectedValue()
		// Consumers (e.g. the NTT EventProcessor) log this limit when reporting size-skipped
		// messages, so it must be public and must match the guard's actual behaviour.
		=> LogItemExtensions.MaxRegexDescriptionLength.Should().Be(ExpectedMaxRegexDescriptionLength);
}
