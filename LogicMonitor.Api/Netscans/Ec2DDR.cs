namespace LogicMonitor.Api.Netscans;

/// <summary>
///    The Netscan discovered device rules
/// </summary>
[DataContract]
public class Ec2DDR
{
	/// <summary>
	/// The assignment
	/// </summary>
	[DataMember(Name = "assignment", IsRequired = false)]
	public List<Assignment>? Assignment { get; set; }

	/// <summary>
	/// The change name pattern
	/// </summary>
	[DataMember(Name = "changeName", IsRequired = false)]
	public string? ChangeName { get; set; }
}
