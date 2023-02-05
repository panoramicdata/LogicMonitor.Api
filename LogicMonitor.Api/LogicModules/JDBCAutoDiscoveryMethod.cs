using System;
using System.Collections.Generic;
using System.Text;

namespace LogicMonitor.Api.LogicModules;

/// <summary>
/// JDBCAutoDiscoveryMethod
/// </summary>

[DataContract]
public class JDBCAutoDiscoveryMethod : AutoDiscoveryMethod
{
	/// <summary>
	/// The query
	/// </summary>
	[DataMember(Name = "query", IsRequired = true)]
	public string Query { get; set; } = null!;

	/// <summary>
	/// The ports
	/// </summary>
	[DataMember(Name = "ports", IsRequired = true)]
	public string Ports { get; set; } = null!;

	/// <summary>
	/// The type
	/// </summary>
	[DataMember(Name = "type", IsRequired = true)]
	public string Type { get; set; } = null!;

	/// <summary>
	/// The separator
	/// </summary>
	[DataMember(Name = "separator", IsRequired = false)]
	public string? Separator { get; set; }

	/// <summary>
	/// The url
	/// </summary>
	[DataMember(Name = "url", IsRequired = true)]
	public string Url { get; set; } = null!;

	/// <summary>
	/// The sid
	/// </summary>
	[DataMember(Name = "sid", IsRequired = false)]
	public string? Sid { get; set; }
}
