namespace LogicMonitor.Api.Test.EventLogs;

public partial class AuditEventTests
{
	[Fact]
	public void UpdatePassword_Success()
		=> AssertToAuditEventSucceeds(
			@"some.user.admin update password change password",
			new()
			{
				MatchedRegExId = 26,
				UserName = "some.user.admin",
				ActionType = AuditEventActionType.Update,
				EntityType = AuditEventEntityType.Account,
				OutcomeType = AuditEventOutcomeType.Success
			}
		);

	[Fact]
	public void DataSourceImport_Success()
		=> AssertToAuditEventSucceeds(
			@"Import DataSource from repository.  Change details : Change datasource : NetApp_Cluster_FibreChannel, dsId=1211 {\nDataSourceContent\n}",
			new()
			{
				// This is no longer matched by regex 27 since David excluded these messages from regex parsing
				MatchedRegExId = 27,
				//ActionType = AuditEventActionType.Update,
				EntityType = AuditEventEntityType.None,
				//LogicModuleId = 1211,
				//LogicModuleName = "NetApp_Cluster_FibreChannel",
				OutcomeType = AuditEventOutcomeType.Success
			}
		);

	[Fact]
	public void ChangeHostCollectors_LargePayload_Success()
		=> AssertToAuditEventSucceeds(
			@"Change host collectors: host<378754>(EXAMPLE_SRV_D sgcaplmprod002), preferred<55> , current<55> to collector <57>(DC\SGCAPLMPROD003)",
			new()
			{
				MatchedRegExId = 100,
				ActionType = AuditEventActionType.GeneralApi,
				EntityType = AuditEventEntityType.Collector,
				OutcomeType = AuditEventOutcomeType.Success
			}
		);

	[Fact]
	public void AddDataSourceGraph_Success()
		=> AssertToAuditEventSucceeds(
			@"""Action=Add""; ""Type=DataSourceGraph""; ""DataSourceName=test_NetApp_Cluster_FibreChannel""; ""Device=NA""; ""Description=Add datasource graph, graph=Signal/Sync Loss(8702), """,
			new()
			{
				MatchedRegExId = 28,
				ActionType = AuditEventActionType.Create,
				EntityType = AuditEventEntityType.DataSourceGraph,
				LogicModuleName = "test_NetApp_Cluster_FibreChannel",
				OutcomeType = AuditEventOutcomeType.Success
			}
		);

	[Fact]
	public void EventAlertDiscarded_Success()
		=> AssertToAuditEventSucceeds(
			@"An event alert was discarded for EventSource Azure Advisor Recommendations because it exceeded the rate limit of 150 events per 60 seconds. Adding filters to your EventSource may help reduce the number of alerts triggered.",
			new()
			{
				MatchedRegExId = 29,
				ActionType = AuditEventActionType.DiscardedEventAlert,
				LogicModuleName = "Azure Advisor Recommendations",
				OutcomeType = AuditEventOutcomeType.Success
			}
		);

	[Fact]
	public void UpdateEventSource_Success()
		=> AssertToAuditEventSucceeds(
		  @"""Action=Update""; ""Type=EventSource""; ""LogicModuleName=EXAMPLE_EVENTSOURCE_ALERTS_NEW""; ""Device=NA""; ""LogicModuleId=61""; ""Description=Updated directly by user; Diff=Update display name from EXAMPLE_EVENTSOURCE_ALERTS to EXAMPLE_EVENTSOURCE_ALERTS_NEW; ; "";",
			new()
			{
				MatchedRegExId = 115,
				ActionType = AuditEventActionType.Update,
				EntityType = AuditEventEntityType.EventSource,
				LogicModuleName = "EXAMPLE_EVENTSOURCE_ALERTS_NEW",
				LogicModuleId = 61,
				Description = "Updated directly by user; Diff=Update display name from EXAMPLE_EVENTSOURCE_ALERTS to EXAMPLE_EVENTSOURCE_ALERTS_NEW; ; ",
				OutcomeType = AuditEventOutcomeType.Success
			}
		);

	[Fact]
	public void DeleteSdt_Success()
		=> AssertToAuditEventSucceeds(
			@"Delete SDT from 2022-05-18 08:51:27 GMT to 2022-05-18 09:51:27 GMT from Datasource Collector DNS Resolving on Host somehost name via API token TOKEN_EXAMPLE_2",
			new()
			{
				MatchedRegExId = 30,
				ActionType = AuditEventActionType.Delete,
				EntityType = AuditEventEntityType.ScheduledDownTime,
				ResourceNames = ["somehost name"],
				OutcomeType = AuditEventOutcomeType.Success,
				ApiTokenId = "TOKEN_EXAMPLE_2"
			}
		);

	[Fact]
	public void DeleteSdt_GroupQuotedFormat_Success()
		=> AssertToAuditEventSucceeds(
		   @"""Action=Delete""; ""Type=SDT""; ""Description= Delete SDT for Group Example Bakeries on Group Path Example Integrator/Example Bakeries with scheduled downtime from 2025-12-31 04:25:00 GMT to 2027-06-30 05:25:00 GMT ""; ""DeviceGroupName=Example Bakeries""; ""DeviceGroupId=3786""; ""StartDownTime=2025-12-31 04:25:00 GMT""; ""EndDownTime=2027-06-30 05:25:00 GMT"";",
			new()
			{
				MatchedRegExId = 38,
				ActionType = AuditEventActionType.Delete,
				EntityType = AuditEventEntityType.ScheduledDownTime,
				Description = " Delete SDT for Group Example Bakeries on Group Path Example Integrator/Example Bakeries with scheduled downtime from 2025-12-31 04:25:00 GMT to 2027-06-30 05:25:00 GMT ",
				OutcomeType = AuditEventOutcomeType.Success,
				ResourceNames = ["Example Bakeries"],
				ResourceIds = [3786],
				StartDownTime = "2025-12-31 04:25:00 GMT",
				EndDownTime = "2027-06-30 05:25:00 GMT"
			}
		);

	[Fact]
	public void AddSdt_Success()
		=> AssertToAuditEventSucceeds(
			@"Add SDT for Datasource Collector DNS Resolving on Host somehost name with scheduled downtime from 2022-05-18 08:53:39 GMT to 2022-05-18 09:53:39 GMT via API token TOKEN_EXAMPLE_2",
			new()
			{
				MatchedRegExId = 48,
				ActionType = AuditEventActionType.Create,
				EntityType = AuditEventEntityType.ScheduledDownTime,
				ResourceNames = ["somehost name"],
				OutcomeType = AuditEventOutcomeType.Success,
				ApiTokenId = "TOKEN_EXAMPLE_2"
			}
		);
}
