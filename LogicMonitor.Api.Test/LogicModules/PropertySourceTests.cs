using LogicMonitor.Api.LogicModules;
using Xunit;
using Xunit.Abstractions;

// Older, now deprecated methods are still tested here
#pragma warning disable 618

namespace LogicMonitor.Api.Test.LogicModules
{
	public class PropertySourceTests : TestWithOutput
	{
		public PropertySourceTests(ITestOutputHelper iTestOutputHelper) : base(iTestOutputHelper)
		{
		}

		/// <summary>
		/// Get a PropertySource definition (which is JSON)
		/// </summary>
		[Fact]
		public async void GetJson()
		{
			var propertySource = await LogicMonitorClient.GetByNameAsync<PropertySource>("Test PropertySource").ConfigureAwait(false);
			var json = await LogicMonitorClient.GetPropertySourceJsonAsync(propertySource.Id).ConfigureAwait(false);

			Assert.NotNull(json);
		}
	}
}

#pragma warning restore 618