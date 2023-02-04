using System;
using System.Collections.Generic;
using System.Text;

namespace LogicMonitor.Api.Devices;

/// <summary>
/// Netflow device info
/// </summary>

[DataContract]
public class NetflowDeviceInfo : IdentifiedItem
{
	/// <summary>
	/// deleted
	/// </summary>
	[DataMember(Name = "deleted", IsRequired = false)]
	public bool Deleted { get; set; }

	/// <summary>
	/// displayName
	/// </summary>
	[DataMember(Name = "displayName", IsRequired = false)]
	public string DisplayName { get; set; } = string.Empty;
}
