namespace LogicMonitor.Api.Test.EventLogs;

public partial class AuditEventTests
{
	[Fact]
	public void SetAllInstancesDatapointThresholdOnDevice_Success()
		=> AssertToAuditEventSucceeds(
			@"Set all instances' datapoint (18371:NoData) alert threshold as (NO CHANGE), alert enable as (false) under the instance groups(0:@default) of device(1416411:EXAMPLE_SRV_B Tatenen)",
			new()
			{
				MatchedRegExId = 94,
				ActionType = AuditEventActionType.Update,
				EntityType = AuditEventEntityType.ResourceDataSourceInstance,
				OutcomeType = AuditEventOutcomeType.Success,
				ResourceIds = [1416411],
				ResourceNames = ["EXAMPLE_SRV_B Tatenen"],
				Description = "false"
			}
		);

	[Fact]
	public void ScheduleDebugCommand_Success()
		=> AssertToAuditEventSucceeds(
			@"""Action=Schedule debug command""; ""Command=help""; ""AgentId=21""",
			new()
			{
				MatchedRegExId = 95,
				ActionType = AuditEventActionType.Run,
				EntityType = AuditEventEntityType.Collector,
				OutcomeType = AuditEventOutcomeType.Success,
				CollectorId = 21,
				Command = "help"
			}
		);

	[Fact]
	public void ScheduleDebugCommand_WithScriptBody_Success()
		=> AssertToAuditEventSucceeds(
			@"""Action=Schedule debug command""; ""Command=!groovy""; ""AgentId=698""; ""DeviceId=256234""; ""DeviceName=Juniper-Mist-Bangalore""; ""ScriptBody=/* big script */""",
			new()
			{
				MatchedRegExId = 95,
				ActionType = AuditEventActionType.Run,
				EntityType = AuditEventEntityType.Collector,
				OutcomeType = AuditEventOutcomeType.Success,
				CollectorId = 698,
				Command = "!groovy"
			}
		);

	[Fact]
	public void ScheduleDebugCommand_WithLargeScriptBody_Success()
	{
		var message = "\"Action=Schedule debug command\"; \"Command=!groovy\"; \"AgentId=698\"; \"DeviceId=256234\"; \"DeviceName=Juniper-Mist-Bangalore\"; \"ScriptBody=" + new string('x', 20_000) + "\"";

		AssertToAuditEventSucceeds(
			message,
			new()
			{
				MatchedRegExId = 95,
				ActionType = AuditEventActionType.Run,
				EntityType = AuditEventEntityType.Collector,
				OutcomeType = AuditEventOutcomeType.Success,
				CollectorId = 698,
				Command = "!groovy"
			}
		);
	}

	[Fact]
	public void UnknownDebugCommand_Failure()
		=> AssertToAuditEventSucceeds(
		  @"""Unknown debug command""; ""Command=!script""; ""AgentId=60""; ""Company=examplecompany"";",
			new()
			{
				MatchedRegExId = 98,
				ActionType = AuditEventActionType.Run,
				EntityType = AuditEventEntityType.Collector,
				OutcomeType = AuditEventOutcomeType.Failure,
				CollectorId = 60,
				Command = "!script"
			}
		);

	[Fact]
	public void UpdateDatasourceInstancesDisableMonitoringAndAlerting_Success()
		=> AssertToAuditEventSucceeds(
			@"Update the datasource instances, disable monitoring of instances : [SNMP_Network_Interfaces-pc-0/2/0 [ID:588] id=426808630 hid=489168,SNMP_Network_Interfaces-pc-0/2/0.16383 [ID:591] id=426808624 hid=489168,SNMP_Network_Interfaces-pc-0/2/0.16384 [ID:592] id=426808618 hid=489168]disable alerting on instances : [SNMP_Network_Interfaces-pc-0/2/0 [ID:588] id=426808630 hid=489168,SNMP_Network_Interfaces-pc-0/2/0.16383 [ID:591] id=426808624 hid=489168,SNMP_Network_Interfaces-pc-0/2/0.16384 [ID:592] id=426808618 hid=489168]",
			new()
			{
				MatchedRegExId = 96,
				ActionType = AuditEventActionType.Update,
				EntityType = AuditEventEntityType.ResourceDataSourceInstance,
				OutcomeType = AuditEventOutcomeType.Success,
				Description = "disable monitoring of instances : [SNMP_Network_Interfaces-pc-0/2/0 [ID:588] id=426808630 hid=489168,SNMP_Network_Interfaces-pc-0/2/0.16383 [ID:591] id=426808624 hid=489168,SNMP_Network_Interfaces-pc-0/2/0.16384 [ID:592] id=426808618 hid=489168]disable alerting on instances : [SNMP_Network_Interfaces-pc-0/2/0 [ID:588] id=426808630 hid=489168,SNMP_Network_Interfaces-pc-0/2/0.16383 [ID:591] id=426808624 hid=489168,SNMP_Network_Interfaces-pc-0/2/0.16384 [ID:592] id=426808618 hid=489168]",
				DataSourceNewInstanceNames = [
					"SNMP_Network_Interfaces-pc-0/2/0",
					"SNMP_Network_Interfaces-pc-0/2/0.16383",
					"SNMP_Network_Interfaces-pc-0/2/0.16384"
				],
				DataSourceNewInstanceIds = [426808630, 426808624, 426808618],
				ResourceIds = [489168]
			}
		);

	[Fact]
	public void UpdateDatasourceInstancesEnableMonitoringAndAlerting_Success()
		=> AssertToAuditEventSucceeds(
			@"Update the datasource instances, enable monitoring of instances : [SNMP_Network_Interfaces-pc-0/2/0 [ID:588] id=426808630 hid=489168,SNMP_Network_Interfaces-pc-0/2/0.16383 [ID:591] id=426808624 hid=489168,SNMP_Network_Interfaces-pc-0/2/0.16384 [ID:592] id=426808618 hid=489168]enable alerting on instances : [SNMP_Network_Interfaces-pc-0/2/0 [ID:588] id=426808630 hid=489168,SNMP_Network_Interfaces-pc-0/2/0.16383 [ID:591] id=426808624 hid=489168,SNMP_Network_Interfaces-pc-0/2/0.16384 [ID:592] id=426808618 hid=489168]",
			new()
			{
				MatchedRegExId = 108,
				ActionType = AuditEventActionType.Update,
				EntityType = AuditEventEntityType.ResourceDataSourceInstance,
				OutcomeType = AuditEventOutcomeType.Success,
				Description = "enable monitoring of instances : [SNMP_Network_Interfaces-pc-0/2/0 [ID:588] id=426808630 hid=489168,SNMP_Network_Interfaces-pc-0/2/0.16383 [ID:591] id=426808624 hid=489168,SNMP_Network_Interfaces-pc-0/2/0.16384 [ID:592] id=426808618 hid=489168]enable alerting on instances : [SNMP_Network_Interfaces-pc-0/2/0 [ID:588] id=426808630 hid=489168,SNMP_Network_Interfaces-pc-0/2/0.16383 [ID:591] id=426808624 hid=489168,SNMP_Network_Interfaces-pc-0/2/0.16384 [ID:592] id=426808618 hid=489168]",
				DataSourceNewInstanceNames = [
					"SNMP_Network_Interfaces-pc-0/2/0",
					"SNMP_Network_Interfaces-pc-0/2/0.16383",
					"SNMP_Network_Interfaces-pc-0/2/0.16384"
				],
				DataSourceNewInstanceIds = [426808630, 426808624, 426808618],
				ResourceIds = [489168]
			}
		);

	[Fact]
	public void ShareDashboard_Success()
		=> AssertToAuditEventSucceeds(
		  @"user@example.com share a dashboard(DBA PROD-APP-SEA02)",
			new()
			{
				MatchedRegExId = 109,
				ActionType = AuditEventActionType.Update,
				EntityType = AuditEventEntityType.Dashboard,
				OutcomeType = AuditEventOutcomeType.Success,
				UserName = "user@example.com",
				DashboardName = "DBA PROD-APP-SEA02"
			}
		);

	[Fact]
	public void UpdateDatasourceInstancesBareMessage_Success()
		=> AssertToAuditEventSucceeds(
			@"Update the datasource instances,",
			new()
			{
				MatchedRegExId = 110,
				ActionType = AuditEventActionType.Update,
				EntityType = AuditEventEntityType.ResourceDataSourceInstance,
				OutcomeType = AuditEventOutcomeType.Success
			}
		);
}
