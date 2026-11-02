namespace LogicMonitor.Api.Settings;

/// <summary>
/// A DNS mapping for a range of IP addresses
/// </summary>
[DataContract]
public class DnsMapping
{
	/// <summary>
	/// The id
	/// </summary>
	[DataMember(Name = "id")]
	public int Id { get; set; }

	/// <summary>
	/// The collector description
	/// </summary>
	[DataMember(Name = "collectorDescription")]
	public string CollectorDescription { get; set; } = string.Empty;

	/// <summary>
	/// The collector id
	/// </summary>
	[DataMember(Name = "collectorId")]
	public int CollectorId { get; set; }

	/// <summary>
	/// The dns
	/// </summary>
	[DataMember(Name = "dns")]
	public string Dns { get; set; } = string.Empty;

	/// <summary>
	/// The end ip
	/// </summary>
	[DataMember(Name = "endIp")]
	public string EndIp { get; set; } = string.Empty;

	/// <summary>
	/// The start ip
	/// </summary>
	[DataMember(Name = "startIp")]
	public string StartIp { get; set; } = string.Empty;
}
