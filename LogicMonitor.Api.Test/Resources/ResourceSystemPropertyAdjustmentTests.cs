namespace LogicMonitor.Api.Test.Resources;

public class ResourceSystemPropertyAdjustmentTests(ITestOutputHelper iTestOutputHelper, Fixture fixture) : TestWithOutput(iTestOutputHelper, fixture)
{
	[Fact]
	public async Task GetUnmonitoredResources()
	{
		var resourcesToAdjust = await LogicMonitorClient
			.GetAllAsync<Resource>("device/devices?filter=customProperties.name:\"meraki_datamagic.network_id\"", default);

		foreach (var resource in resourcesToAdjust)
		{
			var categoriesSystemProperty = resource.CustomProperties.FirstOrDefault(sp => sp.Name == "system.categories" && sp.Value != "NoPing");

			if (categoriesSystemProperty is null)
			{
				continue;
			}

			categoriesSystemProperty.Value = "NoPing";

			await LogicMonitorClient.PutAsync(resource, default);
		}
	}
}
