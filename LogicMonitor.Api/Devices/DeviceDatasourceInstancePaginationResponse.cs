using System;
using System.Collections.Generic;
using System.Text;

namespace LogicMonitor.Api.Devices;

/// <summary>
/// DeviceDatasourceInstancePaginationResponse
/// </summary>

[DataContract]
public class DeviceDatasourceInstancePaginationResponse
{
	/// <summary>
	/// total
	/// </summary>
	[DataMember(Name = "total", IsRequired = false)]
	public int Total { get; set; }

	/// <summary>
	/// searchId
	/// </summary>
	[DataMember(Name = "searchId", IsRequired = false)]
	public string? SearchId { get; set; }

	/// <summary>
	/// items
	/// </summary>
	[DataMember(Name = "items", IsRequired = false)]
	public List<DeviceDataSourceInstance>? Items { get; set; }
}
