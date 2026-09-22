namespace LogicMonitor.Api.Reports;

/// <summary>
/// The property filters a report applies when selecting resources and instances.
/// Both are empty when the report filters on neither.
/// </summary>
[DataContract]
public class PropertyFilterMetric
{
	/// <summary>
	/// The resource property filter
	/// </summary>
	[DataMember(Name = "resourceFilter")]
	public string ResourceFilter { get; set; } = string.Empty;

	/// <summary>
	/// The instance property filter
	/// </summary>
	[DataMember(Name = "instanceFilter")]
	public string InstanceFilter { get; set; } = string.Empty;
}