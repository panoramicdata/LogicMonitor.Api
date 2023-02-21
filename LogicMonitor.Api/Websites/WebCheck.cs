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
	[DataMember(Name = "schema", IsRequired = false)]
	public HttpSchema Schema { get; set; }

	/// <summary>
	/// Whether or not SSL expiration alerts should be triggered
	/// </summary>
	[DataMember(Name = "triggerSSLExpirationAlert", IsRequired = false)]
	public bool TriggerSslExpirationAlerts { get; set; }

	/// <summary>
	/// Whether or not SSL status alerts should be triggered
	/// </summary>
	[DataMember(Name = "triggerSSLStatusAlert", IsRequired = false)]
	public bool TriggerSslStatusAlerts { get; set; }

	/// <summary>
	/// The time in milliseconds that the page must load within for each step to avoid triggering an alert
	/// </summary>
	[DataMember(Name = "pageLoadAlertTimeInMS", IsRequired = false)]
	public long PageLoadAlertTimeInMS { get; set; }

	/// <summary>
	/// Whether or not SSL should be ignored, the default value is true
	/// </summary>
	[DataMember(Name = "ignoreSSL", IsRequired = false)]
	public bool IgnoreSsl { get; set; }

	/// <summary>
	/// The threshold (in days) for triggering SSL certification alerts
	/// </summary>
	[DataMember(Name = "alertExpr", IsRequired = false)]
	public string? AlertExpression { get; set; }
}
