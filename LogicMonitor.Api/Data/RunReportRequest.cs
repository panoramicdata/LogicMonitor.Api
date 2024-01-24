namespace LogicMonitor.Api.Data;

/// <summary>
/// A run report request
/// </summary>
[DataContract]
public class RunReportRequest
{
	/// <summary>
	/// The With Admin ID
	/// </summary>
	[DataMember(Name = "withAdminId")]
	public int WithAdminId { get; set; } = 0;
}
