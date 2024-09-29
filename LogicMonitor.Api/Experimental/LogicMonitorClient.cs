namespace LogicMonitor.Api.Experimental;

internal class LogicMonitorClient : IDisposable
{
	private bool _disposedValue;
	private readonly HttpClient _httpClient;

	internal LogicMonitorClient(LogicMonitorClientOptions options)
	{
		_httpClient = new HttpClient(new SecureHttpClientHandler(options))
		{
			BaseAddress = new Uri($"https://{options.Account}.logicmonitor.com/santaba/")
		};

		_httpClient.DefaultRequestHeaders.Accept.Clear();
		_httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
		_httpClient.DefaultRequestHeaders.Add("X-Requested-With", "XMLHttpRequest");
		_httpClient.DefaultRequestHeaders.Add("X-version", "3");
		_httpClient.DefaultRequestHeaders.Add("X-CSRF-Token", "Fetch");
		_httpClient.Timeout = TimeSpan.FromSeconds(options.HttpClientTimeoutSeconds);
	}

	internal async Task<ICollection<T>> GetAsync<T>(
		LogicMonitorRequest<T> request,
		CancellationToken cancellationToken
		) where T : IHasEndpoint, new()
	{
		var collection = new List<T>();
		var pagingRequest = new PagingRequest<T>(request);
		while (true)
		{
			var pagingResponse = await GetPageAsync(pagingRequest, cancellationToken).ConfigureAwait(false);
			collection.AddRange(pagingResponse.Items);
			if (collection.Count >= request.Take || pagingResponse.Items.Count <= request.Take || pagingResponse.Items.Count == 0)
			{
				break;
			}

			pagingRequest.IncrementPage();
		}

		return collection;
	}

	private async Task<Page<T>> GetPageAsync<T>(PagingRequest<T> request, CancellationToken cancellationToken) where T : IHasEndpoint, new()
	{
		// Determine the suburl from T
		var subUrl = request.ToString();
		var response = await _httpClient
			.GetAsync(subUrl, cancellationToken)
			.ConfigureAwait(false);

		// Extract the response as a Page<T> via a string
		var value = await response.Content.ReadAsStringAsync().ConfigureAwait(false);

		var page = JsonConvert.DeserializeObject<Page<T>>(value);
		return page ?? throw new InvalidOperationException("The response was not a Page<T>");
	}

	protected virtual void Dispose(bool disposing)
	{
		if (!_disposedValue)
		{
			if (disposing)
			{
				// TODO: dispose managed state (managed objects)
			}

			// TODO: free unmanaged resources (unmanaged objects) and override finalizer
			// TODO: set large fields to null
			_disposedValue = true;
		}
	}

	public void Dispose()
	{
		// Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
		Dispose(disposing: true);
		GC.SuppressFinalize(this);
	}
}
