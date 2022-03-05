namespace LogicMonitor.Api.Test.LogicModules;

public class AuditEventTests : TestWithOutput
{
	public AuditEventTests(ITestOutputHelper iTestOutputHelper) : base(iTestOutputHelper)
	{
	}

	[Theory]
	[InlineData(
		"\"Action=Add\"; \"Type=Device\"; \"Device=ReportMagic alpha-Scheduler (0)\"; \"Description=Failed device operation,  adddevice_failed : error  (invalid normal device name: ReportMagic alpha-Scheduler\n)\"",
		AuditEventActionType.Create,
		AuditEventEntityType.Resource,
		AuditEventOutcomeType.Failure
		)]
	[InlineData(
		@"""Action=Update""; ""Type=Instance""; ""Device=NA""; ""InstanceId=NA""; ""Description=Instance(s) disappeared from: PDL-FW-01 (CollectorID=249) [DS--1.2.3.4]; """,
		AuditEventActionType.Update,
		AuditEventEntityType.DeviceDataSourceInstance,
		AuditEventOutcomeType.Success
		)]
	[InlineData(
		@"""Action=Update""; ""Type=Instance""; ""Device=NA""; ""InstanceName=NA""; ""Description=Value(s) changed for: pdl-k8s-test-03.panoramicdata.com-node-PDL-K8S-TEST (CollectorID=297) [Critical Linux Processes-java from 9947 to 22713]; valueChanges=[deviceId=3271,dataSourceId=94545589,instanceChanges=[instanceId=263219850,oldValue=22713,newValue=9947];];'""",
		AuditEventActionType.Update,
		AuditEventEntityType.DeviceDataSourceInstance,
		AuditEventOutcomeType.Success
		)]
	[InlineData(
		"Update host<4030, docker-registry-deploy-default-PDL-K8S-PROD> (monitored by collector <295, pdl-k8s-prod-0>), ,  via API token MZkW3Ldwg5S84s5eWUc7",
		AuditEventActionType.Update,
		AuditEventEntityType.Resource,
		AuditEventOutcomeType.Success
		)]
	[InlineData(
		@"""Action=Update""; ""Type=Instance""; ""Device=NA""; ""InstanceName=NA""; ""Description=Found new instance(s) for: PDL-LM-circleitsolutions.logicmonitor.com (CollectorID=249) [LogicMonitor_Portal_DataSources-Win_WMI_UACTroubleshooter]; New_InstanceIds=[deviceId=2781,dataSourceId=112813425,dataSourceNewInstanceId(s)=263395102];""",
		AuditEventActionType.Update,
		AuditEventEntityType.DeviceDataSourceInstance,
		AuditEventOutcomeType.Success
		)]
	[InlineData(
		@"""Action=Update""; ""Type=Instance""; ""Device=NA""; ""InstanceName=NA""; ""Description=Instance(s) disappeared from: PDL-K8S-TEST-03 (CollectorID=249) [Critical Linux Processes-java]; New_InstanceIds=[deviceId=1525,dataSourceId=94545589,dataSourceDeletedInstanceId(s)=263219849];""",
		AuditEventActionType.Update,
		AuditEventEntityType.DeviceDataSourceInstance,
		AuditEventOutcomeType.Success
		)]
	public void ToAuditEvent_Succeeds(
		string description,
		AuditEventActionType auditEventActionType,
		AuditEventEntityType auditEventEntityType,
		AuditEventOutcomeType auditEventOutcomeType
		)
	{
		var nowUtc = DateTime.UtcNow;
		const string ipAddress = "127.0.0.1";
		var logItem = new LogItem
		{
			HappenedOnTimeStampUtc = nowUtc.SecondsSinceTheEpoch(),
			HappenedOnLocalString = "",
			Id = Guid.NewGuid().ToString(),
			IpAddress = ipAddress,
			SessionId = Guid.NewGuid().ToString(),
			UserName = "test",
			Description = description
		};

		var auditEvent = logItem.ToAuditEvent();

		auditEvent.ActionType.Should().NotBe(AuditEventActionType.None);
		auditEvent.ActionType.Should().Be(auditEventActionType);

		(auditEvent.DateTime - nowUtc).TotalSeconds.Should().BeLessThanOrEqualTo(1);

		auditEvent.EntityType.Should().NotBe(AuditEventEntityType.None);
		auditEvent.EntityType.Should().Be(auditEventEntityType);

		auditEvent.Host.Should().Be(ipAddress);

		auditEvent.OriginalDescription.Should().Be(description);

		auditEvent.OutcomeType.Should().NotBe(AuditEventOutcomeType.None);
		auditEvent.OutcomeType.Should().Be(auditEventOutcomeType);
	}

	[Fact]
	public async void GetEventLog_Succeeds()
	{
		var endDateTimeUtc = DateTime.UtcNow;
		var startDateTimeUtc = endDateTimeUtc.AddHours(-118);
		for (var i = 0; i < 3000; i += 300)
		{
			var logItems = await LogicMonitorClient
				.GetLogItemsAsync(new LogFilter(300, 300, startDateTimeUtc, endDateTimeUtc, LogFilterSortOrder.HappenedOnAsc))
				.ConfigureAwait(false);

			foreach (var logItem in logItems)
			{
				var auditEvent = logItem.ToAuditEvent();
				auditEvent.Should().NotBeNull();
				auditEvent.ActionType.Should().NotBe(AuditEventActionType.None);
				auditEvent.EntityType.Should().NotBe(AuditEventEntityType.None);
				auditEvent.OutcomeType.Should().NotBe(AuditEventOutcomeType.None);
			}
		}
	}
}