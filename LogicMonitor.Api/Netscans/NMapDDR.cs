namespace LogicMonitor.Api.Netscans;

/// <summary>
///    The Netscan discovered device rules
/// </summary>
[DataContract]
public class NMapDDR
{
	/// <summary>
	/// Information about how discovered devices are included in or excluded from monitoring. Assignment objects can be created for different types of discovered devices
	/// </summary>
	[DataMember(Name = "assignment")]
	public List<Assignment>? Assignment { get; set; }

	/// <summary>
	/// The change name pattern
	/// </summary>
	[DataMember(Name = "changeName")]
	public string? ChangeName { get; set; }
}
