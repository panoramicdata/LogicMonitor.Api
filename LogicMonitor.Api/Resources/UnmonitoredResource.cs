namespace LogicMonitor.Api.Resources;

/// <summary>
///    An unmonitored Resource
/// </summary>
[DataContract]
public class UnmonitoredResource : IdentifiedItem, IHasEndpoint
{
	/// <summary>
	///    The status
	/// </summary>
	[DataMember(Name = "status")]
	public string Status { get; set; } = string.Empty;

	/// <summary>
	///    The IP
	/// </summary>
	[DataMember(Name = "ip")]
	public string IpAddress { get; set; } = string.Empty;

	/// <summary>
	///    The DNS
	/// </summary>
	[DataMember(Name = "dns")]
	public string Dns { get; set; } = string.Empty;

	/// <summary>
	///    The forward IP
	/// </summary>
	[DataMember(Name = "forwardIp")]
	public string ForwardIp { get; set; } = string.Empty;

	/// <summary>
	///    The ports
	/// </summary>
	[DataMember(Name = "ports")]
	public string Ports { get; set; } = string.Empty;

	/// <summary>
	///    The SysName
	/// </summary>
	[DataMember(Name = "sysName")]
	public string SysName { get; set; } = string.Empty;

	/// <summary>
	///    The displayAs
	/// </summary>
	[DataMember(Name = "displayAs")]
	public string DisplayAs { get; set; } = string.Empty;

	/// <summary>
	///    The Netscan id
	/// </summary>
	[DataMember(Name = "nspId")]
	public int NetscanId { get; set; }

	/// <summary>
	///    The Netscan name
	/// </summary>
	[DataMember(Name = "nspName")]
	public string NetscanName { get; set; } = string.Empty;

	/// <summary>
	///    The Netscan Event Id
	/// </summary>
	[DataMember(Name = "nseId")]
	public int NetscanEventId { get; set; }

	/// <summary>
	///    The Netscan Event Id
	/// </summary>
	[DataMember(Name = "nseScanId")]
	public string NetscanEventScanId { get; set; } = string.Empty;

	/// <summary>
	///    The end timestamp
	/// </summary>
	[DataMember(Name = "endTimestamp")]
	public int EndDateTimeUtcSeconds { get; set; }

	/// <summary>
	///    The UTC DateTime
	/// </summary>
	[IgnoreDataMember]
	public DateTime EndDateTimeUtc => EndDateTimeUtcSeconds.ToDateTimeUtc();

	/// <summary>
	///    The human-readable EndDate
	/// </summary>
	[DataMember(Name = "endDate")]
	public string EndDateTimeLocal { get; set; } = string.Empty;

	/// <summary>
	///    The Resource type
	/// </summary>
	[DataMember(Name = "deviceType")]
	public string ResourceType { get; set; } = string.Empty;

	/// <summary>
	/// Obsolete
	/// </summary>
	[JsonIgnore, IgnoreDataMember]
	[Obsolete("Use ResourceType", true)]
	public string DeviceType => ResourceType;

	/// <summary>
	///    Whether collector id
	/// </summary>
	[DataMember(Name = "collectorId")]
	public int CollectorId { get; set; }

	/// <summary>
	///    The collector description
	/// </summary>
	[DataMember(Name = "collectorDescription")]
	public string CollectorDescription { get; set; } = string.Empty;

	/// <summary>
	///    The device status
	/// </summary>
	[DataMember(Name = "deviceStatus")]
	public string ResourceStatus { get; set; } = string.Empty;

	/// <summary>
	/// Obsolete
	/// </summary>
	[JsonIgnore, IgnoreDataMember]
	[Obsolete("Use ResourceStatus", true)]
	public string DeviceStatus => ResourceStatus;

	/// <summary>
	///    The manufacturer
	/// </summary>
	[DataMember(Name = "manufacturer")]
	public string Manufacturer { get; set; } = string.Empty;

	/// <inheritdoc />
	public string Endpoint() => "device/unmonitoreddevices";
}
