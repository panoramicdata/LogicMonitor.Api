namespace LogicMonitor.Api.Netscans;

/// <summary>
/// Netscan ports
/// </summary>
[DataContract]
public class NetscanPorts
{
	/// <summary>
	/// Whether or not default ports should be used
	/// </summary>
	[DataMember(Name = "isGlobalDefault")]
	public bool IsGlobalDefault { get; set; }

	/// <summary>
	/// The ports that should be used in the Netscan
	/// </summary>
	[DataMember(Name = "value")]
	public string? Value { get; set; }
}
