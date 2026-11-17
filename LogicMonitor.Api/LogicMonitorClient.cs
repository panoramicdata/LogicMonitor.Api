using System.Security.Cryptography;

namespace LogicMonitor.Api;

/// <summary>
///     The main client for querying the portal.
/// </summary>
public partial class LogicMonitorClient : IDisposable
{
	#region Fields

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

	#endregion Properties

	#region Constructor / Dispose

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

	#endregion Constructor / Dispose

	#region Web Interaction

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

			var expectedTotal = GetExpectedPagedTotal(page?.TotalCount ?? 0, requestedTake);
			if (IsLastPage<T>(pageItems.Count, list.Count, expectedTotal))
			{
				// Do strict checking if required
				if (StrictPagingTotalChecking && expectedTotal != list.Count)
				{
					throw new PagingException($"Mismatch between expected total: {expectedTotal} and received count: {list.Count}");
				}

				return list;
			}

			// Increment the skip value
			filter.Skip += filter.Take;
		}
	}

	// Some endpoints return a negative total count
	private static int GetExpectedPagedTotal(int pageTotalCount, int requestedTake)
		=> Math.Min(pageTotalCount < 0 ? int.MaxValue : pageTotalCount, requestedTake);

	// A page that is empty, one that completes the total, and OpsNoteTags (which do not page) all end the loop.
	private static bool IsLastPage<T>(int pageItemCount, int totalItemCount, int expectedTotal)
		=> pageItemCount == 0
			|| totalItemCount >= expectedTotal
			|| typeof(T) == typeof(OpsNoteTag);

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
	public virtual Task<T> GetAsync<T>(int id, Filter<T>? filter, CancellationToken cancellationToken) where T : IdentifiedItem, IHasEndpoint, new()
		=> GetBySubUrlAsync<T>($"{new T().Endpoint()}/{id}?{filter}", cancellationToken);

	/// <summary>
	///    Gets something by its name
	/// </summary>
	/// <typeparam name="T"></typeparam>
	/// <param name="name"></param>
	/// <param name="cancellationToken"></param>
	public virtual async Task<T?> GetByNameAsync<T>(string name, CancellationToken cancellationToken)
	where T : NamedItem, IHasEndpoint, new()
	{
		ArgumentNullException.ThrowIfNull(name);

		var items = await GetAllAsync(new Filter<T>
		{
			FilterItems =
				[
					new Eq<T>(nameof(NamedEntity.Name), name)
				]
		}, cancellationToken: cancellationToken).ConfigureAwait(false);

		// If only one, return that.
		return items.Count switch
		{
			0 => null,
			1 => items[0],
			_ => throw new InvalidOperationException($"An unexpected number of {typeof(T).Name} have name: {name}"),
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
	/// <param name="item"></param>
	/// <param name="cancellationToken"></param>
	public virtual Task<T> ExecuteAsync<T>(T @item, CancellationToken cancellationToken) where T : IdentifiedItem, IExecutable, new()
	=> GetBySubUrlAsync<T>($"{@item.Endpoint()}/{@item.Id}/executenow", cancellationToken);

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
	public static string GetSerializationName<T>(string propertyName)
	{
		var propertyInfos = typeof(T).GetProperties();
		var property = propertyInfos.SingleOrDefault(p => p.Name == propertyName)
			?? throw new ArgumentOutOfRangeException(nameof(propertyName), $"{propertyName} is not a property of {typeof(T).Name}.");

		// Use reflection to find the DataMember Name
		return property.GetCustomAttributes(typeof(DataMemberAttribute), true).Cast<DataMemberAttribute>().SingleOrDefault()!.Name!;
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
		var field = fieldInfos.SingleOrDefault(f => f.Name == enumObject.ToString())
			?? throw new ArgumentOutOfRangeException(nameof(enumObject), $"{@enumObject} is not a member of enum {type.Name}.");

		// Use reflection to find the DataMember Name
		var list = field.GetCustomAttributes(typeof(EnumMemberAttribute), true).Cast<EnumMemberAttribute>().ToList();
		return list.SingleOrDefault()!.Value!;
	}

	/// <summary>
	///     Update an identified item
	/// </summary>
	/// <param name="item">The object to update</param>
	/// <typeparam name="T"></typeparam>
	/// <param name="cancellationToken">The cancellation token</param>
	/// <exception cref="AggregateException"></exception>
	public virtual Task PutAsync<T>(T @item, CancellationToken cancellationToken) where T : IdentifiedItem, IHasEndpoint
		// The ignoreReference permits forcing appliesTo functions and is ignored for other types
		=> PutAsync($"{@item.Endpoint()}/{@item.Id}?ignoreReference=true", @item, cancellationToken);

	/// <summary>
	///     Update a string identified item
	/// </summary>
	/// <param name="item">The object to update</param>
	/// <typeparam name="T"></typeparam>
	/// <param name="cancellationToken">The cancellation token</param>
	/// <exception cref="AggregateException"></exception>
	public virtual Task PutStringIdentifiedItemAsync<T>(T @item, CancellationToken cancellationToken) where T : StringIdentifiedItem, IHasEndpoint
		// The ignoreReference permits forcing appliesTo functions and is ignored for other types
		=> PutAsync($"{@item.Endpoint()}/{@item.Id}?ignoreReference=true", @item, cancellationToken);

	/// <summary>
	/// Update an item
	/// </summary>
	/// <param name="subUrl">The subURL</param>
	/// <param name="item">The updated object</param>
	/// <param name="cancellationToken">An optional CancellationToken</param>
	public async Task PutAsync(string subUrl, object @item, CancellationToken cancellationToken)
	{
		var httpMethod = HttpMethod.Put;
		var prefix = GetPrefix(httpMethod);
		_logger.LogDebug("{Prefix} {SubUrl}", prefix, subUrl);

		var jsonString = JsonConvert.SerializeObject(@item);
		_logger.LogTrace("{Prefix} jsonString:\r\n{JsonString}", prefix, jsonString);

		var httpResponseMessage = await SendWithRetryAsync(httpMethod, subUrl, jsonString, prefix, false, cancellationToken).ConfigureAwait(false);

		var portalResponse = new PortalResponse<EmptyResponse>(httpResponseMessage);
		if (!portalResponse.IsSuccessStatusCode)
		{
			throw new LogicMonitorApiException(portalResponse.ErrorMessage) { HttpStatusCode = portalResponse.HttpStatusCode };
		}
	}

	/// <summary>
	///     Deletes an item
	/// </summary>
	/// <param name="item">The object to delete</param>
	/// <param name="hardDelete">Whether to hard delete.</param>
	/// <param name="cancellationToken">The optional cancellation token</param>
	/// <typeparam name="T">The type of the item to delete</typeparam>
	public virtual Task DeleteAsync<T>(
		T @item,
		bool hardDelete,
		CancellationToken cancellationToken)
		where T : IdentifiedItem, IHasEndpoint, new()
		=> DeleteAsync($"{new T().Endpoint()}/{@item.Id}{(!hardDelete ? "?deleteHard=false" : string.Empty)}", cancellationToken);

	/// <summary>
	///     Hard deletes an item
	/// </summary>
	/// <param name="item">The object to delete</param>
	/// <param name="cancellationToken">The optional cancellation token</param>
	/// <typeparam name="T">The type of the item to delete</typeparam>
	public virtual Task DeleteAsync<T>(
		T @item,
		CancellationToken cancellationToken)
		where T : IdentifiedItem, IHasEndpoint, new()
		=> DeleteAsync($"{new T().Endpoint()}/{@item.Id}", cancellationToken);

	/// <summary>
	///     Deletes an item
	/// </summary>
	/// <param name="item">The object to delete</param>
	/// <param name="hardDelete">Whether to hard delete.</param>
	/// <param name="cancellationToken">The optional cancellation token</param>
	/// <typeparam name="T">The type of the item to delete</typeparam>
	public virtual Task DeleteStringIdentifiedAsync<T>(
		T @item,
		bool hardDelete,
		CancellationToken cancellationToken
	) where T : StringIdentifiedItem, IHasEndpoint, new()
		=> DeleteAsync(
			$"{new T().Endpoint()}/{@item.Id}{(!hardDelete ? "?deleteHard=false" : string.Empty)}",
			cancellationToken
		);

	/// <summary>
	///     Hard deletes an item
	/// </summary>
	/// <param name="item">The object to delete</param>
	/// <param name="cancellationToken">The optional cancellation token</param>
	/// <typeparam name="T">The type of the item to delete</typeparam>
	public virtual Task DeleteStringIdentifiedAsync<T>(
		T @item,
		CancellationToken cancellationToken
	) where T : StringIdentifiedItem, IHasEndpoint, new()
		=> DeleteAsync(
			$"{new T().Endpoint()}/{@item.Id}",
			cancellationToken
		);

	/// <summary>
	///     Deletes an item by id
	/// </summary>
	/// <param name="id">The item id</param>
	/// <param name="hardDelete">Whether to hard delete.</param>
	/// <param name="cancellationToken">The optional cancellation token</param>
	/// <typeparam name="T">The type of the item to delete</typeparam>
	public virtual Task DeleteAsync<T>(
		int id,
		bool hardDelete,
		CancellationToken cancellationToken
	) where T : IdentifiedItem, IHasEndpoint, new()
		=> DeleteAsync($"{new T().Endpoint()}/{id}{(!hardDelete ? "?deleteHard=false" : string.Empty)}", cancellationToken);

	/// <summary>
	///     Hard deletes an item by integer id
	/// </summary>
	/// <param name="id">The item id</param>
	/// <param name="cancellationToken">The optional cancellation token</param>
	/// <typeparam name="T">The type of the item to delete</typeparam>
	public virtual Task DeleteAsync<T>(
		int id,
		CancellationToken cancellationToken
	) where T : IdentifiedItem, IHasEndpoint, new()
		=> DeleteAsync($"{new T().Endpoint()}/{id}", cancellationToken);

	/// <summary>
	///     Deletes an item by string id
	/// </summary>
	/// <param name="id">The item id</param>
	/// <param name="hardDelete">Whether to hard delete.</param>
	/// <param name="cancellationToken">The optional cancellation token</param>
	/// <typeparam name="T">The type of the item to delete</typeparam>
	public virtual Task DeleteAsync<T>(
		string id,
		bool hardDelete,
		CancellationToken cancellationToken
	) where T : StringIdentifiedItem, IHasEndpoint, new()
		=> DeleteAsync(
			$"{new T().Endpoint()}/{id}{(!hardDelete ? "?deleteHard=false" : string.Empty)}",
			cancellationToken);


	/// <summary>
	///     Hard deletes an item by string id
	/// </summary>
	/// <param name="id">The item id</param>
	/// <param name="cancellationToken">The optional cancellation token</param>
	/// <typeparam name="T">The type of the item to delete</typeparam>
	public virtual Task DeleteAsync<T>(
		 string id,
		 CancellationToken cancellationToken
	) where T : StringIdentifiedItem, IHasEndpoint, new()
		=> DeleteAsync($"{new T().Endpoint()}/{id}", cancellationToken);

	/// <summary>
	///     Deletes a Resource DataSource Instance
	/// </summary>
	/// <param name="resourceDataSourceInstance">The resourceDataSourceInstance to delete</param>
	/// <param name="hardDelete">Whether to hard delete.</param>
	/// <param name="cancellationToken">The optional cancellation token</param>
	public virtual Task DeleteAsync(
		ResourceDataSourceInstance resourceDataSourceInstance,
		bool hardDelete,
		CancellationToken cancellationToken)
		=> DeleteAsync(
			$"device/devices/{resourceDataSourceInstance.ResourceId}/devicedatasources/{resourceDataSourceInstance.ResourceDataSourceId}/instances/{resourceDataSourceInstance.Id}{(!hardDelete ? "?deleteHard=false" : string.Empty)}",
			cancellationToken);

	/// <summary>
	///     Hard deletes a ResourceDataSourceInstance
	/// </summary>
	/// <param name="resourceDataSourceInstance">The resourceDataSourceInstance to delete</param>
	/// <param name="cancellationToken">The optional cancellation token</param>
	public virtual Task DeleteAsync(
		ResourceDataSourceInstance resourceDataSourceInstance,
		CancellationToken cancellationToken)
		=> DeleteAsync(
			$"device/devices/{resourceDataSourceInstance.ResourceId}/devicedatasources/{resourceDataSourceInstance.ResourceDataSourceId}/instances/{resourceDataSourceInstance.Id}",
			cancellationToken);

	/// <summary>
	///     Create an item
	/// </summary>
	/// <param name="creationDto"></param>
	/// <param name="cancellationToken">The cancellation token</param>
	/// <typeparam name="T"></typeparam>
	public virtual Task<T> CreateAsync<T>(CreationDto<T> creationDto, CancellationToken cancellationToken) where T : IHasEndpoint, new()
	{
		var instance = new T();
		var endpoint = instance is IHasCreationEndpoint hasCreationEndpoint
			? hasCreationEndpoint.CreationEndpoint()
			: instance.Endpoint();
		// Uptime checks are provisioned entirely by the v3 creation body (see UptimeResourceWireMapper): the
		// structured payload is what makes the server register the device as an Uptime resource and apply the
		// Ping_Check_*/Web_Check_* DataSources. No post-create property patching is required.
		return PostAsync<CreationDto<T>, T>(creationDto, endpoint, cancellationToken);
	}

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

	private string RequestUri(string subUrl) => subUrl is "log/ingest" or "metric/ingest"
		? $"https://{AccountName}.logicmonitor.com/rest/{subUrl}"
		: $"https://{AccountName}.logicmonitor.com/santaba/rest/{subUrl}";

	private async Task<HttpResponseMessage> GetHttpResponseMessage(HttpRequestMessage requestMessage, string subUrl, string? data, CancellationToken cancellationToken)
	{
		ReadOnlyGuard.ThrowIfWrite(_options, requestMessage.Method, subUrl);

		var epoch = (long)(DateTime.UtcNow - new DateTime(1970, 1, 1)).TotalMilliseconds;
		var subUrl2 = subUrl.Contains('?')
			? subUrl.Substring(0, subUrl.IndexOf('?'))
			: subUrl;
		var httpVerb = requestMessage.Method.ToString().ToUpperInvariant();
		var resourcePath = $"/{subUrl2}";
		data ??= string.Empty;

		// Auth header
		var authHeaderValue =
			_accessKey.StartsWith("lmb_", StringComparison.Ordinal)
				? $"Bearer {_accessKey}"
				: $"LMv1 {_accessId}:{GetSignature(httpVerb, epoch, data, resourcePath, _accessKey)}:{epoch}";

		requestMessage.Headers.Add("Authorization", authHeaderValue);

		_logger.LogHttpHeaders(true, "", requestMessage.Headers);
		var response = await _client
			.SendAsync(requestMessage, cancellationToken)
			.ConfigureAwait(false);

		UpdateSummary(requestMessage, response);

		return response;
	}

	private static readonly Dictionary<string, Action<Statistics>> _methodCounters = new(StringComparer.Ordinal)
	{
		["DELETE"] = s => s.ApiCallDeleteCount++,
		["GET"] = s => s.ApiCallGetCount++,
		["POST"] = s => s.ApiCallPostCount++,
		["TRACE"] = s => s.ApiCallTraceCount++,
		["HEAD"] = s => s.ApiCallHeadCount++,
		["PUT"] = s => s.ApiCallPutCount++,
		["OPTIONS"] = s => s.ApiCallOptionsCount++,
		["PATCH"] = s => s.ApiCallPatchCount++
	};

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

		if (_methodCounters.TryGetValue(requestMessage.Method.ToString(), out var count))
		{
			count(Statistics);
		}
		else
		{
			Statistics.ApiCallOtherCount++;
		}

		Statistics.DataTransferUplinkBytes += requestMessage.Content?.Headers.ContentLength ?? 0;
		Statistics.DataTransferDownlinkBytes += response.Content?.Headers.ContentLength ?? 0;
	}

	private Task<Page<T>> FilteredGetAsync<T>(string subUrl, Filter<T>? filter, CancellationToken cancellationToken) where T : new()
		=> GetAsync<Page<T>>(UseCache, $"{subUrl}?{filter}", cancellationToken);


	/// <summary>
	///     Gets a filtered page of items
	/// </summary>
	/// <param name="filter">The filter</param>
	/// <param name="cancellationToken">An optional cancellation token</param>
	/// <typeparam name="T">The item type</typeparam>
	/// <returns>The filtered list</returns>
	public virtual Task<Page<T>> GetAsync<T>(Filter<T>? filter, CancellationToken cancellationToken) where T : IHasEndpoint, new()
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
		ArgumentNullException.ThrowIfNull(subUrl);

		var httpMethod = HttpMethod.Delete;
		var prefix = GetPrefix(httpMethod);
		_logger.LogDebug("{Prefix} {SubUrl}", prefix, subUrl);

		var httpResponseMessage = await SendWithRetryAsync(httpMethod, subUrl, null, prefix, true, cancellationToken).ConfigureAwait(false);

		var jsonString = await httpResponseMessage.Content.ReadAsStringAsync(cancellationToken).ConfigureAwait(false);
		if (jsonString.Contains("group is not empty"))
		{
			_logger.LogDebug("{Prefix} failed - group is not empty for suburl", prefix);
			throw new LogicMonitorApiException(httpMethod, subUrl, HttpStatusCode.PreconditionFailed, jsonString, $"{prefix} failed - group is not empty for suburl");
		}
	}

	private static string GetPrefix(HttpMethod method) => $"{Guid.NewGuid()}: {method}";

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
			if (cacheObject is not null)
			{
				return (T)cacheObject;
			}
		}

		var httpResponseMessage = await SendGetWithRetryAsync(subUrl, prefix, stopwatch, cancellationToken).ConfigureAwait(false);

		// File and raw responses are returned as they are, and never cached
		var rawResponse = await ReadRawResponseAsync<T>(httpResponseMessage, cancellationToken).ConfigureAwait(false);
		if (rawResponse is not null)
		{
			return rawResponse;
		}

		var deserializedObject = await ReadPortalResponseAsync<T>(httpResponseMessage, subUrl, prefix, cancellationToken).ConfigureAwait(false);

		// Cache the result
		if (useCache && deserializedObject is not null)
		{
			_cache.AddOrUpdate(subUrl, deserializedObject);
		}

		return deserializedObject ?? new();
	}

	private async Task<T?> ReadRawResponseAsync<T>(HttpResponseMessage httpResponseMessage, CancellationToken cancellationToken) where T : class
	{
		if (typeof(T).Name == nameof(XmlResponse))
		{
			var content = await httpResponseMessage.Content.ReadAsStringAsync(cancellationToken).ConfigureAwait(false);
			_logger.LogTrace("RESPONSE:\r\n\r\n {Content}", content);
			return new XmlResponse { Content = content } as T
				?? throw new FormatException($"Could not convert content to {typeof(T).Name}");
		}

		if (typeof(T) == typeof(List<byte>))
		{
			var content = await httpResponseMessage.Content.ReadAsByteArrayAsync(cancellationToken).ConfigureAwait(false);
			_logger.LogTrace("Byte Array");
			return content.ToList() as T
				?? throw new FormatException($"Could not convert content to {typeof(T).Name}");
		}

		if (typeof(T) == typeof(DownloadFileInfo))
		{
			var tempFileInfo = new FileInfo(Path.GetTempFileName());
			using Stream output = File.OpenWrite(tempFileInfo.FullName);
			using var input = await httpResponseMessage.Content.ReadAsStreamAsync(cancellationToken).ConfigureAwait(false);
			await input.CopyToAsync(output, cancellationToken).ConfigureAwait(false);

			return new DownloadFileInfo
			{
				FileInfo = tempFileInfo
			} as T
			?? throw new FormatException($"Could not convert content to {typeof(T).Name}");
		}

		return null;
	}

	private async Task<T?> ReadPortalResponseAsync<T>(HttpResponseMessage httpResponseMessage, string subUrl, string prefix, CancellationToken cancellationToken) where T : class, new()
	{
		var portalResponse = new PortalResponse<T>(httpResponseMessage);

		if (!portalResponse.IsSuccessStatusCode)
		{
			var responseBody = await httpResponseMessage.Content.ReadAsStringAsync(cancellationToken).ConfigureAwait(false);
			_logger.LogHttpHeaders(false, prefix, httpResponseMessage.Headers);
			_logger.LogDebug("{Prefix} failed ({StatusCode}): {ResponseBody}",
				prefix,
				httpResponseMessage.StatusCode,
				responseBody);
			_logger.LogTrace("RESPONSE:\r\n\r\n{ResponseBody}", responseBody);
			throw new LogicMonitorApiException(HttpMethod.Get, subUrl, portalResponse.HttpStatusCode, responseBody, $"{prefix} failed ({httpResponseMessage.StatusCode}): {responseBody}");
		}

		_logger.LogTrace("RESPONSE:\r\n\r\n{Data}:", portalResponse.Data?.ToString());
		try
		{
			return portalResponse.GetObject(_jsonConverters);
		}
		catch (DeserializationException e)
		{
			_logger.LogError(e, "{Prefix} Unable to deserialize\n{ResponseBody}\n{Message}",
				prefix,
				e.ResponseBody,
				e.Message);
			throw;
		}
	}

	/// <summary>
	/// Post an item
	/// </summary>
	/// <typeparam name="TIn">The posted object type</typeparam>
	/// <typeparam name="TOut">The returned object type</typeparam>
	/// <param name="postedObject">The posted object </param>
	/// <param name="subUrl">The endpoint</param>
	/// <param name="cancellationToken">An optional CancellationToken</param>
	public async Task<TOut> PostAsync<TIn, TOut>(TIn postedObject, string subUrl, CancellationToken cancellationToken) where TOut : new()
	{
		ArgumentNullException.ThrowIfNull(subUrl);

		var httpMethod = HttpMethod.Post;
		var prefix = GetPrefix(httpMethod);
		_logger.LogDebug("{Prefix} {SubUrl}", prefix, subUrl);

		// LMREP-1042: "d:\"EBSDB [prod24778]\" does not work, however "d:\"EBSDB *prod24778*\" matches. Unrelated to URl encoding, etc...
		var data = JsonConvert.SerializeObject(postedObject, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });
		_logger.LogTrace("{Prefix} body:\r\n{Data}", prefix, data);

		var httpResponseMessage = await SendWithRetryAsync(httpMethod, subUrl, data, prefix, false, cancellationToken).ConfigureAwait(false);

		var portalResponse = new PortalResponse<TOut>(httpResponseMessage);
		if (!portalResponse.IsSuccessStatusCode)
		{
			throw new LogicMonitorApiException(portalResponse.ErrorMessage) { HttpStatusCode = portalResponse.HttpStatusCode };
		}

		var deserializedObject = portalResponse.GetObject();
		return deserializedObject != null ? deserializedObject : new();
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
		CancellationToken cancellationToken)
	{
		ArgumentNullException.ThrowIfNull(subUrl);

		var httpMethod = HttpMethod.Post;
		var prefix = GetPrefix(httpMethod);
		_logger.LogDebug("{Prefix} {SubUrl}", prefix, subUrl);

		var data = jObject.ToString();
		_logger.LogTrace("{Prefix} body:\r\n{Data}", prefix, data);

		var httpResponseMessage = await SendWithRetryAsync(httpMethod, subUrl, data, prefix, false, cancellationToken).ConfigureAwait(false);

		var portalResponse = new PortalResponse<JObject>(httpResponseMessage);
		if (!portalResponse.IsSuccessStatusCode)
		{
			throw new LogicMonitorApiException(portalResponse.ErrorMessage) { HttpStatusCode = portalResponse.HttpStatusCode };
		}

		return portalResponse.GetObject();
	}

	private sealed class RetryState
	{
		public int FailureCount { get; set; }

		public int RateLimitAttempt { get; set; }

		public long RateLimitTotalMs { get; set; }
	}

	private async Task<HttpResponseMessage> SendOnceAsync(HttpMethod httpMethod, string subUrl, string? data, CancellationToken cancellationToken)
	{
		using var content = data is null ? null : new StringContent(data, Encoding.UTF8, "application/json");
		using var requestMessage = new HttpRequestMessage(httpMethod, RequestUri(subUrl)) { Content = content };
		return await GetHttpResponseMessage(requestMessage, subUrl, data, cancellationToken).ConfigureAwait(false);
	}

	private async Task<HttpResponseMessage> SendWithRetryAsync(
		HttpMethod httpMethod,
		string subUrl,
		string? data,
		string prefix,
		bool retryTransportFailures,
		CancellationToken cancellationToken)
	{
		var state = new RetryState();
		while (true)
		{
			HttpResponseMessage httpResponseMessage;
			try
			{
				httpResponseMessage = await SendOnceAsync(httpMethod, subUrl, data, cancellationToken).ConfigureAwait(false);
			}
			catch (Exception e) when (retryTransportFailures && LogTransportFailure(e, prefix, state))
			{
				continue;
			}

			_logger.LogDebug("{Prefix} complete", prefix);
			if (httpResponseMessage.IsSuccessStatusCode)
			{
				return httpResponseMessage;
			}

			await WaitBeforeRetryOrThrowAsync(httpMethod, subUrl, httpResponseMessage, prefix, state, false, cancellationToken).ConfigureAwait(false);
		}
	}

	private async Task<HttpResponseMessage> SendGetWithRetryAsync(string subUrl, string prefix, Stopwatch stopwatch, CancellationToken cancellationToken)
	{
		var state = new RetryState();
		while (true)
		{
			HttpResponseMessage httpResponseMessage;
			try
			{
				httpResponseMessage = await SendOnceAsync(HttpMethod.Get, subUrl, null, cancellationToken).ConfigureAwait(false);
				_logger.LogDebug("{Prefix} complete (from remote: {ElapsedMilliseconds:N0}ms)",
					prefix,
					stopwatch.ElapsedMilliseconds);
			}
			catch (Exception e) when (LogTransportFailure(e, prefix, state))
			{
				await Task.Delay(1000, cancellationToken).ConfigureAwait(false);
				continue;
			}

			if (httpResponseMessage.IsSuccessStatusCode)
			{
				return httpResponseMessage;
			}

			await WaitBeforeRetryOrThrowAsync(HttpMethod.Get, subUrl, httpResponseMessage, prefix, state, true, cancellationToken).ConfigureAwait(false);
		}
	}

	// Returns true when the caller should retry; false lets the original exception propagate.
	private bool LogTransportFailure(Exception e, string prefix, RetryState state)
	{
		_logger.LogDebug(e, "{Prefix} failed on attempt {FailureCount}: {StackTrace}",
			prefix,
			++state.FailureCount,
			e.ToString());
		if (state.FailureCount < AttemptCount)
		{
			_logger.LogDebug("{Prefix} Retrying..", prefix);
			return true;
		}

		_logger.LogDebug("{Prefix} Giving up.", prefix);
		return false;
	}

	// Returns when the request should be retried; throws when it should not.
	private async Task WaitBeforeRetryOrThrowAsync(
		HttpMethod httpMethod,
		string subUrl,
		HttpResponseMessage httpResponseMessage,
		string prefix,
		RetryState state,
		bool retryServerErrors,
		CancellationToken cancellationToken)
	{
		var statusCode = (int)httpResponseMessage.StatusCode;
		if (statusCode == 429)
		{
			await BackOffAsync(httpMethod, subUrl, httpResponseMessage, prefix, state, cancellationToken).ConfigureAwait(false);
			return;
		}

		if (WaitDuringLogicMonitorUpgrades && httpResponseMessage.StatusCode == HttpStatusCode.ServiceUnavailable)
		{
			_logger.LogDebug("{Prefix} Service Unavailable. Waiting 10000ms", prefix);
			await Task.Delay(10000, cancellationToken).ConfigureAwait(false);
			return;
		}

		var responseBody = await httpResponseMessage.Content.ReadAsStringAsync(cancellationToken).ConfigureAwait(false);

		// HTTP 400 - 499 are never retried
		if (retryServerErrors && statusCode is not (>= 400 and < 500))
		{
			_logger.LogHttpHeaders(false, prefix, httpResponseMessage.Headers);
			_logger.LogDebug("{Prefix} failed on attempt {FailureCount}: {ResponseBody}", prefix, ++state.FailureCount, responseBody);

			if (state.FailureCount < AttemptCount)
			{
				_logger.LogDebug("{Prefix} Retrying..", prefix);
				await Task.Delay(1000, cancellationToken).ConfigureAwait(false);
				return;
			}
		}

		_logger.LogDebug("{Prefix} failed ({StatusCode}): {ResponseBody}", prefix, httpResponseMessage.StatusCode, responseBody);
		throw new LogicMonitorApiException(httpMethod, subUrl, httpResponseMessage.StatusCode, responseBody, $"{prefix} failed ({httpResponseMessage.StatusCode}): {responseBody}");
	}

	// We have been told to back off: wait for the rate limit window, up to the configured maximum.
	private async Task BackOffAsync(
		HttpMethod httpMethod,
		string subUrl,
		HttpResponseMessage httpResponseMessage,
		string prefix,
		RetryState state,
		CancellationToken cancellationToken)
	{
		var limitCount = await GetIntegerHeaderAsync(httpMethod, subUrl, httpResponseMessage.StatusCode, httpResponseMessage, "X-Rate-Limit-Limit").ConfigureAwait(false);
		var remainingCount = await GetIntegerHeaderAsync(httpMethod, subUrl, httpResponseMessage.StatusCode, httpResponseMessage, "X-Rate-Limit-Remaining").ConfigureAwait(false);
		var windowSeconds = await GetIntegerHeaderAsync(httpMethod, subUrl, httpResponseMessage.StatusCode, httpResponseMessage, "X-Rate-Limit-Window").ConfigureAwait(false);

		var rateLimitInformation = $"Window: {windowSeconds}s, Count: {limitCount}, Remaining {remainingCount}";
		var delayMs = 1000 * windowSeconds;
		if (windowSeconds > MaximumBackOffSeconds)
		{
			delayMs = 1000 * MaximumBackOffSeconds;
			_logger.LogDebug("{Prefix} Rate limit window of {Seconds} seconds is greater than our max of {Max} seconds, so using the max value instead.", prefix, windowSeconds, MaximumBackOffSeconds);
		}

		state.RateLimitTotalMs = await WaitForRateLimitAsync(
			prefix,
			rateLimitInformation,
			delayMs,
			++state.RateLimitAttempt,
			state.RateLimitTotalMs,
			cancellationToken).ConfigureAwait(false);
	}

	/// <summary>
	///     Patch specific fields
	/// </summary>
	/// <typeparam name="T"></typeparam>
	/// <param name="entity"></param>
	/// <param name="fieldsToUpdate">The fields to update</param>
	/// <param name="cancellationToken">The cancellation token</param>
	/// <exception cref="AggregateException"></exception>
	public virtual Task PatchAsync<T>(T entity, Dictionary<string, object> fieldsToUpdate, CancellationToken cancellationToken) where T : IPatchable
		=> PatchAsync($"{entity.Endpoint()}/{entity.Id}", fieldsToUpdate, cancellationToken);

	/// <summary>
	///    Patch specific fields
	/// </summary>
	/// <param name="subUrl"></param>
	/// <param name="fieldsToUpdate"></param>
	/// <param name="cancellationToken"></param>
	/// <returns></returns>
	/// <exception cref="LogicMonitorApiException"></exception>
	public async Task PatchAsync(string subUrl, Dictionary<string, object> fieldsToUpdate, CancellationToken cancellationToken)
	{
		var prefix = GetPrefix(_patchHttpMethod);
		var jsonString = JsonConvert.SerializeObject(fieldsToUpdate);

		_logger.LogDebug("{Prefix}", prefix);
		HttpResponseMessage httpResponseMessage;
		while (true)
		{
			using (var content = new StringContent(jsonString, Encoding.UTF8, "application/json"))
			{
				var patchSpec = $"?patchFields={string.Join(",", fieldsToUpdate.Keys)}";
				using var requestMessage = new HttpRequestMessage(_patchHttpMethod, RequestUri(subUrl) + patchSpec) { Content = content };
				httpResponseMessage = await GetHttpResponseMessage(requestMessage, subUrl, jsonString, cancellationToken).ConfigureAwait(false);
			}

			_logger.LogDebug("{Prefix} complete", prefix);

			if (!httpResponseMessage.IsSuccessStatusCode)
			{
				if (httpResponseMessage.StatusCode == HttpStatusCode.Forbidden)
				{
					var body = await httpResponseMessage.Content.ReadAsStringAsync(cancellationToken).ConfigureAwait(false);
					throw new LogicMonitorApiException(HttpMethod.Post, subUrl, HttpStatusCode.Forbidden, body, $"{prefix} failed ({httpResponseMessage.StatusCode}): {body}");
				}

				if (WaitDuringLogicMonitorUpgrades && httpResponseMessage.StatusCode == HttpStatusCode.ServiceUnavailable)
				{
					// TODO: could also check the reason phrase, and / or the RESPONSE (which contains "Service Temporarily Unavailable")
					_logger.LogDebug("{Prefix} Service Unavailable. Waiting 10000ms", prefix);
					await Task.Delay(10000, cancellationToken).ConfigureAwait(false);
					continue;
				}
				// If a success code was not received, throw an exception
				var responseBody = await httpResponseMessage.Content.ReadAsStringAsync(cancellationToken).ConfigureAwait(false);
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
	public Task<Page<T>> GetPageAsync<T>(Filter<T>? filter, string subUrl, CancellationToken cancellationToken) where T : new()
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
	private Task<Page<T>> GetPageInternalAsync<T>(Filter<T>? filter, string subUrl, CancellationToken cancellationToken) where T : new()
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
	public async Task<ExecuteDebugCommandResponse?> ExecuteDebugCommandAndWaitForResultAsync(
		int collectorId,
		string commandText,
		int timeoutMs,
		int sleepIntervalMs,
		CancellationToken cancellationToken)
	{
		var executeDebugCommandResponse = await ExecuteDebugCommandAsync(collectorId, commandText, cancellationToken).ConfigureAwait(false);

		var stopwatch = Stopwatch.StartNew();
		ExecuteDebugCommandResponse? debugCommandResult = null;
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
		string? value,
		SetPropertyMode mode,
		string subUrl,
		CancellationToken cancellationToken)
	{
		var propertiesSubUrl = $"{subUrl}/{id}/properties";
		var task = mode switch
		{
			SetPropertyMode.Automatic => SetCustomPropertyAutomaticAsync(propertiesSubUrl, name, value, cancellationToken),
			SetPropertyMode.Create => CreateCustomPropertyAsync(propertiesSubUrl, name, value, cancellationToken),
			SetPropertyMode.Update => UpdateCustomPropertyAsync(propertiesSubUrl, name, value, cancellationToken),
			SetPropertyMode.Delete => DeleteCustomPropertyAsync(propertiesSubUrl, name, value, cancellationToken),
			_ => throw new ArgumentOutOfRangeException(nameof(mode), mode, null)
		};
		await task.ConfigureAwait(false);
	}

	private async Task SetCustomPropertyAutomaticAsync(string propertiesSubUrl, string name, string? value, CancellationToken cancellationToken)
	{
		// Determine whether there is an existing property
		try
		{
			_ = await GetBySubUrlAsync<EntityProperty>($"{propertiesSubUrl}/{name}", cancellationToken).ConfigureAwait(false);

			// No exception thrown? It exists, so delete it when the new value is null, otherwise PUT the replacement
			if (value is null)
			{
				await DeleteAsync($"{propertiesSubUrl}/{name}", cancellationToken).ConfigureAwait(false);
			}
			else
			{
				await PutAsync($"{propertiesSubUrl}/{name}", new { value }, cancellationToken).ConfigureAwait(false);
			}
		}
		catch (Exception)
		{
			// It doesn't exist, so POST a new one (unless it's null, in which case nothing to do)
			if (value is not null)
			{
				_ = await PostAsync<EntityProperty, EntityProperty>(
					new EntityProperty { Name = name, Value = value },
					propertiesSubUrl,
					cancellationToken).ConfigureAwait(false);
			}
		}
	}

	private async Task CreateCustomPropertyAsync(string propertiesSubUrl, string name, string? value, CancellationToken cancellationToken)
	{
		if (value is null)
		{
			throw new InvalidOperationException("Value must not be set to null when creating the property.");
		}

		await PostAsync<EntityProperty, EntityProperty>(new EntityProperty { Name = name, Value = value }, propertiesSubUrl, cancellationToken).ConfigureAwait(false);
	}

	private async Task UpdateCustomPropertyAsync(string propertiesSubUrl, string name, string? value, CancellationToken cancellationToken)
	{
		if (value is null)
		{
			throw new InvalidOperationException("Value must not be set to null when updating the property.");
		}

		await PutAsync($"{propertiesSubUrl}/{name}", new { value }, cancellationToken).ConfigureAwait(false);
	}

	private async Task DeleteCustomPropertyAsync(string propertiesSubUrl, string name, string? value, CancellationToken cancellationToken)
	{
		if (value is not null)
		{
			throw new InvalidOperationException("Value must be set to null when deleting the value.");
		}

		await DeleteAsync($"{propertiesSubUrl}/{name}", cancellationToken).ConfigureAwait(false);
	}

	/// <summary>
	/// Returns true if the specified property on the class has the SantabaReadOnly attribute defined
	/// </summary>
	/// <param name="name">The name to match</param>
	/// <param name="logicMonitorClassType">The type of class to query</param>
	/// <returns>True if the property is read only</returns>
	public static bool IsPropertyReadOnly(string name, Type logicMonitorClassType)
		=> IsPropertyReadOnly(name, logicMonitorClassType, false);

	/// <summary>
	/// Returns true if the specified property on the class has the SantabaReadOnly attribute defined
	/// </summary>
	/// <param name="name">The name to match</param>
	/// <param name="logicMonitorClassType">The type of class to query</param>
	/// <param name="tryJsonNameFirst">If true, will use the DataMember/JSON property name to match before using property name</param>
	/// <returns>True if the property is read only</returns>
	public static bool IsPropertyReadOnly(string name, Type logicMonitorClassType, bool tryJsonNameFirst)
	{
		PropertyInfo? propertyInfo;
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

		return propertyInfo is null
			? throw new PropertyNotFoundException($"Could not find property on {logicMonitorClassType.Name} with name {name}.")
			: Attribute.IsDefined(propertyInfo, typeof(SantabaReadOnly));
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

	/// <summary>
	/// Waits out a rate-limit backoff, reporting it at a level a caller can actually see.
	///
	/// Issue #38: the aggregate wait here is unbounded - the 429 loop retries until the portal relents -
	/// and it is silent by every other measure: no CPU, no open socket, no exception. Logged at Debug, a
	/// consumer running at Information could not distinguish a polite backoff from a hung call. The
	/// cumulative attempt and total are included because the tenth five-minute wait reads very
	/// differently from the first, and only this client is in a position to know which it is.
	/// </summary>
	/// <returns>The new cumulative wait for this request, in milliseconds.</returns>
	private async Task<long> WaitForRateLimitAsync(
		string prefix,
		string rateLimitInformation,
		int delayMs,
		int attempt,
		long previousTotalMs,
		CancellationToken cancellationToken)
	{
		var totalMs = previousTotalMs + delayMs;

		// Once more than a single maximum backoff has been spent waiting, this has stopped being routine.
		if (totalMs > 1000L * MaximumBackOffSeconds)
		{
			_logger.LogWarning(
				"{Prefix} Rate limiting hit (attempt {Attempt}, {TotalWaitedMs:N0}ms waited so far): {RateLimitInformation}, waiting {DelayMs:N0}ms",
				prefix,
				attempt,
				totalMs,
				rateLimitInformation,
				delayMs);
		}
		else
		{
			_logger.LogInformation(
				"{Prefix} Rate limiting hit (attempt {Attempt}, {TotalWaitedMs:N0}ms waited so far): {RateLimitInformation}, waiting {DelayMs:N0}ms",
				prefix,
				attempt,
				totalMs,
				rateLimitInformation,
				delayMs);
		}

		await Task.Delay(delayMs, cancellationToken).ConfigureAwait(false);

		return totalMs;
	}
}
