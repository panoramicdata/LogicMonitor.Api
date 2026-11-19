namespace LogicMonitor.Api.Test.EventLogs;

public partial class AuditEventTests
{
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
}
