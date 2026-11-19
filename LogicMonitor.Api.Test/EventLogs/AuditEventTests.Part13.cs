namespace LogicMonitor.Api.Test.EventLogs;

public partial class AuditEventTests
{
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
}
