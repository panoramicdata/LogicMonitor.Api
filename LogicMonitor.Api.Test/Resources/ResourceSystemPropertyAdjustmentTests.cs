namespace LogicMonitor.Api.Test.Resources;

public class ResourceSystemPropertyAdjustmentTests(ITestOutputHelper iTestOutputHelper, Fixture fixture) : TestWithOutput(iTestOutputHelper, fixture), IClassFixture<Fixture>
{
	[Fact]
	public async Task GetUnmonitoredResources()
	{
		var resourcesToAdjust = await LogicMonitorClient
			.GetAllAsync<Resource>("device/devices?filter=customProperties.name:\"meraki_datamagic.network_id\"", CancellationToken);

		foreach (var resource in resourcesToAdjust)
		{
			var categoriesSystemProperty = resource.CustomProperties.FirstOrDefault(sp => sp.Name == "system.categories" && sp.Value != "NoPing");

			if (categoriesSystemProperty is null)
			{
				continue;
			}

			categoriesSystemProperty.Value = "NoPing";

			await LogicMonitorClient.PutAsync(resource, CancellationToken);
		}
	}
}
