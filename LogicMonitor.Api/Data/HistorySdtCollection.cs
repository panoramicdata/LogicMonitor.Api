namespace LogicMonitor.Api.Data;

/// <summary>
/// A collection of History SDTs
/// </summary>
[DataContract]
public class HistorySdtCollection
{
	/// <summary>
	/// The total number of items
	/// </summary>
	[DataMember(Name = "total")]
	public int Total { get; set; }

	/// <summary>
	/// The History SDT items
	/// </summary>
	[DataMember(Name = "items")]
	public List<ScheduledDownTimeHistory> Items { get; set; } = new();

	/// <summary>
	/// The search ID
	/// </summary>
	[DataMember(Name = "searchId")]
	public string SearchId { get; set; } = string.Empty;

	/// <summary>
	/// Is Min
	/// </summary>
	[DataMember(Name = "isMin")]
	public bool IsMin { get; set; }
}
