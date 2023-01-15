namespace LogicMonitor.Api;

/// <summary>
///     The main client for querying the portal.
/// </summary>
public partial class LogicMonitorClient : IDisposable
{
	#region Fields

	private static readonly HttpMethod PatchHttpMethod = new("PATCH");

	private readonly HttpClientHandler _handler;

	private readonly HttpClient _client;

	private static readonly JsonConverter[] JsonConverters =
	{
		new WidgetConverter(),
		new WidgetDataConverter(),
		new ReportConverter(),
		new FlagsEnumConverter()
	};

	private readonly Cache<string, object> _cache;

	private readonly string _accessId;

	private readonly string _accessKey;

	/// <summary>
	///     The connected account name
	/// </summary>
	public string AccountName { get; }

	#endregion Fields

	#region Properties

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

	private int attemptCount = 5;

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
		get => attemptCount;
		set
		{
			if (value < 1)
			{
				throw new ArgumentOutOfRangeException(nameof(value), "Must be >= 1.");
			}

			attemptCount = value;
		}
	}

	/// <summary>
	/// Whether to wait (10 second delay) during upgrades
	/// </summary>
	public bool WaitDuringLogicMonitorUpgrades { get; set; } = true;

	/// <summary>
	/// Whether to throw an exception when paging over results and the total does not match the count of items retrieved.
	/// This can happen for example when an item in the first page is deleted during paging.
	/// </summary>
	public bool StrictPagingTotalChecking { get; set; }

	#endregion Properties

	#region Constructor / Dispose

	/// <summary>
	/// Create a LogicMonitor client
	/// </summary>
	/// <param name="logicMonitorClientOptions">The options</param>
	public LogicMonitorClient(
		LogicMonitorClientOptions logicMonitorClientOptions)
	{
		// Set up the logger
		_logger = logicMonitorClientOptions.Logger ?? new NullLogger<LogicMonitorClient>();

		_cache = new Cache<string, object>(TimeSpan.FromMinutes(5), _logger);

		AccountName = logicMonitorClientOptions.Account ?? throw new ArgumentException(nameof(LogicMonitorClientOptions));
		_accessId = logicMonitorClientOptions.AccessId ?? throw new ArgumentException(nameof(LogicMonitorClientOptions));
		_accessKey = logicMonitorClientOptions.AccessKey ?? throw new ArgumentException(nameof(LogicMonitorClientOptions));

		_handler = new HttpClientHandler { CookieContainer = new CookieContainer() };
		_client = new HttpClient(_handler)
		{
			BaseAddress = new Uri($"https://{AccountName}.logicmonitor.com/santaba/")
		};
		_client.DefaultRequestHeaders.Accept.Clear();
		_client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
		_client.DefaultRequestHeaders.Add("X-Requested-With", "XMLHttpRequest");
		_client.DefaultRequestHeaders.Add("X-version", "3");
		_client.DefaultRequestHeaders.Add("X-CSRF-Token", "Fetch");
		_client.Timeout = TimeSpan.FromMinutes(1);
	}

	private static string GetSignature(string httpVerb, long epoch, string data, string resourcePath, string accessKey)
	{
		// Construct signature
		using var hmac = new System.Security.Cryptography.HMACSHA256 { Key = Encoding.UTF8.GetBytes(accessKey) };
		var compoundString = $"{httpVerb}{epoch}{data}{resourcePath}";
		var signatureBytes = hmac.ComputeHash(Encoding.UTF8.GetBytes(compoundString));
		var signatureHex = BitConverter.ToString(signatureBytes).Replace("-", "").ToLower(CultureInfo.InvariantCulture);
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

	#endregion Constructor / Dispose

	#region Web Interaction

	/// <summary>
	/// Get all
	/// </summary>
	/// <typeparam name="T"></typeparam>
	/// <param name="cancellationToken"></param>
	public Task<List<T>> GetAllAsync<T>(CancellationToken cancellationToken) where T : IHasEndpoint, new()
		=> GetAllInternalAsync((Filter<T>)null, new T().Endpoint(), cancellationToken);

	/// <summary>
	/// Get all
	/// </summary>
	/// <typeparam name="T"></typeparam>
	/// <param name="filter"></param>
	/// <param name="subUrl"></param>
	/// <param name="cancellationToken"></param>
	public Task<List<T>> GetAllAsync<T>(Filter<T> filter, string subUrl, CancellationToken cancellationToken) where T : new()
		=> GetAllInternalAsync(filter, subUrl, cancellationToken);

	/// <summary>
	/// Get all
	/// </summary>
	/// <typeparam name="T"></typeparam>
	/// <param name="subUrl"></param>
	/// <param name="cancellationToken"></param>
	public Task<List<T>> GetAllAsync<T>(string subUrl, CancellationToken cancellationToken) where T : new()
		=> GetAllInternalAsync(default(Filter<T>), subUrl, cancellationToken);

	/// <summary>
	/// Get all
	/// </summary>
	/// <typeparam name="T"></typeparam>
	/// <param name="filter"></param>
	/// <param name="cancellationToken"></param>
	public Task<List<T>> GetAllAsync<T>(Filter<T> filter = null, CancellationToken cancellationToken = default) where T : IHasEndpoint, new()
		=> GetAllInternalAsync(filter, new T().Endpoint(), cancellationToken);

	/// <summary>
	///     Get all T
	/// </summary>
	/// <typeparam name="T"></typeparam>
	/// <param name="filter"></param>
	/// <param name="subUrl"></param>
	/// <param name="cancellationToken"></param>
	private async Task<List<T>> GetAllInternalAsync<T>(Filter<T> filter, string subUrl, CancellationToken cancellationToken) where T : new()
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
			var pageItems = page.Items ?? new List<T>();
			var pageTotalCount = page?.TotalCount ?? 0;
			list.AddRange(pageItems);

			// Some endpoints return a negative total count
			var expectedTotal = Math.Min(pageTotalCount < 0 ? int.MaxValue : pageTotalCount, requestedTake);

			// Did we zero this time
			// OR do we already have all items?
			// OR Special case - OpsNotesTags don't like being paged.
			if (pageItems.Count == 0
				|| list.Count >= expectedTotal
				|| typeof(T) == typeof(OpsNoteTag))
			{
				// Yes.

				// Do strict checking if required
				if (StrictPagingTotalChecking && expectedTotal != list.Count)
				{
					throw new PagingException($"Mismatch between expected total: {expectedTotal} and received count: {list.Count}");
				}
				// Return list.
				return list;
			}

			// Increment the skip value
			filter.Skip += filter.Take;
		}
	}

	/// <summary>
	///     Gets an item singleton
	/// </summary>
	/// <typeparam name="T">The item to get</typeparam>
	/// <param name="cancellationToken"></param>
	public virtual Task<T> GetAsync<T>(CancellationToken cancellationToken) where T : class, IHasSingletonEndpoint, new()
		=> GetBySubUrlAsync<T>(new T().Endpoint(), cancellationToken);

	/// <summary>
	///     Gets an item by id
	/// </summary>
	/// <param name="id">The item id</param>
	/// <param name="cancellationToken">The cancellation token</param>
	/// <typeparam name="T">The item to get</typeparam>
	public virtual Task<T> GetAsync<T>(int id, CancellationToken cancellationToken) where T : IdentifiedItem, IHasEndpoint, new()
		=> GetBySubUrlAsync<T>($"{new T().Endpoint()}/{id}", cancellationToken);

	/// <summary>
	///     Gets a single item by id, bringing back only specific properties as specified in a filter
	/// </summary>
	/// <param name="id">The item id</param>
	/// <param name="filter"></param>
	/// <param name="cancellationToken">The cancellation token</param>
	/// <typeparam name="T">The item to get</typeparam>
	public virtual Task<T> GetAsync<T>(int id, Filter<T> filter, CancellationToken cancellationToken) where T : IdentifiedItem, IHasEndpoint, new()
		=> GetBySubUrlAsync<T>($"{new T().Endpoint()}/{id}?{filter}", cancellationToken);

	/// <summary>
	///    Gets something by its name
	/// </summary>
	/// <typeparam name="T"></typeparam>
	/// <param name="name"></param>
	/// <param name="cancellationToken"></param>
	public virtual async Task<T> GetByNameAsync<T>(string name, CancellationToken cancellationToken)
	where T : NamedItem, IHasEndpoint, new()
	{
		if (name is null)
		{
			throw new ArgumentNullException(nameof(name));
		}

		var items = await GetAllAsync(new Filter<T>
		{
			FilterItems = new List<FilterItem<T>>
				{
					new Eq<T>(nameof(NamedEntity.Name), name)
				}
		}, cancellationToken: cancellationToken).ConfigureAwait(false);

		// If only one, return that.
		return items.Count switch
		{
			0 => null,
			1 => items[0],
			_ => throw new Exception($"An unexpected number of {typeof(T).Name} have name: {name}"),
		};
	}

	/// <summary>
	///     Gets an item by id
	/// </summary>
	/// <param name="id">The item id</param>
	/// <param name="cancellationToken">The cancellation token</param>
	/// <typeparam name="T">The item to get</typeparam>
	public virtual Task<T> GetAsync<T>(string id, CancellationToken cancellationToken) where T : StringIdentifiedItem, IHasEndpoint, new()
	=> GetBySubUrlAsync<T>($"{new T().Endpoint()}/{id}", cancellationToken);

	/// <summary>
	///     Executes an item by id
	/// </summary>
	/// <typeparam name="T"></typeparam>
	/// <param name="object"></param>
	/// <param name="cancellationToken"></param>
	public virtual Task<T> ExecuteAsync<T>(T @object, CancellationToken cancellationToken) where T : IdentifiedItem, IExecutable, new()
	=> GetBySubUrlAsync<T>($"{@object.Endpoint()}/{@object.Id}/executenow", cancellationToken);

	/// <summary>
	///     Executes an item by id
	/// </summary>
	/// <param name="id">The item id</param>
	/// <param name="cancellationToken">The cancellation token</param>
	/// <typeparam name="T">The item to get</typeparam>
	public virtual Task<T> ExecuteAsync<T>(int id, CancellationToken cancellationToken) where T : IdentifiedItem, IExecutable, new()
	=> GetBySubUrlAsync<T>($"{new T().Endpoint()}/{id}/executenow", cancellationToken);

	/// <summary>
	///     Patch a single IPatchable entity's property.
	/// </summary>
	/// <param name="entity">The entity</param>
	/// <param name="propertyName">The name of the property</param>
	/// <param name="value">The value</param>
	/// <param name="cancellationToken">The cancellation token</param>
	/// <typeparam name="T">The entity type</typeparam>
	/// <exception cref="ArgumentOutOfRangeException">Throw if the property does not exist on the entity.</exception>
	public virtual async Task PatchPropertyAsync<T>(T entity, string propertyName, object value, CancellationToken cancellationToken) where T : IPatchable =>
		await PatchAsync(entity, new Dictionary<string, object>
		{
				{ GetSerializationName<T>(propertyName), value }
		}, cancellationToken)
	  .ConfigureAwait(false);

	/// <summary>
	/// Get the serialization name for a specified type
	/// </summary>
	/// <typeparam name="T">The type</typeparam>
	/// <param name="propertyName">The property</param>
	public static string? GetSerializationName<T>(string propertyName)
	{
		var propertyInfos = typeof(T).GetProperties();
		var property = propertyInfos.SingleOrDefault(p => p.Name == propertyName);
		if (property is null)
		{
			throw new ArgumentOutOfRangeException(nameof(propertyName), $"{propertyName} is not a property of {typeof(T).Name}.");
		}

		// Use reflection to find the DataMember Name
		return property.GetCustomAttributes(typeof(DataMemberAttribute), true).Cast<DataMemberAttribute>().SingleOrDefault()?.Name;
	}

	/// <summary>
	/// Get the serialization name for a specified enum type
	/// </summary>
	/// <param name="enumObject">The property</param>
	public static string GetSerializationNameFromEnumMember(object enumObject)
	{
		var type = enumObject.GetType();
		if (!type.IsEnum)
		{
			throw new ArgumentException($"{enumObject} is not an enum", nameof(enumObject));
		}

		var fieldInfos = type.GetFields();
		var field = fieldInfos.SingleOrDefault(f => f.Name == enumObject.ToString());
		if (field is null)
		{
			throw new ArgumentOutOfRangeException(nameof(enumObject), $"{@enumObject} is not a member of enum {type.Name}.");
		}

		// Use reflection to find the DataMember Name
		var list = field.GetCustomAttributes(typeof(EnumMemberAttribute), true).Cast<EnumMemberAttribute>().ToList();
		return list.SingleOrDefault()?.Value;
	}

	/// <summary>
	///     Gets the current portal version
	/// </summary>
	/// <returns>The portal version</returns>
	/// <exception cref="FormatException">Thrown if the portal version cannot be determined</exception>
	[Obsolete("Use GetVersionAsync() or GetVersionAsync(\"<accountName>\") instead.", true)]
	public async Task<int> GetPortalVersionAsync()
	{
		using var versionHttpClient = new HttpClient
		{
			BaseAddress = new Uri($"https://{AccountName}.logicmonitor.com/"),
			Timeout = TimeOut
		};
		var response = await versionHttpClient.GetAsync("").ConfigureAwait(false);
		var responseText = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
		var versionRegex = new Regex("sbui(?<version>\\d+)-");
		var match = versionRegex.Match(responseText);
		if (match.Success)
		{
			return int.Parse(match.Groups["version"].Value, CultureInfo.InvariantCulture);
		}

		throw new FormatException("Could not determine the portal version.");
	}

	/// <summary>
	///     Update an identified item
	/// </summary>
	/// <param name="object">The object to update</param>
	/// <typeparam name="T"></typeparam>
	/// <param name="cancellationToken">The cancellation token</param>
	/// <exception cref="AggregateException"></exception>
	public virtual async Task PutAsync<T>(T @object, CancellationToken cancellationToken) where T : IdentifiedItem, IHasEndpoint
	// The ignoreReference permits forcing appliesTo functions and is ignored for other types
	=> await PutAsync($"{@object.Endpoint()}/{@object.Id}?ignoreReference=true", @object, cancellationToken).ConfigureAwait(false);

	/// <summary>
	///     Update a string identified item
	/// </summary>
	/// <param name="object">The object to update</param>
	/// <typeparam name="T"></typeparam>
	/// <param name="cancellationToken">The cancellation token</param>
	/// <exception cref="AggregateException"></exception>
	public virtual async Task PutStringIdentifiedItemAsync<T>(T @object, CancellationToken cancellationToken) where T : StringIdentifiedItem, IHasEndpoint
	// The ignoreReference permits forcing appliesTo functions and is ignored for other types
	=> await PutAsync($"{@object.Endpoint()}/{@object.Id}?ignoreReference=true", @object, cancellationToken).ConfigureAwait(false);

	/// <summary>
	/// Update an item
	/// </summary>
	/// <param name="subUrl">The subURL</param>
	/// <param name="object">The updated object</param>
	/// <param name="cancellationToken">An optional CancellationToken</param>
	public async Task PutAsync(string subUrl, object @object, CancellationToken cancellationToken)
	{
		var httpMethod = HttpMethod.Put;
		var prefix = GetPrefix(httpMethod);
		_logger.LogDebug("{Prefix} {SubUrl}", prefix, subUrl);

		var jsonString = JsonConvert.SerializeObject(@object);
		_logger.LogTrace("{Prefix} jsonString:\r\n{JsonString}", prefix, jsonString);
		using var content = new StringContent(jsonString, Encoding.UTF8, "application/json");
		HttpResponseMessage httpResponseMessage;
		// Handle rate limiting (see https://www.logicmonitor.com/support/rest-api-developers-guide/overview/using-logicmonitors-rest-api/)
		while (true)
		{
			// Determine the cancellationToken
			// We will always use one
			using (var requestMessage = new HttpRequestMessage(httpMethod, RequestUri(subUrl)) { Content = content })
			{
				httpResponseMessage = await GetHttpResponseMessage(requestMessage, subUrl, jsonString, cancellationToken).ConfigureAwait(false);
			}

			_logger.LogDebug("{Prefix} complete", prefix);

			// Check the outer HTTP status code
			if (!httpResponseMessage.IsSuccessStatusCode)
			{
				if ((int)httpResponseMessage.StatusCode != 429 && httpResponseMessage.ReasonPhrase != "Too Many Requests")
				{
					if (WaitDuringLogicMonitorUpgrades && httpResponseMessage.StatusCode == HttpStatusCode.ServiceUnavailable)
					{
						// TODO: could also check the reason phrase, and / or the RESPONSE (which contains "Service Temporarily Unavailable")
						_logger.LogDebug("{Prefix} Service Unavailable. Waiting 10000ms", prefix);
						await Task.Delay(10000, cancellationToken).ConfigureAwait(false);
						continue;
					}

					var responseBody = await httpResponseMessage.Content.ReadAsStringAsync().ConfigureAwait(false);
					_logger.LogDebug("{Prefix} failed ({StatusCode}): {ResponseBody}",
						prefix,
						httpResponseMessage.StatusCode,
						responseBody);
					throw new LogicMonitorApiException(httpMethod, subUrl, httpResponseMessage.StatusCode, responseBody);
				}
				// We have been told to back off

				// Check that all rate limit headers are present
				var xRateLimitWindowLimitCount = await GetIntegerHeaderAsync(httpMethod, subUrl, httpResponseMessage.StatusCode, httpResponseMessage, "X-Rate-Limit-Limit").ConfigureAwait(false);
				var xRateLimitWindowRemainingCount = await GetIntegerHeaderAsync(httpMethod, subUrl, httpResponseMessage.StatusCode, httpResponseMessage, "X-Rate-Limit-Remaining").ConfigureAwait(false);
				var xRateLimitWindowSeconds = await GetIntegerHeaderAsync(httpMethod, subUrl, httpResponseMessage.StatusCode, httpResponseMessage, "X-Rate-Limit-Window").ConfigureAwait(false);

				var rateLimitInformation = $"Window: {xRateLimitWindowSeconds}s, Count: {xRateLimitWindowLimitCount}, Remaining {xRateLimitWindowRemainingCount}";
				// Wait for the full window
				var delayMs = 1000 * xRateLimitWindowSeconds;

				// Wait some time and try again
				_logger.LogInformation("{Prefix} Rate limiting hit (with cancellation token): {RateLimitInformation}, waiting {DelayMs:N0}ms",
					prefix,
					rateLimitInformation,
					delayMs);
				await Task.Delay(delayMs, cancellationToken).ConfigureAwait(false);

				// Try again
				continue;
			}

			// Success - can be cached
			break;
		}

		var portalResponse = new PortalResponse<EmptyResponse>(httpResponseMessage);

		// Check the outer HTTP status code
		if (!portalResponse.IsSuccessStatusCode)
		{
			// If a success code was not received, throw an exception
			throw new LogicMonitorApiException(portalResponse.ErrorMessage) { HttpStatusCode = portalResponse.HttpStatusCode };
		}
	}

	/// <summary>
	///     Deletes an item
	/// </summary>
	/// <param name="object">The object to delete</param>
	/// <param name="hardDelete">Whether to hard delete.</param>
	/// <param name="cancellationToken">The optional cancellation token</param>
	/// <typeparam name="T">The type of the item to delete</typeparam>
	public virtual async Task DeleteAsync<T>(
		T @object,
		bool hardDelete = true,
		CancellationToken cancellationToken = default)
		where T : IdentifiedItem, IHasEndpoint, new()
	=> await DeleteAsync($"{new T().Endpoint()}/{@object.Id}{(!hardDelete ? "?deleteHard=false" : string.Empty)}", cancellationToken)
		.ConfigureAwait(false);

	/// <summary>
	///     Deletes an item
	/// </summary>
	/// <param name="object">The object to delete</param>
	/// <param name="hardDelete">Whether to hard delete.</param>
	/// <param name="cancellationToken">The optional cancellation token</param>
	/// <typeparam name="T">The type of the item to delete</typeparam>
	public virtual async Task DeleteStringIdentifiedAsync<T>(
	T @object,
	bool hardDelete = true,
	CancellationToken cancellationToken = default)
	where T : StringIdentifiedItem, IHasEndpoint, new()
	=> await DeleteAsync($"{new T().Endpoint()}/{@object.Id}{(!hardDelete ? "?deleteHard=false" : string.Empty)}", cancellationToken)
	.ConfigureAwait(false);

	/// <summary>
	///     Deletes an item by id
	/// </summary>
	/// <param name="id">The item id</param>
	/// <param name="hardDelete">Whether to hard delete.</param>
	/// <param name="cancellationToken">The optional cancellation token</param>
	/// <typeparam name="T">The type of the item to delete</typeparam>
	public virtual async Task DeleteAsync<T>(
	int id,
	bool hardDelete = true,
	CancellationToken cancellationToken = default
	) where T : IdentifiedItem, IHasEndpoint, new()
	=> await DeleteAsync($"{new T().Endpoint()}/{id}{(!hardDelete ? "?deleteHard=false" : string.Empty)}", cancellationToken)
	.ConfigureAwait(false);

	/// <summary>
	///     Deletes an item by id
	/// </summary>
	/// <param name="id">The item id</param>
	/// <param name="hardDelete">Whether to hard delete.</param>
	/// <param name="cancellationToken">The optional cancellation token</param>
	/// <typeparam name="T">The type of the item to delete</typeparam>
	public virtual async Task DeleteAsync<T>(
	 string id,
	 bool hardDelete = true,
	 CancellationToken cancellationToken = default) where T : StringIdentifiedItem, IHasEndpoint, new()
	 => await DeleteAsync($"{new T().Endpoint()}/{id}{(!hardDelete ? "?deleteHard=false" : string.Empty)}", cancellationToken).ConfigureAwait(false);

	/// <summary>
	///     Deletes an item by id
	/// </summary>
	/// <param name="deviceDataSourceInstance">The DeviceDataSourceInstance to delete</param>
	/// <param name="hardDelete">Whether to hard delete.</param>
	/// <param name="cancellationToken">The optional cancellation token</param>
	public virtual async Task DeleteAsync(
		DeviceDataSourceInstance deviceDataSourceInstance,
		bool hardDelete = true,
		CancellationToken cancellationToken = default)
		=> await DeleteAsync($"device/devices/{deviceDataSourceInstance.DeviceId}/devicedatasources/{deviceDataSourceInstance.DeviceDataSourceId}/instances/{deviceDataSourceInstance.Id}{(!hardDelete ? "?deleteHard=false" : string.Empty)}", cancellationToken).ConfigureAwait(false);

	/// <summary>
	///     Create an item
	/// </summary>
	/// <param name="creationDto"></param>
	/// <param name="cancellationToken">The cancellation token</param>
	/// <typeparam name="T"></typeparam>
	public virtual Task<T> CreateAsync<T>(CreationDto<T> creationDto, CancellationToken cancellationToken) where T : IHasEndpoint, new()
		=> PostAsync<CreationDto<T>, T>(creationDto, new T().Endpoint(), cancellationToken);

	/// <summary>
	/// Gets an integer header
	/// </summary>
	/// <param name="httpMethod"></param>
	/// <param name="subUrl"></param>
	/// <param name="httpStatusCode"></param>
	/// <param name="httpResponseMessage">The response message</param>
	/// <param name="header">The required header</param>
	/// <exception cref="LogicMonitorApiException">Thrown when the header is not a single integer.</exception>
	/// <returns>The integer value</returns>
	private async Task<int> GetIntegerHeaderAsync(HttpMethod httpMethod, string subUrl, HttpStatusCode httpStatusCode, HttpResponseMessage httpResponseMessage, string header)
	{
		var responseBody = await httpResponseMessage.Content.ReadAsStringAsync().ConfigureAwait(false);

		if (!httpResponseMessage.Headers.TryGetValues(header, out var valueStringEnumerable))
		{
			var message = $"Response header '{header}' is not an integer.";
			_logger.LogDebug("{Message}", message);
			throw new LogicMonitorApiException(httpMethod, subUrl, httpStatusCode, responseBody, message);
		}

		var valueStringList = valueStringEnumerable.ToList();

		var firstOrDefaultMatchingHeader = valueStringList.FirstOrDefault();

		if (firstOrDefaultMatchingHeader is null)
		{
			var message = $"'{header}' header value contains {valueStringList.Count} values.  Expecting just one.";
			throw new LogicMonitorApiException(httpMethod, subUrl, httpStatusCode, responseBody, message);
		}

		if (!int.TryParse(firstOrDefaultMatchingHeader, out var valueInt))
		{
			var message = $"'{header}' header value '{firstOrDefaultMatchingHeader}' is not an integer.";
			throw new LogicMonitorApiException(httpMethod, subUrl, httpStatusCode, responseBody, message);
		}

		return valueInt;
	}

	private string RequestUri(string subUrl) => subUrl == "log/ingest" || subUrl == "metric/ingest"
		? $"https://{AccountName}.logicmonitor.com/rest/{subUrl}"
		: $"https://{AccountName}.logicmonitor.com/santaba/rest/{subUrl}";

	private async Task<HttpResponseMessage> GetHttpResponseMessage(HttpRequestMessage requestMessage, string subUrl, string data, CancellationToken cancellationToken)
	{
		var epoch = (long)(DateTime.UtcNow - new DateTime(1970, 1, 1)).TotalMilliseconds;
		var subUrl2 = subUrl.Contains("?")
			? subUrl.Substring(0, subUrl.IndexOf("?", StringComparison.Ordinal))
			: subUrl;
		var httpVerb = requestMessage.Method.ToString().ToUpperInvariant();
		var resourcePath = $"/{subUrl2}";

		// Auth header
		var authHeaderValue = $"LMv1 {_accessId}:{GetSignature(httpVerb, epoch, data, resourcePath, _accessKey)}:{epoch}";
		requestMessage.Headers.Add("Authorization", authHeaderValue);

		_logger.LogHttpHeaders(true, null, requestMessage.Headers);
		var response = await _client
			.SendAsync(requestMessage, cancellationToken)
			.ConfigureAwait(false);

		UpdateSummary(requestMessage, response);

		return response;
	}

	private void UpdateSummary(HttpRequestMessage requestMessage, HttpResponseMessage response)
	{
		if (response.IsSuccessStatusCode)
		{
			Statistics.ApiCallSuccessCount++;
		}
		else
		{
			Statistics.ApiCallFailureCount++;
		}

		switch (requestMessage.Method.ToString())
		{
			case "DELETE":
				Statistics.ApiCallDeleteCount++;
				break;
			case "GET":
				Statistics.ApiCallGetCount++;
				break;
			case "POST":
				Statistics.ApiCallPostCount++;
				break;
			case "TRACE":
				Statistics.ApiCallTraceCount++;
				break;
			case "HEAD":
				Statistics.ApiCallHeadCount++;
				break;
			case "PUT":
				Statistics.ApiCallPutCount++;
				break;
			case "OPTIONS":
				Statistics.ApiCallOptionsCount++;
				break;
			case "PATCH":
				Statistics.ApiCallPatchCount++;
				break;
			default:
				Statistics.ApiCallOtherCount++;
				break;
		}

		Statistics.DataTransferUplinkBytes += requestMessage.Content?.Headers.ContentLength ?? 0;
		Statistics.DataTransferDownlinkBytes += response.Content?.Headers.ContentLength ?? 0;
	}

	private Task<Page<T>> FilteredGetAsync<T>(string subUrl, Filter<T> filter, CancellationToken cancellationToken) where T : new()
		=> GetAsync<Page<T>>(UseCache, $"{subUrl}?{filter}", cancellationToken);


	/// <summary>
	///     Gets a filtered page of items
	/// </summary>
	/// <param name="filter">The filter</param>
	/// <param name="cancellationToken">An optional cancellation token</param>
	/// <typeparam name="T">The item type</typeparam>
	/// <returns>The filtered list</returns>
	public virtual Task<Page<T>> GetAsync<T>(Filter<T> filter, CancellationToken cancellationToken) where T : IHasEndpoint, new()
		=> GetAsync<Page<T>>(UseCache, $"{new T().Endpoint()}?{filter}", cancellationToken);

	private Task<T> GetBySubUrlAsync<T>(string subUrl, CancellationToken cancellationToken) where T : class, new()
	=> GetAsync<T>(UseCache, subUrl, cancellationToken);

	/// <summary>
	/// Gets a JObject directly from the API
	/// </summary>
	/// <param name="subUrl">The subUrl</param>
	/// <param name="cancellationToken">The cancellation token.</param>
	public Task<JObject> GetJObjectAsync(string subUrl, CancellationToken cancellationToken)
	=> GetAsync<JObject>(UseCache, subUrl, cancellationToken);

	/// <summary>
	/// Delete an item
	/// </summary>
	/// <param name="subUrl"></param>
	/// <param name="cancellationToken"></param>
	public async Task DeleteAsync(string subUrl, CancellationToken cancellationToken)
	{
		if (subUrl is null)
		{
			throw new ArgumentNullException(nameof(subUrl));
		}

		var httpMethod = HttpMethod.Delete;
		var prefix = GetPrefix(httpMethod);
		_logger.LogDebug("{Prefix} {SubUrl}", prefix, subUrl);

		var stopwatch = Stopwatch.StartNew();
		HttpResponseMessage httpResponseMessage;
		// Handle rate limiting (see https://www.logicmonitor.com/support/rest-api-developers-guide/overview/using-logicmonitors-rest-api/)
		var failureCount = 0;
		while (true)
		{
			// Determine the cancellationToken
			// We will always use one
			try
			{
				using (var requestMessage = new HttpRequestMessage(httpMethod, RequestUri(subUrl)))
				{
					httpResponseMessage = await GetHttpResponseMessage(requestMessage, subUrl, null, cancellationToken).ConfigureAwait(false);
				}

				_logger.LogDebug("{Prefix} complete (from remote: {ElapsedMilliseconds:N0}ms)",
					prefix,
					stopwatch.ElapsedMilliseconds);
			}
			catch (Exception e)
			{
				_logger.LogDebug("{Prefix} failed on attempt {FailureCount}: {StackTrace}",
					prefix,
					++failureCount,
					e.ToString());
				if (failureCount < AttemptCount)
				{
					// Try again
					_logger.LogDebug("{Prefix} Retrying..", prefix);
					continue;
				}

				_logger.LogDebug("{Prefix} Giving up.", prefix);
				throw;
			}

			// Check the outer HTTP status code
			if (!httpResponseMessage.IsSuccessStatusCode)
			{
				if ((int)httpResponseMessage.StatusCode != 429 && httpResponseMessage.ReasonPhrase != "Too Many Requests")
				{
					if (WaitDuringLogicMonitorUpgrades && httpResponseMessage.StatusCode == HttpStatusCode.ServiceUnavailable)
					{
						// TODO: could also check the reason phrase, and / or the RESPONSE (which contains "Service Temporarily Unavailable")
						_logger.LogDebug("{Prefix} Service Unavailable. Waiting 10000ms", prefix);
						await Task.Delay(10000, cancellationToken).ConfigureAwait(false);
						continue;
					}

					var responseBody = await httpResponseMessage.Content.ReadAsStringAsync().ConfigureAwait(false);
					_logger.LogDebug("{Prefix} failed with {StatusCode}: {ResponseBody}",
						prefix,
						httpResponseMessage.StatusCode,
						responseBody);
					throw new LogicMonitorApiException(HttpMethod.Get, subUrl, httpResponseMessage.StatusCode, responseBody, $"{prefix} failed: {responseBody}");
				}
				// We have been told to back off

				// Check that all rate limit headers are present
				var xRateLimitWindowLimitCount = await GetIntegerHeaderAsync(httpMethod, subUrl, httpResponseMessage.StatusCode, httpResponseMessage, "X-Rate-Limit-Limit").ConfigureAwait(false);
				var xRateLimitWindowRemainingCount = await GetIntegerHeaderAsync(httpMethod, subUrl, httpResponseMessage.StatusCode, httpResponseMessage, "X-Rate-Limit-Remaining").ConfigureAwait(false);
				var xRateLimitWindowSeconds = await GetIntegerHeaderAsync(httpMethod, subUrl, httpResponseMessage.StatusCode, httpResponseMessage, "X-Rate-Limit-Window").ConfigureAwait(false);

				var rateLimitInformation = $"Window: {xRateLimitWindowSeconds}s, Count: {xRateLimitWindowLimitCount}, Remaining {xRateLimitWindowRemainingCount}";
				// Wait for the full window
				var delayMs = 1000 * xRateLimitWindowSeconds;

				// Wait some time and try again
				_logger.LogDebug("{Prefix} Rate limiting hit (with cancellation token): {RateLimitInformation}, waiting {DelayMs:N0}ms",
					prefix,
					rateLimitInformation,
					delayMs);
				await Task.Delay(delayMs, cancellationToken).ConfigureAwait(false);

				// Try again
				continue;
			}

			// Success - can be cached if permitted
			break;
		}

		_logger.LogDebug("{Prefix} complete", prefix);

		// Check the outer HTTP status code
		if (!httpResponseMessage.IsSuccessStatusCode)
		{
			// If a success code was not received, throw an exception
			var responseBody = await httpResponseMessage.Content.ReadAsStringAsync().ConfigureAwait(false);
			_logger.LogDebug("{Prefix} failed ({StatusCode}): {ResponseBody}",
				prefix,
				httpResponseMessage.StatusCode,
				responseBody);
			throw new LogicMonitorApiException(HttpMethod.Get, subUrl, httpResponseMessage.StatusCode, responseBody, $"{prefix} failed ({httpResponseMessage.StatusCode}): {responseBody}");
		}

		// Get the content
		var jsonString = await httpResponseMessage.Content.ReadAsStringAsync().ConfigureAwait(false);
		if (jsonString.Contains("group is not empty"))
		{
			// If a success code was not received, throw an exception
			_logger.LogDebug("{Prefix} failed - group is not empty for suburl", prefix);
			var responseBody = await httpResponseMessage.Content.ReadAsStringAsync().ConfigureAwait(false);
			throw new LogicMonitorApiException(httpMethod, subUrl, HttpStatusCode.PreconditionFailed, responseBody, $"{prefix} failed - group is not empty for suburl");
		}
	}

	private string GetPrefix(HttpMethod method) => $"{Guid.NewGuid()}: {method}";

	/// <summary>
	///     Async Get method
	/// </summary>
	/// <param name="permitCacheIfEnabled"></param>
	/// <param name="subUrl"></param>
	/// <param name="cancellationToken">The cancellation token</param>
	/// <typeparam name="T"></typeparam>
	/// <exception cref="LogicMonitorApiException"></exception>
	private async Task<T> GetAsync<T>(bool permitCacheIfEnabled, string subUrl, CancellationToken cancellationToken) where T : class, new()
	{
		var httpMethod = HttpMethod.Get;
		var prefix = GetPrefix(httpMethod);
		_logger.LogDebug("{Prefix} {SubUrl}", prefix, subUrl);

		// Age the Cache
		_cache.Age();

		var stopwatch = Stopwatch.StartNew();

		var useCache = permitCacheIfEnabled && UseCache;
		if (useCache && _cache.TryGetValue<T>(subUrl, out var cacheObject))
		{
			_logger.LogDebug("{Prefix} complete (from cache: {ElapsedMilliseconds:N0}ms)",
				prefix,
				stopwatch.ElapsedMilliseconds);

			return (T)cacheObject;
		}

		HttpResponseMessage httpResponseMessage;
		// Handle rate limiting (see https://www.logicmonitor.com/support/rest-api-developers-guide/overview/using-logicmonitors-rest-api/)
		var failureCount = 0;
		while (true)
		{
			// Determine the cancellationToken
			// We will always use one
			try
			{
				using (var requestMessage = new HttpRequestMessage(httpMethod, RequestUri(subUrl)))
				{
					httpResponseMessage = await GetHttpResponseMessage(requestMessage, subUrl, null, cancellationToken).ConfigureAwait(false);
				}

				_logger.LogDebug("{Prefix} complete (from remote: {ElapsedMilliseconds:N0}ms)",
					prefix,
					stopwatch.ElapsedMilliseconds);
			}
			catch (Exception e)
			{
				_logger.LogDebug("{Prefix} failed on attempt {FailureCount}: {StackTrace}",
					prefix,
					++failureCount,
					e.ToString());
				if (failureCount < AttemptCount)
				{
					_logger.LogDebug("{Prefix} Retrying..", prefix);
					// Try again (after a short delay)
					await Task.Delay(1000, cancellationToken).ConfigureAwait(false);
					continue;
				}

				_logger.LogDebug("{Prefix} Giving up.", prefix);
				throw;
			}

			// Check the outer HTTP status code
			if (!httpResponseMessage.IsSuccessStatusCode)
			{
				if ((int)httpResponseMessage.StatusCode != 429 && httpResponseMessage.ReasonPhrase != "Too Many Requests")
				{
					if (WaitDuringLogicMonitorUpgrades && httpResponseMessage.StatusCode == HttpStatusCode.ServiceUnavailable)
					{
						// TODO: could also check the reason phrase, and / or the RESPONSE (which contains "Service Temporarily Unavailable")
						_logger.LogDebug("{Prefix} Service Unavailable. Waiting 10000ms", prefix);
						await Task.Delay(10000, cancellationToken).ConfigureAwait(false);
						continue;
					}

					var responseBody = await httpResponseMessage.Content.ReadAsStringAsync().ConfigureAwait(false);

					_logger.LogHttpHeaders(false, prefix, httpResponseMessage.Headers);
					_logger.LogDebug("{Prefix} failed on attempt {FailureCount}: {ResponseBody}",
						prefix,
						++failureCount,
						responseBody);
					if (failureCount < AttemptCount)
					{
						_logger.LogDebug("{Prefix} Retrying..", prefix);
						// Try again (after a short delay)
						await Task.Delay(1000, cancellationToken).ConfigureAwait(false);
						continue;
					}

					throw new LogicMonitorApiException(httpMethod, subUrl, httpResponseMessage.StatusCode, responseBody, $"{prefix} failed ({httpResponseMessage.StatusCode}): {responseBody}");
				}
				// We have been told to back off

				// Check that all rate limit headers are present
				var xRateLimitWindowLimitCount = await GetIntegerHeaderAsync(httpMethod, subUrl, httpResponseMessage.StatusCode, httpResponseMessage, "X-Rate-Limit-Limit").ConfigureAwait(false);
				var xRateLimitWindowRemainingCount = await GetIntegerHeaderAsync(httpMethod, subUrl, httpResponseMessage.StatusCode, httpResponseMessage, "X-Rate-Limit-Remaining").ConfigureAwait(false);
				var xRateLimitWindowSeconds = await GetIntegerHeaderAsync(httpMethod, subUrl, httpResponseMessage.StatusCode, httpResponseMessage, "X-Rate-Limit-Window").ConfigureAwait(false);

				var rateLimitInformation = $"Window: {xRateLimitWindowSeconds}s, Count: {xRateLimitWindowLimitCount}, Remaining {xRateLimitWindowRemainingCount}";
				// Wait for the full window
				var delayMs = 1000 * xRateLimitWindowSeconds;

				// Wait some time and try again
				_logger.LogDebug("{Prefix} Rate limiting hit (with cancellation token): {RateLimitInformation}, waiting {DelayMs:N0}ms",
					prefix,
					rateLimitInformation,
					delayMs);
				await Task.Delay(delayMs, cancellationToken).ConfigureAwait(false);

				// Try again
				continue;
			}

			// Success - can be cached if permitted
			break;
		}

		// If this is a file response, return that
		if (typeof(T).Name == nameof(XmlResponse))
		{
			var content = await httpResponseMessage.Content.ReadAsStringAsync().ConfigureAwait(false);
			_logger.LogTrace("RESPONSE:\r\n\r\n {Content}", content);
			return new XmlResponse { Content = content } as T;
		}
		else if (typeof(T) == typeof(List<byte>))
		{
			var content = await httpResponseMessage.Content.ReadAsByteArrayAsync().ConfigureAwait(false);
			_logger.LogTrace("Byte Array");
			return content.ToList() as T;
		}
		else if (typeof(T) == typeof(DownloadFileInfo))
		{
			var tempFileInfo = new FileInfo(Path.GetTempFileName());
			using Stream output = File.OpenWrite(tempFileInfo.FullName);
			using var input = await httpResponseMessage.Content.ReadAsStreamAsync().ConfigureAwait(false);
			await input.CopyToAsync(output).ConfigureAwait(false);

			return new DownloadFileInfo
			{
				FileInfo = tempFileInfo
			} as T;
		}

		// Create a PortalResponse
		var portalResponse = new PortalResponse<T>(httpResponseMessage);

		// Check the outer HTTP status code
		if (!portalResponse.IsSuccessStatusCode)
		{
			// If a success code was not received, throw an exception
			var responseBody = await httpResponseMessage.Content.ReadAsStringAsync().ConfigureAwait(false);
			_logger.LogHttpHeaders(false, prefix, httpResponseMessage.Headers);
			_logger.LogDebug("{Prefix} failed ({StatusCode}): {ResponseBody}",
				prefix,
				httpResponseMessage.StatusCode,
				responseBody);
			_logger.LogTrace("RESPONSE:\r\n\r\n{ResponseBody}", responseBody);
			throw new LogicMonitorApiException(httpMethod, subUrl, portalResponse.HttpStatusCode, responseBody, $"{prefix} failed ({httpResponseMessage.StatusCode}): {responseBody}");
		}

		// Return the object
		T? deserializedObject;
		_logger.LogTrace("RESPONSE:\r\n\r\n{Data}:", portalResponse.Data?.ToString());
		try
		{
			deserializedObject = portalResponse.GetObject(JsonConverters);
		}
		catch (DeserializationException e)
		{
			_logger.LogError("{Prefix} Unable to deserialize\n{ResponseBody}\n{Message}",
				prefix,
				e.ResponseBody,
				e.Message);
			throw;
		}

		// Cache the result
		if (useCache)
		{
			_cache.AddOrUpdate(subUrl, deserializedObject);
		}

		// Return the result
		return deserializedObject;
	}

	/// <summary>
	/// Post an item
	/// </summary>
	/// <typeparam name="TIn">The posted object type</typeparam>
	/// <typeparam name="TOut">The returned object type</typeparam>
	/// <param name="obj">The posted object </param>
	/// <param name="subUrl">The endpoint</param>
	/// <param name="cancellationToken">An optional CancellationToken</param>
	public async Task<TOut> PostAsync<TIn, TOut>(TIn obj, string subUrl, CancellationToken cancellationToken) where TOut : new()
	{
		if (subUrl is null)
		{
			throw new ArgumentNullException(nameof(subUrl));
		}

		var httpMethod = HttpMethod.Post;
		var prefix = GetPrefix(httpMethod);
		_logger.LogDebug("{Prefix} {SubUrl}", prefix, subUrl);

		// LMREP-1042: "d:\"EBSDB [prod24778]\" does not work, however "d:\"EBSDB *prod24778*\" matches. Unrelated to URl encoding, etc...
		var data = JsonConvert.SerializeObject(obj, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });
		//var subUrl1 = "rest/" + (subUrl ?? "functions/");
		_logger.LogTrace("{Prefix} body:\r\n{Data}", prefix, data);
		HttpResponseMessage httpResponseMessage;
		// Handle rate limiting (see https://www.logicmonitor.com/support/rest-api-developers-guide/overview/using-logicmonitors-rest-api/)
		while (true)
		{
			using (var content = new StringContent(data, Encoding.UTF8, "application/json"))
			{
				using (var requestMessage = new HttpRequestMessage(httpMethod, RequestUri(subUrl)) { Content = content })
				{
					httpResponseMessage = await GetHttpResponseMessage(requestMessage, subUrl, data, cancellationToken)
						.ConfigureAwait(false);
				}

				_logger.LogDebug("{Prefix} complete", prefix);
			}

			// Check the outer HTTP status code
			if (!httpResponseMessage.IsSuccessStatusCode)
			{
				if ((int)httpResponseMessage.StatusCode != 429 && httpResponseMessage.ReasonPhrase != "Too Many Requests")
				{
					if (WaitDuringLogicMonitorUpgrades && httpResponseMessage.StatusCode == HttpStatusCode.ServiceUnavailable)
					{
						// TODO: could also check the reason phrase, and / or the RESPONSE (which contains "Service Temporarily Unavailable")
						_logger.LogDebug("{Prefix} Service Unavailable. Waiting 10000ms", prefix);
						await Task.Delay(10000, cancellationToken).ConfigureAwait(false);
						continue;
					}
					// If a success code was not received, throw an exception
					var responseBody = await httpResponseMessage.Content.ReadAsStringAsync().ConfigureAwait(false);
					_logger.LogDebug("{Prefix} failed ({StatusCode}): {ResponseBody}",
						prefix,
						httpResponseMessage.StatusCode,
						responseBody);
					throw new LogicMonitorApiException(httpMethod, subUrl, httpResponseMessage.StatusCode, responseBody, $"{prefix} failed ({httpResponseMessage.StatusCode}): {responseBody}");
				}

				// We have been told to back off
				// Check that all rate limit headers are present
				var xRateLimitWindowLimitCount = await GetIntegerHeaderAsync(httpMethod, subUrl, httpResponseMessage.StatusCode, httpResponseMessage, "X-Rate-Limit-Limit").ConfigureAwait(false);
				var xRateLimitWindowRemainingCount = await GetIntegerHeaderAsync(httpMethod, subUrl, httpResponseMessage.StatusCode, httpResponseMessage, "X-Rate-Limit-Remaining").ConfigureAwait(false);
				var xRateLimitWindowSeconds = await GetIntegerHeaderAsync(httpMethod, subUrl, httpResponseMessage.StatusCode, httpResponseMessage, "X-Rate-Limit-Window").ConfigureAwait(false);

				var rateLimitInformation = $"Window: {xRateLimitWindowSeconds}s, Count: {xRateLimitWindowLimitCount}, Remaining {xRateLimitWindowRemainingCount}";
				// Wait for the full window
				var delayMs = 1000 * xRateLimitWindowSeconds;

				// Wait some time and try again
				_logger.LogDebug("{Prefix} Rate limiting hit (with cancellation token): {RateLimitInformation}, waiting {DelayMs:N0}ms",
					prefix,
					rateLimitInformation,
					delayMs);
				await Task.Delay(delayMs, cancellationToken).ConfigureAwait(false);

				// Try again
				continue;
			}

			// Success - can be cached
			break;
		}
		// Success - can cache

		// Create a PortalResponse
		var portalResponse = new PortalResponse<TOut>(httpResponseMessage);

		// Check the outer HTTP status code
		if (!portalResponse.IsSuccessStatusCode)
		{
			// If a success code was not received, throw an exception
			throw new LogicMonitorApiException(portalResponse.ErrorMessage) { HttpStatusCode = portalResponse.HttpStatusCode };
		}

		// Deserialize the JSON
		var deserializedObject = portalResponse.GetObject();

		// Return
		return deserializedObject;
	}

	/// <summary>
	/// Post a jObect
	/// </summary>
	/// <param name="jObject">The posted jObject</param>
	/// <param name="subUrl">The endpoint</param>
	/// <param name="cancellationToken">An optional CancellationToken</param>
	/// <returns></returns>
	/// <exception cref="ArgumentNullException"></exception>
	/// <exception cref="LogicMonitorApiException"></exception>
	public async Task<JObject?> PostJObjectAsync(
		JObject jObject,
		string subUrl,
		CancellationToken cancellationToken = default)
	{
		if (subUrl is null)
		{
			throw new ArgumentNullException(nameof(subUrl));
		}

		var httpMethod = HttpMethod.Post;
		var prefix = GetPrefix(httpMethod);
		_logger.LogDebug("{Prefix} {SubUrl}", prefix, subUrl);

		var data = jObject.ToString();
		_logger.LogTrace("{Prefix} body:\r\n{Data}", prefix, data);
		HttpResponseMessage httpResponseMessage;
		while (true)
		{
			using (var content = new StringContent(data, Encoding.UTF8, "application/json"))
			{
				using (var requestMessage = new HttpRequestMessage(httpMethod, RequestUri(subUrl)) { Content = content })
				{
					httpResponseMessage = await GetHttpResponseMessage(requestMessage, subUrl, data, cancellationToken)
						.ConfigureAwait(false);
				}

				_logger.LogDebug("{Prefix} complete", prefix);
			}

			// Check the outer HTTP status code
			if (!httpResponseMessage.IsSuccessStatusCode)
			{
				if ((int)httpResponseMessage.StatusCode != 429 && httpResponseMessage.ReasonPhrase != "Too Many Requests")
				{
					if (WaitDuringLogicMonitorUpgrades && httpResponseMessage.StatusCode == HttpStatusCode.ServiceUnavailable)
					{
						// TODO: could also check the reason phrase, and / or the RESPONSE (which contains "Service Temporarily Unavailable")
						_logger.LogDebug("{Prefix} Service Unavailable. Waiting 10000ms", prefix);
						await Task.Delay(10000, cancellationToken).ConfigureAwait(false);
						continue;
					}
					// If a success code was not received, throw an exception
					var responseBody = await httpResponseMessage.Content.ReadAsStringAsync().ConfigureAwait(false);
					_logger.LogDebug("{Prefix} failed ({StatusCode}): {ResponseBody}",
						prefix,
						httpResponseMessage.StatusCode,
						responseBody);
					throw new LogicMonitorApiException(httpMethod, subUrl, httpResponseMessage.StatusCode, responseBody, $"{prefix} failed ({httpResponseMessage.StatusCode}): {responseBody}");
				}

				// We have been told to back off
				// Check that all rate limit headers are present
				var xRateLimitWindowLimitCount = await GetIntegerHeaderAsync(httpMethod, subUrl, httpResponseMessage.StatusCode, httpResponseMessage, "X-Rate-Limit-Limit").ConfigureAwait(false);
				var xRateLimitWindowRemainingCount = await GetIntegerHeaderAsync(httpMethod, subUrl, httpResponseMessage.StatusCode, httpResponseMessage, "X-Rate-Limit-Remaining").ConfigureAwait(false);
				var xRateLimitWindowSeconds = await GetIntegerHeaderAsync(httpMethod, subUrl, httpResponseMessage.StatusCode, httpResponseMessage, "X-Rate-Limit-Window").ConfigureAwait(false);

				var rateLimitInformation = $"Window: {xRateLimitWindowSeconds}s, Count: {xRateLimitWindowLimitCount}, Remaining {xRateLimitWindowRemainingCount}";
				// Wait for the full window
				var delayMs = 1000 * xRateLimitWindowSeconds;

				// Wait some time and try again
				_logger.LogDebug("{Prefix} Rate limiting hit (with cancellation token): {RateLimitInformation}, waiting {DelayMs:N0}ms",
					prefix,
					rateLimitInformation,
					delayMs);
				await Task.Delay(delayMs, cancellationToken).ConfigureAwait(false);

				// Try again
				continue;
			}

			// Success - can be cached
			break;
		}
		// Success - can cache

		// Create a PortalResponse
		var portalResponse = new PortalResponse<JObject>(httpResponseMessage);

		// Check the outer HTTP status code
		if (!portalResponse.IsSuccessStatusCode)
		{
			// If a success code was not received, throw an exception
			throw new LogicMonitorApiException(portalResponse.ErrorMessage) { HttpStatusCode = portalResponse.HttpStatusCode };
		}

		// Return
		return portalResponse.GetObject();
	}

	/// <summary>
	///     Patch specific fields
	/// </summary>
	/// <typeparam name="T"></typeparam>
	/// <param name="entity"></param>
	/// <param name="fieldsToUpdate">The fields to update</param>
	/// <param name="cancellationToken">The cancellation token</param>
	/// <exception cref="AggregateException"></exception>
	public virtual async Task PatchAsync<T>(T entity, Dictionary<string, object> fieldsToUpdate, CancellationToken cancellationToken) where T : IPatchable
	=> await PatchAsync($"{entity.Endpoint()}/{entity.Id}", fieldsToUpdate, cancellationToken).ConfigureAwait(false);

	private async Task PatchAsync(string subUrl, Dictionary<string, object> fieldsToUpdate, CancellationToken cancellationToken)
	{
		var prefix = GetPrefix(PatchHttpMethod);
		var jsonString = JsonConvert.SerializeObject(fieldsToUpdate);

		_logger.LogDebug("{Prefix}", prefix);
		HttpResponseMessage httpResponseMessage;
		while (true)
		{
			using (var content = new StringContent(jsonString, Encoding.UTF8, "application/json"))
			{
				var patchSpec = $"?patchFields={string.Join(",", fieldsToUpdate.Keys)}";
				using var requestMessage = new HttpRequestMessage(PatchHttpMethod, RequestUri(subUrl) + patchSpec) { Content = content };
				httpResponseMessage = await GetHttpResponseMessage(requestMessage, subUrl, jsonString, cancellationToken).ConfigureAwait(false);
			}

			_logger.LogDebug("{Prefix} complete", prefix);

			if (!httpResponseMessage.IsSuccessStatusCode)
			{
				if (WaitDuringLogicMonitorUpgrades && httpResponseMessage.StatusCode == HttpStatusCode.ServiceUnavailable)
				{
					// TODO: could also check the reason phrase, and / or the RESPONSE (which contains "Service Temporarily Unavailable")
					_logger.LogDebug("{Prefix} Service Unavailable. Waiting 10000ms", prefix);
					await Task.Delay(10000, cancellationToken).ConfigureAwait(false);
					continue;
				}
				// If a success code was not received, throw an exception
				var responseBody = await httpResponseMessage.Content.ReadAsStringAsync().ConfigureAwait(false);
				_logger.LogDebug("{Prefix} failed ({StatusCode}): {ResponseBody}",
					prefix,
					httpResponseMessage.StatusCode,
					responseBody);
				throw new LogicMonitorApiException(HttpMethod.Get, subUrl, httpResponseMessage.StatusCode, responseBody, $"{prefix} failed ({httpResponseMessage.StatusCode}): {responseBody}");
			}

			// Success, so finish
			break;
		}
	}

	/// <summary>
	///     Gets a page of items
	/// </summary>
	/// <typeparam name="T"></typeparam>
	/// <param name="filter"></param>
	/// <param name="subUrl"></param>
	/// <param name="cancellationToken"></param>
	/// <returns>A list of Collectors</returns>
	public Task<Page<T>> GetPageAsync<T>(Filter<T> filter, string subUrl, CancellationToken cancellationToken) where T : new()
		=> GetPageInternalAsync(filter, subUrl, cancellationToken);

	/// <summary>
	///     Gets a page of items
	/// </summary>
	/// <typeparam name="T"></typeparam>
	/// <param name="filter"></param>
	/// <param name="cancellationToken"></param>
	/// <returns>A list of Collectors</returns>
	public Task<Page<T>> GetPageAsync<T>(Filter<T> filter, CancellationToken cancellationToken) where T : IHasEndpoint, new()
		=> GetPageInternalAsync(filter, new T().Endpoint(), cancellationToken);

	/// <summary>
	///     Gets a page of items
	/// </summary>
	/// <typeparam name="T"></typeparam>
	/// <param name="filter"></param>
	/// <param name="subUrl"></param>
	/// <param name="cancellationToken"></param>
	/// <returns>A list of Collectors</returns>
	private Task<Page<T>> GetPageInternalAsync<T>(Filter<T> filter, string subUrl, CancellationToken cancellationToken) where T : new()
		=> GetBySubUrlAsync<Page<T>>(subUrl.Contains('?')
			? $"{subUrl}&{filter}"
			: $"{subUrl}?{filter}"
			, cancellationToken);
	#endregion Web Interaction

	#region Debug Commands

	/// <summary>
	///     Starts the execution of a debug command
	/// </summary>
	/// <param name="collectorId">The ID of the collector on which to execute the command</param>
	/// <param name="commandText">The command to execute</param>
	/// <param name="cancellationToken"></param>
	/// <returns>The ExecuteDebugCommandResponse containing the sessionId</returns>
	public Task<ExecuteDebugCommandResponse> ExecuteDebugCommandAsync(int collectorId, string commandText, CancellationToken cancellationToken)
	=> PostAsync<ExecuteDebugCommandRequest, ExecuteDebugCommandResponse>(new ExecuteDebugCommandRequest { Command = commandText }, $"debug?collectorId={collectorId}", cancellationToken);

	/// <summary>
	/// Get the result of a Collector debug command using sessionId
	/// </summary>
	/// <param name="collectorId">The ID of the collector on which the command was executed</param>
	/// <param name="sessionId">The request ID from the ExecuteDebugCommandResponse</param>
	/// <param name="cancellationToken">The optional cancellation token</param>
	public Task<ExecuteDebugCommandResponse> GetDebugCommandResultAsync(int collectorId, int sessionId, CancellationToken cancellationToken)
	=> GetBySubUrlAsync<ExecuteDebugCommandResponse>($"debug/{sessionId}?collectorId={collectorId}&_={DateTime.UtcNow.Ticks}", cancellationToken);

	/// <summary>
	///     Waits for a Debug command response
	/// </summary>
	/// <param name="collectorId">The ID of the collector on which the command was executed</param>
	/// <param name="commandText">The command to execute</param>
	/// <param name="timeoutMs">The maximum amount of time to wait (default 10000 ms)</param>
	/// <param name="sleepIntervalMs">The sleep interval between attempts to retrieve the response (default 500ms)</param>
	/// <param name="cancellationToken">The cancellation token</param>
	public async Task<ExecuteDebugCommandResponse> ExecuteDebugCommandAndWaitForResultAsync(
		int collectorId,
		string commandText,
		int timeoutMs = 10000,
		int sleepIntervalMs = 500,
		CancellationToken cancellationToken = default)
	{
		var executeDebugCommandResponse = await ExecuteDebugCommandAsync(collectorId, commandText, cancellationToken).ConfigureAwait(false);

		var stopwatch = Stopwatch.StartNew();
		ExecuteDebugCommandResponse debugCommandResult = null;
		while (stopwatch.ElapsedMilliseconds < timeoutMs)
		{
			try
			{
				debugCommandResult = await GetDebugCommandResultAsync(collectorId, executeDebugCommandResponse.SessionId, cancellationToken).ConfigureAwait(false);
			}
			catch
			{
				// ignored
			}

			// Do we have a response?
			if (!string.IsNullOrWhiteSpace(debugCommandResult?.Output))
			{
				break;
			}

			await Task.Delay(sleepIntervalMs, cancellationToken).ConfigureAwait(false);
		}

		return debugCommandResult;
	}

	#endregion Debug Commands

	private async Task SetCustomPropertyAsync(
		int id,
		string name,
		string value,
		SetPropertyMode mode,
		string subUrl,
		CancellationToken cancellationToken)
	{
		var propertiesSubUrl = $"{subUrl}/{id}/properties";
		switch (mode)
		{
			case SetPropertyMode.Automatic:
				// Determine whether there is an existing property
				try
				{
					var _ = await GetBySubUrlAsync<Property>($"{propertiesSubUrl}/{name}", cancellationToken).ConfigureAwait(false);

					// No exception thrown? It exists
					// Are we deleting?
					if (value is null)
					{
						// Yes.
						await DeleteAsync($"{propertiesSubUrl}/{name}", cancellationToken).ConfigureAwait(false);
					}
					else
					{
						// No
						// PUT the replacement value
						await PutAsync($"{propertiesSubUrl}/{name}", new { value }, cancellationToken).ConfigureAwait(false);
					}
				}
				catch (Exception)
				{
					// It doesn't exist
					// so POST a new one (unless it's null, in which case nothing to do)
					if (value is not null)
					{
						var _ = await PostAsync<Property, Property>(
							new Property { Name = name, Value = value },
							$"{propertiesSubUrl}",
							cancellationToken).ConfigureAwait(false);
					}
				}

				break;

			case SetPropertyMode.Create:
				if (value is null)
				{
					throw new InvalidOperationException("Value must not be set to null when creating the property.");
				}

				await PostAsync<Property, Property>(new Property { Name = name, Value = value }, $"{propertiesSubUrl}", cancellationToken).ConfigureAwait(false);
				break;

			case SetPropertyMode.Update:
				if (value is null)
				{
					throw new InvalidOperationException("Value must not be set to null when updating the property.");
				}
				// PUT the replacement value
				await PutAsync($"{propertiesSubUrl}/{name}", new { value }, cancellationToken).ConfigureAwait(false);
				break;

			case SetPropertyMode.Delete:
				if (value is not null)
				{
					throw new InvalidOperationException("Value must be set to null when deleting the value.");
				}
				// Delete the value
				await DeleteAsync($"{propertiesSubUrl}/{name}", cancellationToken).ConfigureAwait(false);
				break;

			default:
				throw new ArgumentOutOfRangeException(nameof(mode), mode, null);
		}
	}

	/// <summary>
	/// Returns true if the specified property on the class has the SantabaReadOnly attribute defined
	/// </summary>
	/// <param name="name">The name to match</param>
	/// <param name="logicMonitorClassType">The type of class to query</param>
	/// <param name="tryJsonNameFirst">If true, will use the DataMember/JSON property name to match before using property name</param>
	/// <returns>True if the property is read only</returns>
	public static bool IsPropertyReadOnly(string name, Type logicMonitorClassType, bool tryJsonNameFirst = false)
	{
		PropertyInfo propertyInfo;
		if (tryJsonNameFirst)
		{
			// Try and find a property which matches the DataMemberAttributes name if it's set
			propertyInfo = logicMonitorClassType.GetProperties()
				.SingleOrDefault(p =>
					p.GetCustomAttributes(typeof(DataMemberAttribute), false)
					.Cast<DataMemberAttribute>()
					.SingleOrDefault(entity => entity.IsNameSetExplicitly)?
					.Name == name)
				// Also try and get the property by it's normal name
				?? logicMonitorClassType.GetProperty(name);
		}
		else
		{
			// Just a simple GetProperty
			propertyInfo = logicMonitorClassType.GetProperty(name);
		}

		if (propertyInfo is null)
		{
			throw new PropertyNotFoundException($"Could not find property on {logicMonitorClassType.Name} with name {name}.");
		}

		return Attribute.IsDefined(propertyInfo, typeof(SantabaReadOnly));
	}

	/// <summary>
	/// Clone an ICloneableItem
	/// </summary>
	/// <typeparam name="T">The type of the item being cloned</typeparam>
	/// <param name="id">The id of the item being cloned</param>
	/// <param name="cloneRequest">The clone request</param>
	/// <param name="cancellationToken">An optional CancellationToken</param>
	public Task<T> CloneAsync<T>(int id, CloneRequest<T> cloneRequest, CancellationToken cancellationToken) where T : IHasEndpoint, ICloneableItem, new()
		=> PostAsync<CloneRequest<T>, T>(cloneRequest, $"{new T().Endpoint()}/{id}/clone", cancellationToken);
}
