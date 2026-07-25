namespace LogicMonitor.Api.Test.LogicModules;

public class DataSourceDeserializationTests(ITestOutputHelper iTestOutputHelper, Fixture fixture) : TestWithOutput(iTestOutputHelper, fixture), IClassFixture<Fixture>
{
	[Fact]
	public async Task GetAllDataSources_DeserialisesEveryDataSource()
	{
		// Full enumeration deserialises every DataSource (including autoDiscoveryConfig.method). With
		// the client's strict MissingMemberHandling.Error, this throws if any server field is unmodelled
		// — regression guard for e.g. autoDiscoveryConfig.method.expiringSoonDays.
		var dataSources = await LogicMonitorClient
			.GetAllAsync<DataSource>(CancellationToken);

		dataSources.Should().NotBeNull();
		TestOutputHelper.WriteLine($"Deserialised {dataSources.Count} DataSources.");
	}
}
