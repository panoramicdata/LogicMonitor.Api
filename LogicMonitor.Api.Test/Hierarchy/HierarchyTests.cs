namespace LogicMonitor.Api.Test.Hierarchy;

public class HierarchyTests(ITestOutputHelper iTestOutputHelper, Fixture fixture) : TestWithOutput(iTestOutputHelper, fixture)
{
	[Fact]
	public async Task GetResourceDataSourceInstance_Succeeds()
	{
		var dataSource = await LogicMonitorClient
			.GetDataSourceByUniqueNameAsync("SNMP_Network_Interfaces", default)
			.ConfigureAwait(true);

		dataSource.Should().NotBeNull();

		var things = await LogicMonitorClient
			.GetResourceDataSourceInstanceSummaryAsync(WindowsDeviceId, dataSource.Id, default);
		things.Should().NotBeNull();
	}
}
