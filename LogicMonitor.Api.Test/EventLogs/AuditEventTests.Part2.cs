namespace LogicMonitor.Api.Test.EventLogs;

public partial class AuditEventTests
{
	[Fact]
	public void Update_DeviceDataSourceInstance_New_Success()
		=> AssertToAuditEventSucceeds(
			@"""Action=Update""; ""Type=Instance""; ""Device=NA""; ""InstanceName=NA""; ""Description=Found new instance(s) for: EXAMPLE-LM.example.com (CollectorID=249) [LogicMonitor_Portal_DataSources-Win_WMI_UACTroubleshooter]; New_InstanceIds=[deviceId=2781,dataSourceId=112813425,dataSourceNewInstanceId(s)=263395102];""",
			new()
			{
				MatchedRegExId = 8,
				ActionType = AuditEventActionType.Update,
				CollectorId = 249,
				EntityType = AuditEventEntityType.ResourceDataSourceInstance,
				OutcomeType = AuditEventOutcomeType.Success,
				ResourceIds = [2781],
				ResourceNames = ["EXAMPLE-LM.example.com"],
				LogicModuleId = 112813425,
				InstanceName = "NA",
				DataSourceNewInstanceNames = ["LogicMonitor_Portal_DataSources-Win_WMI_UACTroubleshooter"],
				DataSourceNewInstanceIds = [263395102]
			}
		);

	[Fact]
	public void Update_DeviceDataSourceInstance_Disappeared2_Success()
		=> AssertToAuditEventSucceeds(
			@"""Action=Update""; ""Type=Instance""; ""Device=NA""; ""InstanceName=NA""; ""Description=Instance(s) disappeared from: EXAMPLE-K8S-TEST-03 (CollectorID=249) [Critical Linux Processes-java]; New_InstanceIds=[deviceId=1525,dataSourceId=94545589,dataSourceDeletedInstanceId(s)=263219849];""",
			new()
			{
				MatchedRegExId = 7,
				ActionType = AuditEventActionType.Update,
				CollectorId = 249,
				EntityType = AuditEventEntityType.ResourceDataSourceInstance,
				OutcomeType = AuditEventOutcomeType.Success,
				ResourceIds = [1525],
				ResourceNames = ["EXAMPLE-K8S-TEST-03"],
				LogicModuleId = 94545589,
				LogicModuleName = "Critical Linux Processes-java",
				InstanceName = "NA",
				DataSourceDeletedInstanceIds = [263219849]
			}
		);

	[Fact]
	public void Update_DeviceDataSourceInstance_NewAndDisappeared_Success()
		=> AssertToAuditEventSucceeds(
			@"""Action=Update""; ""Type=Instance""; ""Device=NA""; ""InstanceName=NA""; ""Description=Found new instance(s) for: EXAMPLE-HAPROXY-TEST-02 (CollectorID=249) [HA Proxy Backend-ui_alpha_reportmagic,HA Proxy Backend-pdl_app_jira_test_01]; Instance(s) disappeared from: EXAMPLE-HAPROXY-TEST-02 (CollectorID=249) [HA Proxy Backend-uiv3_alpha_reportmagic]; New_InstanceIds=[deviceId=2365,dataSourceId=111613364,dataSourceNewInstanceId(s)=263956258,263956259,dataSourceDeletedInstanceId(s)=256832296];""",
			new()
			{
				MatchedRegExId = 9,
				ActionType = AuditEventActionType.Update,
				CollectorId = 249,
				EntityType = AuditEventEntityType.ResourceDataSourceInstance,
				OutcomeType = AuditEventOutcomeType.Success,
				ResourceIds = [2365],
				ResourceNames = ["EXAMPLE-HAPROXY-TEST-02"],
				InstanceName = "NA",
				LogicModuleId = 111613364,
				DataSourceNewInstanceIds = [263956258, 263956259],
				DataSourceNewInstanceNames = ["HA Proxy Backend-ui_alpha_reportmagic", "HA Proxy Backend-pdl_app_jira_test_01"],
				DataSourceDeletedInstanceIds = [256832296],
				DataSourceDeletedInstanceNames = ["HA Proxy Backend-uiv3_alpha_reportmagic"]
			}
		);

	[Fact]
	public void Update_DeviceDataSourceInstance_NewAndDisappeared2_Success()
		=> AssertToAuditEventSucceeds(
			@"""Action=Update""; ""Type=Instance""; ""Device=NA""; ""InstanceName=NA""; ""Description=Found new instance(s) for: EXAMPLE-W1:recoveryservices:pambackup (CollectorID=-2) [Microsoft_Azure_BackupJobStatus-xxx,Microsoft_Azure_BackupJobStatus-yyy]; Instance(s) disappeared from: EXAMPLE-W1:recoveryservices:pambackup (CollectorID=-2) [Microsoft_Azure_BackupJobStatus-aaa,Microsoft_Azure_BackupJobStatus-bbb]; New_InstanceIds=[deviceId=2571,dataSourceId=39016161,dataSourceNewInstanceId(s)=570930097,570930098,dataSourceDeletedInstanceId(s)=569154776,569154777];""",
			new()
			{
				MatchedRegExId = 9,
				ActionType = AuditEventActionType.Update,
				CollectorId = -2,
				EntityType = AuditEventEntityType.ResourceDataSourceInstance,
				OutcomeType = AuditEventOutcomeType.Success,
				ResourceIds = [2571],
				ResourceNames = ["EXAMPLE-W1:recoveryservices:pambackup"],
				InstanceName = "NA",
				LogicModuleId = 39016161,
				DataSourceNewInstanceIds = [570930097, 570930098],
				DataSourceNewInstanceNames = ["Microsoft_Azure_BackupJobStatus-xxx", "Microsoft_Azure_BackupJobStatus-yyy"],
				DataSourceDeletedInstanceIds = [569154776, 569154777],
				DataSourceDeletedInstanceNames = ["Microsoft_Azure_BackupJobStatus-aaa", "Microsoft_Azure_BackupJobStatus-bbb"]
			}
		);

	[Fact]
	public void Update_DeviceGroup_NothingChanged_Success()
		=> AssertToAuditEventSucceeds(
			"Update the ResourceGroup EXAMPLE - Panoramic Data/Datacenter/Private/Servers/Kubernetes Cluster: EXAMPLE-K8S-TEST.Nothing has been changed. via API token TOKEN_EXAMPLE",
			new()
			{
				MatchedRegExId = 13,
				ActionType = AuditEventActionType.Update,
				EntityType = AuditEventEntityType.ResourceGroup,
				OutcomeType = AuditEventOutcomeType.Success,
				ResourceGroupName = "EXAMPLE - Panoramic Data/Datacenter/Private/Servers/Kubernetes Cluster: EXAMPLE-K8S-TEST",
				ApiTokenId = "TOKEN_EXAMPLE"
			}
		);

	[Fact]
	public void Added_DeviceGroup_Success()
		=> AssertToAuditEventSucceeds(
			"Added ResourceGroup Path1/Path2/Path3 (6686)  via API token TOKEN_EXAMPLE, ",
			new()
			{
				MatchedRegExId = 14,
				ActionType = AuditEventActionType.Create,
				EntityType = AuditEventEntityType.ResourceGroup,
				OutcomeType = AuditEventOutcomeType.Success,
				ResourceGroupId = 6686,
				ResourceGroupName = "Path1/Path2/Path3",
				ApiTokenId = "TOKEN_EXAMPLE"
			}
		);

	[Fact]
	public void Added_DataSource_Success()
		=> AssertToAuditEventSucceeds(
			@"""Action=Add""; ""Type=DataSource""; ""DataSourceName=Whois_TTL_Expiry""; ""DeviceName=EXAMPLE-S1:appserviceplan:CappedAndHooked (EXAMPLE-S1:cappedandhooked:appserviceplan:CappedAndHooked-ID)""; ""DeviceId=8555""; ""Description=Addition of datasource to device""; ""DataSourceId=114345723""; ""DeviceDataSourceId=615826""",
			new()
			{
				MatchedRegExId = 15,
				ResourceIds = [8555],
				ResourceNames = ["EXAMPLE-S1:cappedandhooked:appserviceplan:CappedAndHooked-ID"],
				ActionType = AuditEventActionType.Create,
				EntityType = AuditEventEntityType.DataSource,
				OutcomeType = AuditEventOutcomeType.Success,
				LogicModuleId = 114345723,
				LogicModuleName = "Whois_TTL_Expiry"
			}
		);
}
