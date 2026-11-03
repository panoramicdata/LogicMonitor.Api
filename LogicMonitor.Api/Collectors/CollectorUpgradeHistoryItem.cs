namespace LogicMonitor.Api.Collectors;

/// <summary>
/// A collector upgrade history entry
/// </summary>
[DataContract]
public class CollectorUpgradeHistoryItem
{
	/// <summary>
	/// The id
	/// </summary>
	[DataMember(Name = "id")]
	public string Id { get; set; } = string.Empty;

	/// <summary>
	/// The collector id
	/// </summary>
	[DataMember(Name = "collectorId")]
	public int CollectorId { get; set; }

	/// <summary>
	/// The collector type
	/// </summary>
	[DataMember(Name = "collectorType")]
	public int? CollectorType { get; set; }

	/// <summary>
	/// The happened on
	/// </summary>
	[DataMember(Name = "happenedOn")]
	public long HappenedOn { get; set; }

	/// <summary>
	/// The new version
	/// </summary>
	[DataMember(Name = "newVersion")]
	public string NewVersion { get; set; } = string.Empty;

	/// <summary>
	/// The notes
	/// </summary>
	[DataMember(Name = "notes")]
	public string Notes { get; set; } = string.Empty;

	/// <summary>
	/// The previous version
	/// </summary>
	[DataMember(Name = "previousVersion")]
	public string PreviousVersion { get; set; } = string.Empty;

	/// <summary>
	/// The status
	/// </summary>
	[DataMember(Name = "status")]
	public int Status { get; set; }
}
