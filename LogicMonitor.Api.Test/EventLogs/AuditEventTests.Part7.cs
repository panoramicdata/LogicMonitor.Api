namespace LogicMonitor.Api.Test.EventLogs;

public partial class AuditEventTests
{
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
}
