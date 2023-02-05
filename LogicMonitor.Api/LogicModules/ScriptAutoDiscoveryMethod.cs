using System;
using System.Collections.Generic;
using System.Text;

namespace LogicMonitor.Api.LogicModules;

/// <summary>
/// ScriptAutoDiscoveryMethod
/// </summary>

[DataContract]
public class ScriptAutoDiscoveryMethod : AutoDiscoveryMethod
{
	/// <summary>
	/// The Windows script
	/// </summary>
	[DataMember(Name = "winScript", IsRequired = false)]
	public string? WindowsScript { get; set; }

	/// <summary>
	/// The groovyScript
	/// </summary>
	[DataMember(Name = "groovyScript", IsRequired = false)]
	public string? GroovyScript { get; set; }

	/// <summary>
	/// Type
	/// </summary>
	[DataMember(Name = "type", IsRequired = true)]
	public string Type { get; set; } = null!;

	/// <summary>
	/// The Linux command line
	/// </summary>
	[DataMember(Name = "linuxCmdLine", IsRequired = false)]
	public string? LinuxCommandLine { get; set; }

	/// <summary>
	/// The Linux script
	/// </summary>
	[DataMember(Name = "linuxScript", IsRequired = false)]
	public string? LinuxScript { get; set; }

	/// <summary>
	/// The Windows command line
	/// </summary>
	[DataMember(Name = "winCmdLine", IsRequired = false)]
	public string? WindowsCommandLine { get; set; }
}
