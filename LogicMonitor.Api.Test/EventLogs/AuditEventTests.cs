namespace LogicMonitor.Api.Test.EventLogs;

public class AuditEventTests : TestWithOutput
{
	public AuditEventTests(ITestOutputHelper iTestOutputHelper) : base(iTestOutputHelper)
	{
	}

	private const string TestUsername = "test";
	private const string TestIpAddress = "127.0.0.1";


	[Fact]
	public void Create_DeviceFailure_Success()
	{
		AssertToAuditEventSucceeds(
			@"""Action=Add""; ""Type=Device""; ""Device=ReportMagic alpha-Scheduler (0)""; ""Description=Failed device operation,  adddevice_failed : error  (invalid normal device name: ReportMagic alpha-Scheduler\n)""",
			new()
			{
				MatchedRegExId = 01,
				ActionType = AuditEventActionType.Create,
				EntityType = AuditEventEntityType.Resource,
				OutcomeType = AuditEventOutcomeType.Failure,
				ResourceIds = new() { 0 },
				ResourceNames = new() { "ReportMagic alpha-Scheduler" }
			}
		);
	}

	[Fact]
	public void Update_DeviceDataSourceInstance_Disappeared_Success()
	{
		AssertToAuditEventSucceeds(
			@"""Action=Update""; ""Type=Instance""; ""Device=NA""; ""InstanceId=NA""; ""Description=Instance(s) disappeared from: PDL-FW-01 (CollectorID=249) [DS--1.2.3.4]; """,
			new()
			{
				MatchedRegExId = 11,
				ActionType = AuditEventActionType.Update,
				EntityType = AuditEventEntityType.DeviceDataSourceInstance,
				OutcomeType = AuditEventOutcomeType.Success,
				ResourceNames = new() { "NA" }
			}
		);
	}

	[Fact]
	public void Update_DeviceDataSourceInstance_Changed_Success()
	{
		AssertToAuditEventSucceeds(
			@"""Action=Update""; ""Type=Instance""; ""Device=NA""; ""InstanceName=NA""; ""Description=Value(s) changed for: PDL-K8S-TEST (CollectorID=297) [Critical Linux Processes-java from 9947 to 22713]; valueChanges=[deviceId=3271,dataSourceId=94545589,instanceChanges=[instanceId=263219850,oldValue=22713,newValue=9947];];'""",
			new()
			{
				MatchedRegExId = 6,
				ActionType = AuditEventActionType.Update,
				EntityType = AuditEventEntityType.DeviceDataSourceInstance,
				OutcomeType = AuditEventOutcomeType.Success,
				ResourceNames = new() { "NA" },
				ResourceIds = new() { 3271 },
				DataSourceId = 94545589,
				InstanceId = 263219850,
				InstanceName = "NA"
			}
		);
	}


	[Fact]
	public void Update_Device_Success()
	{
		AssertToAuditEventSucceeds(
			"Update host<4030, docker-registry-deploy-default-PDL-K8S-PROD> (monitored by collector <295, pdl-k8s-prod-0>), ,  via API token xx123",
			new()
			{
				MatchedRegExId = 3,
				ActionType = AuditEventActionType.Update,
				EntityType = AuditEventEntityType.Resource,
				OutcomeType = AuditEventOutcomeType.Success,
				ResourceIds = new() { 4030 },
				ResourceNames = new() { "docker-registry-deploy-default-PDL-K8S-PROD" },
				CollectorId = 295,
				CollectorName = "pdl-k8s-prod-0",
				ApiTokenId = "xx123"
			}
		);
	}

	[Fact]
	public void Update_DeviceDataSourceInstance_New_Success()
	{
		AssertToAuditEventSucceeds(
			@"""Action=Update""; ""Type=Instance""; ""Device=NA""; ""InstanceName=NA""; ""Description=Found new instance(s) for: PDL-LM.logicmonitor.com (CollectorID=249) [LogicMonitor_Portal_DataSources-Win_WMI_UACTroubleshooter]; New_InstanceIds=[deviceId=2781,dataSourceId=112813425,dataSourceNewInstanceId(s)=263395102];""",
			new()
			{
				MatchedRegExId = 8,
				ActionType = AuditEventActionType.Update,
				CollectorId = 249,
				EntityType = AuditEventEntityType.DeviceDataSourceInstance,
				OutcomeType = AuditEventOutcomeType.Success,
				ResourceIds = new() { 2781 },
				ResourceNames = new() { "PDL-LM.logicmonitor.com" },
				DataSourceId = 112813425,
				InstanceName = "NA",
				DataSourceNewInstanceNames = new[] { "LogicMonitor_Portal_DataSources-Win_WMI_UACTroubleshooter" },
				DataSourceNewInstanceIds = new[] { 263395102 }
			}
		);
	}

	[Fact]
	public void Update_DeviceDataSourceInstance_Disappeared2_Success()
	{
		AssertToAuditEventSucceeds(
			@"""Action=Update""; ""Type=Instance""; ""Device=NA""; ""InstanceName=NA""; ""Description=Instance(s) disappeared from: PDL-K8S-TEST-03 (CollectorID=249) [Critical Linux Processes-java]; New_InstanceIds=[deviceId=1525,dataSourceId=94545589,dataSourceDeletedInstanceId(s)=263219849];""",
			new()
			{
				MatchedRegExId = 7,
				ActionType = AuditEventActionType.Update,
				CollectorId = 249,
				EntityType = AuditEventEntityType.DeviceDataSourceInstance,
				OutcomeType = AuditEventOutcomeType.Success,
				ResourceIds = new() { 1525 },
				ResourceNames = new() { "PDL-K8S-TEST-03" },
				DataSourceId = 94545589,
				DataSourceName = "Critical Linux Processes-java",
				InstanceName = "NA",
				DataSourceDeletedInstanceIds = new[] { 263219849 }
			}
		);
	}

	[Fact]
	public void Update_DeviceDataSourceInstance_NewAndDisappeared_Success()
	{
		AssertToAuditEventSucceeds(
			@"""Action=Update""; ""Type=Instance""; ""Device=NA""; ""InstanceName=NA""; ""Description=Found new instance(s) for: PDL-HAPROXY-TEST-02 (CollectorID=249) [HA Proxy Backend-ui_alpha_reportmagic,HA Proxy Backend-pdl_app_jira_test_01]; Instance(s) disappeared from: PDL-HAPROXY-TEST-02 (CollectorID=249) [HA Proxy Backend-uiv3_alpha_reportmagic]; New_InstanceIds=[deviceId=2365,dataSourceId=111613364,dataSourceNewInstanceId(s)=263956258,263956259,dataSourceDeletedInstanceId(s)=256832296];""",
			new()
			{
				MatchedRegExId = 9,
				ActionType = AuditEventActionType.Update,
				CollectorId = 249,
				EntityType = AuditEventEntityType.DeviceDataSourceInstance,
				OutcomeType = AuditEventOutcomeType.Success,
				ResourceIds = new() { 2365 },
				ResourceNames = new() { "PDL-HAPROXY-TEST-02" },
				InstanceName = "NA",
				DataSourceId = 111613364,
				DataSourceNewInstanceIds = new[] { 263956258, 263956259 },
				DataSourceNewInstanceNames = new[] { "HA Proxy Backend-ui_alpha_reportmagic", "HA Proxy Backend-pdl_app_jira_test_01" },
				DataSourceDeletedInstanceIds = new[] { 256832296 },
				DataSourceDeletedInstanceNames = new[] { "HA Proxy Backend-uiv3_alpha_reportmagic" }
			}
		);
	}

	[Fact]
	public void Update_DeviceDataSourceInstance_NewAndDisappeared2_Success()
	{
		AssertToAuditEventSucceeds(
			@"""Action=Update""; ""Type=Instance""; ""Device=NA""; ""InstanceName=NA""; ""Description=Found new instance(s) for: EU-W1:recoveryservices:pambackup (CollectorID=-2) [Microsoft_Azure_BackupJobStatus-xxx,Microsoft_Azure_BackupJobStatus-yyy]; Instance(s) disappeared from: EU-W1:recoveryservices:pambackup (CollectorID=-2) [Microsoft_Azure_BackupJobStatus-aaa,Microsoft_Azure_BackupJobStatus-bbb]; New_InstanceIds=[deviceId=2571,dataSourceId=39016161,dataSourceNewInstanceId(s)=570930097,570930098,dataSourceDeletedInstanceId(s)=569154776,569154777];""",
			new()
			{
				MatchedRegExId = 9,
				ActionType = AuditEventActionType.Update,
				CollectorId = -2,
				EntityType = AuditEventEntityType.DeviceDataSourceInstance,
				OutcomeType = AuditEventOutcomeType.Success,
				ResourceIds = new() { 2571 },
				ResourceNames = new() { "EU-W1:recoveryservices:pambackup" },
				InstanceName = "NA",
				DataSourceId = 39016161,
				DataSourceNewInstanceIds = new[] { 570930097, 570930098 },
				DataSourceNewInstanceNames = new[] { "Microsoft_Azure_BackupJobStatus-xxx", "Microsoft_Azure_BackupJobStatus-yyy" },
				DataSourceDeletedInstanceIds = new[] { 569154776, 569154777 },
				DataSourceDeletedInstanceNames = new[] { "Microsoft_Azure_BackupJobStatus-aaa", "Microsoft_Azure_BackupJobStatus-bbb" }
			}
		);
	}

	[Fact]
	public void Update_DeviceGroup_NothingChanged_Success()
	{
		AssertToAuditEventSucceeds(
			"Update the device group PDL - Panoramic Data/Datacenter/Private/Servers/Kubernetes Cluster: PDL-K8S-TEST.Nothing has been changed. via API token TOKENID",
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
	}

	[Fact]
	public void Added_DeviceGroup_Success()
	{
		AssertToAuditEventSucceeds(
			"Added device group Path1/Path2/Path3 (6686)  via API token TOKENID, ",
			new()
			{
				MatchedRegExId = 14,
				ActionType = AuditEventActionType.Create,
				EntityType = AuditEventEntityType.ResourceGroup,
				OutcomeType = AuditEventOutcomeType.Success,
				ResourceIds = new() { 6686 },
				ResourceGroupName = "Path1/Path2/Path3",
				ApiTokenId = "TOKENID"
			}
		);
	}


	[Fact]
	public void Added_DataSource_Success()
	{
		AssertToAuditEventSucceeds(
			@"""Action=Add""; ""Type=DataSource""; ""DataSourceName=Whois_TTL_Expiry""; ""DeviceName=UK-S1:appserviceplan:CappedAndHooked (UK-S1:cappedandhooked:appserviceplan:CappedAndHooked-ID)""; ""DeviceId=8555""; ""Description=Addition of datasource to device""; ""DataSourceId=114345723""; ""DeviceDataSourceId=615826""",
			new()
			{
				MatchedRegExId = 15,
				ResourceIds = new() { 8555 },
				ResourceNames = new() { "UK-S1:cappedandhooked:appserviceplan:CappedAndHooked-ID" },
				ActionType = AuditEventActionType.Create,
				EntityType = AuditEventEntityType.DataSource,
				OutcomeType = AuditEventOutcomeType.Success,
				DataSourceId = 114345723,
				DataSourceName = "Whois_TTL_Expiry"
			}
		);
	}

	private static void AssertToAuditEventSucceeds(
		string description,
		AuditEvent expectedAuditEvent
		)
	{

		if (expectedAuditEvent.OutcomeType == AuditEventOutcomeType.None)
		{
			throw new InvalidDataException("Unit test does not have a valid AuditEventOutcomeType");
		}

		var nowUnixTimeStamp = DateTime.UtcNow.SecondsSinceTheEpoch();
		var logItem = new LogItem
		{
			HappenedOnTimeStampUtc = nowUnixTimeStamp,
			HappenedOnLocalString = "",
			Id = Guid.NewGuid().ToString(),
			IpAddress = TestIpAddress,
			SessionId = Guid.NewGuid().ToString(),
			UserName = TestUsername,
			Description = description
		};

		var auditEvent = logItem.ToAuditEvent();
		auditEvent.DateTime.ToUnixTimeSeconds().Should().Be(nowUnixTimeStamp);
		auditEvent.UserName.Should().Be(TestUsername);
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
					.Excluding(ae => ae.UserName)
			);

		//auditEvent.ActionType.Should().Be(expectedAuditEventActionType);
		//auditEvent.EntityType.Should().Be(expectedAuditEventEntityType);
		//auditEvent.Host.Should().Be(ipAddress);
		//auditEvent.OriginalDescription.Should().Be(description);
		//auditEvent.OutcomeType.Should().NotBe(AuditEventOutcomeType.None);
		//auditEvent.OutcomeType.Should().Be(expectedAuditEventOutcomeType);
	}

	[Fact]
	public void Added_ResourceProperty_Success()
	{
		AssertToAuditEventSucceeds(
			@"Add property(name=propname, value=propvalue) to host(resourceName) via API token TOKENID.",
			new()
			{
				MatchedRegExId = 16,
				ResourceNames = new() { "resourceName" },
				ActionType = AuditEventActionType.Create,
				EntityType = AuditEventEntityType.ResourceProperty,
				OutcomeType = AuditEventOutcomeType.Success,
				ApiTokenId = "TOKENID",
				PropertyName = "propname",
				PropertyValue = "propvalue"
			}
		);
	}

	[Fact]
	public void HostGroups_Success()
	{
		AssertToAuditEventSucceeds(
			@"host(id= 4581 ,name= LM Push Server (LM-Push-Server))add to groups, list:   group name: Misc ,appliesTo= isMisc()  ,group name: Minimal Monitoring ,appliesTo= system.sysinfo == """" && system.sysoid == """" && isDevice() && !(system.virtualization) && (monitoring != ""basic"")  ,group name: LM Push SG ,appliesTo= servicenow.servicegroup_sys_id == ""xxx"" or getPropValue(""system.aws.tag.xxx_service-group"") == ""xxx"" or getPropValue(""system.azure.tag.xxx_service-group"") == ""xxx"" or getPropValue(""system.gcp.tag.xxx_service-group"") == ""xxx""  ,group name: lmsupport ,appliesTo= cms_service_level != ""cet_8x5"" ,add group number is 4",
			new()
			{
				MatchedRegExId = 17,
				ResourceIds = new() { 4581 },
				ResourceNames = new() { "LM Push Server (LM-Push-Server)" },
				ActionType = AuditEventActionType.Create,
				EntityType = AuditEventEntityType.ResourceGroups,
				OutcomeType = AuditEventOutcomeType.Success
			}
		);
	}

	[Fact]
	public void HostGroupsWithUnicodeCloseBracketAfterHostName_Success()
	{
		AssertToAuditEventSucceeds(
			@"host(id= 4581 ,name= LM Push Server (LM-Push-Server)）add to groups, list:   group name: Misc ,appliesTo= isMisc()  ,group name: Minimal Monitoring ,appliesTo= system.sysinfo == \""\"" && system.sysoid == \""\"" && isDevice() && !(system.virtualization) && (monitoring != \""basic\"")  ,group name: LM Push SG ,appliesTo= servicenow.servicegroup_sys_id == \""xxx\"" or getPropValue(\""system.aws.tag.xxx_service-group\"") == \""xxx\"" or getPropValue(\""system.azure.tag.xxx_service-group\"") == \""xxx\"" or getPropValue(\""system.gcp.tag.xxx_service-group\"") == \""xxx\""  ,group name: lmsupport ,appliesTo= cms_service_level != \""cet_8x5\"" ,add group number is 4",
			new()
			{
				MatchedRegExId = 17,
				ResourceIds = new() { 4581 },
				ResourceNames = new() { "LM Push Server (LM-Push-Server)" },
				ActionType = AuditEventActionType.Create,
				EntityType = AuditEventEntityType.ResourceGroups,
				OutcomeType = AuditEventOutcomeType.Success
			}
		);
	}

	[Fact]
	public void HostGroupsWithDeleteAndAddNumber_Success()
	{
		AssertToAuditEventSucceeds(
			@" host(id=4582,name =resourceName) add to LM Push SG ,appliesTo=servicenow.servicegroup_sys_id == \""xxx\"" or getPropValue(\""system.aws.tag.xxx_service-group\"") == \""xxx\"" or getPropValue(\""system.azure.tag.xxx_service-group\"") == \""xxx\"" or getPropValue(\""system.gcp.tag.xxx_service-group\"") == \""xxx\"" ,delete number is 0 ,add number is 1",
			new()
			{
				MatchedRegExId = 19,
				ResourceIds = new() { 4582 },
				ResourceNames = new() { "resourceName" },
				ActionType = AuditEventActionType.Create,
				EntityType = AuditEventEntityType.ResourceGroups,
				OutcomeType = AuditEventOutcomeType.Success
			}
		);
	}

	[Fact]
	public void DeleteKubernetesHosts_Success()
	{
		AssertToAuditEventSucceeds(
			@"Delete the Kubernetes hosts those were marked for deletion [argus-5848fb564c-v7h75-pod-logicmonitor-PDL-K8S-TEST-636946876(id=8443)]",
			new()
			{
				MatchedRegExId = 18,
				ResourceIds = new() { 8443 },
				ResourceNames = new() { "argus-5848fb564c-v7h75-pod-logicmonitor-PDL-K8S-TEST-636946876" },
				ActionType = AuditEventActionType.Delete,
				EntityType = AuditEventEntityType.KubernetesHosts,
				OutcomeType = AuditEventOutcomeType.Success
			}
		);
	}

	[Fact]
	public void DeleteKubernetesHostsMultiple_Success()
	{
		AssertToAuditEventSucceeds(
			@"Delete the Kubernetes hosts those were marked for deletion [collectorset-controller-54f4644c65-59jmm-pod-logicmonitor-PDL-K8S-TEST-442068781(id=8595), argus-5848fb564c-kl52v-pod-logicmonitor-PDL-K8S-TEST-4069678789(id=8603), argus-5848fb564c-tbx4r-pod-logicmonitor-PDL-K8S-TEST-460934296-2144132493(id=8581), collectorset-controller-54f4644c65-mqnrr-pod-logicmonitor-PDL-K8S-TEST-199135028-2350553716(id=8438)]",
			new()
			{
				MatchedRegExId = 18,
				ResourceIds = new() {
					8595,
					8603,
					8581,
					8438
				},
				ResourceNames = new() {
					"collectorset-controller-54f4644c65-59jmm-pod-logicmonitor-PDL-K8S-TEST-442068781",
					"argus-5848fb564c-kl52v-pod-logicmonitor-PDL-K8S-TEST-4069678789",
					"argus-5848fb564c-tbx4r-pod-logicmonitor-PDL-K8S-TEST-460934296-2144132493",
					"collectorset-controller-54f4644c65-mqnrr-pod-logicmonitor-PDL-K8S-TEST-199135028-2350553716"
				},
				ActionType = AuditEventActionType.Delete,
				EntityType = AuditEventEntityType.KubernetesHosts,
				OutcomeType = AuditEventOutcomeType.Success
			}
		);
	}

	[Fact]
	public void AddDeviceDataSourceInstance_Success()
	{
		AssertToAuditEventSucceeds(
			@"""Action=Add""; ""Type=Instance""; ""Device=LM Push Server""; ""InstanceName=Google (https://google.com)""; ""Description=DataSourceName: HTTP per Page- """,
			new()
			{
				MatchedRegExId = 20,
				ResourceNames = new() { "LM Push Server" },
				DataSourceName = "HTTP per Page-",
				InstanceName = "Google",
				WildValue = "https://google.com",
				ActionType = AuditEventActionType.Create,
				EntityType = AuditEventEntityType.DeviceDataSourceInstance,
				OutcomeType = AuditEventOutcomeType.Success
			}
		);
	}

	[Fact]
	public void SAMLLogin_Success()
	{
		AssertToAuditEventSucceeds(
			@"some.user@domain.com signs in via SAML",
			new()
			{
				MatchedRegExId = 21,
				UserName = "some.user@domain.com",
				ActionType = AuditEventActionType.Login,
				EntityType = AuditEventEntityType.None,
				OutcomeType = AuditEventOutcomeType.Success
			}
		);
	}

	[Fact]
	public void FailedApiRequest_Failure()
	{
		AssertToAuditEventSucceeds(
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
	}
}