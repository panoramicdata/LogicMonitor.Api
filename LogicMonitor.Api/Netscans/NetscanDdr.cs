using System.Collections.Generic;
using System.Runtime.Serialization;

namespace LogicMonitor.Api.Netscans;

/// <summary>
///    The Netscan discovered device rules
/// </summary>
[DataContract(Name = "ddr")]
public class NetscanDdr
{
	/// <summary>
	///    The assignment
	/// </summary>
	[DataMember(Name = "assignment")]
	public List<NetscanAssignment> Assignment { get; set; }

	/// <summary>
	///    The change name pattern
	/// </summary>
	[DataMember(Name = "changeName")]
	public object ChangeName { get; set; }
}
