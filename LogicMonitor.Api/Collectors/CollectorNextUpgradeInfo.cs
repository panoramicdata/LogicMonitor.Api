namespace LogicMonitor.Api.Collectors;

/// <summary>
///    Information about the next upgrade
/// </summary>
[DataContract]
public class CollectorNextUpgradeInfo
{
	/// <summary>
	///    The type
	/// </summary>
	[DataMember(Name = "type")]
	public string Type { get; set; }

	/// <summary>
	///    Upgrade time
	/// </summary>
	[DataMember(Name = "upgradeTime")]
	public string UpgradeTime { get; set; }

	/// <summary>
	///    Major version
	/// </summary>
	[DataMember(Name = "majorVersion")]
	public int MajorVersion { get; set; }

	/// <summary>
	///    Minor version
	/// </summary>
	[DataMember(Name = "minorVersion")]
	public string MinorVersion { get; set; }

	/// <summary>
	///    Whether the upgrade is mandatory
	/// </summary>
	[DataMember(Name = "mandatory")]
	public bool Mandatory { get; set; }

	/// <summary>
	///    Whether the upgrade is stable
	/// </summary>
	[DataMember(Name = "stable")]
	public bool Stable { get; set; }

	/// <summary>
	///     The upgrade time in seconds since the Epoch
	/// </summary>
	[DataMember(Name = "upgradeTimeEpoch")]
	public long UpgradeTimeUtcSeconds { get; set; }
}
