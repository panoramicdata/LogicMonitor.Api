namespace LogicMonitor.Api;

/// <summary>
/// LogicMonitor Client options
/// </summary>
public class LogicMonitorClientOptions
{
	/// <summary>
	/// The Account subdomain.
	/// For example, if your LogicMonitor URL is https://acme.logicmonitor.com/ , use "acme"
	/// </summary>
	public string Account { get; set; } = string.Empty;

	/// <summary>
	/// The Access Token's ID. Can be left blank when using a Bearer Token set in the AccessKey property
	/// </summary>
	public string AccessId { get; set; } = string.Empty;

	/// <summary>
	/// The Account Token's Key (or Bearer Token, in which case the AccessId can be left blank)
	/// </summary>
	public string AccessKey { get; set; } = string.Empty;

	/// <summary>
	/// Allow overriding the HttpClient Timeout - defaults to original 60 seconds
	/// </summary>
	public int HttpClientTimeoutSeconds { get; set; } = 60;

	/// <summary>
	/// The maximum number of seconds to back off (wait) after receiving a 429 response (too many requests)
	/// </summary>
	public int MaximumBackOffSeconds { get; set; } = int.MaxValue;

	/// <summary>
	/// An optional ILogger
	/// </summary>
	public ILogger? Logger { get; set; }
}
