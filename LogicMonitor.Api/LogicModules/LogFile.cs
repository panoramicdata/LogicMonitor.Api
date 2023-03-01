namespace LogicMonitor.Api.LogicModules;

/// <summary>
/// An EventSource log file
/// </summary>
[DataContract]
public class LogFile
{
	/// <summary>
	/// The path of the log file to monitor
	/// </summary>
	[DataMember(Name = "path")]
	public string Path { get; set; } = string.Empty;

	/// <summary>
	/// Origin id
	/// </summary>
	[DataMember(Name = "originId")]
	public int? OriginId { get; set; }

	/// <summary>
	/// The regex or plain text to look for in the file and not trigger alert if found
	/// </summary>
	[DataMember(Name = "excludes")]
	public List<string> Excludes { get; set; } = new();

	/// <summary>
	/// Whether or not glob is used in the path
	/// </summary>
	[DataMember(Name = "useGlob")]
	public bool UseGlob { get; set; }

	/// <summary>
	/// The file encoding: default | auto | UTF-8 | UTF-16
	/// </summary>
	[DataMember(Name = "encoding")]
	public string Encoding { get; set; } = string.Empty;

	/// <summary>
	/// The regex or plain text to look for in the file and trigger alert if found
	/// </summary>
	[DataMember(Name = "matches")]
	public List<EventSourceMatch> Matches { get; set; } = new();
}
