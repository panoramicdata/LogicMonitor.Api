using System;
using System.Collections.Generic;
using System.Text;

namespace LogicMonitor.Api.Devices;

/// <summary>
/// GroupNetFlowRecord
/// </summary>

[DataContract]
public class GroupNetFlowRecord : NetflowDataBase
{
	/// <summary>
	/// srcIP
	/// </summary>
	[DataMember(Name = "srcIP", IsRequired = false)]
	public string? SrcIP { get; set; }

	/// <summary>
	/// percentUsage
	/// </summary>
	[DataMember(Name = "percentUsage", IsRequired = false)]
	public double PercentUsage { get; set; }

	/// <summary>
	/// lastEpochInSec
	/// </summary>
	[DataMember(Name = "lastEpochInSec", IsRequired = false)]
	public long lastEpochInSec { get; set; }

	/// <summary>
	/// ifOut
	/// </summary>
	[DataMember(Name = "ifOut", IsRequired = false)]
	public long ifOut { get; set; }

	/// <summary>
	/// usage
	/// </summary>
	[DataMember(Name = "usage", IsRequired = false)]
	public double Usage { get; set; }

	/// <summary>
	/// srcASN
	/// </summary>
	[DataMember(Name = "srcASN", IsRequired = false)]
	public long SrcASN { get; set; }

	/// <summary>
	/// dstDNS
	/// </summary>
	[DataMember(Name = "dstDNS", IsRequired = false)]
	public string? DstDNS { get; set; }

	/// <summary>
	/// srcPort
	/// </summary>
	[DataMember(Name = "srcPort", IsRequired = false)]
	public int SrcPort { get; set; }

	/// <summary>
	/// deviceDisplayName
	/// </summary>
	[DataMember(Name = "deviceDisplayName", IsRequired = false)]
	public string? DeviceDisplayName { get; set; }

	/// <summary>
	/// ifInDisplayName
	/// </summary>
	[DataMember(Name = "ifInDisplayName", IsRequired = false)]
	public string? IfInDisplayName { get; set; }

	/// <summary>
	/// firstEpochInSec
	/// </summary>
	[DataMember(Name = "firstEpochInSec", IsRequired = false)]
	public long FirstEpochInSec { get; set; }

	/// <summary>
	/// protocol
	/// </summary>
	[DataMember(Name = "protocol", IsRequired = false)]
	public string? Protocol { get; set; }

	/// <summary>
	/// dstPort
	/// </summary>
	[DataMember(Name = "dstPort", IsRequired = false)]
	public int DstPort { get; set; }

	/// <summary>
	/// ifIn
	/// </summary>
	[DataMember(Name = "ifIn", IsRequired = false)]
	public long IfIn { get; set; }

	/// <summary>
	/// sourceMBytes
	/// </summary>
	[DataMember(Name = "sourceMBytes", IsRequired = false)]
	public double SourceMBytes { get; set; }

	/// <summary>
	/// srcAsnName
	/// </summary>
	[DataMember(Name = "srcAsnName", IsRequired = false)]
	public string? SrcAsnName { get; set; }

	/// <summary>
	/// srcDNS
	/// </summary>
	[DataMember(Name = "srcDNS", IsRequired = false)]
	public string? SrcDNS { get; set; }

	/// <summary>
	/// destinationMBytes
	/// </summary>
	[DataMember(Name = "destinationMBytes", IsRequired = false)]
	public double DestinationMBytes { get; set; }

	/// <summary>
	/// dstASN
	/// </summary>
	[DataMember(Name = "dstASN", IsRequired = false)]
	public long DstASN { get; set; }

	/// <summary>
	/// dstIP
	/// </summary>
	[DataMember(Name = "dstIP", IsRequired = false)]
	public string? DstIP { get; set; }

	/// <summary>
	/// ifOutDisplayName
	/// </summary>
	[DataMember(Name = "ifOutDisplayName", IsRequired = false)]
	public string? IfOutDisplayName { get; set; }

	/// <summary>
	/// destAsnName
	/// </summary>
	[DataMember(Name = "destAsnName", IsRequired = false)]
	public string? DestAsnName { get; set; }
}
