namespace LogicMonitor.Api.Resources.Uptime;

/// <summary>
/// A single step of an Uptime web check. Uptime-owned so the new surface stays decoupled from the
/// legacy <c>Websites.WebCheckStep</c> type.
/// </summary>
public class UptimeWebCheckStep
{
	/// <summary>
	/// The step name (for example <c>__step0</c>).
	/// </summary>
	[JsonProperty("name")]
	public string Name { get; set; } = "__step0";

	/// <summary>
	/// Whether the step is enabled.
	/// </summary>
	[JsonProperty("enable")]
	public bool Enabled { get; set; } = true;

	/// <summary>
	/// Whether the <see cref="Url"/> is appended to the root URL/domain.
	/// </summary>
	[JsonProperty("useDefaultRoot")]
	public bool UseDefaultRoot { get; set; } = true;

	/// <summary>
	/// The full or relative URL for the step.
	/// </summary>
	[JsonProperty("url")]
	public string Url { get; set; } = string.Empty;

	/// <summary>
	/// The HTTP version used by the request (for example <c>1.1</c>).
	/// </summary>
	[JsonProperty("HTTPVersion")]
	public string HttpVersion { get; set; } = "1.1";

	/// <summary>
	/// The HTTP method used by the request (GET, HEAD or POST).
	/// </summary>
	[JsonProperty("HTTPMethod")]
	public string HttpMethod { get; set; } = "GET";

	/// <summary>
	/// Whether the check follows HTTP redirects.
	/// </summary>
	[JsonProperty("followRedirection")]
	public bool FollowRedirection { get; set; } = true;

	/// <summary>
	/// Whether the request waits for the full page (including dependent resources) to load.
	/// </summary>
	[JsonProperty("fullpageLoad")]
	public bool FullPageLoad { get; set; }

	/// <summary>
	/// The expected response status code(s), comma separated.
	/// </summary>
	[JsonProperty("statusCode")]
	public string StatusCode { get; set; } = string.Empty;

	/// <summary>
	/// A keyword that must be present (or absent) in the response.
	/// </summary>
	[JsonProperty("keyword")]
	public string Keyword { get; set; } = string.Empty;
}

