namespace LogicMonitor.Api.Settings;

/// <summary>
///    A time zone setting
/// </summary>
[DataContract(Name = "timezoneSetting")]
public class TimeZoneSetting
{
	/// <summary>
	///    Offset from UTC in seconds
	/// </summary>
	[DataMember(Name = "offset")]
	public int UtcOffsetSeconds { get; set; }

	/// <summary>
	///    Timezone as text
	/// </summary>
	[DataMember(Name = "timezone")]
	public string TimeZone { get; set; } = string.Empty;

	/// <summary>
	///    Timezone as text
	/// </summary>
	[DataMember(Name = "timezoneShortName")]
	public string ShortName { get; set; } = string.Empty;

	/// <summary>
	///    Timezone as text
	/// </summary>
	[DataMember(Name = "timestamp")]
	public long Timestamp { get; set; }
}
