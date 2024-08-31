using Microsoft.Extensions.DependencyInjection;
using Xunit.Microsoft.DependencyInjection;
using Xunit.Microsoft.DependencyInjection.Abstracts;

namespace LogicMonitor.Api.Test;

public class Fixture : TestBedFixture
{
	private IConfigurationRoot _configuration;

	protected override void AddServices(IServiceCollection services, IConfiguration? configuration)
		=> services
			.AddScoped<CancellationTokenSource>()
			.AddScoped(x =>
				{
					var testPortalConfig = new TestPortalConfig();
					_configuration.GetSection("Config").Bind(testPortalConfig);
					return testPortalConfig;
				});

	protected override ValueTask DisposeAsyncCore()
		=> default;

	protected override IEnumerable<TestAppSettings> GetTestAppSettings()
	{
		_configuration = new ConfigurationBuilder()
			 .SetBasePath(Directory.GetCurrentDirectory())
			 .AddUserSecrets<Fixture>()
			 .Build();

		// This is not used
		return [
			new TestAppSettings
			{
				IsOptional = true,
				Filename = null,
			}
		];
	}
}
