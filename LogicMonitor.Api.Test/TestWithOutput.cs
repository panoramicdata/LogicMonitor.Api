using Microsoft.Extensions.Options;

namespace LogicMonitor.Api.Test;

public abstract class TestWithOutput(ITestOutputHelper testOutputHelper, Fixture fixture) : IDisposable
{
	protected ITestOutputHelper TestOutputHelper { get; } = testOutputHelper ?? throw new ArgumentNullException(nameof(testOutputHelper));
	protected Fixture Fixture { get; } = fixture ?? throw new ArgumentNullException(nameof(fixture));
	protected ILogger Logger { get; } = CreateLogger(testOutputHelper);

	protected int CollectorId { get; } = GetConfig(fixture).CollectorId;
	protected int DownCollectorId { get; } = GetConfig(fixture).DownCollectorId;
	protected string WebsiteGroupFullPath { get; } = GetConfig(fixture).WebsiteGroupFullPath;
	protected string AlertRuleName { get; } = GetConfig(fixture).AlertRuleName;
	protected string WebsiteName { get; } = GetConfig(fixture).WebsiteName;
	protected string DeviceGroupFullPath { get; } = GetConfig(fixture).DeviceGroupFullPath;
	protected string ResourceGroupFullPath { get; } = GetConfig(fixture).ResourceGroupFullPath;
	protected int ServiceDeviceId { get; } = GetConfig(fixture).ServiceDeviceId;
	protected int WindowsDeviceId { get; } = GetConfig(fixture).WindowsDeviceId;
	protected int ReportId { get; } = GetConfig(fixture).ReportId;
	protected int SdtResourceGroupId { get; } = GetConfig(fixture).SdtResourceGroupId;
	protected int WindowsDeviceLargeDeviceDataSourceId { get; } = GetConfig(fixture).WindowsDeviceLargeDeviceDataSourceId;
	protected int NetflowDeviceId { get; } = GetConfig(fixture).NetflowDeviceId;
	protected int SnmpDeviceId { get; } = GetConfig(fixture).SnmpDeviceId;
	protected int AllWidgetsDashboardId { get; } = GetConfig(fixture).AllWidgetsDashboardId;
	protected bool AccountHasBillingInformation { get; } = GetConfig(fixture).AccountHasBillingInformation;
	protected int TestDashboardId { get; } = GetConfig(fixture).TestDashboardId;

	protected static CancellationToken CancellationToken => TestContext.Current.CancellationToken;
	protected Stopwatch TestStopwatch { get; } = Stopwatch.StartNew();
	protected int StartEpoch { get; } = DateTime.UtcNow.AddDays(-30).SecondsSinceTheEpoch();
	protected int EndEpoch { get; } = DateTime.UtcNow.SecondsSinceTheEpoch();
	
	internal Api.Experimental.LogicMonitorClient ExperimentalLogicMonitorClient { get; } = CreateExperimentalClient(fixture, CreateLogger(testOutputHelper));
	protected LogicMonitorClient LogicMonitorClient { get; } = CreateClient(fixture, CreateLogger(testOutputHelper));

	private static TestPortalConfig GetConfig(Fixture fixture)
		=> fixture.GetService<IOptions<TestPortalConfig>>().Value;

	private static ILogger CreateLogger(ITestOutputHelper testOutputHelper)
	{
		// Create a logger factory with XUnit output
		var loggerFactory = LoggerFactory.Create(builder =>
		{
			builder
				.AddProvider(new XunitLoggerProvider(testOutputHelper))
				.SetMinimumLevel(LogLevel.Debug);
		});

		return loggerFactory.CreateLogger("LogicMonitor.Api.Test");
	}

	private static Api.Experimental.LogicMonitorClient CreateExperimentalClient(Fixture fixture, ILogger logger)
	{
		var config = GetConfig(fixture);
		return new Api.Experimental.LogicMonitorClient(new LogicMonitorClientOptions
		{
			Account = config.Account,
			AccessId = config.AccessId,
			AccessKey = config.AccessKey,
			Logger = logger
		});
	}

	private static LogicMonitorClient CreateClient(Fixture fixture, ILogger logger)
	{
		var config = GetConfig(fixture);
		return new LogicMonitorClient(new LogicMonitorClientOptions
		{
			Account = config.Account,
			AccessId = config.AccessId,
			AccessKey = config.AccessKey,
			Logger = logger
		});
	}

	protected void AssertIsFast(int durationSeconds)
		=> TestStopwatch.ElapsedMilliseconds.Should().BeLessThan(durationSeconds * 1000);

	protected static long DaysAgoAsUnixSeconds(int days)
		=> DateTimeOffset.UtcNow.AddDays(-days).ToUnixTimeSeconds();

	internal Task<Resource> GetNetflowDeviceAsync(CancellationToken cancellationToken)
		=> LogicMonitorClient.GetAsync<Resource>(NetflowDeviceId, cancellationToken);

	protected Task<Resource> GetServiceDeviceAsync(CancellationToken cancellationToken)
		=> LogicMonitorClient.GetAsync<Resource>(ServiceDeviceId, cancellationToken);

	protected Task<Resource> GetWindowsResourceAsync(CancellationToken cancellationToken)
		=> LogicMonitorClient.GetAsync<Resource>(WindowsDeviceId, cancellationToken);

	protected Task<Resource> GetSnmpResourceAsync(CancellationToken cancellationToken)
		=> LogicMonitorClient.GetAsync<Resource>(SnmpDeviceId, cancellationToken);

	protected Task<Dashboard> GetAllWidgetsDashboardAsync(CancellationToken cancellationToken)
		=> LogicMonitorClient.GetAsync<Dashboard>(AllWidgetsDashboardId, cancellationToken);

	public virtual void Dispose()
	{
		LogicMonitorClient?.Dispose();
		ExperimentalLogicMonitorClient?.Dispose();
		GC.SuppressFinalize(this);
	}
}
