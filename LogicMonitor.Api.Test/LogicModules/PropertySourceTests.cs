// Older, now deprecated methods are still tested here
namespace LogicMonitor.Api.Test.LogicModules;

public class PropertySourceTests : TestWithOutput
{
	public PropertySourceTests(ITestOutputHelper iTestOutputHelper) : base(iTestOutputHelper)
	{
	}

	/// <summary>
	/// Get a PropertySource definition (which is JSON)
	/// </summary>
	[Fact]
	public async Task GetJson()
	{
		var propertySource = await LogicMonitorClient
			.GetByNameAsync<PropertySource>("Test PropertySource", CancellationToken.None)
			.ConfigureAwait(false);
		var json = await LogicMonitorClient
			.GetPropertySourceJsonAsync(propertySource.Id, CancellationToken.None)
			.ConfigureAwait(false);

		json.Should().NotBeNull();
	}
}
