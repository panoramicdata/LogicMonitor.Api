namespace LogicMonitor.Api.Test.EventLogs;

public class AuditEventTests(ITestOutputHelper iTestOutputHelper, Fixture fixture) : TestWithOutput(iTestOutputHelper, fixture), IClassFixture<Fixture>
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
			@"""Action=Update""; ""Type=Instance""; ""Device=NA""; ""InstanceId=NA""; ""Description=Instance(s) disappeared from: PDL-FW-01 (CollectorID=249) [DS--1.2.3.4]; """,
			new()
			{
				MatchedRegExId = 11,
				ActionType = AuditEventActionType.Update,
				EntityType = AuditEventEntityType.ResourceDataSourceInstance,
				OutcomeType = AuditEventOutcomeType.Success,
				Description = "Instance(s) disappeared from: PDL-FW-01 (CollectorID=249) [DS--1.2.3.4]; ",
				ResourceNames = ["NA"]
			}
		);

	[Fact]
	public void Update_DeviceDataSourceInstance_Changed_Success()
		=> AssertToAuditEventSucceeds(
			@"""Action=Update""; ""Type=Instance""; ""Device=NA""; ""InstanceName=NA""; ""Description=TemporaryReportName(s) changed for: PDL-K8S-TEST (CollectorID=297) [Critical Linux Processes-java from 9947 to 22713]; valueChanges=[deviceId=3271,dataSourceId=94545589,instanceChanges=[instanceId=263219850,oldValue=22713,newValue=9947];];'""",
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
		"Update host<4030, docker-registry-deploy-default-PDL-K8S-PROD> (monitored by collector <295, pdl-k8s-prod-0>), ,  via API token xx123",
		3,
		4030,
		"docker-registry-deploy-default-PDL-K8S-PROD",
		295,
		"pdl-k8s-prod-0",
		"xx123"
		)]
	[InlineData(
"""
"Action=Update"; "Type=Device"; "Device=ecloud-vm-124345 (124345) (1661057)"; "Description=(monitored by collector <1007, Workgroup\I-A7F966D2>),   {
	[
		getAlertEnable: update value=false, old value=true.
	]
	}[Added property (system.categories:)]"
""",
		2,
		1661057,
		"ecloud-vm-124345 (124345)",
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
			@"""Action=Update""; ""Type=Instance""; ""Device=NA""; ""InstanceName=NA""; ""Description=Found new instance(s) for: PDL-LM.logicmonitor.com (CollectorID=249) [LogicMonitor_Portal_DataSources-Win_WMI_UACTroubleshooter]; New_InstanceIds=[deviceId=2781,dataSourceId=112813425,dataSourceNewInstanceId(s)=263395102];""",
			new()
			{
				MatchedRegExId = 8,
				ActionType = AuditEventActionType.Update,
				CollectorId = 249,
				EntityType = AuditEventEntityType.ResourceDataSourceInstance,
				OutcomeType = AuditEventOutcomeType.Success,
				ResourceIds = [2781],
				ResourceNames = ["PDL-LM.logicmonitor.com"],
				LogicModuleId = 112813425,
				InstanceName = "NA",
				DataSourceNewInstanceNames = ["LogicMonitor_Portal_DataSources-Win_WMI_UACTroubleshooter"],
				DataSourceNewInstanceIds = [263395102]
			}
		);

	[Fact]
	public void Update_DeviceDataSourceInstance_Disappeared2_Success()
		=> AssertToAuditEventSucceeds(
			@"""Action=Update""; ""Type=Instance""; ""Device=NA""; ""InstanceName=NA""; ""Description=Instance(s) disappeared from: PDL-K8S-TEST-03 (CollectorID=249) [Critical Linux Processes-java]; New_InstanceIds=[deviceId=1525,dataSourceId=94545589,dataSourceDeletedInstanceId(s)=263219849];""",
			new()
			{
				MatchedRegExId = 7,
				ActionType = AuditEventActionType.Update,
				CollectorId = 249,
				EntityType = AuditEventEntityType.ResourceDataSourceInstance,
				OutcomeType = AuditEventOutcomeType.Success,
				ResourceIds = [1525],
				ResourceNames = ["PDL-K8S-TEST-03"],
				LogicModuleId = 94545589,
				LogicModuleName = "Critical Linux Processes-java",
				InstanceName = "NA",
				DataSourceDeletedInstanceIds = [263219849]
			}
		);

	[Fact]
	public void Update_DeviceDataSourceInstance_NewAndDisappeared_Success()
		=> AssertToAuditEventSucceeds(
			@"""Action=Update""; ""Type=Instance""; ""Device=NA""; ""InstanceName=NA""; ""Description=Found new instance(s) for: PDL-HAPROXY-TEST-02 (CollectorID=249) [HA Proxy Backend-ui_alpha_reportmagic,HA Proxy Backend-pdl_app_jira_test_01]; Instance(s) disappeared from: PDL-HAPROXY-TEST-02 (CollectorID=249) [HA Proxy Backend-uiv3_alpha_reportmagic]; New_InstanceIds=[deviceId=2365,dataSourceId=111613364,dataSourceNewInstanceId(s)=263956258,263956259,dataSourceDeletedInstanceId(s)=256832296];""",
			new()
			{
				MatchedRegExId = 9,
				ActionType = AuditEventActionType.Update,
				CollectorId = 249,
				EntityType = AuditEventEntityType.ResourceDataSourceInstance,
				OutcomeType = AuditEventOutcomeType.Success,
				ResourceIds = [2365],
				ResourceNames = ["PDL-HAPROXY-TEST-02"],
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
			@"""Action=Update""; ""Type=Instance""; ""Device=NA""; ""InstanceName=NA""; ""Description=Found new instance(s) for: EU-W1:recoveryservices:pambackup (CollectorID=-2) [Microsoft_Azure_BackupJobStatus-xxx,Microsoft_Azure_BackupJobStatus-yyy]; Instance(s) disappeared from: EU-W1:recoveryservices:pambackup (CollectorID=-2) [Microsoft_Azure_BackupJobStatus-aaa,Microsoft_Azure_BackupJobStatus-bbb]; New_InstanceIds=[deviceId=2571,dataSourceId=39016161,dataSourceNewInstanceId(s)=570930097,570930098,dataSourceDeletedInstanceId(s)=569154776,569154777];""",
			new()
			{
				MatchedRegExId = 9,
				ActionType = AuditEventActionType.Update,
				CollectorId = -2,
				EntityType = AuditEventEntityType.ResourceDataSourceInstance,
				OutcomeType = AuditEventOutcomeType.Success,
				ResourceIds = [2571],
				ResourceNames = ["EU-W1:recoveryservices:pambackup"],
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
			"Update the ResourceGroup PDL - Panoramic Data/Datacenter/Private/Servers/Kubernetes Cluster: PDL-K8S-TEST.Nothing has been changed. via API token TOKENID",
			new()
			{
				MatchedRegExId = 13,
				ActionType = AuditEventActionType.Update,
				EntityType = AuditEventEntityType.ResourceGroup,
				OutcomeType = AuditEventOutcomeType.Success,
				ResourceGroupName = "PDL - Panoramic Data/Datacenter/Private/Servers/Kubernetes Cluster: PDL-K8S-TEST",
				ApiTokenId = "TOKENID"
			}
		);

	[Fact]
	public void Added_DeviceGroup_Success()
		=> AssertToAuditEventSucceeds(
			"Added ResourceGroup Path1/Path2/Path3 (6686)  via API token TOKENID, ",
			new()
			{
				MatchedRegExId = 14,
				ActionType = AuditEventActionType.Create,
				EntityType = AuditEventEntityType.ResourceGroup,
				OutcomeType = AuditEventOutcomeType.Success,
				ResourceGroupId = 6686,
				ResourceGroupName = "Path1/Path2/Path3",
				ApiTokenId = "TOKENID"
			}
		);


	[Fact]
	public void Added_DataSource_Success()
		=> AssertToAuditEventSucceeds(
			@"""Action=Add""; ""Type=DataSource""; ""DataSourceName=Whois_TTL_Expiry""; ""DeviceName=UK-S1:appserviceplan:CappedAndHooked (UK-S1:cappedandhooked:appserviceplan:CappedAndHooked-ID)""; ""DeviceId=8555""; ""Description=Addition of datasource to device""; ""DataSourceId=114345723""; ""DeviceDataSourceId=615826""",
			new()
			{
				MatchedRegExId = 15,
				ResourceIds = [8555],
				ResourceNames = ["UK-S1:cappedandhooked:appserviceplan:CappedAndHooked-ID"],
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
			@"Add property(name=propname, value=propvalue) to host(resourceName) via API token TOKENID.",
			new()
			{
				MatchedRegExId = 16,
				ResourceNames = ["resourceName"],
				ActionType = AuditEventActionType.Create,
				EntityType = AuditEventEntityType.ResourceProperty,
				OutcomeType = AuditEventOutcomeType.Success,
				ApiTokenId = "TOKENID",
				PropertyName = "propname",
				PropertyValue = "propvalue"
			}
		);

	[Fact]
	public void HostGroups_Success()
		=> AssertToAuditEventSucceeds(
			@"host(id= 4581 ,name= LM Push Server (LM-Push-Server))add to groups, list:   group name: Misc ,appliesTo= isMisc()  ,group name: Minimal Monitoring ,appliesTo= system.sysinfo == """" && system.sysoid == """" && isDevice() && !(system.virtualization) && (monitoring != ""basic"")  ,group name: LM Push SG ,appliesTo= servicenow.servicegroup_sys_id == ""xxx"" or getPropValue(""system.aws.tag.xxx_service-group"") == ""xxx"" or getPropValue(""system.azure.tag.xxx_service-group"") == ""xxx"" or getPropValue(""system.gcp.tag.xxx_service-group"") == ""xxx""  ,group name: lmsupport ,appliesTo= cms_service_level != ""cet_8x5"" ,add group number is 4",
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

	[Fact]
	public void HostGroupsWithDeleteAndAddNumber_Success()
		=> AssertToAuditEventSucceeds(
			@" host(id=4582,name =resourceName) add to LM Push SG ,appliesTo=servicenow.servicegroup_sys_id == \""xxx\"" or getPropValue(\""system.aws.tag.xxx_service-group\"") == \""xxx\"" or getPropValue(\""system.azure.tag.xxx_service-group\"") == \""xxx\"" or getPropValue(\""system.gcp.tag.xxx_service-group\"") == \""xxx\"" ,delete number is 0 ,add number is 1",
			new()
			{
				MatchedRegExId = 19,
				ResourceIds = [4582],
				ResourceNames = ["resourceName"],
				ActionType = AuditEventActionType.Create,
				EntityType = AuditEventEntityType.ResourceGroups,
				OutcomeType = AuditEventOutcomeType.Success
			}
		);

	[Fact]
	public void DeleteKubernetesHosts_Success()
		=> AssertToAuditEventSucceeds(
			@"Delete the Kubernetes hosts those were marked for deletion [argus-5848fb564c-v7h75-pod-logicmonitor-PDL-K8S-TEST-636946876(id=8443)]",
			new()
			{
				MatchedRegExId = 18,
				ResourceIds = [8443],
				ResourceNames = ["argus-5848fb564c-v7h75-pod-logicmonitor-PDL-K8S-TEST-636946876"],
				ActionType = AuditEventActionType.Delete,
				EntityType = AuditEventEntityType.Resource,
				OutcomeType = AuditEventOutcomeType.Success
			}
		);

	[Fact]
	public void DeleteKubernetesHostsMultiple_Success()
		=> AssertToAuditEventSucceeds(
			@"Delete the Kubernetes hosts those were marked for deletion [collectorset-controller-54f4644c65-59jmm-pod-logicmonitor-PDL-K8S-TEST-442068781(id=8595), argus-5848fb564c-kl52v-pod-logicmonitor-PDL-K8S-TEST-4069678789(id=8603), argus-5848fb564c-tbx4r-pod-logicmonitor-PDL-K8S-TEST-460934296-2144132493(id=8581), collectorset-controller-54f4644c65-mqnrr-pod-logicmonitor-PDL-K8S-TEST-199135028-2350553716(id=8438)]",
			new()
			{
				MatchedRegExId = 18,
				ResourceIds = [
					8595,
					8603,
					8581,
					8438
				],
				ResourceNames = [
					"collectorset-controller-54f4644c65-59jmm-pod-logicmonitor-PDL-K8S-TEST-442068781",
					"argus-5848fb564c-kl52v-pod-logicmonitor-PDL-K8S-TEST-4069678789",
					"argus-5848fb564c-tbx4r-pod-logicmonitor-PDL-K8S-TEST-460934296-2144132493",
					"collectorset-controller-54f4644c65-mqnrr-pod-logicmonitor-PDL-K8S-TEST-199135028-2350553716"
				],
				ActionType = AuditEventActionType.Delete,
				EntityType = AuditEventEntityType.Resource,
				OutcomeType = AuditEventOutcomeType.Success
			}
		);

	[Fact]
	public void AddDeviceDataSourceInstance_Success()
		=> AssertToAuditEventSucceeds(
			@"""Action=Add""; ""Type=Instance""; ""Device=LM Push Server""; ""InstanceName=Google (https://google.com)""; ""Description=DataSourceName: HTTP per Page- """,
			new()
			{
				MatchedRegExId = 20,
				ResourceNames = ["LM Push Server"],
				LogicModuleName = "HTTP per Page-",
				InstanceName = "Google",
				WildValue = "https://google.com",
				ActionType = AuditEventActionType.Create,
				EntityType = AuditEventEntityType.ResourceDataSourceInstance,
				OutcomeType = AuditEventOutcomeType.Success
			}
		);

	[Fact]
	public void SAMLLogin_Success()
		=> AssertToAuditEventSucceeds(
			@"some.user123@domain.com signs in via SAML",
			new()
			{
				MatchedRegExId = 21,
				UserName = "some.user123@domain.com",
				ActionType = AuditEventActionType.Login,
				EntityType = AuditEventEntityType.None,
				OutcomeType = AuditEventOutcomeType.Success
			}
		);

	[Fact]
	public void FailedApiRequest_Failure()
		=> AssertToAuditEventSucceeds(
			@"Failed API request: API token TOKENID attempted to access path '/santaba/rest/device/groups/1613/devices' with Method: GET",
			new()
			{
				MatchedRegExId = 22,
				ActionType = AuditEventActionType.GeneralApi,
				OutcomeType = AuditEventOutcomeType.Failure,
				ApiTokenId = "TOKENID",
				ApiMethod = "GET",
				ApiPath = "/santaba/rest/device/groups/1613/devices"
			}
		);

	[Fact]
	public void DeleteAwsHostsMultiple_Success()
		=> AssertToAuditEventSucceeds(
			@"Delete the aws hosts [EU-W1:i-0ad560910aee79179(id=4573), EU-W1:i-0070bf1c74503d8ed(id=4574)]",
			new()
			{
				MatchedRegExId = 23,
				ResourceIds = [
					4573,
					4574
				],
				ResourceNames = [
					"EU-W1:i-0ad560910aee79179",
					"EU-W1:i-0070bf1c74503d8ed"
				],
				ActionType = AuditEventActionType.Delete,
				EntityType = AuditEventEntityType.Resource,
				OutcomeType = AuditEventOutcomeType.Success
			}
		);

	[Theory]
	[InlineData("some.user.admin signs in (adminId=123).", "some.user.admin", 123, 24)]
	[InlineData("alice.brown log in.", "alice.brown", null, 66)]
	public void Login_Success(
		string logItemMessage,
		string expectedUsername,
		int? expectedId,
		int expectedMatchedRegExId)
		=> AssertToAuditEventSucceeds(
			logItemMessage,
			new()
			{
				MatchedRegExId = expectedMatchedRegExId,
				UserName = expectedUsername,
				UserId = expectedId,
				ActionType = AuditEventActionType.Login,
				EntityType = AuditEventEntityType.Account,
				OutcomeType = AuditEventOutcomeType.Success
			}
		);

	[Theory]
	[InlineData("remco.bekker@global.ntt signs out (adminId=1037).", "remco.bekker@global.ntt", 1037, 87)]
	public void Logout_Success(
		string logItemMessage,
		string expectedUsername,
		int? expectedId,
		int expectedMatchedRegExId)
		=> AssertToAuditEventSucceeds(
			logItemMessage,
			new()
			{
				MatchedRegExId = expectedMatchedRegExId,
				UserName = expectedUsername,
				UserId = expectedId,
				ActionType = AuditEventActionType.Logout,
				EntityType = AuditEventEntityType.Account,
				OutcomeType = AuditEventOutcomeType.Success
			}
		);

	[Fact]
	public void AddNewAccountAdmin_Success()
		=> AssertToAuditEventSucceeds(
			@"Add a new account some.user.admin (administrator)",
			new()
			{
				MatchedRegExId = 25,
				UserName = "some.user.admin",
				ActionType = AuditEventActionType.Create,
				EntityType = AuditEventEntityType.Account,
				OutcomeType = AuditEventOutcomeType.Success
			}
		);

	[Fact]
	public void AddApiTokenForApiTokenUser_Success()
		=> AssertToAuditEventSucceeds(
			@"Add new api token - I9fc6ID2QyCH27scuRZC for API token user",
			new()
			{
				MatchedRegExId = 88,
				ActionType = AuditEventActionType.Create,
				EntityType = AuditEventEntityType.ApiToken,
				OutcomeType = AuditEventOutcomeType.Success,
				ApiTokenId = "I9fc6ID2QyCH27scuRZC"
			}
		);

	[Fact]
	public void AddWidgetToDashboard_Success()
		=> AssertToAuditEventSucceeds(
			@"Add a widget SRV000024036 eul1900684-phxdbpro-Fra reclaimable overview to dashboard Wolters Oracle Capacity",
			new()
			{
				MatchedRegExId = 89,
				ActionType = AuditEventActionType.Create,
				EntityType = AuditEventEntityType.Widget,
				OutcomeType = AuditEventOutcomeType.Success,
				WidgetName = "SRV000024036 eul1900684-phxdbpro-Fra reclaimable overview",
				DashboardName = "Wolters Oracle Capacity"
			}
		);

	[Fact]
	public void EditDashboard_Success()
		=> AssertToAuditEventSucceeds(
			@"Edit the dashboard Wolters Oracle Capacity",
			new()
			{
				MatchedRegExId = 90,
				ActionType = AuditEventActionType.Update,
				EntityType = AuditEventEntityType.Dashboard,
				OutcomeType = AuditEventOutcomeType.Success,
				DashboardName = "Wolters Oracle Capacity"
			}
		);

	[Fact]
	public void CreateDashboard_Success()
		=> AssertToAuditEventSucceeds(
			@"Create a dashboard DBA PROD-APP-SEA02",
			new()
			{
				MatchedRegExId = 99,
				ActionType = AuditEventActionType.Create,
				EntityType = AuditEventEntityType.Dashboard,
				OutcomeType = AuditEventOutcomeType.Success,
				DashboardName = "DBA PROD-APP-SEA02"
			}
		);

	[Fact]
	public void UpdateReport_Success()
		=> AssertToAuditEventSucceeds(
			@"Update report AEM-CPU",
			new()
			{
				MatchedRegExId = 101,
				ActionType = AuditEventActionType.Update,
				EntityType = AuditEventEntityType.Report,
				OutcomeType = AuditEventOutcomeType.Success,
				ResourceNames = ["AEM-CPU"]
			}
		);

	[Fact]
	public void AddReport_Success()
		=> AssertToAuditEventSucceeds(
			@"Add report Standard Bank - Alert Thresholds",
			new()
			{
				MatchedRegExId = 103,
				ActionType = AuditEventActionType.Create,
				EntityType = AuditEventEntityType.Report,
				OutcomeType = AuditEventOutcomeType.Success,
				ResourceNames = ["Standard Bank - Alert Thresholds"]
			}
		);

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
			@"Dashboard 'AirLife (Alias Sunmed)' renamed to 'DBA PROD-APP-SEA02'",
			new()
			{
				MatchedRegExId = 105,
				ActionType = AuditEventActionType.Update,
				EntityType = AuditEventEntityType.Dashboard,
				OutcomeType = AuditEventOutcomeType.Success,
				DashboardName = "DBA PROD-APP-SEA02",
				Description = "AirLife (Alias Sunmed)"
			}
		);

	[Fact]
	public void DeleteDashboardGroup_Success()
		=> AssertToAuditEventSucceeds(
			@"Delete the dashboard group AirLife (Alias Sunmed)",
			new()
			{
				MatchedRegExId = 106,
				ActionType = AuditEventActionType.Delete,
				EntityType = AuditEventEntityType.DashboardGroup,
				OutcomeType = AuditEventOutcomeType.Success,
				ResourceGroupName = "AirLife (Alias Sunmed)"
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
			@"Edit the widget phxdbpro - Fra usage overview of dashboard Wolters Oracle Capacity",
			new()
			{
				MatchedRegExId = 91,
				ActionType = AuditEventActionType.Update,
				EntityType = AuditEventEntityType.Widget,
				OutcomeType = AuditEventOutcomeType.Success,
				WidgetName = "phxdbpro - Fra usage overview",
				DashboardName = "Wolters Oracle Capacity"
			}
		);

	[Fact]
	public void DeleteWidgetOfDashboard_Success()
		=> AssertToAuditEventSucceeds(
			@"Delete the widget phxdbpro - Fra usage overview of dashboard Wolters Oracle Capacity",
			new()
			{
				MatchedRegExId = 92,
				ActionType = AuditEventActionType.Delete,
				EntityType = AuditEventEntityType.Widget,
				OutcomeType = AuditEventOutcomeType.Success,
				WidgetName = "phxdbpro - Fra usage overview",
				DashboardName = "Wolters Oracle Capacity"
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
	public void SetAllInstancesDatapointThresholdOnDevice_Success()
		=> AssertToAuditEventSucceeds(
			@"Set all instances' datapoint (18371:NoData) alert threshold as (NO CHANGE), alert enable as (false) under the instance groups(0:@default) of device(1416411:SRV000013100 Tatenen)",
			new()
			{
				MatchedRegExId = 94,
				ActionType = AuditEventActionType.Update,
				EntityType = AuditEventEntityType.ResourceDataSourceInstance,
				OutcomeType = AuditEventOutcomeType.Success,
				ResourceIds = [1416411],
				ResourceNames = ["SRV000013100 Tatenen"],
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
	public void UnknownDebugCommand_Failure()
		=> AssertToAuditEventSucceeds(
			@"""Unknown debug command""; ""Command=!script""; ""AgentId=60""; ""Company=nttdatagtsus1"";",
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
				DataSourceNewInstanceIds = [426808630, 426808624, 426808618]
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
				DataSourceNewInstanceIds = [426808630, 426808624, 426808618]
			}
		);

	[Fact]
	public void ShareDashboard_Success()
		=> AssertToAuditEventSucceeds(
			@"ankit15.patel@global.ntt share a dashboard(DBA PROD-APP-SEA02)",
			new()
			{
				MatchedRegExId = 109,
				ActionType = AuditEventActionType.Update,
				EntityType = AuditEventEntityType.Dashboard,
				OutcomeType = AuditEventOutcomeType.Success,
				UserName = "ankit15.patel@global.ntt",
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
				DataSourceNewInstanceIds = [426199595, 426199584]
			}
		);

	[Fact]
	public void UpdateWebsite_Success()
		=> AssertToAuditEventSucceeds(
			@"Update website http:__10.41.10.25:8081_sapdashboard - SRV000056568 - WEB0004720 ,   {
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
}""Action=Update""; ""Type=Group""; ""DeviceGroup=NTTCMS Integrator/EisnerAmper/EA-AZURE-SUBSCRIPTIONS - Prod - PL00008552/ea-clientportal-sandbox (SG00023621)/CLA0003221 ea-clientportal-sandbox/ea-clientportal-sandbox""; ""Description=""",
			new()
			{
				MatchedRegExId = 97,
				ActionType = AuditEventActionType.Update,
				EntityType = AuditEventEntityType.ResourceGroup,
				OutcomeType = AuditEventOutcomeType.Success,
				ResourceGroupName = "NTTCMS Integrator/EisnerAmper/EA-AZURE-SUBSCRIPTIONS - Prod - PL00008552/ea-clientportal-sandbox (SG00023621)/CLA0003221 ea-clientportal-sandbox/ea-clientportal-sandbox",
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
			+ "\"Action=Update\"; \"Type=Group\"; \"DeviceGroup=NTTCMS Integrator/EisnerAmper/EA-AZURE-SUBSCRIPTIONS - Prod - PL00008552/ea-clientportal-sandbox (SG00023621)/CLA0003221 ea-clientportal-sandbox/ea-clientportal-sandbox\"; \"Description=\"";

		AssertToAuditEventSucceeds(
			message,
			new()
			{
				MatchedRegExId = 97,
				ActionType = AuditEventActionType.Update,
				EntityType = AuditEventEntityType.ResourceGroup,
				OutcomeType = AuditEventOutcomeType.Success,
				ResourceGroupName = "NTTCMS Integrator/EisnerAmper/EA-AZURE-SUBSCRIPTIONS - Prod - PL00008552/ea-clientportal-sandbox (SG00023621)/CLA0003221 ea-clientportal-sandbox/ea-clientportal-sandbox",
				Description = string.Empty,
			}
		);
	}

	[Fact]
	public void UpdateGroupThresholdsWithEmptyAlertThresholdChanges_Success()
		=> AssertToAuditEventSucceeds(
			@"""Action=Update""; ""Type=Group""; ""Device=NA""; ""GroupName=Toppan Group""; ""Description= Enable alerting on datapoint syncStatus ""; ""Alert_threshold_changes=""; ""DataSource=Fortinet_FortiGate_HighAvailabilityPeers""; ""DataSourceId=5804476""; ""Reason=Datapoint alerting enabled""",
			new()
			{
				MatchedRegExId = 46,
				ActionType = AuditEventActionType.Update,
				EntityType = AuditEventEntityType.ResourceGroup,
				OutcomeType = AuditEventOutcomeType.Success,
				ResourceGroupName = "Toppan Group",
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
			@"Change host collectors: host<378754>(SRV000109569 sgcaplmprod002), preferred<55> , current<55> to collector <57>(DC\SGCAPLMPROD003)",
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
			@"""Action=Update""; ""Type=EventSource""; ""LogicModuleName=NTT_MCN_Prisma_SDWAN_Alerts_new""; ""Device=NA""; ""LogicModuleId=61""; ""Description=Updated directly by user; Diff=Update display name from NTT_MCN_Prisma_SDWAN_Alerts to NTT_MCN_Prisma_SDWAN_Alerts_new; ; "";",
			new()
			{
				MatchedRegExId = 115,
				ActionType = AuditEventActionType.Update,
				EntityType = AuditEventEntityType.EventSource,
				LogicModuleName = "NTT_MCN_Prisma_SDWAN_Alerts_new",
				LogicModuleId = 61,
				Description = "Updated directly by user; Diff=Update display name from NTT_MCN_Prisma_SDWAN_Alerts to NTT_MCN_Prisma_SDWAN_Alerts_new; ; ",
				OutcomeType = AuditEventOutcomeType.Success
			}
		);

	[Fact]
	public void DeleteSdt_Success()
		=> AssertToAuditEventSucceeds(
			@"Delete SDT from 2022-05-18 08:51:27 GMT to 2022-05-18 09:51:27 GMT from Datasource Collector DNS Resolving on Host somehost name via API token xx123xxx",
			new()
			{
				MatchedRegExId = 30,
				ActionType = AuditEventActionType.Delete,
				EntityType = AuditEventEntityType.ScheduledDownTime,
				ResourceNames = ["somehost name"],
				OutcomeType = AuditEventOutcomeType.Success,
				ApiTokenId = "xx123xxx"
			}
		);

	[Fact]
	public void DeleteSdt_GroupQuotedFormat_Success()
		=> AssertToAuditEventSucceeds(
			@"""Action=Delete""; ""Type=SDT""; ""Description= Delete SDT for Group CSM Bakeries on Group Path NTTData GTS Integrator/CSM Bakeries with scheduled downtime from 2025-12-31 04:25:00 GMT to 2027-06-30 05:25:00 GMT ""; ""DeviceGroupName=CSM Bakeries""; ""DeviceGroupId=3786""; ""StartDownTime=2025-12-31 04:25:00 GMT""; ""EndDownTime=2027-06-30 05:25:00 GMT"";",
			new()
			{
				MatchedRegExId = 38,
				ActionType = AuditEventActionType.Delete,
				EntityType = AuditEventEntityType.ScheduledDownTime,
				Description = " Delete SDT for Group CSM Bakeries on Group Path NTTData GTS Integrator/CSM Bakeries with scheduled downtime from 2025-12-31 04:25:00 GMT to 2027-06-30 05:25:00 GMT ",
				OutcomeType = AuditEventOutcomeType.Success,
				ResourceNames = ["CSM Bakeries"],
				ResourceIds = [3786],
				StartDownTime = "2025-12-31 04:25:00 GMT",
				EndDownTime = "2027-06-30 05:25:00 GMT"
			}
		);

	[Fact]
	public void AddSdt_Success()
		=> AssertToAuditEventSucceeds(
			@"Add SDT for Datasource Collector DNS Resolving on Host somehost name with scheduled downtime from 2022-05-18 08:53:39 GMT to 2022-05-18 09:53:39 GMT via API token xx123xxx",
			new()
			{
				MatchedRegExId = 48,
				ActionType = AuditEventActionType.Create,
				EntityType = AuditEventEntityType.ScheduledDownTime,
				ResourceNames = ["somehost name"],
				OutcomeType = AuditEventOutcomeType.Success,
				ApiTokenId = "xx123xxx"
			}
		);

	[Theory]
	[InlineData(
		@"Added ResourceGroup Integration Testing/Test (6704) , ",
		"Integration Testing/Test",
		6704
		)]
	[InlineData(
		"Added ResourceGroup BLAH (1234) ,  appliesTo=join(system.staticgroups,\",\")=~\"WOO\" && system.azure.tag.application == \"Azure virtual desktop\" && system.displayname =~ \"YAY\" ,add hosts total=6 ,hosts list :  host(id=1, name=AAA (BBB)) , host(id=2, name=CCC (DDD)) , host(id=3, name=EEE (FFF)) , host(id=4, name=GGG (HHH)) , host(id=5, name=III (JJJ)) , host(id=6, name=KKK (LLL))",
		"BLAH",
		1234
		)]
	public void AddDeviceGroup_Success(
		string logItemMessage,
		string expectedResourceGroupName,
		int expectedResourceGroupId
		)
		=> AssertToAuditEventSucceeds(
	logItemMessage,
	new()
	{
		MatchedRegExId = 32,
		EntityType = AuditEventEntityType.ResourceGroup,
		ActionType = AuditEventActionType.Create,
		OutcomeType = AuditEventOutcomeType.Success,
		ResourceGroupName = expectedResourceGroupName,
		ResourceGroupId = expectedResourceGroupId
	}
);

	[Fact]
	public void AddDataSource_Succeeds()
		=> AssertToAuditEventSucceeds(
			@"""Action=Add""; ""Type=DataSource""; ""DataSourceName=nttcms_ALL_ALL_IP_Addresses""; ""DeviceName=127.0.0.1""; ""DeviceId=4808""; ""Description=Addition of datasource to device""; ""DataSourceId=33514257""; ""DeviceDataSourceId=52050""",
			new()
			{
				MatchedRegExId = 33,
				ActionType = AuditEventActionType.Create,
				EntityType = AuditEventEntityType.DataSource,
				ResourceIds = [4808],
				LogicModuleId = 33514257,
				LogicModuleName = "nttcms_ALL_ALL_IP_Addresses",
				OutcomeType = AuditEventOutcomeType.Success
			}
		);

	[Fact]
	public void BangAccount_Success()
		=> AssertToAuditEventSucceeds(
			@"!account run by bob@bob.com on collector (id=123, hostname=Woo, desc=Yay)",
			new()
			{
				MatchedRegExId = 40,
				ActionType = AuditEventActionType.Run,
				CollectorId = 123,
				CollectorName = "Woo",
				CollectorDescription = "Yay",
				EntityType = AuditEventEntityType.AllCollectors,
				OutcomeType = AuditEventOutcomeType.Success,
				Command = "!account"
			}
		);

	[Fact]
	public void AddAccount_Success()
		=> AssertToAuditEventSucceeds(
			@"Add a new account bob@bob.com (Default Role)",
			new()
			{
				MatchedRegExId = 64,
				ActionType = AuditEventActionType.Create,
				EntityType = AuditEventEntityType.Account,
				OutcomeType = AuditEventOutcomeType.Success,
				UserName = "bob@bob.com",
				UserRole = "Default Role",
			}
		);

	[Theory]
	[InlineData("Enable WinVolumeUsage- for hostgroup Virtual Machine", "WinVolumeUsage-", "Virtual Machine")]
	[InlineData("Enable WinUDP for hostgroup Woo", "WinUDP", "Woo")]
	public void EnableDataSourceForResourceGroup_Success(
		string logItemMessage,
		string expectedLogicModuleName,
		string expectedResourceGroupName)
		=> AssertToAuditEventSucceeds(
	logItemMessage,
	new()
	{
		MatchedRegExId = 67,
		ActionType = AuditEventActionType.Enable,
		EntityType = AuditEventEntityType.DataSource,
		OutcomeType = AuditEventOutcomeType.Success,
		LogicModuleName = expectedLogicModuleName,
		ResourceGroupName = expectedResourceGroupName
	}
);

	[Theory]
	[InlineData("Disable WinVolumeUsage- for hostgroup Virtual Machine", "WinVolumeUsage-", "Virtual Machine")]
	[InlineData("Disable WinUDP for hostgroup Woo", "WinUDP", "Woo")]
	public void DisableDataSourceForResourceGroup_Success(
		string logItemMessage,
		string expectedLogicModuleName,
		string expectedResourceGroupName)
		=> AssertToAuditEventSucceeds(
	logItemMessage,
	new()
	{
		MatchedRegExId = 67,
		ActionType = AuditEventActionType.Disable,
		EntityType = AuditEventEntityType.DataSource,
		OutcomeType = AuditEventOutcomeType.Success,
		LogicModuleName = expectedLogicModuleName,
		ResourceGroupName = expectedResourceGroupName
	}
);

	[Theory]
	[InlineData("Request remote ssh session to 1.2.3.4", "ssh", "1.2.3.4")]
	[InlineData("Request remote rdp session to 5.6.7.8", "rdp", "5.6.7.8")]
	public void RequestRemoteSession_Success(
		string logItemMessage,
		string expectedRemoteSessionType,
		string expectedResourceHostname)
		=> AssertToAuditEventSucceeds(
	logItemMessage,
	new()
	{
		MatchedRegExId = 68,
		ActionType = AuditEventActionType.RequestRemoteSession,
		EntityType = AuditEventEntityType.Resource,
		OutcomeType = AuditEventOutcomeType.Success,
		ResourceHostname = expectedResourceHostname,
		RemoteSessionType = expectedRemoteSessionType
	}
);

	[Theory]
	[InlineData("Suspended SAML user bob@cratchett.com tried to login", "bob@cratchett.com")]
	[InlineData("Suspended SAML user bob2@cratchett.com tried to login", "bob2@cratchett.com")]
	public void FailedLogin_Success(
		string logItemMessage,
		string expectedUserName)
		=> AssertToAuditEventSucceeds(
	logItemMessage,
	new()
	{
		MatchedRegExId = 69,
		ActionType = AuditEventActionType.Login,
		EntityType = AuditEventEntityType.Account,
		OutcomeType = AuditEventOutcomeType.Failure,
		UserName = expectedUserName,
	}
);

	[Theory]
	[InlineData("alice@bob.com enabled Two Factor Authentication.", "alice@bob.com")]
	[InlineData("bob@cratchett.com disabled Two Factor Authentication.", "bob@cratchett.com")]
	public void Enable2fa_Success(
		string logItemMessage,
		string expectedUserName)
		=> AssertToAuditEventSucceeds(
	logItemMessage,
	new()
	{
		MatchedRegExId = 70,
		ActionType = AuditEventActionType.Update,
		EntityType = AuditEventEntityType.Account,
		OutcomeType = AuditEventOutcomeType.Success,
		UserName = expectedUserName,
	}
);

	[Theory]
	[InlineData(
		"Download ConfigSource<running-config>. \"ConfigSourceInstanceId=1234\"; \"ConfigSourceName=Cisco_IOS\"; \"ConfigVersion=4\"; \"DeviceName=DeviceA\"; \"DeviceId=5678\";",
		73,
		AuditEventEntityType.ConfigSource,
		"running-config",
		1234,
		"Cisco_IOS",
		4,
		"DeviceA",
		5678)]
	[InlineData(
		"Download DataSource<running-config>. \"DataSourceInstanceId=1234\"; \"DataSourceName=Cisco_IOS\"; \"DataSourceVersion=4\"; \"DeviceName=DeviceA\"; \"DeviceId=5678\";",
		74,
		AuditEventEntityType.DataSource,
		"running-config",
		1234,
		"Cisco_IOS",
		4,
		"DeviceA",
		5678)]
	public void DownloadLogicModule_Success(
		string logItemMessage,
		int expectedMatchRegexId,
		AuditEventEntityType expectedAuditEventEntityType,
		string expectedInstanceName,
		int expectedInstanceId,
		string expectedLogicModuleName,
		int expectedLogicModuleVersion,
		string expectedResourceName,
		int expectedResourceId)
		=> AssertToAuditEventSucceeds(
	logItemMessage,
	new()
	{
		MatchedRegExId = expectedMatchRegexId,
		ActionType = AuditEventActionType.Download,
		OutcomeType = AuditEventOutcomeType.Success,
		EntityType = expectedAuditEventEntityType,
		InstanceName = expectedInstanceName,
		InstanceId = expectedInstanceId,
		LogicModuleName = expectedLogicModuleName,
		LogicModuleVersion = expectedLogicModuleVersion,
		ResourceNames = [expectedResourceName],
		ResourceIds = [expectedResourceId]
	}
);

	[Theory]
	[InlineData("User(name=alice@bob.com, email=alice@bob.com) forgot password", "alice@bob.com", "alice@bob.com")]
	public void RequestPasswordReset_Success(
		string logItemMessage,
		string expectedUserName,
		string expectedUserEmail)
		=> AssertToAuditEventSucceeds(
	logItemMessage,
	new()
	{
		MatchedRegExId = 75,
		ActionType = AuditEventActionType.RequestPasswordReset,
		EntityType = AuditEventEntityType.Account,
		OutcomeType = AuditEventOutcomeType.Success,
		UserName = expectedUserName,
		UserEmail = expectedUserEmail
	}
);

	[Theory]
	[InlineData("update  account alice@bob.com (xxx)", "alice@bob.com", "xxx")]
	[InlineData("update  account bob@cratchett.com ()", "bob@cratchett.com", "")]
	public void UpdateAccount_Success(
		string logItemMessage,
		string expectedUserName,
		string expectedDescription)
		=> AssertToAuditEventSucceeds(
	logItemMessage,
	new()
	{
		MatchedRegExId = 76,
		ActionType = AuditEventActionType.Update,
		EntityType = AuditEventEntityType.Account,
		OutcomeType = AuditEventOutcomeType.Success,
		Description = expectedDescription,
		UserName = expectedUserName
	}
);

	[Theory]
	[InlineData(
		"\"Action=Update\"; \"Type=Instance\"; \"Device=DeviceA\"; \"InstanceName=WinVolumeUsage_old-C:\\\"; \"Description=enable alerting on this instance;  \"",
		"DeviceA",
		"WinVolumeUsage_old-C:\\",
		"enable alerting on this instance;  "
	)]
	public void UpdateInstanceDisableAlerting_Success(
		string logItemMessage,
		string expectedResourceName,
		string expectedInstanceName,
		string expectedDescription)
		=> AssertToAuditEventSucceeds(
	logItemMessage,
	new()
	{
		MatchedRegExId = 77,
		ActionType = AuditEventActionType.Update,
		EntityType = AuditEventEntityType.ResourceDataSourceInstance,
		OutcomeType = AuditEventOutcomeType.Success,
		ResourceNames = [expectedResourceName],
		InstanceName = expectedInstanceName,
		Description = expectedDescription,
	}
);

	[Fact]
	public void DeleteInstanceWithEmptyDescription_Success()
		=> AssertToAuditEventSucceeds(
		@"""Action=Delete""; ""Type=Instance""; ""Device=SRV000129223 vfrawapdpaprd03""; ""InstanceName=SSL_Certificates-HTTPS""; ""Description=""",
		new()
		{
			MatchedRegExId = 77,
			ActionType = AuditEventActionType.Delete,
			EntityType = AuditEventEntityType.ResourceDataSourceInstance,
			OutcomeType = AuditEventOutcomeType.Success,
			ResourceNames = ["SRV000129223 vfrawapdpaprd03"],
			InstanceName = "SSL_Certificates-HTTPS",
			Description = string.Empty,
		}
	);

	[Theory]
	[InlineData(
		"Remote rdp session 1234 to 5.6.7.8 started at 09:10",
		"rdp",
		1234,
		"5.6.7.8",
		AuditEventActionType.Start,
		"09:10")]
	[InlineData(
		"Remote ssh session 4321 to 8.7.6.5 terminated at 12:34",
		"ssh",
		4321,
		"8.7.6.5",
		AuditEventActionType.End,
		"12:34")]
	public void RemoteSession_Success(
		string logItemMessage,
		string expectedRemoteSessionType,
		int expectedRemoteSessionId,
		string expectedResourceHostname,
		AuditEventActionType expectedActionType,
		string expectedTime)
		=> AssertToAuditEventSucceeds(
	logItemMessage,
	new()
	{
		MatchedRegExId = 78,
		EntityType = AuditEventEntityType.Resource,
		OutcomeType = AuditEventOutcomeType.Success,
		ActionType = expectedActionType,
		RemoteSessionType = expectedRemoteSessionType,
		RemoteSessionId = expectedRemoteSessionId,
		ResourceHostname = expectedResourceHostname,
		Time = expectedTime

	}
);

	[Theory]
	[InlineData(
		"lmsupport signs in on behalf of alice.brown - restrictSSO=false (adminId=2).",
		"alice.brown",
		false,
		2)]
	[InlineData(
		"lmsupport signs in on behalf of clive.down - restrictSSO=true (adminId=4).",
		"clive.down",
		true,
		4)]
	public void SupportLogin_Success(
		string logItemMessage,
		string expectedUserName,
		bool expectedRestrictSso,
		int expectedAdminId)
		=> AssertToAuditEventSucceeds(
	logItemMessage,
	new()
	{
		MatchedRegExId = 80,
		EntityType = AuditEventEntityType.Account,
		ActionType = AuditEventActionType.Login,
		OutcomeType = AuditEventOutcomeType.Success,
		UserName = expectedUserName,
		UserId = expectedAdminId,
		RestrictSso = expectedRestrictSso
	}
);

	[Theory]
	[InlineData(
		"Re-balanced collector group ABC(123),XXX",
		"ABC",
		123,
		"XXX")]
	public void ResourceRebalanceCollectorGroup_Success(
		string logItemMessage,
		string expectedCollectorGroupName,
		int expectedCollectorGroupId,
		string expectedDescription)
		=> AssertToAuditEventSucceeds(
	logItemMessage,
	new()
	{
		MatchedRegExId = 81,
		EntityType = AuditEventEntityType.CollectorGroup,
		ActionType = AuditEventActionType.Rebalance,
		OutcomeType = AuditEventOutcomeType.Success,
		CollectorGroupName = expectedCollectorGroupName,
		CollectorGroupId = expectedCollectorGroupId,
		Description = expectedDescription
	}
);

	[Theory]
	[InlineData(
		"Schedule collect now for ConfigSource instance<123>. \"ConfigSourceInstanceName=ABC\"; \"ConfigSourceName=IOS Configs\"; \"DeviceName=DeviceA\"; \"DeviceId=5678\";",
		123,
		"ABC",
		"IOS Configs",
		"DeviceA",
		5678)]
	public void ScheduleCollectNow_Success(
		string logItemMessage,
		int expectedInstanceId,
		string expectedInstanceName,
		string expectedLogicModuleName,
		string expectedResourceName,
		int expectedResourceId
		)
		=> AssertToAuditEventSucceeds(
	logItemMessage,
	new()
	{
		MatchedRegExId = 82,
		EntityType = AuditEventEntityType.ConfigSourceInstance,
		ActionType = AuditEventActionType.CollectNow,
		OutcomeType = AuditEventOutcomeType.Success,
		InstanceId = expectedInstanceId,
		InstanceName = expectedInstanceName,
		LogicModuleName = expectedLogicModuleName,
		ResourceNames = [expectedResourceName],
		ResourceIds = [expectedResourceId]
	}
);


	[Fact]
	public void TestScriptScheduled_Success()
		=> AssertToAuditEventSucceeds(
			@"""Action=Test script scheduled""; ""Description=Schedule""; ""LogicModuleType=autodiscovery""; ""LogicModuleName=Extreme_Access_Points_Ping""; ""Script=`some script content`""",
			new()
			{
				MatchedRegExId = 0,
				ActionType = AuditEventActionType.TestScriptScheduled,
				EntityType = AuditEventEntityType.TestScriptScheduled,
				OutcomeType = AuditEventOutcomeType.Success,
			}
		);

	[Fact]
	public void AddNewDataSource_LargePayload_Success()
		=> AssertToAuditEventSucceeds(
			"Add new DataSource 'NTT_MCN_ADEM_SITE_ISSUES'. Imported from JSON file. Change details: \"Action=Add\"; \"Type=DataSource\"; \"LogicModuleName=NTT_MCN_ADEM_SITE_ISSUES\"; \"Device=NA\"; \"Description={...very long...}\"",
			new()
			{
				MatchedRegExId = 0,
				ActionType = AuditEventActionType.Create,
				EntityType = AuditEventEntityType.DataSource,
				LogicModuleName = "NTT_MCN_ADEM_SITE_ISSUES",
				OutcomeType = AuditEventOutcomeType.Success,
			}
		);

	[Fact]
	public void AddNewPropertySource_LargePayload_Success()
		=> AssertToAuditEventSucceeds(
			"Add new PropertySource 'addERI_ISIS'. Imported from JSON file. Change details: \"Action=Add\"; \"Type=PropertySource\"; \"LogicModuleName=addERI_ISIS\"; \"Device=NA\"; \"Description={...very long...}\"",
			new()
			{
				MatchedRegExId = 0,
				ActionType = AuditEventActionType.Create,
				EntityType = AuditEventEntityType.PropertySource,
				LogicModuleName = "addERI_ISIS",
				OutcomeType = AuditEventOutcomeType.Success,
			}
		);

	[Fact]
	public void AddNewTopologySource_LargePayload_Success()
		=> AssertToAuditEventSucceeds(
			"Add new TopologySource 'Fortinet_FortiGate_SDWAN'. Imported from JSON file. Change details: \"Action=Add\"; \"Type=TopologySource\"; \"LogicModuleName=Fortinet_FortiGate_SDWAN\"; \"Device=NA\"; \"Description={...very long...}\"",
			new()
			{
				MatchedRegExId = 0,
				ActionType = AuditEventActionType.Create,
				EntityType = AuditEventEntityType.TopologySource,
				LogicModuleName = "Fortinet_FortiGate_SDWAN",
				OutcomeType = AuditEventOutcomeType.Success,
			}
		);

	[Fact]
	public void ImportEventSourceFromXml_Success()
		=> AssertToAuditEventSucceeds(
			"Import EventSource from XML. Change details : \"Action=Add\"; \"Type=EventSource\"; \"LogicModuleName=NTT_MCN_Prisma_SDWAN_Alerts\"; \"Device=NA\"; \"Description=Add eventsource : NTT_MCN_Prisma_SDWAN_Alerts, id = 85\"",
			new()
			{
				MatchedRegExId = 0,
				ActionType = AuditEventActionType.Create,
				EntityType = AuditEventEntityType.EventSource,
				LogicModuleName = "NTT_MCN_Prisma_SDWAN_Alerts",
				OutcomeType = AuditEventOutcomeType.Success,
			}
		);

	[Fact]
	public void AddWebsiteViaUrlCheck_Success()
		=> AssertToAuditEventSucceeds(
			"Add website SpektraEdgeController Availability monitoring via URL check",
			new()
			{
				MatchedRegExId = 0,
				ActionType = AuditEventActionType.Create,
				EntityType = AuditEventEntityType.Website,
				WebsiteName = "SpektraEdgeController Availability monitoring",
				OutcomeType = AuditEventOutcomeType.Success,
			}
		);

	[Fact]
	public void UpdateWebsiteSettings_Success()
		=> AssertToAuditEventSucceeds(
			"Update website SpektraEdgeController Availability monitoring via URL check , [Modified param individualSmAlertEnable: true -> false]",
			new()
			{
				MatchedRegExId = 0,
				ActionType = AuditEventActionType.Update,
				EntityType = AuditEventEntityType.Website,
				WebsiteName = "SpektraEdgeController Availability monitoring",
				OutcomeType = AuditEventOutcomeType.Success,
			}
		);

	[Theory]
	[InlineData(
		@"""Action=Delete""; ""Type=PropertySource""; ""LogicModuleName=addCategory_MSSQL_CLONE""; ""Device=NA""; ""LogicModuleId=300""; ""Description="";",
		"addCategory_MSSQL_CLONE",
		300)]
	public void DeletePropertySource_Success(
		string logItemMessage,
		string expectedLogicModuleName,
		int expectedLogicModuleId
		)
		=> AssertToAuditEventSucceeds(
		logItemMessage,
		new()
		{
			MatchedRegExId = 113,
			ActionType = AuditEventActionType.Delete,
			EntityType = AuditEventEntityType.PropertySource,
			OutcomeType = AuditEventOutcomeType.Success,
			Description = string.Empty,
			LogicModuleName = expectedLogicModuleName,
			LogicModuleId = expectedLogicModuleId
		}
	);

	[Theory]
	[InlineData(
	@"""Action=Delete""; ""Type=TopologySource""; ""LogicModuleName=Fortinet_FortiGate_SDWAN""; ""Device=NA""; ""LogicModuleId=67""; ""Description="";",
	"Fortinet_FortiGate_SDWAN",
	67)]
	public void DeleteTopologySource_Success(
	string logItemMessage,
	string expectedLogicModuleName,
	int expectedLogicModuleId
	)
	=> AssertToAuditEventSucceeds(
		logItemMessage,
		new()
		{
			MatchedRegExId = 114,
			ActionType = AuditEventActionType.Delete,
			EntityType = AuditEventEntityType.TopologySource,
			OutcomeType = AuditEventOutcomeType.Success,
			Description = string.Empty,
			LogicModuleName = expectedLogicModuleName,
			LogicModuleId = expectedLogicModuleId
		}
	);

	[Theory]
	[InlineData(
		"Delete the collector 123 (hostname=HostName, desc=Description)",
		123,
		"HostName",
		"Description")]
	public void DeleteCollector_Success(
		string logItemMessage,
		int expectedCollectorId,
		string expectedCollectorName,
		string expectedCollectorDescription
		)
		=> AssertToAuditEventSucceeds(
	logItemMessage,
	new()
	{
		MatchedRegExId = 83,
		EntityType = AuditEventEntityType.Collector,
		ActionType = AuditEventActionType.Delete,
		OutcomeType = AuditEventOutcomeType.Success,
		CollectorId = expectedCollectorId,
		CollectorName = expectedCollectorName,
		CollectorDescription = expectedCollectorDescription
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