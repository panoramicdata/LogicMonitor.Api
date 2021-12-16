namespace LogicMonitor.Api.Test;

public class CacheTests : TestWithOutput
{
	public CacheTests(ITestOutputHelper iTestOutputHelper) : base(iTestOutputHelper)
	{
	}

	[Fact]
	public async void CacheTestFasterSecondTimeAround()
	{
		// Enable caching
		LogicMonitorClient.UseCache = true;

		var stopwatch = Stopwatch.StartNew();
		var firstDevice = await GetWindowsDeviceAsync().ConfigureAwait(false);

		var firstDuration = stopwatch.Elapsed;
		Logger.LogInformation("Duration 1 {firstDuration}", firstDuration);
		stopwatch.Restart();

		var secondDevice = await GetWindowsDeviceAsync().ConfigureAwait(false);

		var secondDuration = stopwatch.Elapsed;
		Logger.LogInformation("Duration 2 {secondDuration}", secondDuration);

		// The second time should be shorter
		Assert.True(secondDuration < firstDuration);

		// The second time should be really fast
		Assert.True(secondDuration < TimeSpan.FromMilliseconds(100));

		// The devices should be identical
		Assert.Equal(firstDevice.DisplayName, secondDevice.DisplayName);
	}

	[Fact]
	public async void CacheRefetchesAfterTimeout()
	{
		// Enable caching
		LogicMonitorClient.UseCache = true;
		LogicMonitorClient.CacheTimeSpan = TimeSpan.FromSeconds(3);

		// Make a call to force authentication
		var _ = await LogicMonitorClient.GetTimeZoneSettingAsync().ConfigureAwait(false);

		// Fetch a result
		var stopwatch = Stopwatch.StartNew();
		var firstDevice = await GetWindowsDeviceAsync().ConfigureAwait(false);
		Assert.NotNull(firstDevice);
		var firstDuration = stopwatch.Elapsed;
		Logger.LogInformation($"Duration 1 {firstDuration}");

		// Wait for the cache timeout duration
		await Task.Delay(LogicMonitorClient.CacheTimeSpan).ConfigureAwait(false);

		// Re-fetch a result
		stopwatch.Restart();
		var secondDevice = await GetWindowsDeviceAsync().ConfigureAwait(false);
		Assert.NotNull(secondDevice);
		var secondDuration = stopwatch.Elapsed;
		Logger.LogInformation($"Duration 2 {secondDuration}");

		// The second time should NOT be really fast
		Assert.True(secondDuration > TimeSpan.FromMilliseconds(100));

		// The devices should be identical
		Assert.Equal(firstDevice.DisplayName, secondDevice.DisplayName);
	}

	[Fact]
	public async void RateLimitCheckGetDeviceByDisplayNameAsync()
	{
		var stopwatch = Stopwatch.StartNew();
		LogicMonitorClient.UseCache = true;
		for (var n = 0; n < 1000; n++)
		{
			var innerStopwatch = Stopwatch.StartNew();
			var _ = await GetWindowsDeviceAsync().ConfigureAwait(false);
			Logger.LogInformation($"Run {n}: {innerStopwatch.ElapsedMilliseconds}ms");
		}

		Logger.LogInformation($"Complete in {stopwatch.ElapsedMilliseconds}ms");
	}
}
