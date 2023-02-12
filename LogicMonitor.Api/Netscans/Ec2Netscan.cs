using System;
using System.Collections.Generic;
using System.Text;

namespace LogicMonitor.Api.Netscans;

/// <summary>
/// Ec2Netscan
/// </summary>

[DataContract]
public class Ec2Netscan : Netscan
{
	/// <summary>
	/// The Discovered device rules
	/// </summary>
	[DataMember(Name = "ddr", IsRequired = false)]
	public Ec2DDR? Ddr { get; set; }

	/// <summary>
	/// The credentials to be used for the scan
	/// </summary>
	[DataMember(Name = "credentials", IsRequired = true)]
	public EC2NetscanPolicyCredential Credentials { get; set; } = null!;

	/// <summary>
	/// Which IP the EC2 instance should be monitored with for nec2 scans: private or public
	/// </summary>
	[DataMember(Name = "accessibility", IsRequired = true)]
	public string Accessibility { get; set; } = null!;

	/// <summary>
	/// How dead EC2 instances should be handled for nec2 scans. Must be Manually
	/// </summary>
	[DataMember(Name = "deadOperation", IsRequired = false)]
	public string? DeadOperation { get; set; }

	/// <summary>
	/// The ports
	/// </summary>
	[DataMember(Name = "ports", IsRequired = false)]
	public RestNetscanPorts? Ports { get; set; }
}
