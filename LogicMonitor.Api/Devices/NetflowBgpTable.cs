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
	[DataMember(Name = "percentUsage")]
	public double PercentUsage { get; set; }

	/// <summary>
	/// asNumber
	/// </summary>
	[DataMember(Name = "asNumber")]
	public long AsNumber { get; set; }

	/// <summary>
	/// usage
	/// </summary>
	[DataMember(Name = "usage")]
	public double Usage { get; set; }

	/// <summary>
	/// description
	/// </summary>
	[DataMember(Name = "description")]
	public string Description { get; set; } = string.Empty;
}
