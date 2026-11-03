namespace LogicMonitor.Api.Reports;

/// <summary>
/// The result of a report generation task
/// </summary>
[DataContract]
public class ReportTaskResult
{
	/// <summary>
	/// The id of the report
	/// </summary>
	[DataMember(Name = "reportId")]
	public int ReportId { get; set; }

	/// <summary>
	/// The url of the generated report
	/// </summary>
	[SantabaReadOnly]
	[DataMember(Name = "resulturl")]
	public string ResultUrl { get; set; } = string.Empty;

	/// <summary>
	/// The task id of the generating process
	/// </summary>
	[DataMember(Name = "taskId")]
	public long TaskId { get; set; }
}
