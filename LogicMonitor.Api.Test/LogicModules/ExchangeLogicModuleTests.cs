using Newtonsoft.Json;

namespace LogicMonitor.Api.Test.LogicModules;

public class ExchangeLogicModuleTests(ITestOutputHelper iTestOutputHelper, Fixture fixture) : TestWithOutput(iTestOutputHelper, fixture), IClassFixture<Fixture>
{
	/// <summary>
	/// Live: the metadata endpoint returns all module types in one call, with unique ids.
	/// </summary>
	[Fact]
	public async Task GetExchangeLogicModules_ReturnsAllTypesInOneCall()
	{
		var modules = await LogicMonitorClient.GetExchangeLogicModulesAsync(CancellationToken);

		modules.Should().NotBeNullOrEmpty();

		foreach (var group in modules.GroupBy(m => m.Type).OrderByDescending(g => g.Count()))
		{
			TestOutputHelper.WriteLine($"{group.Key}: {group.Count()}");
		}

		// DataSources should always be present on any real portal.
		modules.Select(m => m.Type).Should().Contain(LogicModuleType.DataSource);

		var updatable = modules.Count(m => m.HasUpdateAvailable);
		TestOutputHelper.WriteLine($"Updates available: {updatable} / {modules.Count}");
	}

	/// <summary>
	/// Offline: HasUpdateAvailable and the installation/lifecycle flags are derived correctly.
	/// </summary>
	[Fact]
	public void ExchangeLogicModule_DerivedFlags_AreCorrect()
	{
		const string json = """
		[
			{ "id":"1","name":"AtLatest","type":"CONFIGSOURCE","installationStatuses":["IS_INSTALLED"],
			  "originRegistryId":"aaa","upgradeableRegistryId":"aaa","originStatus":"CORE","isInUse":true },
			{ "id":"2","name":"Updatable","type":"DATASOURCE","installationStatuses":["IS_INSTALLED"],
			  "originRegistryId":"aaa","upgradeableRegistryId":"bbb","originStatus":"DEPRECATED","isInUse":true },
			{ "id":"3","name":"Customized","type":"EVENTSOURCE","installationStatuses":["IS_INSTALLED","IS_CUSTOMIZED"],
			  "originRegistryId":"ccc","upgradeableRegistryId":"","originStatus":"CORE","isInUse":false },
			{ "id":"guid-4","name":"CatalogOnly","type":"PROPERTYSOURCE","installationStatuses":[],
			  "version":"1.0.0","status":"CORE","authorPortalName":"core" }
		]
		""";

		var serializer = JsonSerializer.CreateDefault(new JsonSerializerSettings { MissingMemberHandling = MissingMemberHandling.Ignore });
		var modules = JArray.Parse(json).Select(t => t.ToObject<ExchangeLogicModule>(serializer)!).ToList();

		modules.Should().HaveCount(4);

		var atLatest = modules.Single(m => m.Name == "AtLatest");
		atLatest.HasUpdateAvailable.Should().BeFalse();
		atLatest.IsInstalled.Should().BeTrue();
		atLatest.Type.Should().Be(LogicModuleType.ConfigSource);

		var updatable = modules.Single(m => m.Name == "Updatable");
		updatable.HasUpdateAvailable.Should().BeTrue();
		updatable.IsDeprecated.Should().BeTrue();
		updatable.Type.Should().Be(LogicModuleType.DataSource);

		var customized = modules.Single(m => m.Name == "Customized");
		customized.HasUpdateAvailable.Should().BeFalse();   // upgradeableRegistryId empty
		customized.IsCustomized.Should().BeTrue();

		var catalog = modules.Single(m => m.Name == "CatalogOnly");
		catalog.HasUpdateAvailable.Should().BeFalse();
		catalog.IsInstalled.Should().BeFalse();
		catalog.Version.Should().Be("1.0.0");
	}
}
