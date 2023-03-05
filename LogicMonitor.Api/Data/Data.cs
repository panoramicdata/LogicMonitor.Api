namespace LogicMonitor.Api.Data;

/// <summary>
/// Raw LogicMonitor Data
/// </summary>
[DataContract]
public class Data
{
	/// <summary>
	/// Data Values
	/// </summary>
	[DataMember(Name = "values")]
	public Dictionary<string, object[][]> DataValues { get; set; } = new();

	/// <summary>
	/// Data Points
	/// </summary>
	[DataMember(Name = "dataPoints")]
	public string[] DataPoints { get; set; } = Array.Empty<string>();

	/// <summary>
	/// Data Points
	/// </summary>
	[DataMember(Name = "tzoffset")]
	public int TimeZoneOffsetMilliseconds { get; set; }
}
