namespace LogicMonitor.Api.Websites;

/// <summary>
/// A test location
/// </summary>
[DataContract]
public class TestLocation
{
	/// <summary>
	/// The Smg Ids
	/// </summary>
	[DataMember(Name = "smgIds")]
	public List<int> SmgIds { get; set; }

	/// <summary>
	/// Whether to poll from all
	/// </summary>
	[DataMember(Name = "all")]
	public bool All { get; set; }

	/// <summary>
	/// The collector Ids
	/// </summary>
	[DataMember(Name = "collectorIds")]
	public List<int> CollectorIds { get; set; }

	/// <summary>
	/// The collectors
	/// </summary>
	[DataMember(Name = "collectors")]
	public List<object> Collectors { get; set; }
}
