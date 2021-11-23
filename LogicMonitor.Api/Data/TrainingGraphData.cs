namespace LogicMonitor.Api.Data;

/// <summary>
/// Training graph data
/// </summary>
[DataContract]
public class TrainingGraphData
{
	/// <summary>
	///    The milliseconds that the graph end time is since the Epoch
	/// </summary>
	[DataMember(Name = "endTime")]
	public long EndTimeTimestampMsUtc { get; set; }

	/// <summary>
	///    The graph end DateTime
	/// </summary>
	[IgnoreDataMember]
	public DateTime EndTimeUtc => (EndTimeTimestampMsUtc / 1000).ToDateTimeUtc();

	/// <summary>
	///    The milliseconds that the graph start time is since the Epoch
	/// </summary>
	[DataMember(Name = "startTime")]
	public long StartTimeTimestampMsUtc { get; set; }

	/// <summary>
	///    The graph start DateTime
	/// </summary>
	[IgnoreDataMember]
	public DateTime StartTimeUtc => (StartTimeTimestampMsUtc / 1000).ToDateTimeUtc();

	/// <summary>
	///    The x-axis timestamps in ms since the Epoch
	/// </summary>
	[DataMember(Name = "timestamps")]
	public List<long> TimeStamps { get; set; }

	/// <summary>
	///    The graph lines
	/// </summary>
	[DataMember(Name = "lines")]
	public List<Line> Lines { get; set; }
}
