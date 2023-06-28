namespace LogicMonitor.Api.Devices;

/// <summary>
/// NetflowNbarApplication
/// </summary>
[DataContract]
public class NetflowNbarApplication : NetflowDataBase
{
	/// <summary>
	///	percentUsage
	/// </summary>
	[DataMember(Name = "percentUsage")]
	public double PercentUsage { get; set; }

	/// <summary>
	///	usage
	/// </summary>
	[DataMember(Name = "usage")]
	public double Usage { get; set; }

	/// <summary>
	///	name
	/// </summary>
	[DataMember(Name = "name")]
	public string Name { get; set; } = string.Empty;

	/// <summary>
	///	type
	/// </summary>
	[DataMember(Name = "type")]
	public string Type { get; set; } = string.Empty;

	/// <summary>
	///	applicationTag
	/// </summary>
	[DataMember(Name = "applicationTag")]
	public long ApplicationTag { get; set; }
}
