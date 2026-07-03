using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace LogicMonitor.Api.Test;

public class Fixture : IAsyncLifetime, IDisposable
{
	private readonly ServiceProvider _serviceProvider;
	private LogicMonitorClient? _client;
	private bool _disposed;

	public const string NugetDashboardGroupName = "NugetTest";
	public int NugetDashboardGroupId { get; private set; }

	public Fixture()
	{
		var configuration = new ConfigurationBuilder()
			.SetBasePath(Directory.GetCurrentDirectory())
			.AddUserSecrets<Fixture>()
			.Build();

		var services = new ServiceCollection();

		services
			.AddLogging(builder => builder.AddDebug().SetMinimumLevel(LogLevel.Debug))
			.AddScoped<CancellationTokenSource>()
			.Configure<TestPortalConfig>(configuration.GetSection("Config"));

		_serviceProvider = services.BuildServiceProvider();
	}

	public async ValueTask InitializeAsync()
	{
		var config = _serviceProvider.GetRequiredService<IOptions<TestPortalConfig>>().Value;
		_client = new LogicMonitorClient(new LogicMonitorClientOptions
		{
			Account = config.Account,
			AccessId = config.AccessId,
			AccessKey = config.AccessKey,
		});

		var existing = await _client.GetByNameAsync<DashboardGroup>(NugetDashboardGroupName, CancellationToken.None);
		if (existing is not null)
		{
			NugetDashboardGroupId = existing.Id;
		}
		else
		{
			var created = await _client.CreateAsync(new DashboardGroupCreationDto
			{
				Name = NugetDashboardGroupName,
				ParentId = "1",
				Description = "Root group for Nuget integration tests — safe to delete",
			}, CancellationToken.None);
			NugetDashboardGroupId = created.Id;
		}
	}

	public async ValueTask DisposeAsync()
	{
		if (_disposed)
		{
			return;
		}

		_disposed = true;
		try
		{
			if (NugetDashboardGroupId > 0 && _client is not null)
			{
				await _client.DeleteDashboardGroupRecursivelyByIdAsync(NugetDashboardGroupId, CancellationToken.None);
			}
		}
		finally
		{
			_client?.Dispose();
			_serviceProvider?.Dispose();
			GC.SuppressFinalize(this);
		}
	}

	public T GetService<T>() where T : notnull
		=> _serviceProvider.GetRequiredService<T>();

	void IDisposable.Dispose()
	{
		if (_disposed)
		{
			return;
		}

		_disposed = true;
		_client?.Dispose();
		_serviceProvider?.Dispose();
		GC.SuppressFinalize(this);
	}
}
