namespace LogicMonitor.Api.Netscans;

/// <summary>
///    The Netscan discovered Resource rules
///    (was DDR - discovered device rule)
/// </summary>
[DataContract]
public class DiscoveredResourceRule
{
	/// <summary>
	/// The assignment
	/// </summary>
	[DataMember(Name = "assignment")]
	public List<NetscanAssignment>? Assignment { get; set; }

	/// <summary>
	/// The change name pattern
	/// </summary>
	[DataMember(Name = "changeName")]
	public string? ChangeName { get; set; }
}
