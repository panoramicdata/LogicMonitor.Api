namespace LogicMonitor.Api.Websites;

/// <summary>
/// Website authentication credentials
/// </summary>
[DataContract]
public class WebsiteAuthentication
{
	/// <summary>
	/// User name
	/// </summary>
	public string UserName { get; set; }

	/// <summary>
	/// Authentication type
	/// </summary>
	public WebsiteAuthenticationType WebsiteAuthenticationType { get; set; }

	/// <summary>
	/// Password
	/// </summary>
	public string Password { get; set; }
}
