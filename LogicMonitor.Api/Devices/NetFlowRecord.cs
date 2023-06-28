namespace LogicMonitor.Api.Devices;

/// <summary>
/// NetFlowRecord
/// </summary>

[DataContract]
public class NetFlowRecord
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
	public int LastEpochInSec { get; set; }

	/// <summary>
	/// ifOut
	/// </summary>
	[DataMember(Name = "ifOut")]
	public int IfOut { get; set; }

	/// <summary>
	/// dataType
	/// </summary>
	[DataMember(Name = "dataType")]
	public string DataType { get; set; } = string.Empty;

	/// <summary>
	/// usage
	/// </summary>
	[DataMember(Name = "usage")]
	public double Usage { get; set; }

	/// <summary>
	/// srcASN
	/// </summary>
	[DataMember(Name = "srcASN")]
	public int SrcASN { get; set; }

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
	/// firstEpochInSec
	/// </summary>
	[DataMember(Name = "firstEpochInSec")]
	public int FirstEpochInSec { get; set; }

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
	public int IfIn { get; set; }

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
	public int DstASN { get; set; }

	/// <summary>
	/// dstIP
	/// </summary>
	[DataMember(Name = "dstIP")]
	public string DstIP { get; set; } = string.Empty;

	/// <summary>
	/// destAsnName
	/// </summary>
	[DataMember(Name = "destAsnName")]
	public string DestAsnName { get; set; } = string.Empty;
}
