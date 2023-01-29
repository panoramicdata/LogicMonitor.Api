using System;
using System.Collections.Generic;
using System.Text;

namespace LogicMonitor.Api.Devices;

/// <summary>
/// DeviceDataSourceAssociated
/// </summary>

[DataContract]
public class DeviceDataSourceAssociated : NamedItem
{
	/// <summary>
	/// The instance list associated to the datasource
	/// </summary>
	[DataMember(Name = "instance", IsRequired = false)]
	public List<DeviceDataSourceAssociatedInstance>? Instances { get; set; }

	/// <summary>
	/// displayName
	/// </summary>
	[DataMember(Name = "displayName", IsRequired = false)]
	public string? DisplayName { get; set; }

	/// <summary>
	/// Whether has more instance. 0 no more, 1 has more
	/// </summary>
	[DataMember(Name = "hasMore", IsRequired = false)]
	public int HasMore { get; set; }

	/// <summary>
	/// Whether has active instance
	/// </summary>
	[DataMember(Name = "hasActiveInstance", IsRequired = false)]
	public bool HasActiveInstance { get; set; }
}
