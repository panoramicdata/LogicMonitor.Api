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
	[DataMember(Name = "srcIP")]
	public string SrcIP { get; set; } = string.Empty;

	/// <summary>
	/// percentUsage
	/// </summary>
	[DataMember(Name = "percentUsage")]
	public double PercentUsage { get; set; }

	/// <summary>
	/// lastEpochInSec
	/// </summary>
	[DataMember(Name = "lastEpochInSec")]
	public long lastEpochInSec { get; set; }

	/// <summary>
	/// ifOut
	/// </summary>
	[DataMember(Name = "ifOut")]
	public long ifOut { get; set; }

	/// <summary>
	/// usage
	/// </summary>
	[DataMember(Name = "usage")]
	public double Usage { get; set; }

	/// <summary>
	/// srcASN
	/// </summary>
	[DataMember(Name = "srcASN")]
	public long SrcASN { get; set; }

	/// <summary>
	/// dstDNS
	/// </summary>
	[DataMember(Name = "dstDNS")]
	public string DstDNS { get; set; } = string.Empty;

	/// <summary>
	/// srcPort
	/// </summary>
	[DataMember(Name = "srcPort")]
	public int SrcPort { get; set; }

	/// <summary>
	/// deviceDisplayName
	/// </summary>
	[DataMember(Name = "deviceDisplayName")]
	public string DeviceDisplayName { get; set; } = string.Empty;

	/// <summary>
	/// ifInDisplayName
	/// </summary>
	[DataMember(Name = "ifInDisplayName")]
	public string IfInDisplayName { get; set; } = string.Empty;

	/// <summary>
	/// firstEpochInSec
	/// </summary>
	[DataMember(Name = "firstEpochInSec")]
	public long FirstEpochInSec { get; set; }

	/// <summary>
	/// protocol
	/// </summary>
	[DataMember(Name = "protocol")]
	public string Protocol { get; set; } = string.Empty;

	/// <summary>
	/// dstPort
	/// </summary>
	[DataMember(Name = "dstPort")]
	public int DstPort { get; set; }

	/// <summary>
	/// ifIn
	/// </summary>
	[DataMember(Name = "ifIn")]
	public long IfIn { get; set; }

	/// <summary>
	/// sourceMBytes
	/// </summary>
	[DataMember(Name = "sourceMBytes")]
	public double SourceMBytes { get; set; }

	/// <summary>
	/// srcAsnName
	/// </summary>
	[DataMember(Name = "srcAsnName")]
	public string SrcAsnName { get; set; } = string.Empty;

	/// <summary>
	/// srcDNS
	/// </summary>
	[DataMember(Name = "srcDNS")]
	public string SrcDNS { get; set; } = string.Empty;

	/// <summary>
	/// destinationMBytes
	/// </summary>
	[DataMember(Name = "destinationMBytes")]
	public double DestinationMBytes { get; set; }

	/// <summary>
	/// dstASN
	/// </summary>
	[DataMember(Name = "dstASN")]
	public long DstASN { get; set; }

	/// <summary>
	/// dstIP
	/// </summary>
	[DataMember(Name = "dstIP")]
	public string DstIP { get; set; } = string.Empty;

	/// <summary>
	/// ifOutDisplayName
	/// </summary>
	[DataMember(Name = "ifOutDisplayName")]
	public string IfOutDisplayName { get; set; } = string.Empty;

	/// <summary>
	/// destAsnName
	/// </summary>
	[DataMember(Name = "destAsnName")]
	public string DestAsnName { get; set; } = string.Empty;
}
