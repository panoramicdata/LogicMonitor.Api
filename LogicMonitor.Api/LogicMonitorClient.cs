using System.Security.Cryptography;

namespace LogicMonitor.Api;

/// <summary>
///     The main client for querying the portal.
/// </summary>
public partial class LogicMonitorClient : IDisposable
{

	/// <summary>
	/// Dispose
	/// </summary>
	public void Dispose()
	{
		// Sign out
		_client.Dispose();
		_handler.Dispose();
		GC.SuppressFinalize(this);
	}
	private static readonly HttpMethod _patchHttpMethod = new("PATCH");

	private readonly HttpMessageHandler _handler;

	private readonly HttpClient _client;

	private readonly LogicMonitorClientOptions _options;

	private static readonly JsonConverter[] _jsonConverters =
	[
		new WidgetConverter(),
		new WidgetDataConverter(),
		new ReportConverter(),
		new FlagsEnumConverter()
	];

	private readonly Cache<string, object> _cache;

	private readonly string _accessId;

	private readonly string _accessKey;

	/// <summary>
	/// The maximum back off to use, in seconds, when a 429 response occurs
	/// </summary>
	public int MaximumBackOffSeconds { get; set; }

	/// <summary>
	///     The connected account name
	/// </summary>
	public string AccountName { get; }

	/// <summary>
	///     The Cache TimeSpan
	/// </summary>
	public TimeSpan CacheTimeSpan
	{
		get => _cache.MaxAge;
		set => _cache.MaxAge = value;
	}

	/// <summary>
	/// Summary information about API traffic.
	/// </summary>
	public Statistics Statistics { get; } = new();

	/// <summary>
	/// Clear the cache
	/// </summary>
	public void ClearCache()
		=> _cache.Clear();

	/// <summary>
	///     Whether to use the cache
	/// </summary>
	public bool UseCache { get; set; }

	private readonly ILogger _logger;

	/// <summary>
	///     The query timeout
	/// </summary>
	public TimeSpan TimeOut
	{
		get => _client.Timeout;
		set => _client.Timeout = value;
	}

	/// <summary>
	///     The AttemptCount
	/// </summary>
	public int AttemptCount
	{
		get;
		set
		{
			if (value < 1)
			{
				throw new ArgumentOutOfRangeException(nameof(value), "Must be >= 1.");
			}

			field = value;
		}
	} = 5;

	/// <summary>
	/// Whether to wait (10 second delay) during upgrades
	/// </summary>
	public bool WaitDuringLogicMonitorUpgrades { get; set; } = true;

	/// <summary>
	/// Whether to throw an exception when paging over results and the total does not match the count of items retrieved.
	/// This can happen for example when an item in the first page is deleted during paging.
	/// </summary>
	public bool StrictPagingTotalChecking { get; set; }

	/// <summary>
	/// Create a LogicMonitor client
	/// </summary>
	/// <param name="logicMonitorClientOptions">The options</param>
	public LogicMonitorClient(
		LogicMonitorClientOptions logicMonitorClientOptions)
		: this(logicMonitorClientOptions, new HttpClientHandler { CookieContainer = new CookieContainer() })
	{
	}

	internal LogicMonitorClient(
		LogicMonitorClientOptions logicMonitorClientOptions,
		HttpMessageHandler handler)
	{
		_options = logicMonitorClientOptions;

		// Set up the logger
		_logger = logicMonitorClientOptions.Logger ?? new NullLogger<LogicMonitorClient>();

		_cache = new Cache<string, object>(TimeSpan.FromMinutes(5), _logger);

		AccountName = logicMonitorClientOptions.Account ?? throw new ArgumentException(nameof(LogicMonitorClientOptions));
		_accessId = logicMonitorClientOptions.AccessId ?? throw new ArgumentException(nameof(LogicMonitorClientOptions));
		_accessKey = logicMonitorClientOptions.AccessKey ?? throw new ArgumentException(nameof(LogicMonitorClientOptions));

		_handler = handler;
		_client = new HttpClient(_handler)
		{
			BaseAddress = new Uri($"https://{AccountName}.logicmonitor.com/santaba/")
		};
		_client.DefaultRequestHeaders.Accept.Clear();
		_client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
		_client.DefaultRequestHeaders.Add("X-Requested-With", "XMLHttpRequest");
		_client.DefaultRequestHeaders.Add("X-version", "3");
		_client.DefaultRequestHeaders.Add("X-CSRF-Token", "Fetch");
		_client.Timeout = TimeSpan.FromSeconds(logicMonitorClientOptions.HttpClientTimeoutSeconds);

		MaximumBackOffSeconds = logicMonitorClientOptions.MaximumBackOffSeconds;
	}
}
