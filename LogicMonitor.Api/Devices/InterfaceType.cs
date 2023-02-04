using System;
using System.Collections.Generic;
using System.Text;

namespace LogicMonitor.Api.Devices;

/// <summary>
/// InterfaceType
/// </summary>
[DataContract]
public class InterfaceType
{
	/// <summary>
	/// ifPosition
	/// </summary>
	[DataMember(Name = "ifPosition", IsRequired = false)]
	public string IfPosition { get; set; } = string.Empty;
}
