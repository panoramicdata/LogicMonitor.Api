using System;
using System.Collections.Generic;
using System.Text;

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
	[DataMember(Name = "password", IsRequired = true)]
	public string Password { get; set; } = null!;

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
}
