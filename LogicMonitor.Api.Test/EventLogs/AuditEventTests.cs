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
	public void HostGroupsWithUnicodeCloseBracketAfterHostName_Success()
		=> AssertToAuditEventSucceeds(
			@"host(id= 4581 ,name= LM Push Server (LM-Push-Server)）add to groups, list:   group name: Misc ,appliesTo= isMisc()  ,group name: Minimal Monitoring ,appliesTo= system.sysinfo == \""\"" && system.sysoid == \""\"" && isDevice() && !(system.virtualization) && (monitoring != \""basic\"")  ,group name: LM Push SG ,appliesTo= servicenow.servicegroup_sys_id == \""xxx\"" or getPropValue(\""system.aws.tag.xxx_service-group\"") == \""xxx\"" or getPropValue(\""system.azure.tag.xxx_service-group\"") == \""xxx\"" or getPropValue(\""system.gcp.tag.xxx_service-group\"") == \""xxx\""  ,group name: lmsupport ,appliesTo= cms_service_level != \""cet_8x5\"" ,add group number is 4",
			new()
			{
				MatchedRegExId = 17,
				ResourceIds = [4581],
				ResourceNames = ["LM Push Server (LM-Push-Server)"],
				ActionType = AuditEventActionType.Create,
				EntityType = AuditEventEntityType.ResourceGroups,
				OutcomeType = AuditEventOutcomeType.Success
			}
		);
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


	[Fact]
	public void Added_ResourceProperty_Success()
		=> AssertToAuditEventSucceeds(
			@"Add property(name=propname, value=propvalue) to host(resourceName) via API token TOKEN_EXAMPLE.",
			new()
			{
				MatchedRegExId = 16,
				ResourceNames = ["resourceName"],
				ActionType = AuditEventActionType.Create,
				EntityType = AuditEventEntityType.ResourceProperty,
				OutcomeType = AuditEventOutcomeType.Success,
				ApiTokenId = "TOKEN_EXAMPLE",
				PropertyName = "propname",
				PropertyValue = "propvalue"
			}
		);
}
