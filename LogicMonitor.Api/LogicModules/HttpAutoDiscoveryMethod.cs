using System;
using System.Collections.Generic;
using System.Text;

namespace LogicMonitor.Api.LogicModules;

/// <summary>
/// HttpAutoDiscoveryMethod
/// </summary>

[DataContract]
public class HttpAutoDiscoveryMethod : AutoDiscoveryMethod
{
	/// <summary>
	/// The regular expression
	/// </summary>
	[DataMember(Name = "regex", IsRequired = true)]
	public string Regex { get; set; } = null!;

	/// <summary>
	/// Whether it is case sensitive
	/// </summary>
	[DataMember(Name = "caseSensitive", IsRequired = true)]
	public bool IsCaseSensitive { get; set; }

	/// <summary>
	/// followRedirect
	/// </summary>
	[DataMember(Name = "followRedirect", IsRequired = true)]
	public string FollowRedirect { get; set; } = null!;

	/// <summary>
	/// The ports
	/// </summary>
	[DataMember(Name = "ports", IsRequired = true)]
	public string Ports { get; set; } = null!;

	/// <summary>
	/// The URI
	/// </summary>
	[DataMember(Name = "uri", IsRequired = true)]
	public string Uri { get; set; } = null!;

	/// <summary>
	/// Whether to use SSL
	/// </summary>
	[DataMember(Name = "useSSL", IsRequired = true)]
	public bool UseSsl { get; set; }
}
