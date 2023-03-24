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
	/// The Access Token's ID
	/// </summary>
	public string AccessId { get; set; } = string.Empty;

	/// <summary>
	/// The Account Token's Key
	/// </summary>
	public string AccessKey { get; set; } = string.Empty;

	/// <summary>
	/// Allow overriding the HttpClient Timeout - defaults to original 60 seconds
	/// </summary>
	public int HttpClientTimeoutSeconds { get; set; } = 60;

	/// <summary>
	/// An optional ILogger
	/// </summary>
	public ILogger? Logger { get; set; }
}
