using System.Runtime.Serialization;

namespace LogicMonitor.Api.Netscans;

/// <summary>
///    A Netscan Policy
/// </summary>
[DataContract]
public class NetscanGroupCreationDto : CreationDto<NetscanGroup>
{
	/// <summary>
	///    Name
	/// </summary>
	[DataMember(Name = "name")]
	public string Name { get; set; }

	/// <summary>
	///    Description
	/// </summary>
	[DataMember(Name = "description")]
	public string Description { get; set; }
}
