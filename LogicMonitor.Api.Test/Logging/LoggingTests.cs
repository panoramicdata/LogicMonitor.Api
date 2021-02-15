using FluentAssertions;
using LogicMonitor.Api.Logging;
using Xunit;
using Xunit.Abstractions;

namespace LogicMonitor.Api.Test.Logging
{
	public class PushMetricTests : TestWithOutput
	{
		public PushMetricTests(ITestOutputHelper iTestOutputHelper) : base(iTestOutputHelper)
		{
		}

		[Fact]
		public async void WriteLogAsync_Succeeds()
		{
			var response = await LogicMonitorClient
				.WriteLogAsync(WindowsDeviceId, "Test log message.")
				.ConfigureAwait(false);
			response.Should().NotBeNull();
		}
	}
}