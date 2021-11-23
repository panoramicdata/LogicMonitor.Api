using System.Runtime.Serialization;

namespace LogicMonitor.Api.Netscans;

/// <summary>
/// Netscan ports
/// </summary>
[DataContract]
public class NetscanPorts
{
	/// <summary>
	/// Whether this is a global default
	/// </summary>
	[DataMember(Name = "isGlobalDefault")]
	public bool IsGlobalDefault { get; set; }

	/// <summary>
	/// A comma-separated list of positive integers
	/// </summary>
	[DataMember(Name = "value")]
	public string Value { get; set; }
}
