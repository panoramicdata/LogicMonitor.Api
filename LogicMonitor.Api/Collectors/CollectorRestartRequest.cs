namespace LogicMonitor.Api.Collectors;

/// <summary>
/// A request to restart a collector, optionally with new configuration files
/// </summary>
[DataContract]
public class CollectorRestartRequest
{
	/// <summary>
	/// The collector conf
	/// </summary>
	[DataMember(Name = "collectorConf")]
	public string CollectorConf { get; set; } = string.Empty;

	/// <summary>
	/// The collector size
	/// </summary>
	[DataMember(Name = "collectorSize")]
	public string CollectorSize { get; set; } = string.Empty;

	/// <summary>
	/// The sbproxy conf
	/// </summary>
	[DataMember(Name = "sbproxyConf")]
	public string SbproxyConf { get; set; } = string.Empty;

	/// <summary>
	/// The watchdog conf
	/// </summary>
	[DataMember(Name = "watchdogConf")]
	public string WatchdogConf { get; set; } = string.Empty;

	/// <summary>
	/// The website conf
	/// </summary>
	[DataMember(Name = "websiteConf")]
	public string WebsiteConf { get; set; } = string.Empty;

	/// <summary>
	/// The wrapper conf
	/// </summary>
	[DataMember(Name = "wrapperConf")]
	public string WrapperConf { get; set; } = string.Empty;
}
