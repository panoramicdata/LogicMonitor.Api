namespace LogicMonitor.Api.Test.EventLogs;

public partial class AuditEventTests
{
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
}
