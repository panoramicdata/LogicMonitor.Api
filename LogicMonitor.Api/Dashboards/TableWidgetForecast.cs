namespace LogicMonitor.Api.Dashboards;

/// <summary>
/// A TableWidgetForecast
/// </summary>
[DataContract]
public class TableWidgetForecast
{
	/// <summary>
	///     The algorithm
	/// </summary>
	[DataMember(Name = "algorithm")]
	public string Algorithm { get; set; } = string.Empty;

	/// <summary>
	///     The confidence
	/// </summary>
	[DataMember(Name = "confidence")]
	public int Confidence { get; set; }

	/// <summary>
	///     The timeRange
	/// </summary>
	[DataMember(Name = "timeRange")]
	public string TimeRange { get; set; } = string.Empty;

	/// <summary>
	///     The severity
	/// </summary>
	[DataMember(Name = "severity")]
	public string Severity { get; set; } = string.Empty;
}
