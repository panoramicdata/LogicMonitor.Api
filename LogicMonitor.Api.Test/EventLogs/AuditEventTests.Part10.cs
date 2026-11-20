namespace LogicMonitor.Api.Test.EventLogs;

public partial class AuditEventTests
{
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
}
