namespace LogicMonitor.Api.Test.Hierarchy;

public class HierarchyTests(ITestOutputHelper iTestOutputHelper, Fixture fixture) : TestWithOutput(iTestOutputHelper, fixture), IClassFixture<Fixture>
{
	[Fact]
	public async Task GetResourceDataSourceInstance_Succeeds()
	{
		var dataSource = await LogicMonitorClient
			.GetDataSourceByUniqueNameAsync("SNMP_Network_Interfaces", CancellationToken);

		dataSource.Should().NotBeNull();

		var things = await LogicMonitorClient
			.GetResourceDataSourceInstanceSummaryAsync(WindowsDeviceId, dataSource.Id, CancellationToken);
		things.Should().NotBeNull();
	}
}
