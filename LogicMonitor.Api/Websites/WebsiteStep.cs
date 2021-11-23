using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System.Runtime.Serialization;

namespace LogicMonitor.Api.Websites;

/// <summary>
/// Website step
/// </summary>
[DataContract]
public class WebsiteStep : NamedItem
{
	/// <summary>
	/// Whether to follow redirection
	/// </summary>
	[DataMember(Name = "followRedirection")]
	public bool FollowRedirection { get; set; }

	/// <summary>
	/// The HTTP headers
	/// </summary>
	[DataMember(Name = "HTTPHeaders")]
	public string HttpHeaders { get; set; }

	/// <summary>
	/// The Status code
	/// </summary>
	[DataMember(Name = "statusCode")]
	public string StatusCode { get; set; }

	/// <summary>
	/// The Type
	/// </summary>
	[DataMember(Name = "type")]
	public string Type { get; set; }

	/// <summary>
	/// The match type
	/// </summary>
	[DataMember(Name = "matchType")]
	[JsonConverter(typeof(StringEnumConverter))]
	public MatchType MatchType { get; set; }

	/// <summary>
	/// The Keyword
	/// </summary>
	[DataMember(Name = "keyword")]
	public string Keyword { get; set; }

	/// <summary>
	/// The HTTP body
	/// </summary>
	[DataMember(Name = "HTTPBody")]
	public string HttpBody { get; set; }

	/// <summary>
	/// The HTTP Method
	/// </summary>
	[DataMember(Name = "HTTPMethod")]
	public string HttpMethod { get; set; }

	/// <summary>
	/// The post data edit type
	/// </summary>
	[DataMember(Name = "postDataEditType")]
	[JsonConverter(typeof(StringEnumConverter))]
	public PostDataEditType? PostDataEditType { get; set; }

	/// <summary>
	/// The label
	/// </summary>
	[DataMember(Name = "label")]
	public string Label { get; set; }

	/// <summary>
	/// The URL
	/// </summary>
	[DataMember(Name = "url")]
	public string Url { get; set; }

	/// <summary>
	/// Whether full page load is being tested
	/// </summary>
	[DataMember(Name = "fullpageLoad")]
	public bool FullpageLoad { get; set; }

	/// <summary>
	/// Whether authentication is required
	/// </summary>
	[DataMember(Name = "requireAuth")]
	public bool RequiresAuthentication { get; set; }

	/// <summary>
	/// The auth token
	/// </summary>
	[DataMember(Name = "auth")]
	public WebsiteStepAuthentication Auth { get; set; }

	/// <summary>
	/// Domain
	/// </summary>
	[DataMember(Name = "domain")]
	public string Domain { get; set; }

	/// <summary>
	/// Whether the step is enabled
	/// </summary>
	[DataMember(Name = "enable")]
	public bool Enable { get; set; }

	/// <summary>
	/// The HTTP Version
	/// </summary>
	[DataMember(Name = "HTTPVersion")]
	public string HttpVersion { get; set; }

	/// <summary>
	/// Whether to invert the match criterion
	/// </summary>
	[DataMember(Name = "invertMatch")]
	public bool InvertMatch { get; set; }

	/// <summary>
	/// The request script
	/// </summary>
	[DataMember(Name = "reqScript")]
	public string RequestScript { get; set; }

	/// <summary>
	/// The request type
	/// </summary>
	[DataMember(Name = "reqType")]
	public string RequestType { get; set; }

	/// <summary>
	/// The response type
	/// </summary>
	[DataMember(Name = "respType")]
	public string ResponseType { get; set; }

	/// <summary>
	/// The response script
	/// </summary>
	[DataMember(Name = "respScript")]
	public string ResponseScript { get; set; }

	/// <summary>
	/// The timeout
	/// </summary>
	[DataMember(Name = "timeout")]
	public int Timeout { get; set; }

	/// <summary>
	/// The HTTP Schema
	/// </summary>
	[DataMember(Name = "schema")]
	[JsonConverter(typeof(StringEnumConverter))]
	public HttpSchema HttpSchema { get; set; }

	/// <summary>
	/// The path
	/// </summary>
	[DataMember(Name = "path")]
	public string Path { get; set; }

	/// <summary>
	/// Whether to use DefaultRoot
	/// </summary>
	[DataMember(Name = "useDefaultRoot")]
	public bool UseDefaultRoot { get; set; }
}
