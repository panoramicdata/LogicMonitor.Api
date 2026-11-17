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
			@"Delete the Kubernetes hosts those were marked for deletion [argus-5848fb564c-v7h75-pod-logicmonitor-EXAMPLE-K8S-TEST-636946876(id=8443)]",
			new()
			{
				MatchedRegExId = 18,
				ResourceIds = [8443],
				ResourceNames = ["argus-5848fb564c-v7h75-pod-logicmonitor-EXAMPLE-K8S-TEST-636946876"],
				ActionType = AuditEventActionType.Delete,
				EntityType = AuditEventEntityType.Resource,
				OutcomeType = AuditEventOutcomeType.Success
			}
		);

	[Fact]
	public void DeleteKubernetesHostsMultiple_Success()
		=> AssertToAuditEventSucceeds(
			@"Delete the Kubernetes hosts those were marked for deletion [collectorset-controller-54f4644c65-59jmm-pod-logicmonitor-EXAMPLE-K8S-TEST-442068781(id=8595), argus-5848fb564c-kl52v-pod-logicmonitor-EXAMPLE-K8S-TEST-4069678789(id=8603), argus-5848fb564c-tbx4r-pod-logicmonitor-EXAMPLE-K8S-TEST-460934296-2144132493(id=8581), collectorset-controller-54f4644c65-mqnrr-pod-logicmonitor-EXAMPLE-K8S-TEST-199135028-2350553716(id=8438)]",
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
					"collectorset-controller-54f4644c65-59jmm-pod-logicmonitor-EXAMPLE-K8S-TEST-442068781",
					"argus-5848fb564c-kl52v-pod-logicmonitor-EXAMPLE-K8S-TEST-4069678789",
					"argus-5848fb564c-tbx4r-pod-logicmonitor-EXAMPLE-K8S-TEST-460934296-2144132493",
					"collectorset-controller-54f4644c65-mqnrr-pod-logicmonitor-EXAMPLE-K8S-TEST-199135028-2350553716"
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
			@"Failed API request: API token TOKEN_EXAMPLE attempted to access path '/santaba/rest/device/groups/1613/devices' with Method: GET",
			new()
			{
				MatchedRegExId = 22,
				ActionType = AuditEventActionType.GeneralApi,
				OutcomeType = AuditEventOutcomeType.Failure,
				ApiTokenId = "TOKEN_EXAMPLE",
				ApiMethod = "GET",
				ApiPath = "/santaba/rest/device/groups/1613/devices"
			}
		);

	[Fact]
	public void DeleteAwsHostsMultiple_Success()
		=> AssertToAuditEventSucceeds(
			@"Delete the aws hosts [EXAMPLE-W1:i-0ad560910aee79179(id=4573), EXAMPLE-W1:i-0070bf1c74503d8ed(id=4574)]",
			new()
			{
				MatchedRegExId = 23,
				ResourceIds = [
					4573,
					4574
				],
				ResourceNames = [
					"EXAMPLE-W1:i-0ad560910aee79179",
					"EXAMPLE-W1:i-0070bf1c74503d8ed"
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
	[InlineData("user@example.com signs out (adminId=1037).", "user@example.com", 1037, 87)]
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
			@"Add new api token - API_TOKEN_EXAMPLE_2 for API token user",
			new()
			{
				MatchedRegExId = 88,
				ActionType = AuditEventActionType.Create,
				EntityType = AuditEventEntityType.ApiToken,
				OutcomeType = AuditEventOutcomeType.Success,
				ApiTokenId = "API_TOKEN_EXAMPLE_2"
			}
		);

	[Fact]
	public void AddWidgetToDashboard_Success()
		=> AssertToAuditEventSucceeds(
		 @"Add a widget EXAMPLE_SRV_A eul1900684-phxdbpro-Fra reclaimable overview to dashboard Sample Oracle Capacity",
			new()
			{
				MatchedRegExId = 89,
				ActionType = AuditEventActionType.Create,
				EntityType = AuditEventEntityType.Widget,
				OutcomeType = AuditEventOutcomeType.Success,
				WidgetName = "EXAMPLE_SRV_A eul1900684-phxdbpro-Fra reclaimable overview",
				DashboardName = "Sample Oracle Capacity"
			}
		);

	[Fact]
	public void EditDashboard_Success()
		=> AssertToAuditEventSucceeds(
		  @"Edit the dashboard Sample Oracle Capacity",
			new()
			{
				MatchedRegExId = 90,
				ActionType = AuditEventActionType.Update,
				EntityType = AuditEventEntityType.Dashboard,
				OutcomeType = AuditEventOutcomeType.Success,
				DashboardName = "Sample Oracle Capacity"
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
		 @"Add report Example Bank - Alert Thresholds",
			new()
			{
				MatchedRegExId = 103,
				ActionType = AuditEventActionType.Create,
				EntityType = AuditEventEntityType.Report,
				OutcomeType = AuditEventOutcomeType.Success,
				ResourceNames = ["Example Bank - Alert Thresholds"]
			}
		);
}
