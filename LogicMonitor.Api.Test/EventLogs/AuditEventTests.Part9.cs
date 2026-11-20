namespace LogicMonitor.Api.Test.EventLogs;

public partial class AuditEventTests
{
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
}
