namespace LogicMonitor.Api.Test.EventLogs;

public partial class AuditEventTests
{
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
}
