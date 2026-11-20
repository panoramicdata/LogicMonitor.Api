namespace LogicMonitor.Api.Test.EventLogs;

/// <summary>
/// Tests for <see cref="LogItemExtensions.ToAuditEvent"/>.
/// </summary>
/// <remarks>
/// These parse fixed log strings and touch no portal, so the class deliberately takes no
/// <see cref="Fixture"/> - taking one would build a LogicMonitorClient and make the whole class
/// fail wherever portal credentials are absent, such as CI.
/// </remarks>
public partial class AuditEventTests
{
	private const string TestUsername = "test";
	private const string TestIpAddress = "127.0.0.1";

	[Fact]
	public void Create_DeviceFailure_Success()
		=> AssertToAuditEventSucceeds(
			@"""Action=Add""; ""Type=Device""; ""Device=ReportMagic alpha-Scheduler (0)""; ""Description=Failed device operation,  adddevice_failed : error  (invalid normal device name: ReportMagic alpha-Scheduler\n)""",
			new()
			{
				MatchedRegExId = 01,
				ActionType = AuditEventActionType.Create,
				EntityType = AuditEventEntityType.Resource,
				OutcomeType = AuditEventOutcomeType.Failure,
				ResourceIds = [0],
				ResourceNames = ["ReportMagic alpha-Scheduler"]
			}
		);

	[Fact]
	public void Update_DeviceDataSourceInstance_Disappeared_Success()
		=> AssertToAuditEventSucceeds(
			@"""Action=Update""; ""Type=Instance""; ""Device=NA""; ""InstanceId=NA""; ""Description=Instance(s) disappeared from: EXAMPLE-FW-01 (CollectorID=249) [DS--192.0.2.4]; """,
			new()
			{
				MatchedRegExId = 11,
				ActionType = AuditEventActionType.Update,
				EntityType = AuditEventEntityType.ResourceDataSourceInstance,
				OutcomeType = AuditEventOutcomeType.Success,
				Description = "Instance(s) disappeared from: EXAMPLE-FW-01 (CollectorID=249) [DS--192.0.2.4]; ",
				ResourceNames = ["NA"]
			}
		);

	[Fact]
	public void Update_DeviceDataSourceInstance_Changed_Success()
		=> AssertToAuditEventSucceeds(
			@"""Action=Update""; ""Type=Instance""; ""Device=NA""; ""InstanceName=NA""; ""Description=TemporaryReportName(s) changed for: EXAMPLE-K8S-TEST (CollectorID=297) [Critical Linux Processes-java from 9947 to 22713]; valueChanges=[deviceId=3271,dataSourceId=94545589,instanceChanges=[instanceId=263219850,oldValue=22713,newValue=9947];];'""",
			new()
			{
				MatchedRegExId = 6,
				ActionType = AuditEventActionType.Update,
				EntityType = AuditEventEntityType.ResourceDataSourceInstance,
				OutcomeType = AuditEventOutcomeType.Success,
				ResourceNames = ["NA"],
				ResourceIds = [3271],
				LogicModuleId = 94545589,
				InstanceId = 263219850,
				InstanceName = "NA"
			}
		);

	[Theory]
	[InlineData(
		"Update host<4030, docker-registry-deploy-default-EXAMPLE-K8S-PROD> (monitored by collector <295, example-k8s-prod-0>), ,  via API token API_TOKEN_EXAMPLE",
		3,
		4030,
		"docker-registry-deploy-default-EXAMPLE-K8S-PROD",
		295,
		"example-k8s-prod-0",
		"API_TOKEN_EXAMPLE"
		)]
	[InlineData(
"""
"Action=Update"; "Type=Device"; "Device=example-vm-124345 (124345) (1661057)"; "Description=(monitored by collector <1007, Workgroup\EXAMPLE-HOST>),   {
	[
		getAlertEnable: update value=false, old value=true.
	]
	}[Added property (system.categories:)]"
""",
		2,
		1661057,
		"example-vm-124345 (124345)",
		null,
		null,
		null
		)]
	public void Update_Device_Success(
		string message,
		int expectedRegexId,
		int expectedResourceId,
		string expectedResourceName,
		int? expectedCollectorId,
		string? expectedCollectorName,
		string? expectedTokenId)
		=> AssertToAuditEventSucceeds(
			message,
			new()
			{
				MatchedRegExId = expectedRegexId,
				ActionType = AuditEventActionType.Update,
				EntityType = AuditEventEntityType.Resource,
				OutcomeType = AuditEventOutcomeType.Success,
				ResourceIds = [expectedResourceId],
				ResourceNames = [expectedResourceName],
				CollectorId = expectedCollectorId,
				CollectorName = expectedCollectorName,
				ApiTokenId = expectedTokenId
			}
		);

	private static void AssertToAuditEventSucceeds(
		string description,
		AuditEvent expectedAuditEvent
		)
	{
		expectedAuditEvent.Id = Guid.NewGuid().ToString();
		expectedAuditEvent.PerformedByUsername = TestUsername;

		if (expectedAuditEvent.OutcomeType == AuditEventOutcomeType.None)
		{
			throw new InvalidDataException("Unit test does not have a valid AuditEventOutcomeType");
		}

		var nowUnixTimeStamp = DateTime.UtcNow.SecondsSinceTheEpoch();
		var logItem = new LogItem
		{
			HappenedOnTimeStampUtc = nowUnixTimeStamp,
			HappenedOnLocalString = "",
			Id = expectedAuditEvent.Id,
			IpAddress = TestIpAddress,
			SessionId = Guid.NewGuid().ToString(),
			PerformedByUsername = TestUsername,
			Description = description
		};

		var auditEvent = logItem.ToAuditEvent();
		auditEvent.DateTime.ToUnixTimeSeconds().Should().Be(nowUnixTimeStamp);
		auditEvent.PerformedByUsername.Should().Be(TestUsername);
		auditEvent.Host.Should().Be(TestIpAddress);
		auditEvent.OriginalDescription.Should().Be(description);
		auditEvent.OriginatorType.Should().Be(AuditEventOriginatorType.User);

		auditEvent
			.Should().BeEquivalentTo(
				expectedAuditEvent,
				opt => opt
					.Excluding(ae => ae.DateTime)
					.Excluding(ae => ae.Host)
					.Excluding(ae => ae.OriginalDescription)
					.Excluding(ae => ae.OriginatorType)
					.Excluding(ae => ae.SessionId)
			);
	}
}
