namespace LogicMonitor.Api.LogicModules;

/// <summary>
/// ScriptERIDiscoveryAttributeV3
/// </summary>

[DataContract]
public class ScriptERIDiscoveryAttributeV3 : NamedItem
{
	/// <summary>
	/// windows script
	/// </summary>
	[DataMember(Name = "winScript")]
	public string WinScript { get; set; } = string.Empty;

	/// <summary>
	/// groovy script
	/// </summary>
	[DataMember(Name = "groovyScript")]
	public string GroovyScript { get; set; } = string.Empty;

	/// <summary>
	/// script type
	/// </summary>
	[DataMember(Name = "type")]
	public string Type { get; set; } = string.Empty;

	/// <summary>
	/// linux script command line
	/// </summary>
	[DataMember(Name = "linuxCmdline")]
	public string LinuxCmdLine { get; set; } = string.Empty;

	/// <summary>
	/// linux script file
	/// </summary>
	[DataMember(Name = "linuxScript")]
	public string LinuxScript { get; set; } = string.Empty;

	/// <summary>
	/// windows script command line
	/// </summary>
	[DataMember(Name = "winCmdline")]
	public string WindowsCmdLine { get; set; } = string.Empty;
}
