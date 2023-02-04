namespace LogicMonitor.Api.Flows;

/// <summary>
/// A flow port
/// </summary>
[DataContract]
public class NetflowPort : NetflowDataBase
{
	/// <summary>
	/// Description
	/// </summary>
	[DataMember(Name = "description", IsRequired = false)]
	private string Description { get; set; } = string.Empty;

	/// <summary>
	/// Description
	/// </summary>
	[DataMember(Name = "port", IsRequired = false)]
	private int Port { get; set; }

	/// <summary>
	/// Percent Usage
	/// </summary>
	[DataMember(Name = "percentUsage", IsRequired = false)]
	private double PercentUsage { get; set; }

	/// <summary>
	/// Usage in bytes
	/// </summary>
	[DataMember(Name = "usage", IsRequired = false)]
	private long UsageBytes { get; set; }

	/// <summary>
	/// Returns a string that represents the current object.
	/// </summary>
	public override string ToString() => $"{Description} ({Port}) {UsageBytes:N0} {PercentUsage:F1}%";
}
