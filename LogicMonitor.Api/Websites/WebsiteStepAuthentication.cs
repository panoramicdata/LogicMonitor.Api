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
	[DataMember(Name = "type", IsRequired = true)]
	public string Type { get; set; } = null!;

	/// <summary>
	/// NTLM  authentication userName
	/// </summary>
	[DataMember(Name = "userName", IsRequired = true)]
	public string UserName { get; set; } = null!;

	/// <summary>
	/// NTLM authentication password
	/// </summary>
	[DataMember(Name = "password", IsRequired = true)]
	public string Password { get; set; } = null!;
}
