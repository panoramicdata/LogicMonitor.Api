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

	[Fact]
	public void AddCustomGraphWidgetToDashboard_Success()
		=> AssertToAuditEventSucceeds(
			@"Add custom graph widget Buffer Cache Hit Ratio <id=57456> from instance graph Buffer Cache Hit Ratio <id=12968> to dashboard DBA Dev-App-WCUS03 <id=5705>",
			new()
			{
				MatchedRegExId = 102,
				ActionType = AuditEventActionType.Create,
				EntityType = AuditEventEntityType.Widget,
				OutcomeType = AuditEventOutcomeType.Success,
				WidgetName = "Buffer Cache Hit Ratio",
				DashboardName = "DBA Dev-App-WCUS03"
			}
		);

	[Fact]
	public void DeleteDashboardWithVisibility_Success()
		=> AssertToAuditEventSucceeds(
			@"Delete the dashboard DBA PROD-APP-SEA02 (Private)",
			new()
			{
				MatchedRegExId = 104,
				ActionType = AuditEventActionType.Delete,
				EntityType = AuditEventEntityType.Dashboard,
				OutcomeType = AuditEventOutcomeType.Success,
				DashboardName = "DBA PROD-APP-SEA02"
			}
		);

	[Fact]
	public void RenameDashboard_Success()
		=> AssertToAuditEventSucceeds(
		  @"Dashboard 'Sample Dashboard (Alias Demo)' renamed to 'DBA PROD-APP-SEA02'",
			new()
			{
				MatchedRegExId = 105,
				ActionType = AuditEventActionType.Update,
				EntityType = AuditEventEntityType.Dashboard,
				OutcomeType = AuditEventOutcomeType.Success,
				DashboardName = "DBA PROD-APP-SEA02",
				Description = "Sample Dashboard (Alias Demo)"
			}
		);

	[Fact]
	public void DeleteDashboardGroup_Success()
		=> AssertToAuditEventSucceeds(
		   @"Delete the dashboard group Sample Dashboard (Alias Demo)",
			new()
			{
				MatchedRegExId = 106,
				ActionType = AuditEventActionType.Delete,
				EntityType = AuditEventEntityType.DashboardGroup,
				OutcomeType = AuditEventOutcomeType.Success,
				ResourceGroupName = "Sample Dashboard (Alias Demo)"
			}
		);

	[Fact]
	public void UpdateDashboardGroup_Success()
		=> AssertToAuditEventSucceeds(
			@"Update a dashboard group DBA",
			new()
			{
				MatchedRegExId = 107,
				ActionType = AuditEventActionType.Update,
				EntityType = AuditEventEntityType.DashboardGroup,
				OutcomeType = AuditEventOutcomeType.Success,
				ResourceGroupName = "DBA"
			}
		);

	[Fact]
	public void EditWidgetOfDashboard_Success()
		=> AssertToAuditEventSucceeds(
		  @"Edit the widget phxdbpro - Fra usage overview of dashboard Sample Oracle Capacity",
			new()
			{
				MatchedRegExId = 91,
				ActionType = AuditEventActionType.Update,
				EntityType = AuditEventEntityType.Widget,
				OutcomeType = AuditEventOutcomeType.Success,
				WidgetName = "phxdbpro - Fra usage overview",
				DashboardName = "Sample Oracle Capacity"
			}
		);

	[Fact]
	public void DeleteWidgetOfDashboard_Success()
		=> AssertToAuditEventSucceeds(
			@"Delete the widget phxdbpro - Fra usage overview of dashboard Sample Oracle Capacity",
			new()
			{
				MatchedRegExId = 92,
				ActionType = AuditEventActionType.Delete,
				EntityType = AuditEventEntityType.Widget,
				OutcomeType = AuditEventOutcomeType.Success,
				WidgetName = "phxdbpro - Fra usage overview",
				DashboardName = "Sample Oracle Capacity"
			}
		);

	[Fact]
	public void UpdateAzureAccount_Success()
		=> AssertToAuditEventSucceeds(
			@"Update a Azure account - ea-clientportal-sandbox;",
			new()
			{
				MatchedRegExId = 93,
				ActionType = AuditEventActionType.Update,
				EntityType = AuditEventEntityType.Account,
				OutcomeType = AuditEventOutcomeType.Success,
				UserName = "ea-clientportal-sandbox"
			}
		);

	[Fact]
	public void DeleteAzureAccount_WithLargeBody_Success()
	{
		var message = "Delete Azure account ea-eisnerai-prod," + new string('x', 20_000);

		AssertToAuditEventSucceeds(
			message,
			new()
			{
				MatchedRegExId = 0,
				ActionType = AuditEventActionType.Delete,
				EntityType = AuditEventEntityType.AzureAccount,
				OutcomeType = AuditEventOutcomeType.Success,
				ResourceNames = ["ea-eisnerai-prod"]
			}
		);
	}

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

	[Fact]
	public void UpdateDatasourceInstancesEnableMonitoringOnly_Success()
		=> AssertToAuditEventSucceeds(
			@"Update the datasource instances, enable monitoring of instances : [SNMP_Network_Interfaces_CLONE-GigabitEthernet1/0/1 [ID:9] id=426199595 hid=416898,SNMP_Network_Interfaces_CLONE-GigabitEthernet1/0/2 [ID:10] id=426199584 hid=416898]",
			new()
			{
				MatchedRegExId = 111,
				ActionType = AuditEventActionType.Update,
				EntityType = AuditEventEntityType.ResourceDataSourceInstance,
				OutcomeType = AuditEventOutcomeType.Success,
				Description = "enable monitoring of instances : [SNMP_Network_Interfaces_CLONE-GigabitEthernet1/0/1 [ID:9] id=426199595 hid=416898,SNMP_Network_Interfaces_CLONE-GigabitEthernet1/0/2 [ID:10] id=426199584 hid=416898]",
				DataSourceNewInstanceNames = [
					"SNMP_Network_Interfaces_CLONE-GigabitEthernet1/0/1",
					"SNMP_Network_Interfaces_CLONE-GigabitEthernet1/0/2"
				],
				DataSourceNewInstanceIds = [426199595, 426199584],
				ResourceIds = [416898]
			}
		);

	[Fact]
	public void UpdateDatasourceInstancesEnableMonitoringOnly_VMwareDatastoreStatus_Success()
		=> AssertToAuditEventSucceeds(
			@"Update the datasource instances, enable monitoring of instances : [VMware_vCenter_DatastoreStatus_CLONE-LSP-DX0201ESXA_VMFS_P_NRP_PUR54_13ED_251 id=70959579 hid=276643,VMware_vCenter_DatastoreStatus_CLONE-LSP-DX0201ESXB_VMFS_P_NRP_PUR54_13EE_250 id=70959578 hid=276643]",
			new()
			{
				MatchedRegExId = 111,
				ActionType = AuditEventActionType.Update,
				EntityType = AuditEventEntityType.ResourceDataSourceInstance,
				OutcomeType = AuditEventOutcomeType.Success,
				Description = "enable monitoring of instances : [VMware_vCenter_DatastoreStatus_CLONE-LSP-DX0201ESXA_VMFS_P_NRP_PUR54_13ED_251 id=70959579 hid=276643,VMware_vCenter_DatastoreStatus_CLONE-LSP-DX0201ESXB_VMFS_P_NRP_PUR54_13EE_250 id=70959578 hid=276643]",
				DataSourceNewInstanceNames = [
					"VMware_vCenter_DatastoreStatus_CLONE-LSP-DX0201ESXA_VMFS_P_NRP_PUR54_13ED_251",
					"VMware_vCenter_DatastoreStatus_CLONE-LSP-DX0201ESXB_VMFS_P_NRP_PUR54_13EE_250"
				],
				DataSourceNewInstanceIds = [70959579, 70959578],
				ResourceIds = [276643]
			}
		);

	[Fact]
	public void UpdateDatasourceInstancesEnableMonitoringOnly_VMwareScratch_Success()
		=> AssertToAuditEventSucceeds(
			@"Update the datasource instances, enable monitoring of instances : [VMware_vCenter_DatastoreStatus_CLONE-LSP-DX0201ESXB_SCRATCH_PUR54_3 id=70959582 hid=276643]",
			new()
			{
				MatchedRegExId = 111,
				ActionType = AuditEventActionType.Update,
				EntityType = AuditEventEntityType.ResourceDataSourceInstance,
				OutcomeType = AuditEventOutcomeType.Success,
				Description = "enable monitoring of instances : [VMware_vCenter_DatastoreStatus_CLONE-LSP-DX0201ESXB_SCRATCH_PUR54_3 id=70959582 hid=276643]",
				DataSourceNewInstanceNames = ["VMware_vCenter_DatastoreStatus_CLONE-LSP-DX0201ESXB_SCRATCH_PUR54_3"],
				DataSourceNewInstanceIds = [70959582],
				ResourceIds = [276643]
			}
		);

	[Fact]
	public void UpdateWebsite_Success()
		=> AssertToAuditEventSucceeds(
			@"Update website http:__10.41.10.25:8081_sapdashboard - EXAMPLE_SRV_C - WEB0004720 ,   {
[
getParameterAsJSONForWebsiteDevice: update value={""schema"":""http""}, old value={""schema"":""https""}
]
}",
			new()
			{
				MatchedRegExId = 112,
				ActionType = AuditEventActionType.Update,
				EntityType = AuditEventEntityType.Resource,
				OutcomeType = AuditEventOutcomeType.Success,
				ResourceNames = ["http:__10.41.10.25:8081_sapdashboard"],
				InstanceName = "WEB0004720"
			}
		);

	[Fact]
	public void UpdateGroupDeviceGroupDescription_Success()
		=> AssertToAuditEventSucceeds(
			@"  {
[
getExtra: update value={""key"":1}, old value={""key"":0}
]
}""Action=Update""; ""Type=Group""; ""DeviceGroup=Example Integrator/CustomerA/EXAMPLE-AZURE-SUBSCRIPTIONS - Prod - PL00008552/example-sandbox (SG00023621)/CLA0003221 example-sandbox/example-sandbox""; ""Description=""",
			new()
			{
				MatchedRegExId = 97,
				ActionType = AuditEventActionType.Update,
				EntityType = AuditEventEntityType.ResourceGroup,
				OutcomeType = AuditEventOutcomeType.Success,
				ResourceGroupName = "Example Integrator/CustomerA/EXAMPLE-AZURE-SUBSCRIPTIONS - Prod - PL00008552/example-sandbox (SG00023621)/CLA0003221 example-sandbox/example-sandbox",
				Description = string.Empty,
			}
		);

	[Fact]
	public void UpdateGroupDeviceGroupDescription_WithLargeGetExtraPayload_Success()
	{
		var largeValue = new string('x', 25_000);
		var message = "  {\n[\ngetExtra: update value={\"payload\":\""
			+ largeValue
			+ "\"}, old value={\"payload\":\""
			+ largeValue
			+ "\"}\n]\n}"
		  + "\"Action=Update\"; \"Type=Group\"; \"DeviceGroup=Example Integrator/CustomerA/EXAMPLE-AZURE-SUBSCRIPTIONS - Prod - PL00008552/example-sandbox (SG00023621)/CLA0003221 example-sandbox/example-sandbox\"; \"Description=\"";

		AssertToAuditEventSucceeds(
			message,
			new()
			{
				MatchedRegExId = 97,
				ActionType = AuditEventActionType.Update,
				EntityType = AuditEventEntityType.ResourceGroup,
				OutcomeType = AuditEventOutcomeType.Success,
				ResourceGroupName = "Example Integrator/CustomerA/EXAMPLE-AZURE-SUBSCRIPTIONS - Prod - PL00008552/example-sandbox (SG00023621)/CLA0003221 example-sandbox/example-sandbox",
				Description = string.Empty,
			}
		);
	}

	[Fact]
	public void UpdateGroupThresholdsWithEmptyAlertThresholdChanges_Success()
		=> AssertToAuditEventSucceeds(
		 @"""Action=Update""; ""Type=Group""; ""Device=NA""; ""GroupName=Example Group""; ""Description= Enable alerting on datapoint syncStatus ""; ""Alert_threshold_changes=""; ""DataSource=Fortinet_FortiGate_HighAvailabilityPeers""; ""DataSourceId=5804476""; ""Reason=Datapoint alerting enabled""",
			new()
			{
				MatchedRegExId = 46,
				ActionType = AuditEventActionType.Update,
				EntityType = AuditEventEntityType.ResourceGroup,
				OutcomeType = AuditEventOutcomeType.Success,
				ResourceGroupName = "Example Group",
				Description = " Enable alerting on datapoint syncStatus ",
				LogicModuleName = "Fortinet_FortiGate_HighAvailabilityPeers",
				LogicModuleId = 5804476
			}
		);

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
}
