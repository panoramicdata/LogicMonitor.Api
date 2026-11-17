using System.Security.Cryptography;

namespace LogicMonitor.Api;

/// <summary>
///     The main client for querying the portal.
/// </summary>
public partial class LogicMonitorClient
{

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

	internal static string GetSignature(string httpVerb, long epoch, string data, string resourcePath, string accessKey)
	{
		// Construct signature
		using var hmac = new HMACSHA256 { Key = Encoding.UTF8.GetBytes(accessKey) };
		var compoundString = $"{httpVerb}{epoch}{data}{resourcePath}";
		var signatureBytes = hmac.ComputeHash(Encoding.UTF8.GetBytes(compoundString));
		var signatureHex = Convert.ToHexStringLower(signatureBytes);
		return Convert.ToBase64String(Encoding.UTF8.GetBytes(signatureHex));
	}

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

	/// <summary>
	/// Get all
	/// </summary>
	/// <typeparam name="T"></typeparam>
	/// <param name="cancellationToken"></param>
	public Task<List<T>> GetAllAsync<T>(CancellationToken cancellationToken) where T : IHasEndpoint, new()
		=> GetAllInternalAsync((Filter<T>?)null, new T().Endpoint(), cancellationToken);

	/// <summary>
	/// Get all
	/// </summary>
	/// <typeparam name="T"></typeparam>
	/// <param name="filter"></param>
	/// <param name="subUrl"></param>
	/// <param name="cancellationToken"></param>
	public Task<List<T>> GetAllAsync<T>(
		Filter<T>? filter,
		string subUrl,
		CancellationToken cancellationToken
	) where T : new()
		=> GetAllInternalAsync(filter, subUrl, cancellationToken);

	/// <summary>
	/// Get all
	/// </summary>
	/// <typeparam name="T"></typeparam>
	/// <param name="subUrl"></param>
	/// <param name="cancellationToken"></param>
	public Task<List<T>> GetAllAsync<T>(string subUrl, CancellationToken cancellationToken) where T : new()
		=> GetAllInternalAsync(default(Filter<T>?), subUrl, cancellationToken);

	/// <summary>
	/// Get all
	/// </summary>
	/// <typeparam name="T"></typeparam>
	/// <param name="filter"></param>
	/// <param name="cancellationToken"></param>
	public Task<List<T>> GetAllAsync<T>(Filter<T>? filter, CancellationToken cancellationToken) where T : IHasEndpoint, new()
		=> GetAllInternalAsync(filter, new T().Endpoint(), cancellationToken);

	/// <summary>
	///     Get all T
	/// </summary>
	/// <typeparam name="T"></typeparam>
	/// <param name="filter"></param>
	/// <param name="subUrl"></param>
	/// <param name="cancellationToken"></param>
	private async Task<List<T>> GetAllInternalAsync<T>(Filter<T>? filter, string subUrl, CancellationToken cancellationToken) where T : new()
	{
		var requestedTake = filter?.Take ?? int.MaxValue;
		// Ensure filter is set up
		filter ??= new Filter<T>();

		filter.Take = Math.Min(300, requestedTake);
		filter.Skip = 0;
		var list = new List<T>();
		while (true)
		{
			// Get a page
			var page = await GetPageAsync(filter, subUrl, cancellationToken).ConfigureAwait(false);
			var pageItems = page.Items ?? [];
			list.AddRange(pageItems);

			var expectedTotal = GetExpectedPagedTotal(page.TotalCount, requestedTake);
			if (IsLastPage<T>(pageItems.Count, list.Count, expectedTotal))
			{
				ThrowIfPagingTotalMismatch(expectedTotal, list.Count);
				return list;
			}

			// Increment the skip value
			filter.Skip += filter.Take;
		}
	}

	// Strict checking, when enabled, rejects a total that does not match what was received
	private void ThrowIfPagingTotalMismatch(int expectedTotal, int receivedCount)
	{
		if (StrictPagingTotalChecking && expectedTotal != receivedCount)
		{
			throw new PagingException($"Mismatch between expected total: {expectedTotal} and received count: {receivedCount}");
		}
	}

	// Some endpoints return a negative total count
	private static int GetExpectedPagedTotal(int pageTotalCount, int requestedTake)
		=> Math.Min(pageTotalCount < 0 ? int.MaxValue : pageTotalCount, requestedTake);
}
