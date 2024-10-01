using LogicMonitor.Api.Resources;
using Microsoft.Extensions.Options;
using Xunit.Microsoft.DependencyInjection.Abstracts;

namespace LogicMonitor.Api.Test;

[CollectionDefinition("Dependency Injection")]
public abstract class TestWithOutput : TestBed<Fixture>
{
	protected ILogger Logger { get; }

	protected int CollectorId { get; }

	protected int DownCollectorId { get; }

	protected string WebsiteGroupFullPath { get; }

	protected string AlertRuleName { get; }

	protected string WebsiteName { get; }

	protected string DeviceGroupFullPath { get; }

	protected string ResourceGroupFullPath { get; }

	protected int ServiceDeviceId { get; }

	protected int WindowsDeviceId { get; }

	protected int ReportId { get; }

	protected int SdtResourceGroupId { get; }

	protected int WindowsDeviceLargeDeviceDataSourceId { get; }

	protected int NetflowDeviceId { get; }

	protected int SnmpDeviceId { get; }

	protected int AllWidgetsDashboardId { get; }

	protected bool AccountHasBillingInformation { get; }

	protected int TestDashboardId { get; }

	protected TestWithOutput(ITestOutputHelper testOutputHelper, Fixture fixture) : base(testOutputHelper, fixture)
	{
		ArgumentNullException.ThrowIfNull(testOutputHelper, nameof(testOutputHelper));
		ArgumentNullException.ThrowIfNull(fixture, nameof(fixture));

		// Logger
		var loggerFactory = fixture.GetService<ILoggerFactory>(testOutputHelper) ?? throw new InvalidOperationException("LoggerFactory is null");
		Logger = loggerFactory.CreateLogger(GetType());

		// TestPortalConfig
		var testPortalConfigOptions = fixture
			.GetService<IOptions<TestPortalConfig>>(testOutputHelper)
			?? throw new InvalidOperationException("TestPortalConfig is null");

		var testPortalConfig = testPortalConfigOptions.Value;

		ExperimentalLogicMonitorClient = new Api.Experimental.LogicMonitorClient(
		new LogicMonitorClientOptions
		{
			Account = testPortalConfig.Account,
			AccessId = testPortalConfig.AccessId,
			AccessKey = testPortalConfig.AccessKey,
			Logger = Logger
		});

		LogicMonitorClient = new LogicMonitorClient(new LogicMonitorClientOptions
		{
			Account = testPortalConfig.Account,
			AccessId = testPortalConfig.AccessId,
			AccessKey = testPortalConfig.AccessKey,
			Logger = Logger
		});

		CollectorId = testPortalConfig.CollectorId;
		DownCollectorId = testPortalConfig.DownCollectorId;
		WindowsDeviceId = testPortalConfig.WindowsDeviceId;
		WindowsDeviceLargeDeviceDataSourceId = testPortalConfig.WindowsDeviceLargeDeviceDataSourceId;
		ServiceDeviceId = testPortalConfig.ServiceDeviceId;
		NetflowDeviceId = testPortalConfig.NetflowDeviceId;
		SdtResourceGroupId = testPortalConfig.SdtResourceGroupId;
		TestDashboardId = testPortalConfig.TestDashboardId;
		DeviceGroupFullPath = testPortalConfig.DeviceGroupFullPath;
		WebsiteGroupFullPath = testPortalConfig.WebsiteGroupFullPath;
		ResourceGroupFullPath = testPortalConfig.ResourceGroupFullPath;
		ReportId = testPortalConfig.ReportId;
		WebsiteName = testPortalConfig.WebsiteName;
		SnmpDeviceId = testPortalConfig.SnmpDeviceId;
		AllWidgetsDashboardId = testPortalConfig.AllWidgetsDashboardId;
		AccountHasBillingInformation = testPortalConfig.AccountHasBillingInformation;
		AlertRuleName = testPortalConfig.AlertRuleName;
		var nowUtc = DateTime.UtcNow;
		StartEpoch = nowUtc.AddDays(-30).SecondsSinceTheEpoch();
		EndEpoch = nowUtc.SecondsSinceTheEpoch();
		Stopwatch = Stopwatch.StartNew();
	}

	private Stopwatch Stopwatch { get; }

	protected int StartEpoch { get; }

	protected int EndEpoch { get; }
	internal Api.Experimental.LogicMonitorClient ExperimentalLogicMonitorClient { get; }
	protected LogicMonitorClient LogicMonitorClient { get; }

	protected void AssertIsFast(int durationSeconds)
		=> Stopwatch.ElapsedMilliseconds.Should().BeLessThan(durationSeconds * 1000);

	protected static long DaysAgoAsUnixSeconds(int days)
		=> DateTimeOffset.UtcNow.AddDays(-days).ToUnixTimeSeconds();

	internal Task<Resource> GetNetflowDeviceAsync(CancellationToken cancellationToken)
		=> LogicMonitorClient.GetAsync<Resource>(NetflowDeviceId, cancellationToken);

	protected Task<Resource> GetServiceDeviceAsync(CancellationToken cancellationToken)
		=> LogicMonitorClient.GetAsync<Resource>(ServiceDeviceId, cancellationToken);

	protected Task<Resource> GetWindowsDeviceAsync(CancellationToken cancellationToken)
		=> LogicMonitorClient.GetAsync<Resource>(WindowsDeviceId, cancellationToken);

	protected Task<Resource> GetSnmpDeviceAsync(CancellationToken cancellationToken)
		=> LogicMonitorClient.GetAsync<Resource>(SnmpDeviceId, cancellationToken);

	protected Task<Dashboard> GetAllWidgetsDashboardAsync(CancellationToken cancellationToken)
		=> LogicMonitorClient.GetAsync<Dashboard>(AllWidgetsDashboardId, cancellationToken);
}
