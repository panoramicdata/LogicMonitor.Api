namespace LogicMonitor.Api.Data;

/// <summary>
/// A run report response
/// </summary>
[DataContract]
public class RunReportResponse
{
	/// <summary>
	/// The report ID
	/// </summary>
	[DataMember(Name = "reportId")]
	public int ReportId { get; set; }

	/// <summary>
	/// The task ID
	/// </summary>
	[DataMember(Name = "taskId")]
	public long TaskId { get; set; }

	/// <summary>
	/// The result URL
	/// </summary>
	[DataMember(Name = "resulturl")]
	public string ResultUrl { get; set; } = string.Empty;
}
