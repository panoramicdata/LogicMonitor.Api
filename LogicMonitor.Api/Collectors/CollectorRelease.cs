namespace LogicMonitor.Api.Collectors;

/// <summary>
///    Collector Release information
/// </summary>
[DataContract]
public class CollectorRelease
{
	/// <summary>
	///    The general deployment release
	/// </summary>
	[DataMember(Name = "gd")]
	public int Gd { get; set; }

	/// <summary>
	///    The early deployment release
	/// </summary>
	[DataMember(Name = "ed")]
	public int Ed { get; set; }
}
