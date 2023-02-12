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
	[DataMember(Name = "version", IsRequired = false)]
	public long Version { get; set; }
}
