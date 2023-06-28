namespace LogicMonitor.Api.PushMetrics;

/// <summary>
/// A Push Metric Instance
/// </summary>
[DataContract]
public class PushMetricInstance
{
	/// <summary>
	/// Instance name.
	/// If no existing instance matches, a new instance is created with this name.
	/// Required
	/// * 255-character limit
	/// * All characters except , ; / * [ ] ? ' " ` ## and newline are allowed
	/// * Spaces allowed except at start or end
	/// * Should not contain backslashes(\).
	/// * Only supports characters from A-Z, a-z, 0-9, colon, hyphen, underscore, and full stop
	/// * No whitespace allowed
	/// </summary>
	[DataMember(Name = "instanceName")]
	public string Name { get; set; } = string.Empty;

	/// <summary>
	/// The display name
	/// Instance display name. Only considered when creating a new instance.
	/// Optional. Defaults to {instanceName}.
	/// * 255-character limit
	/// * All characters except , ; / * [ ] ? ' " ` ## and newline are allowed
	/// * Spaces allowed except at start or end
	/// * Should not contain backslashes(\).
	/// * *?,;`\\n&lt; characters not allowed
	/// </summary>
	[DataMember(Name = "instanceDisplayName")]
	public string DisplayName { get; set; } = string.Empty;

	/// <summary>
	/// The instance properties
	/// New properties for instance.
	/// Updates to existing instance properties are not considered.
	/// Depending on the property name, we will convert these properties into system, auto, or custom properties.
	/// Optional. Defaults to "".
	/// * Takes input as key-value pairs in the form of property name and assigned value
	/// * System properties are not allowed(example: system.xxx)
	/// * Auto properties are not allowed(example: auto.xxx)
	/// * Keys and values are strings
	/// * All characters except, ; / * [ ] ? ' " ` ## and newline are allowed
	/// * Spaces allowed except at start or end
	/// * Keys and values should not contain backslashes(\)
	/// * Keys have 255-character limits; values have 24000-character limits
	/// * Case insensitive
	/// * Keys and values should not be null, empty, or having trailing spaces
	/// </summary>
	[DataMember(Name = "instanceProperties")]
	public Dictionary<string, string> Properties { get; set; } = new();

	/// <summary>
	/// The DataPoints
	/// </summary>
	[DataMember(Name = "dataPoints")]
	public List<PushMetricDataPoint> DataPoints { get; set; } = new();
}
