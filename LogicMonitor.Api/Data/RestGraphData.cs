namespace LogicMonitor.Api.Data;

/// <summary>
///    REST Graph Data
/// </summary>
[DataContract]
public class RestGraphData : IdentifiedItem
{
	/// <summary>
	///    Timescale
	/// </summary>
	[DataMember(Name = "timeScale")]
	public string TimeScale { get; set; } = string.Empty;

	/// <summary>
	///    Time Zone
	/// </summary>
	[DataMember(Name = "timeZone")]
	public string TimeZone { get; set; } = string.Empty;

	/// <summary>
	///    Status
	/// </summary>
	[DataMember(Name = "status")]
	public Status Status { get; set; }

	/// <summary>
	///    Title
	/// </summary>
	[DataMember(Name = "title")]
	public string TitleString { get; set; } = string.Empty;

	/// <summary>
	///    The end time zone offset
	/// </summary>
	[DataMember(Name = "endTZOffset")]
	public int EndTimeZoneOffset { get; set; }

	/// <summary>
	/// Obsolete
	/// </summary>
	[JsonIgnore, IgnoreDataMember]
	[Obsolete("Use EndTzOffset", true)]
	public int EndTzOffset => EndTimeZoneOffset;

	/// <summary>
	///    The start time zone offset
	/// </summary>
	[DataMember(Name = "startTZOffset")]
	public int StartTimeZoneOffset { get; set; }

	/// <summary>
	/// Obsolete
	/// </summary>
	[JsonIgnore, IgnoreDataMember]
	[Obsolete("Use StartTzOffset", true)]
	public int StartTzOffset => StartTimeZoneOffset;

	/// <summary>
	///    The minimum y-axis value
	/// </summary>
	[DataMember(Name = "minValue")]
	public double? MinValue { get; set; }

	/// <summary>
	///    The maximum y-axis value
	/// </summary>
	[DataMember(Name = "maxValue")]
	public double? MaxValue { get; set; }

	/// <summary>
	///    The x-axis name
	/// </summary>
	[DataMember(Name = "xAxisName")]
	public string XAxisName { get; set; } = string.Empty;

	/// <summary>
	///    The y-axis label
	/// </summary>
	[DataMember(Name = "verticalLabel")]
	public string VerticalLabel { get; set; } = string.Empty;

	/// <summary>
	///    The height in pixels
	/// </summary>
	[DataMember(Name = "height")]
	public int Height { get; set; }

	/// <summary>
	///    The XML title
	/// </summary>
	[DataMember(Name = "xmlTitle")]
	public string XmlTitleString { get; set; } = string.Empty;

	/// <summary>
	///    The display priority
	/// </summary>
	[DataMember(Name = "displayPrio")]
	public int DisplayPriority { get; set; }

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
	public List<string> MissingLines { get; set; } = [];

	/// <summary>
	///    The XML y-axis label
	/// </summary>
	[DataMember(Name = "xmlVerticalLabel")]
	public string XmlVerticalLabel { get; set; } = string.Empty;

	/// <summary>
	///    The data source name
	/// </summary>
	[DataMember(Name = "dsName")]
	public string DataSourceName { get; set; } = string.Empty;

	/// <summary>
	///    The milliseconds that the graph start time is since the Epoch
	/// </summary>
	[DataMember(Name = "startTime")]
	public long StartTimeTimestampMsUtc { get; set; }

	/// <summary>
	///    The graph start DateTime
	/// </summary>
	public DateTime StartTimeUtc => (StartTimeTimestampMsUtc / 1000).ToDateTimeUtc();

	/// <summary>
	///    The milliseconds that the graph end time is since the Epoch
	/// </summary>
	[DataMember(Name = "endTime")]
	public long EndTimeTimestampMsUtc { get; set; }

	/// <summary>
	///    The graph end DateTime
	/// </summary>
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
	public List<long> TimeStamps { get; set; } = [];

	/// <summary>
	///    Whether it is rigid
	/// </summary>
	[DataMember(Name = "rigid")]
	public bool Rigid { get; set; }

	/// <summary>
	///    The graph lines
	/// </summary>
	[DataMember(Name = "lines")]
	public List<Line> Lines { get; set; } = [];

	/// <summary>
	///    The export filename
	/// </summary>
	[DataMember(Name = "exportFileName")]
	public string ExportFileName { get; set; } = string.Empty;

	/// <summary>
	///    The step
	/// </summary>
	[DataMember(Name = "step")]
	public int Step { get; set; }
}
