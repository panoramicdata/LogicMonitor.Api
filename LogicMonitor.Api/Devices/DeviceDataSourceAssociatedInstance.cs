using System;
using System.Collections.Generic;
using System.Text;

namespace LogicMonitor.Api.Devices;

/// <summary>
/// DeviceDataSourceAssociatedInstance
/// </summary>

[DataContract]
public class DeviceDataSourceAssociatedInstance : NamedItem
{
	/// <summary>
	/// instance alias
	/// </summary>
	[DataMember(Name = "alias", IsRequired = false)]
	public string Alias { get; set; }
}
