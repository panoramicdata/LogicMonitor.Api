using System.Runtime.Serialization;

namespace LogicMonitor.Api.Collectors;

/// <summary>
/// A one time upgrade info
/// </summary>
[DataContract]
public class OneTimeUpgradeInfo
{
	/// <summary>
	///    The type
	/// </summary>
	[DataMember(Name = "type")]
	public string Type { get; set; }

	/// <summary>
	///    The description
	/// </summary>
	[DataMember(Name = "description")]
	public string Description { get; set; }

	/// <summary>
	///    Who it was created by
	/// </summary>
	[DataMember(Name = "createdBy")]
	public string CreatedBy { get; set; }

	/// <summary>
	///    The level
	/// </summary>
	[DataMember(Name = "level")]
	public string Level { get; set; }

	/// <summary>
	///    The major version
	/// </summary>
	[DataMember(Name = "majorVersion")]
	public int MajorVersion { get; set; }

	/// <summary>
	///    The minor version
	/// </summary>
	[DataMember(Name = "minorVersion")]
	public int MinorVersion { get; set; }

	/// <summary>
	///    The start Epoch
	/// </summary>
	[DataMember(Name = "startEpoch")]
	public long StartEpoch { get; set; }

	/// <summary>
	///    The end Epoch
	/// </summary>
	[DataMember(Name = "endEpoch")]
	public long EndEpoch { get; set; }
}
