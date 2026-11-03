namespace LogicMonitor.Api.Collectors;

/// <summary>
/// A count of log, trap or NetFlow sources
/// </summary>
[DataContract]
public class CollectorSizingSourceCount
{
	/// <summary>
	/// The count
	/// </summary>
	[DataMember(Name = "count")]
	public int Count { get; set; }
}
