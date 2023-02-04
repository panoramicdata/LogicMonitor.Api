using System;
using System.Collections.Generic;
using System.Text;

namespace LogicMonitor.Api.Devices;

/// <summary>
/// NbarApplicationNames
/// </summary>
[DataContract]

public class NbarApplicationNames
{
	/// <summary>
	/// operator
	/// </summary>
	[DataMember(Name = "operator", IsRequired = false)]
	public string Operator { get; set; } = string.Empty;

	/// <summary>
	/// applicationName
	/// </summary>
	[DataMember(Name = "applicationName", IsRequired = false)]
	public string ApplicationName { get; set; } = string.Empty;
}
