namespace LogicMonitor.Api.Collectors;

/// <summary>
/// A collector log file
/// </summary>
[DataContract]
public class CollectorLogFile
{
	/// <summary>
	/// The file name
	/// </summary>
	[DataMember(Name = "fileName")]
	public string FileName { get; set; } = string.Empty;

	/// <summary>
	/// The last modified
	/// </summary>
	[DataMember(Name = "lastModified")]
	public long LastModified { get; set; }

	/// <summary>
	/// The last modified on on local
	/// </summary>
	[DataMember(Name = "lastModifiedOnOnLocal")]
	public string LastModifiedOnOnLocal { get; set; } = string.Empty;

	/// <summary>
	/// The size
	/// </summary>
	[DataMember(Name = "size")]
	public long Size { get; set; }

	/// <summary>
	/// The uploaded
	/// </summary>
	[DataMember(Name = "uploaded")]
	public long Uploaded { get; set; }
}
