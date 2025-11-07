using Microsoft.Extensions.DependencyInjection;

namespace LogicMonitor.Api.Test;

public class Fixture : IDisposable
{
	private readonly ServiceProvider _serviceProvider;
	private bool _disposed;

	public Fixture()
	{
		// Load configuration
		var configuration = new ConfigurationBuilder()
			.SetBasePath(Directory.GetCurrentDirectory())
			.AddUserSecrets<Fixture>()
			.Build();

		// Build service collection
		var services = new ServiceCollection();
		
		services
			.AddLogging(builder => builder.AddDebug().SetMinimumLevel(LogLevel.Debug))
			.AddScoped<CancellationTokenSource>()
			.Configure<TestPortalConfig>(configuration.GetSection("Config"));

		_serviceProvider = services.BuildServiceProvider();
	}

	public T GetService<T>() where T : notnull
		=> _serviceProvider.GetRequiredService<T>();

	public void Dispose()
	{
		if (_disposed)
		{
			return;
		}

		_serviceProvider?.Dispose();
		_disposed = true;
		GC.SuppressFinalize(this);
	}
}
