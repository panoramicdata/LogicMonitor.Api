namespace LogicMonitor.Api.Settings;

/// <summary>
/// Authentication
/// </summary>

[DataContract]
public class Authentication
{
	/// <summary>
	/// NTLM authentication password
	/// </summary>
	[DataMember(Name = "password")]
	public string Password { get; set; } 

	/// <summary>
	/// Authentication type
	/// </summary>
	[DataMember(Name = "type")]
	public string Type { get; set; } 

	/// <summary>
	/// NTLM  authentication userName
	/// </summary>
	[DataMember(Name = "userName")]
	public string UserName { get; set; } 
}
