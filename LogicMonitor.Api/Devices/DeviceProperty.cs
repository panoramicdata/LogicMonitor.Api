using System;
using System.Collections.Generic;
using System.Text;

namespace LogicMonitor.Api.Devices;

/// <summary>
/// Device Property
/// </summary>

[DataContract]
public class DeviceProperty
{
	/// <summary>
	/// The resource property name
	/// </summary>
	[DataMember(Name = "name", IsRequired = true)]
	public string Name { get; set; } = null!;

	/// <summary>
	/// The resource property value
	/// </summary>
	[DataMember(Name = "value", IsRequired = true)]
	public string Value { get; set; } = null!;
}
