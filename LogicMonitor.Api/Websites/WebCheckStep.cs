namespace LogicMonitor.Api.Websites;

/// <summary>
/// Website step
/// </summary>
[DataContract]
public class WebCheckStep : NamedItem
{
	/// <summary>
	/// HTTP schema
	/// </summary>
	[DataMember(Name = "schema", IsRequired = false)]
	[JsonConverter(typeof(StringEnumConverter))]
	public HttpSchema HttpSchema { get; set; }

	/// <summary>
	/// Plain Text/String | Glob expression | JSON | XML | Multi line key value pair\nStep Response Type
	/// </summary>
	[DataMember(Name = "respType", IsRequired = false)]
	public string? RespType { get; set; }

	/// <summary>
	/// HTTP header
	/// </summary>
	[DataMember(Name = "HTTPHeaders", IsRequired = false)]
	public string? HttpHeaders { get; set; }

	/// <summary>
	/// Authorization Information
	/// </summary>
	[DataMember(Name = "auth", IsRequired = false)]
	public WebsiteStepAuthentication? Auth { get; set; }

	/// <summary>
	/// Body match type
	/// </summary>
	[DataMember(Name = "matchType", IsRequired = false)]
	[JsonConverter(typeof(StringEnumConverter))]
	public MatchType MatchType { get; set; }

	/// <summary>
	/// script | config\nThe type of service step
	/// </summary>
	[DataMember(Name = "type", IsRequired = false)]
	public string? Type { get; set; }

	/// <summary>
	/// Request timeout measured in seconds
	/// </summary>
	[DataMember(Name = "timeout", IsRequired = false)]
	public int Timeout { get; set; }

	/// <summary>
	/// true | falseCheck if using the default root
	/// </summary>
	[DataMember(Name = "useDefaultRoot", IsRequired = false)]
	public bool UseDefaultRoot { get; set; }

	/// <summary>
	/// GET | HEAD | POST\nSpecifies the type of HTTP method
	/// </summary>
	[DataMember(Name = "HTTPMethod", IsRequired = false)]
	public string? HttpMethod { get; set; }

	/// <summary>
	/// Path for JSON, XPATH
	/// </summary>
	[DataMember(Name = "path", IsRequired = false)]
	public string? Path { get; set; }

	/// <summary>
	/// true | false\nSpecifies whether to enable step or not
	/// </summary>
	[DataMember(Name = "enable", IsRequired = false)]
	public bool Enable { get; set; }

	/// <summary>
	/// 1.1 | 1\nSpecifies HTTP version
	/// </summary>
	[DataMember(Name = "HTTPVersion", IsRequired = false)]
	public string? HttpVersion { get; set; }

	/// <summary>
	/// Keyword that matches the body
	/// </summary>
	[DataMember(Name = "keyword", IsRequired = false)]
	public string? Keyword { get; set; }

	/// <summary>
	/// The Step Response Script
	/// </summary>
	[DataMember(Name = "respScript", IsRequired = false)]
	public string? ResponseScript { get; set; }

	/// <summary>
	/// The Label of the Step
	/// </summary>
	[DataMember(Name = "label", IsRequired = false)]
	public string? Label { get; set; }

	/// <summary>
	/// The URL of service step
	/// </summary>
	[DataMember(Name = "url", IsRequired = false)]
	public string? Url { get; set; }

	/// <summary>
	/// true | false\nChecks if invert matches or not
	/// </summary>
	[DataMember(Name = "invertMatch", IsRequired = false)]
	public bool InvertMatch { get; set; }

	/// <summary>
	/// The Request Script
	/// </summary>
	[DataMember(Name = "reqScript", IsRequired = false)]
	public string? RequestScript { get; set; }

	/// <summary>
	/// HTTP Body
	/// </summary>
	[DataMember(Name = "HTTPBody", IsRequired = false)]
	public string? HttpBody { get; set; }

	/// <summary>
	/// true | false\nSpecifies whether to follow redirection or not
	/// </summary>
	[DataMember(Name = "followRedirection", IsRequired = false)]
	public bool FollowRedirection { get; set; }

	/// <summary>
	/// Raw | Formatted Data\nSpecifies POST data type
	/// </summary>
	[DataMember(Name = "postDataEditType", IsRequired = false)]
	public PostDataEditType? PostDataEditType { get; set; }

	/// <summary>
	/// true | false\nChecks if authorization required or not
	/// </summary>
	[DataMember(Name = "requireAuth", IsRequired = false)]
	public bool RequiresAuthentication { get; set; }

	/// <summary>
	/// script | config\nStep Request Type
	/// </summary>
	[DataMember(Name = "reqType", IsRequired = false)]
	public string? RequestType { get; set; }

	/// <summary>
	/// true | false\nChecks if full page should be loaded or not
	/// </summary>
	[DataMember(Name = "fullpageLoad", IsRequired = false)]
	public bool FullpageLoad { get; set; }

	/// <summary>
	/// The expected status code
	/// </summary>
	[DataMember(Name = "statusCode", IsRequired = false)]
	public string? StatusCode { get; set; }
}
