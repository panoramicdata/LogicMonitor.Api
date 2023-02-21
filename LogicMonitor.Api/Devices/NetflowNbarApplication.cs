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
	[DataMember(Name = "percentUsage", IsRequired = false)]
	public double PercentUsage { get; set; }

	/// <summary>
	///	usage
	/// </summary>
	[DataMember(Name = "usage", IsRequired = false)]
	public double Usage { get; set; }

	/// <summary>
	///	name
	/// </summary>
	[DataMember(Name = "name", IsRequired = false)]
	public string Name { get; set; } = string.Empty;

	/// <summary>
	///	type
	/// </summary>
	[DataMember(Name = "type", IsRequired = false)]
	public string Type { get; set; } = string.Empty;

	/// <summary>
	///	applicationTag
	/// </summary>
	[DataMember(Name = "applicationTag", IsRequired = false)]
	public long ApplicationTag { get; set; }
}
