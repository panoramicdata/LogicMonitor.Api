namespace LogicMonitor.Api.Websites;

/// <summary>
/// A website step authentication
/// </summary>
[DataContract]
public class WebsiteStepAuthentication
{
	/// <summary>
	/// Authentication type
	/// </summary>
	[DataMember(Name = "type")]
	public string Type { get; set; } = string.Empty;

	/// <summary>
	/// Authentication userName
	/// </summary>
	[DataMember(Name = "userName")]
	public string UserName { get; set; } = string.Empty;

	/// <summary>
	/// Authentication password
	/// </summary>
	[DataMember(Name = "password")]
	public string Password { get; set; } = string.Empty;

	/// <summary>
	/// Domain - required when type is NTLM
	/// </summary>
	[DataMember(Name = "domain")]
	public string Domain { get; set; } = string.Empty;
}
