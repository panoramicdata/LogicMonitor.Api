namespace LogicMonitor.Api.Test.EventLogs;

public partial class AuditEventTests
{
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
		  @"Dashboard 'Sample Dashboard (Alias Demo)' renamed to 'DBA PROD-APP-SEA02'",
			new()
			{
				MatchedRegExId = 105,
				ActionType = AuditEventActionType.Update,
				EntityType = AuditEventEntityType.Dashboard,
				OutcomeType = AuditEventOutcomeType.Success,
				DashboardName = "DBA PROD-APP-SEA02",
				Description = "Sample Dashboard (Alias Demo)"
			}
		);

	[Fact]
	public void DeleteDashboardGroup_Success()
		=> AssertToAuditEventSucceeds(
		   @"Delete the dashboard group Sample Dashboard (Alias Demo)",
			new()
			{
				MatchedRegExId = 106,
				ActionType = AuditEventActionType.Delete,
				EntityType = AuditEventEntityType.DashboardGroup,
				OutcomeType = AuditEventOutcomeType.Success,
				ResourceGroupName = "Sample Dashboard (Alias Demo)"
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
		  @"Edit the widget phxdbpro - Fra usage overview of dashboard Sample Oracle Capacity",
			new()
			{
				MatchedRegExId = 91,
				ActionType = AuditEventActionType.Update,
				EntityType = AuditEventEntityType.Widget,
				OutcomeType = AuditEventOutcomeType.Success,
				WidgetName = "phxdbpro - Fra usage overview",
				DashboardName = "Sample Oracle Capacity"
			}
		);

	[Fact]
	public void DeleteWidgetOfDashboard_Success()
		=> AssertToAuditEventSucceeds(
			@"Delete the widget phxdbpro - Fra usage overview of dashboard Sample Oracle Capacity",
			new()
			{
				MatchedRegExId = 92,
				ActionType = AuditEventActionType.Delete,
				EntityType = AuditEventEntityType.Widget,
				OutcomeType = AuditEventOutcomeType.Success,
				WidgetName = "phxdbpro - Fra usage overview",
				DashboardName = "Sample Oracle Capacity"
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
	public void DeleteAzureAccount_WithLargeBody_Success()
	{
		var message = "Delete Azure account ea-eisnerai-prod," + new string('x', 20_000);

		AssertToAuditEventSucceeds(
			message,
			new()
			{
				MatchedRegExId = 0,
				ActionType = AuditEventActionType.Delete,
				EntityType = AuditEventEntityType.AzureAccount,
				OutcomeType = AuditEventOutcomeType.Success,
				ResourceNames = ["ea-eisnerai-prod"]
			}
		);
	}
}
