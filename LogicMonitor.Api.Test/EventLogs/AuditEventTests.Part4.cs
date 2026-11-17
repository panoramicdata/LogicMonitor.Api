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
		 @"""Action=Add""; ""Type=DataSource""; ""DataSourceName=example_ALL_ALL_IP_Addresses""; ""DeviceName=127.0.0.1""; ""DeviceId=4808""; ""Description=Addition of datasource to device""; ""DataSourceId=33514257""; ""DeviceDataSourceId=52050""",
			new()
			{
				MatchedRegExId = 33,
				ActionType = AuditEventActionType.Create,
				EntityType = AuditEventEntityType.DataSource,
				ResourceIds = [4808],
				LogicModuleId = 33514257,
				LogicModuleName = "example_ALL_ALL_IP_Addresses",
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
	[InlineData("Request remote ssh session to 192.0.2.10", "ssh", "192.0.2.10")]
	[InlineData("Request remote rdp session to 198.51.100.20", "rdp", "198.51.100.20")]
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
		4)]
	[InlineData(
		"Download DataSource<running-config>. \"DataSourceInstanceId=1234\"; \"DataSourceName=Cisco_IOS\"; \"DataSourceVersion=4\"; \"DeviceName=DeviceA\"; \"DeviceId=5678\";",
		74,
		AuditEventEntityType.DataSource,
		"running-config",
		1234,
		"Cisco_IOS",
		4)]
	public void DownloadLogicModule_Success(
		string logItemMessage,
		int expectedMatchRegexId,
		AuditEventEntityType expectedAuditEventEntityType,
		string expectedInstanceName,
		int expectedInstanceId,
		string expectedLogicModuleName,
		int expectedLogicModuleVersion)
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
		ResourceNames = ["DeviceA"],
		ResourceIds = [5678]
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
		@"""Action=Delete""; ""Type=Instance""; ""Device=EXAMPLE_SRV_E vfrawapdpaprd03""; ""InstanceName=SSL_Certificates-HTTPS""; ""Description=""",
		new()
		{
			MatchedRegExId = 77,
			ActionType = AuditEventActionType.Delete,
			EntityType = AuditEventEntityType.ResourceDataSourceInstance,
			OutcomeType = AuditEventOutcomeType.Success,
			ResourceNames = ["EXAMPLE_SRV_E vfrawapdpaprd03"],
			InstanceName = "SSL_Certificates-HTTPS",
			Description = string.Empty,
		}
	);

	[Theory]
	[InlineData(
		"Remote rdp session 1234 to 198.51.100.20 started at 09:10",
		"rdp",
		1234,
		"198.51.100.20",
		AuditEventActionType.Start,
		"09:10")]
	[InlineData(
		"Remote ssh session 4321 to 203.0.113.30 terminated at 12:34",
		"ssh",
		4321,
		"203.0.113.30",
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
		  "Add new DataSource 'EXAMPLE_DATASOURCE_SITE_ISSUES'. Imported from JSON file. Change details: \"Action=Add\"; \"Type=DataSource\"; \"LogicModuleName=EXAMPLE_DATASOURCE_SITE_ISSUES\"; \"Device=NA\"; \"Description={...very long...}\"",
			new()
			{
				MatchedRegExId = 0,
				ActionType = AuditEventActionType.Create,
				EntityType = AuditEventEntityType.DataSource,
				LogicModuleName = "EXAMPLE_DATASOURCE_SITE_ISSUES",
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
		   "Import EventSource from XML. Change details : \"Action=Add\"; \"Type=EventSource\"; \"LogicModuleName=EXAMPLE_EVENTSOURCE_ALERTS\"; \"Device=NA\"; \"Description=Add eventsource : EXAMPLE_EVENTSOURCE_ALERTS, id = 85\"",
			new()
			{
				MatchedRegExId = 0,
				ActionType = AuditEventActionType.Create,
				EntityType = AuditEventEntityType.EventSource,
				LogicModuleName = "EXAMPLE_EVENTSOURCE_ALERTS",
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

	[Fact]
	public void UpdateDatasourceInstancesEnableMonitoringOnly2_Success()
	=> AssertToAuditEventSucceeds(
		@"Update the datasource instances, enable monitoring of instances : [a.b.c.d id=123 hid=276643,e.f.g.h id=456 hid=276643] ",
		new()
		{
			MatchedRegExId = 111,
			ActionType = AuditEventActionType.Update,
			EntityType = AuditEventEntityType.ResourceDataSourceInstance,
			OutcomeType = AuditEventOutcomeType.Success,
			Description = "enable monitoring of instances : [a.b.c.d id=123 hid=276643,e.f.g.h id=456 hid=276643]",
			DataSourceNewInstanceNames = [
				"a.b.c.d",
				"e.f.g.h"
			],
			DataSourceNewInstanceIds = [123, 456],
			ResourceIds = [276643]
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
