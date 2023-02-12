using System;
using System.Collections.Generic;
using System.Text;

namespace LogicMonitor.Api.Netscans;

/// <summary>
/// ScriptNetscan
/// </summary>

[DataContract]
public class ScriptNetscan : Netscan
{
	/// <summary>
	/// The script path for an external script
	/// </summary>
	[DataMember(Name = "scriptPath", IsRequired = false)]
	public string? ScriptPath { get; set; }

	/// <summary>
	/// The full path of the default group to add discovered devices to
	/// </summary>
	[DataMember(Name = "defaultGroupFullPath", IsRequired = false)]
	public string? DefaultGroupFullPath { get; set; }

	/// <summary>
	/// The ID of the default group to add discovered devices to
	/// </summary>
	[DataMember(Name = "defaultGroup", IsRequired = false)]
	public int DefaultGroupName { get; set; }

	/// <summary>
	/// For embedded script scans, the groovy script contents
	/// </summary>
	[DataMember(Name = "groovyScript", IsRequired = false)]
	public string? GroovyScript { get; set; }

	/// <summary>
	/// The Linux script parameters
	/// </summary>
	[DataMember(Name = "linuxScriptParams", IsRequired = false)]
	public string? LinuxScriptParameters { get; set; }

	/// <summary>
	/// For script scans, the type of script. Options are embeded and external
	/// </summary>
	[DataMember(Name = "scriptType", IsRequired = true)]
	public NetscanScriptType ScriptType { get; set; }

	/// <summary>
	/// For embedded script scans, the groovy script parameters
	/// </summary>
	[DataMember(Name = "groovyScriptParams", IsRequired = false)]
	public string? GroovyScriptParameters { get; set; }

	/// <summary>
	/// The Windows script
	/// </summary>
	[DataMember(Name = "windowsScript", IsRequired = false)]
	public string? WindowsScript { get; set; }

	/// <summary>
	/// The Windows script parameters
	/// </summary>
	[DataMember(Name = "windowsScriptParams", IsRequired = false)]
	public string? WindowsScriptParameters { get; set; }

	/// <summary>
	/// The parameters for an external script
	/// </summary>
	[DataMember(Name = "scriptParams", IsRequired = false)]
	public string? ScriptParameters { get; set; }

	/// <summary>
	/// The Linux script
	/// </summary>
	[DataMember(Name = "linuxScript", IsRequired = false)]
	public string? LinuxScript { get; set; }
}
