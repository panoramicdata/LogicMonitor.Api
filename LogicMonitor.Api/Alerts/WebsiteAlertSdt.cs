namespace LogicMonitor.Api.Alerts;

/// <summary>
/// A website alert SDT
/// </summary>
[DataContract]
public class WebsiteAlertSdt : AlertSdt
{
	/// <summary>
	/// The website id
	/// </summary>
	[DataMember(Name = "websiteId")]
	public int WebsiteId { get; set; }
}
