using System.Runtime.Serialization;

namespace LogicMonitor.Api.Devices;

/// <summary>
/// Manual discovery flags
/// </summary>
[DataContract]
public class ManualDiscoveryFlags
{
	/// <summary>
	/// Windows process
	/// </summary>
	[DataMember(Name = "winprocess")]
	public bool WindowsProcess { get; set; }

	/// <summary>
	/// Linux process
	/// </summary>
	[DataMember(Name = "linuxprocess")]
	public bool LinuxProcess { get; set; }

	/// <summary>
	/// Windows service
	/// </summary>
	[DataMember(Name = "winservice")]
	public bool WindowsService { get; set; }
}
