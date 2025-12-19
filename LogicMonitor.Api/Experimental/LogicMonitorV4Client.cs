namespace LogicMonitor.Api.Experimental;

/// <summary>
/// Provides a client for interacting with the LogicMonitor REST API v4 using account-based authentication.
/// </summary>
/// <remarks>This client manages authentication and HTTP communication with the LogicMonitor platform. It should
/// be disposed when no longer needed to release network resources. The class is not thread-safe; create separate
/// instances for concurrent use in multiple threads.</remarks>
public class LogicMonitorV4Client : IDisposable
{
	private readonly string _account;
	private readonly string _baseUrl;
	private readonly string _username;
	private readonly string _password;
	private readonly HttpClient _client;
	private bool _disposedValue;

	/// <summary>
	/// Initializes a new instance of the LogicMonitorV4Client class using the specified client options.
	/// </summary>
	/// <param name="options">The configuration options used to initialize the LogicMonitor client. Must specify the account name and other
	/// required connection settings.</param>
	public LogicMonitorV4Client(LogicMonitorClientOptions options)
	{
		var handler = new HttpClientHandler { CookieContainer = new CookieContainer() };

		_account = options.Account;
		_baseUrl = $"https://{_account}.logicmonitor.com/santaba/rest";
		_username = ""; // options.Username;
		_password = ""; // options.Password;
		_client = new HttpClient(handler)
		{
			BaseAddress = new Uri(_baseUrl)
		};
	}

	/// <summary>
	/// Performs an asynchronous authentication sequence with the LogicMonitor API using the configured account
	/// credentials.
	/// </summary>
	/// <remarks>This method retrieves required session cookies and CSRF tokens before submitting the authentication
	/// request. If authentication fails, no exception is thrown; instead, the failure is logged. The method should be
	/// awaited to ensure authentication completes before making further API requests with the client.</remarks>
	/// <param name="cancellationToken">A cancellation token that can be used to cancel the authentication operation.</param>
	/// <returns>A task that represents the asynchronous authentication operation.</returns>
	public async Task AuthenticateAsync(CancellationToken cancellationToken)
	{
		try
		{
			// 1. Get JSESSIONID (Implicitly handled by CookieContainer)
			var cookieUrl = $"https://{_account}.logicmonitor.com/santaba";
			var cookieResponse = await _client.GetAsync(cookieUrl, cancellationToken);

			if (!cookieResponse.IsSuccessStatusCode)
			{
				await LogFailure("JSESSIONID Retrieval", cookieResponse);
				return;
			}

			// 2. Fetch CSRF Token
			_client.DefaultRequestHeaders.Clear();
			_client.DefaultRequestHeaders.Add("X-Version", "3");
			_client.DefaultRequestHeaders.Add("X-CSRF-Token", "Fetch");
			_client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

			var csrfUrl = $"{_baseUrl}/functions/dummy";
			var csrfResponse = await _client.GetAsync(csrfUrl, cancellationToken);

			if (!csrfResponse.IsSuccessStatusCode)
			{
				await LogFailure("CSRF Token Retrieval", csrfResponse);
				return;
			}

			// Extract the token from headers
			var csrfToken = csrfResponse.Headers.GetValues("X-CSRF-Token").FirstOrDefault();

			// 3. Authenticate / Sign-in
			_client.DefaultRequestHeaders.Remove("X-CSRF-Token");
			_client.DefaultRequestHeaders.Add("X-CSRF-Token", csrfToken);
			_client.DefaultRequestHeaders.Add("External-User", "true");

			var authUrl = $"{_baseUrl}/setting/admins/services/signin";
			var loginPayload = new
			{
				keepMeSignedIn = "true",
				username = _username,
				password = _password
			};

			var jsonPayload = JsonConvert.SerializeObject(loginPayload);
			var content = new StringContent(jsonPayload, Encoding.UTF8, "application/json");

			var authResponse = await _client.PostAsync(authUrl, content, cancellationToken);

			if (authResponse.IsSuccessStatusCode)
			{
				Console.WriteLine("Authentication successful.");

				// 4. Example follow-up request
				_client.DefaultRequestHeaders.Remove("X-Version");
				_client.DefaultRequestHeaders.Add("X-Version", "4");
				// Proceed to your restEndpoint here...
			}
			else
			{
				await LogFailure("Authentication", authResponse);
			}
		}
		catch (Exception ex)
		{
			Console.WriteLine($"An error occurred: {ex.Message}");
		}
	}

	private static async Task LogFailure(string step, HttpResponseMessage response)
	{
		Console.WriteLine($"{step} failed. Status Code: {(int)response.StatusCode}");
		Console.WriteLine("Headers:");
		foreach (var header in response.Headers)
		{
			Console.WriteLine($"{header.Key}: {string.Join(", ", header.Value)}");
		}

		var body = await response.Content.ReadAsStringAsync();
		Console.WriteLine($"Body: {body}");
	}

	/// <summary>
	/// Releases the unmanaged resources used by the object and optionally releases the managed resources.
	/// </summary>
	/// <remarks>This method is called by public Dispose methods and the finalizer. When disposing is true, this
	/// method disposes all managed resources. Override this method to release additional resources in a derived
	/// class.</remarks>
	/// <param name="disposing">true to release both managed and unmanaged resources; false to release only unmanaged resources.</param>
	protected virtual void Dispose(bool disposing)
	{
		if (!_disposedValue)
		{
			if (disposing)
			{
				_client.Dispose();
			}

			_disposedValue = true;
		}
	}

	/// <inheritdoc/>
	public void Dispose()
	{
		// Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
		Dispose(disposing: true);
		GC.SuppressFinalize(this);
	}
}