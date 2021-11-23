namespace LogicMonitor.Api.Data;

/// <summary>
///    Graph Data
/// </summary>
[DataContract]
public class GraphData : NamedItem
{
	/// <summary>
	///    Data type
	/// </summary>
	[DataMember(Name = "instances")]
	public List<object> Instances { get; set; }

	/// <summary>
	///    Data type
	/// </summary>
	[DataMember(Name = "type")]
	public string Type { get; set; }

	/// <summary>
	///    Timescale
	/// </summary>
	[DataMember(Name = "timescale")]
	public string TimeScale { get; set; }

	/// <summary>
	///    Time Zone
	/// </summary>
	[DataMember(Name = "timezone")]
	public string TimeZone { get; set; }

	/// <summary>
	///    Time Zone Id
	/// </summary>
	[DataMember(Name = "timeZoneId")]
	public string TimeZoneId { get; set; }

	/// <summary>
	///    Status
	/// </summary>
	[DataMember(Name = "status")]
	public Status Status { get; set; }

	/// <summary>
	///    Title
	/// </summary>
	[DataMember(Name = "title")]
	public string TitleString { get; set; }

	/// <summary>
	///    The end timezone offset
	/// </summary>
	[DataMember(Name = "endtzoffset")]
	public int EndTzOffset { get; set; }

	/// <summary>
	///    The start timezone offset
	/// </summary>
	[DataMember(Name = "starttzoffset")]
	public int StartTzOffset { get; set; }

	/// <summary>
	///    The minimum y-axis value
	/// </summary>
	[DataMember(Name = "minvalue")]
	public double? MinValue { get; set; }

	/// <summary>
	///    The maximum y-axis value
	/// </summary>
	[DataMember(Name = "maxvalue")]
	public double? MaxValue { get; set; }

	/// <summary>
	///    The x-axis name
	/// </summary>
	[DataMember(Name = "xaxisname")]
	public string XAxisName { get; set; }

	/// <summary>
	///    The y-axis label
	/// </summary>
	[DataMember(Name = "verticallabel")]
	public string VerticalLabel { get; set; }

	/// <summary>
	///    The height in pixels
	/// </summary>
	[DataMember(Name = "height")]
	public int Height { get; set; }

	/// <summary>
	///    The XML title
	/// </summary>
	[DataMember(Name = "xmlTitle")]
	public string XmlTitleString { get; set; }

	/// <summary>
	///    The display priority
	/// </summary>
	[DataMember(Name = "displayPrio")]
	public int DisplayPriority { get; set; }

	/// <summary>
	///    The forecast period
	/// </summary>
	[DataMember(Name = "forecastPeriod")]
	public ForecastTimePeriod ForecastPeriod { get; set; }

	/// <summary>
	///    Whether base 1024 (or base 1000)
	/// </summary>
	[DataMember(Name = "base1024")]
	public bool Base1024 { get; set; }

	/// <summary>
	///    Whether active
	/// </summary>
	[DataMember(Name = "isActive")]
	public bool IsActive { get; set; }

	/// <summary>
	///    The width in pixels
	/// </summary>
	[DataMember(Name = "width")]
	public int Width { get; set; }

	/// <summary>
	///    The list of missing lines
	/// </summary>
	[DataMember(Name = "missingLines")]
	public List<string> MissingLines { get; set; }

	/// <summary>
	///    The XML y-axis label
	/// </summary>
	[DataMember(Name = "xmlVerticalLabel")]
	public string XmlVerticalLabel { get; set; }

	/// <summary>
	///    The data source name
	/// </summary>
	[DataMember(Name = "dsName")]
	public string DataSourceName { get; set; }

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
	///    The base
	/// </summary>
	[DataMember(Name = "base")]
	public int Base { get; set; }

	/// <summary>
	///    The x-axis timestamps in ms since the Epoch
	/// </summary>
	[DataMember(Name = "timestamps")]
	public List<long> TimeStamps { get; set; }

	/// <summary>
	///    Whether it is rigid
	/// </summary>
	[DataMember(Name = "rigid")]
	public bool Rigid { get; set; }

	/// <summary>
	///    The graph lines
	/// </summary>
	[DataMember(Name = "lines")]
	public List<Line> Lines { get; set; }

	/// <summary>
	///    The export filename
	/// </summary>
	[DataMember(Name = "exportFileName")]
	public string ExportFileName { get; set; }

	/// <summary>
	///    The step
	/// </summary>
	[DataMember(Name = "step")]
	public int Step { get; set; }

	/// <summary>
	///    The scopes
	/// </summary>
	[DataMember(Name = "scopes")]
	public List<GraphDataScope> Scopes { get; set; }

	/// <summary>
	///    The training graph data
	/// </summary>
	[DataMember(Name = "training")]
	public TrainingGraphData TrainingGraphData { get; set; }
}
