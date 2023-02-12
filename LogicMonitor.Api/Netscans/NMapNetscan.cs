using System;
using System.Collections.Generic;
using System.Text;

namespace LogicMonitor.Api.Netscans;

/// <summary>
/// NMapNetscan
/// </summary>

[DataContract]
public class NMapNetscan : Netscan
{
	/// <summary>
	/// Include Network \u0026 Broadcast Address for CIDR based netscan
	/// </summary>
	[DataMember(Name = "includeNetworkAndBroadcast", IsRequired = true)]
	public bool IncludeNetworkAndBroadcast { get; set; }

	/// <summary>
	/// The subnet to scan for nmap scans
	/// </summary>
	[DataMember(Name = "subnet", IsRequired = true)]
	public string SubnetScanRange { get; set; } = null!;

	/// <summary>
	/// Information related to including / excluding discovered devices in / from monitoring
	/// </summary>
	[DataMember(Name = "ddr", IsRequired = false)]
	public NMapDDR? Ddr { get; set; }

	/// <summary>
	/// The credentials to be used for the scan
	/// </summary>
	[DataMember(Name = "credentials", IsRequired = true)]
	public RestNMapNetscanPolicyCredential Credentials { get; set; } = null!;

	/// <summary>
	/// The subnet to exclude from scanning from nmap scans
	/// </summary>
	[DataMember(Name = "exclude", IsRequired = false)]
	public string? Exclude { get; set; }

	/// <summary>
	/// The ports that should be used in the Netscan
	/// </summary>
	[DataMember(Name = "ports", IsRequired = false)]
	public RestNetscanPorts? Ports { get; set; }
}
