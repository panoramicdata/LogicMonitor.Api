namespace LogicMonitor.Api.Collectors;

/// <summary>
/// Acknowledgement of a Collector Down Alert
/// </summary>
[DataContract]
public class CollectorDownAcknowledgement
{
	/// <summary>
	/// The comment with which to acknowledge the collector being down
	/// </summary>
	[DataMember(Name = "comment")]
	public string Comment { get; set; } = string.Empty;
}
