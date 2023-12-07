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
	public Dictionary<string, List<List<object>>> DataValues { get; set; } = [];

	/// <summary>
	/// Data Points
	/// </summary>
	[DataMember(Name = "dataPoints")]
	public List<string> DataPoints { get; set; } = [];

	/// <summary>
	/// Data Points
	/// </summary>
	[DataMember(Name = "tzoffset")]
	public int TimeZoneOffsetMilliseconds { get; set; }
}
