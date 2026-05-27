namespace LogicMonitor.Api.LogicModules;

/// <summary>
/// A LogSource (LM Logs ingestion rule).
/// </summary>
[DataContract]
public class LogSource : LogicModule, IHasEndpoint
{
	/// <summary>
	/// The LogSource type.
	/// </summary>
	[DataMember(Name = "type")]
	public string Type { get; set; } = string.Empty;

	/// <summary>
	/// The applies-to expression used to select matching resources.
	/// </summary>
	[DataMember(Name = "appliesToScript")]
	public string AppliesTo { get; set; } = string.Empty;

	/// <summary>
	/// The LogSource group.
	/// </summary>
	[DataMember(Name = "group")]
	public string Group { get; set; } = string.Empty;

	/// <summary>
	/// The technical notes for the LogSource.
	/// </summary>
	[DataMember(Name = "technicalNotes")]
	public string TechnicalNotes { get; set; } = string.Empty;

	/// <summary>
	/// Tags assigned to the LogSource.
	/// </summary>
	[DataMember(Name = "tags")]
	public List<string> Tags { get; set; } = [];

	/// <summary>
	/// The collection method used by this LogSource.
	/// </summary>
	[DataMember(Name = "collectionMethod")]
	public string CollectionMethod { get; set; } = string.Empty;

	/// <summary>
	/// Collection interval details.
	/// </summary>
	[DataMember(Name = "collectionInterval")]
	public JObject CollectionInterval { get; set; } = new();

	/// <summary>
	/// Collection attribute details.
	/// </summary>
	[DataMember(Name = "collectionAttribute")]
	public JObject CollectionAttribute { get; set; } = new();

	/// <summary>
	/// Whether this LogSource targets collectors directly.
	/// </summary>
	[DataMember(Name = "applyToCollector")]
	public bool? ApplyToCollector { get; set; }

	/// <summary>
	/// Collector mapping details when apply-to-collector is enabled.
	/// </summary>
	[DataMember(Name = "collectorMappings")]
	public List<JObject> CollectorMappings { get; set; } = [];

	/// <summary>
	/// Include filters for matching inbound logs.
	/// </summary>
	[DataMember(Name = "filters")]
	public List<JObject> IncludeFilters { get; set; } = [];

	/// <summary>
	/// Extracted or assigned log fields.
	/// </summary>
	[DataMember(Name = "logFields")]
	public List<JObject> LogFields { get; set; } = [];

	/// <summary>
	/// Resource mapping rules.
	/// </summary>
	[DataMember(Name = "resourceMapping")]
	public List<JObject> ResourceMappings { get; set; } = [];

	/// <summary>
	/// Whether the LogSource is enabled.
	/// </summary>
	[DataMember(Name = "enabled")]
	public bool? IsEnabled { get; set; }

	/// <summary>
	/// Whether the LogSource is disabled.
	/// </summary>
	[DataMember(Name = "disabled")]
	public bool? IsDisabled { get; set; }

	/// <inheritdoc />
	public string Endpoint() => "setting/logsources";

	/// <inheritdoc />
	public override string ToString() => $"{Id} : {Name} ({Type})";
}