namespace LogicMonitor.Api.Collectors;

/// <summary>
/// A collector event
/// </summary>
[DataContract]
public class CollectorEvent
{
	/// <summary>
	/// The id
	/// </summary>
	[DataMember(Name = "id")]
	public string Id { get; set; } = string.Empty;

	/// <summary>
	/// The collector build
	/// </summary>
	[DataMember(Name = "collectorBuild")]
	public string CollectorBuild { get; set; } = string.Empty;

	/// <summary>
	/// The collector description
	/// </summary>
	[DataMember(Name = "collectorDescription")]
	public string CollectorDescription { get; set; } = string.Empty;

	/// <summary>
	/// The collector id
	/// </summary>
	[DataMember(Name = "collectorId")]
	public int CollectorId { get; set; }

	/// <summary>
	/// The collector platform
	/// </summary>
	[DataMember(Name = "collectorPlatform")]
	public string CollectorPlatform { get; set; } = string.Empty;

	/// <summary>
	/// The event code
	/// </summary>
	[DataMember(Name = "eCode")]
	public int EventCode { get; set; }

	/// <summary>
	/// The event message
	/// </summary>
	[DataMember(Name = "eventMsg")]
	public string EventMessage { get; set; } = string.Empty;

	/// <summary>
	/// The report date on local
	/// </summary>
	[DataMember(Name = "reportDateOnLocal")]
	public string ReportDateOnLocal { get; set; } = string.Empty;

	/// <summary>
	/// The reported on
	/// </summary>
	[DataMember(Name = "reportedOn")]
	public long ReportedOn { get; set; }
}
