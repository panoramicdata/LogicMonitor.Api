namespace LogicMonitor.Api.Websites;

/// <summary>
/// WebCheck
/// </summary>

[DataContract]
public class WebCheck : Website
{
	/// <summary>
	/// The scheme or protocol associated with the URL to check. Acceptable values are: http, https
	/// </summary>
	[DataMember(Name = "schema")]
	public HttpSchema Schema { get; set; }

	/// <summary>
	/// The time in milliseconds that the page must load within for each step to avoid triggering an alert
	/// </summary>
	[DataMember(Name = "pageLoadAlertTimeInMS")]
	public long PageLoadAlertTimeInMS { get; set; }
}
