namespace LogicMonitor.Api.Dashboards;

/// <summary>
/// The response to an asynchronous dashboard group clone
/// </summary>
[DataContract]
public class DashboardGroupAsyncCloneResponse
{
	/// <summary>
	/// The jobId of the Clone Dashboard Group
	/// </summary>
	[SantabaReadOnly]
	[DataMember(Name = "jobId")]
	public int JobId { get; set; }
}
