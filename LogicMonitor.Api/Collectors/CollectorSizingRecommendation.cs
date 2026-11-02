namespace LogicMonitor.Api.Collectors;

/// <summary>
/// A collector sizing recommendation for one workload type
/// </summary>
[DataContract]
public class CollectorSizingRecommendation
{
	/// <summary>
	/// The count
	/// </summary>
	[DataMember(Name = "count")]
	public int Count { get; set; }

	/// <summary>
	/// The failover enabled
	/// </summary>
	[DataMember(Name = "failoverEnabled")]
	public bool FailoverEnabled { get; set; }

	/// <summary>
	/// The max capacity
	/// </summary>
	[DataMember(Name = "maxCapacity")]
	public double MaxCapacity { get; set; }

	/// <summary>
	/// The per collector load
	/// </summary>
	[DataMember(Name = "perCollectorLoad")]
	public double PerCollectorLoad { get; set; }

	/// <summary>
	/// The size
	/// </summary>
	[DataMember(Name = "size")]
	public string Size { get; set; } = string.Empty;

	/// <summary>
	/// The total load
	/// </summary>
	[DataMember(Name = "totalLoad")]
	public double TotalLoad { get; set; }

	/// <summary>
	/// The utilization percent
	/// </summary>
	[DataMember(Name = "utilizationPercent")]
	public double UtilizationPercent { get; set; }

	/// <summary>
	/// The workload type
	/// </summary>
	[DataMember(Name = "workloadType")]
	public string WorkloadType { get; set; } = string.Empty;
}
