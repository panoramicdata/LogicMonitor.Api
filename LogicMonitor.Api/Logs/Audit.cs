namespace LogicMonitor.Api.Logs;

/// <summary>
/// An audit
/// </summary>
[DataContract]
public class Audit
{
	/// <summary>
	/// value of audit version
	/// </summary>
	[DataMember(Name = "version")]
	public int Version { get; set; }
}
