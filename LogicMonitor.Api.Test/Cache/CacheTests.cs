namespace LogicMonitor.Api.Test.Cache;

public class CacheTests : TestWithOutput
{
	public CacheTests(ITestOutputHelper iTestOutputHelper) : base(iTestOutputHelper)
	{
	}

	[Fact]
	public async Task CacheTestFasterSecondTimeAround()
	{
		// Enable caching
		LogicMonitorClient.UseCache = true;
		LogicMonitorClient.CacheTimeSpan = TimeSpan.FromSeconds(3);

		// Wait for any cache entry to expire (required for consistent operation)
		await Task.Delay(TimeSpan.FromSeconds(4)).ConfigureAwait(true);

		var stopwatch = Stopwatch.StartNew();
		var firstDevice = await GetWindowsDeviceAsync(default).ConfigureAwait(true);

		var firstDuration = stopwatch.Elapsed;
		Logger.LogInformation("Duration 1 {FirstDuration}", firstDuration);
		stopwatch.Restart();

		var secondDevice = await GetWindowsDeviceAsync(default).ConfigureAwait(true);

		var secondDuration = stopwatch.Elapsed;
		Logger.LogInformation("Duration 2 {SecondDuration}", secondDuration);

		// The second time should be shorter
		secondDuration.Should().BeLessThan(firstDuration);

		// The second time should be really fast
		secondDuration.Should().BeLessThan(TimeSpan.FromMilliseconds(100));

		// The devices should be identical
		secondDevice.DisplayName.Should().Be(firstDevice.DisplayName);
	}

	[Fact]
	public async Task CacheRefetchesAfterTimeout()
	{
		// Enable caching
		LogicMonitorClient.UseCache = true;
		LogicMonitorClient.CacheTimeSpan = TimeSpan.FromSeconds(3);

		// Make a call to force authentication
		_ = await LogicMonitorClient
			.GetTimeZoneSettingAsync(default)
			.ConfigureAwait(true);

		// Fetch a result
		var stopwatch = Stopwatch.StartNew();
		var firstDevice = await GetWindowsDeviceAsync(default).ConfigureAwait(true);
		firstDevice.Should().NotBeNull();
		var firstDuration = stopwatch.Elapsed;
		Logger.LogInformation("Duration 1 {FirstDuration}", firstDuration);

		// Wait for the cache timeout duration
		await Task.Delay(TimeSpan.FromSeconds(4)).ConfigureAwait(true);

		// Re-fetch a result
		stopwatch.Restart();
		var secondDevice = await GetWindowsDeviceAsync(default).ConfigureAwait(true);
		secondDevice.Should().NotBeNull();
		var secondDuration = stopwatch.Elapsed;
		Logger.LogInformation("Duration 2 {SecondDuration}", secondDuration);

		// The second time should NOT be really fast
		secondDuration.Should().BeGreaterThan(TimeSpan.FromMilliseconds(100));

		// The devices should be identical
		secondDevice.DisplayName.Should().Be(firstDevice.DisplayName);
	}

	[Fact]
	public async Task RateLimitCheckGetDeviceByDisplayNameAsync()
	{
		var stopwatch = Stopwatch.StartNew();
		LogicMonitorClient.UseCache = true;
		for (var n = 0; n < 1000; n++)
		{
			var innerStopwatch = Stopwatch.StartNew();
			_ = await GetWindowsDeviceAsync(default).ConfigureAwait(true);
			Logger.LogInformation("Run {RunIndex}: {Milliseconds}ms", n, innerStopwatch.ElapsedMilliseconds);
		}

		Logger.LogInformation("Complete in {Milliseconds}ms", stopwatch.ElapsedMilliseconds);
	}
}
