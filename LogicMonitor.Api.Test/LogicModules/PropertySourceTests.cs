// Older, now deprecated methods are still tested here
namespace LogicMonitor.Api.Test.LogicModules;

public class PropertySourceTests(ITestOutputHelper iTestOutputHelper, Fixture fixture) : TestWithOutput(iTestOutputHelper, fixture), IClassFixture<Fixture>
{

	/// <summary>
	/// Get a PropertySource definition (which is JSON)
	/// </summary>
	[Fact]
	public async Task GetJson()
	{
		var propertySource = await LogicMonitorClient
			.GetByNameAsync<PropertySource>("Test PropertySource", CancellationToken);
		propertySource ??= new();
		var json = await LogicMonitorClient
			.GetPropertySourceJsonAsync(propertySource.Id, CancellationToken);

		json.Should().NotBeNull();
	}
}
