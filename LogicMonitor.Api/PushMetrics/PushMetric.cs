namespace LogicMonitor.Api.PushMetrics;

/// <summary>
/// A Push Metric
/// </summary>
[DataContract]
public class PushMetric : IHasSingletonEndpoint
{
	/// <summary>
	/// Resource unique name. Only considered when creating a new resource.
	/// Required only if the create parameter is set to TRUE (create=true). FALSE is the default.
	/// * 255-character limit
	/// * Must be unique
	/// * Should not start or end with spaces, tabs, etc
	/// * Should not contain line breaks
	/// * Characters from A-Z, a-z, and 0-9 allowed as well as colons, hyphens, underscores and full stops
	/// * No whitespace allowed
	/// </summary>
	[DataMember(Name = "resourceName")]
	public string ResourceName { get; set; } = string.Empty;

	/// <summary>
	/// Resource description. Only considered when creating a new resource.
	/// Optional. Defaults to "".
	/// * 65535-character limit
	/// </summary>
	[DataMember(Name = "resourceDescription")]
	public string ResourceDescription { get; set; } = string.Empty;

	/// <summary>
	/// An array of existing resource properties that will be used to identify the resource.
	/// See Managing Resources that Ingest Push Metrics for information on the types of properties that can be used.
	/// If no resource is matched and the create parameter is set to TRUE, a new resource is created with these specified resource IDs set on it.
	/// If the system.displayname and/or system.hostname property is included as resource IDs, they will be used as host name and display name respectively in the resulting resource.
	/// Required.
	/// * Takes input as key-value pairs in the form of property name and assigned value(example: "system.displayname" : "mcentos")
	/// * Keys and values are strings
	/// * All characters except, ; / * [ ] ? ' " ` ## and newline are allowed
	/// * Spaces allowed except at start or end
	/// * Keys and values should not contain backslashes(\)
	/// * Null key and value are not allowed
	/// * Keys have 255-character limits; values have 24000-character limits
	/// * Case insensitive
	/// </summary>
	[DataMember(Name = "resourceIds")]
	public Dictionary<string, string> ResourceIds { get; set; } = new();

	/// <summary>
	/// New properties for resource.
	/// Updates to existing resource properties are not considered.
	/// Depending on the property name, we will convert these properties into system, auto, or custom properties.
	/// Optional. Defaults to "".
	/// * Takes input as key-value pairs in the form of property name and assigned value (Example: "version" : "5.0")
	/// * System properties are not allowed(example: system.xxx)
	/// * Auto properties are not allowed(example: auto.xxx)
	/// * Keys and values are strings
	/// * All characters except, ; / * [ ] ? ' " ` ## and newline are allowed
	/// * Spaces allowed except at start or end
	/// * Keys and values should not contain backslashes(\)
	/// * Null key and value are not allowed
	/// * Keys have 255-character limits; values have 24000-character limits
	/// * Case insensitive
	/// </summary>
	[DataMember(Name = "resourceProperties")]
	public Dictionary<string, string> ResourceProperties { get; set; } = new();

	/// <summary>
	/// The DataSource id
	/// Either dataSourceId or dataSource is mandatory.
	/// DataSource unique ID. Used only to match an existing DataSource. If no existing DataSource matches the provided ID, an error results.
	/// If this field is used in combination with the dataSource field, both the ID and name provided must match a single DataSource or an error results.
	/// * 9-digit limit
	/// * Only positive whole numbers allowed
	/// </summary>
	[DataMember(Name = "dataSourceId")]
	public string DataSourceId { get; set; } = string.Empty;

	/// <summary>
	/// DataSource unique name.
	/// Used to match an existing DataSource.
	/// If no existing DataSource matches the name provided here, a new DataSource is created with this name.
	/// If this field is used in combination with the dataSourceId field, both the ID and name provided must match a single DataSource or an error is returned.
	/// * 64-character limit
	/// * Must be unique
	/// * All characters except, ; / * [ ] ? ' " ` ## and newline are allowed
	/// * Spaces allowed except at start or end
	/// * Hyphen allowed only at the end; hyphen must be used with at least one other character
	/// </summary>
	[DataMember(Name = "dataSource")]
	public string DataSourceName { get; set; } = string.Empty;

	/// <summary>
	/// DataSource display name.
	/// Only considered when creating a new DataSource.
	/// Optional. Defaults to dataSource.
	/// * 64-character limit
	/// * All characters except , ; / * [ ] ? ' " ` ## and newline are allowed
	/// * Spaces allowed except at start or end
	/// * Keys and values should not contain backslashes(\)
	/// </summary>
	[DataMember(Name = "dataSourceDisplayName")]
	public string DataSourceDisplayName { get; set; } = string.Empty;

	/// <summary>
	/// The DataSource group
	/// DataSource group name.
	/// Only considered when DataSource does not already belong to a group.
	/// Used to organize the DataSource within a DataSource group.
	/// If no existing DataSource group matches, a new group is created with this name and the DataSource is organized under the new group.
	/// Optional. Defaults to "".
	/// </summary>
	[DataMember(Name = "dataSourceGroup")]
	public string DataSourceGroup { get; set; } = string.Empty;

	/// <summary>
	/// An array of DataSource instances.
	/// Required
	/// </summary>
	[DataMember(Name = "instances")]
	public List<PushMetricInstance> Instances { get; set; } = new();

	/// <inheritdoc />
	public string Endpoint() => "metric/ingest";
}
