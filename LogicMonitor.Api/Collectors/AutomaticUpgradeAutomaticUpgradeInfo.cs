namespace LogicMonitor.Api.Collectors;

/// <summary>
/// An automatic upgrade automatic upgrade info
/// </summary>
[DataContract]
public class AutomaticUpgradeAutomaticUpgradeInfo : AutomaticUpgradeInfo
{
	/// <summary>
	/// The description
	/// </summary>
	[DataMember(Name = "description")]
	public string Description { get; set; } = string.Empty;

	/// <summary>
	/// Who this was created by
	/// </summary>
	[DataMember(Name = "createdBy")]
	public string CreatedBy { get; set; } = string.Empty;

	/// <summary>
	/// The level
	/// </summary>
	[DataMember(Name = "level")]
	public string Level { get; set; } = string.Empty;

	/// <summary>
	/// The version
	/// </summary>
	[DataMember(Name = "version")]
	public string Version { get; set; } = string.Empty;

	/// <summary>
	/// The day of week
	/// </summary>
	[DataMember(Name = "dayOfWeek")]
	public string DayOfWeek { get; set; } = string.Empty;

	/// <summary>
	/// The occurrence
	/// </summary>
	[DataMember(Name = "occurrence")]
	public string Occurrence { get; set; } = string.Empty;

	/// <summary>
	/// The hour
	/// </summary>
	[DataMember(Name = "hour")]
	public int Hour { get; set; }

	/// <summary>
	/// The minute
	/// </summary>
	[DataMember(Name = "minute")]
	public int Minute { get; set; }
}
