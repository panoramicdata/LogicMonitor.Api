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
	public string Type { get; set; } = null!;

	/// <summary>
	/// NTLM  authentication userName
	/// </summary>
	[DataMember(Name = "userName")]
	public string UserName { get; set; } = null!;

	/// <summary>
	/// NTLM authentication password
	/// </summary>
	[DataMember(Name = "password")]
	public string Password { get; set; } = null!;
}
