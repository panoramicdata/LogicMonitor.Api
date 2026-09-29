namespace LogicMonitor.Api.Reports;

/// <summary>
/// The property filters a report applies when selecting resources and instances.
/// Each is null when the report does not filter on it.
/// </summary>
[DataContract]
public class PropertyFilterMetric
{
	/// <summary>
	/// The resource property filter
	/// </summary>
	[DataMember(Name = "resourceFilter")]
	public PropertyFilter? ResourceFilter { get; set; }

	/// <summary>
	/// The instance property filter. Only ever seen null, so its shape is assumed to match the resource filter.
	/// </summary>
	[DataMember(Name = "instanceFilter")]
	public PropertyFilter? InstanceFilter { get; set; }
}