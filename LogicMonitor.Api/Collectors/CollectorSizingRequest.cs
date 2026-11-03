namespace LogicMonitor.Api.Collectors;

/// <summary>
/// A request for collector sizing recommendations
/// </summary>
[DataContract]
public class CollectorSizingRequest
{
	/// <summary>
	/// The devices
	/// </summary>
	[DataMember(Name = "devices")]
	public Dictionary<string, CollectorSizingDeviceCount> Devices { get; set; } = [];

	/// <summary>
	/// The logs
	/// </summary>
	[DataMember(Name = "logs")]
	public Dictionary<string, CollectorSizingSourceCount> Logs { get; set; } = [];

	/// <summary>
	/// The netflow
	/// </summary>
	[DataMember(Name = "netflow")]
	public Dictionary<string, CollectorSizingSourceCount> Netflow { get; set; } = [];

	/// <summary>
	/// The options
	/// </summary>
	[DataMember(Name = "options")]
	public CollectorSizingOptions? Options { get; set; }

	/// <summary>
	/// The traps
	/// </summary>
	[DataMember(Name = "traps")]
	public Dictionary<string, CollectorSizingSourceCount> Traps { get; set; } = [];
}
