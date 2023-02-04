using System;
using System.Collections.Generic;
using System.Text;

namespace LogicMonitor.Api.Devices;

/// <summary>
/// NetflowBgpTable
/// </summary>

[DataContract]
public class NetflowBgpTable : NetflowDataBase
{
	/// <summary>
	/// percentUsage
	/// </summary>
	[DataMember(Name = "percentUsage", IsRequired = false)]
	public double PercentUsage { get; set; }

	/// <summary>
	/// asNumber
	/// </summary>
	[DataMember(Name = "asNumber", IsRequired = false)]
	public long AsNumber { get; set; }

	/// <summary>
	/// usage
	/// </summary>
	[DataMember(Name = "usage", IsRequired = false)]
	public double Usage { get; set; }

	/// <summary>
	/// description
	/// </summary>
	[DataMember(Name = "description", IsRequired = false)]
	public string Description { get; set; } = string.Empty;
}
