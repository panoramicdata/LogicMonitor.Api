namespace LogicMonitor.Api.Logs;

/// <summary>
/// A saved log query
/// </summary>
[DataContract]
public class SavedLogQuery : StringIdentifiedItem
{
	/// <summary>
	/// Username associated with the log query
	/// </summary>
	[SantabaReadOnly]
	[DataMember(Name = "adminName")]
	public string AdminName { get; set; } = string.Empty;

	/// <summary>
	/// Time range
	/// </summary>
	[DataMember(Name = "duration")]
	public string Duration { get; set; } = string.Empty;

	/// <summary>
	/// End time epoch in ms for log filter
	/// </summary>
	[DataMember(Name = "endMs")]
	public long EndMs { get; set; }

	/// <summary>
	/// The favourite
	/// </summary>
	[DataMember(Name = "favourite")]
	public bool Favourite { get; set; }

	/// <summary>
	/// Group id
	/// </summary>
	[DataMember(Name = "groupId")]
	public int GroupId { get; set; }

	/// <summary>
	/// Log query group name
	/// </summary>
	[DataMember(Name = "groupName")]
	public string GroupName { get; set; } = string.Empty;

	/// <summary>
	/// The identifier
	/// </summary>
	[DataMember(Name = "identifier")]
	public LogQueryObjectIdentifier? Identifier { get; set; }

	/// <summary>
	/// The into rest response
	/// </summary>
	[DataMember(Name = "intoRestResponse")]
	public JObject? IntoRestResponse { get; set; }

	/// <summary>
	/// Is aggregate query or not
	/// </summary>
	[DataMember(Name = "isAggregate")]
	public bool IsAggregate { get; set; }

	/// <summary>
	/// Label for log filter
	/// </summary>
	[DataMember(Name = "label")]
	public string Label { get; set; } = string.Empty;

	/// <summary>
	/// The model
	/// </summary>
	[DataMember(Name = "model")]
	public string Model { get; set; } = string.Empty;

	/// <summary>
	/// Partitions associated with the query
	/// </summary>
	[DataMember(Name = "partitions")]
	public List<string> Partitions { get; set; } = [];

	/// <summary>
	/// Query for log filter
	/// </summary>
	[DataMember(Name = "query")]
	public string Query { get; set; } = string.Empty;

	/// <summary>
	/// The related into rest response
	/// </summary>
	[DataMember(Name = "relatedIntoRestResponse")]
	public JObject? RelatedIntoRestResponse { get; set; }

	/// <summary>
	/// Start time epoch in ms for log filter
	/// </summary>
	[DataMember(Name = "startMs")]
	public long StartMs { get; set; }

	/// <summary>
	/// The trackable
	/// </summary>
	[DataMember(Name = "trackable")]
	public bool Trackable { get; set; }

	/// <summary>
	/// Query grammar version
	/// </summary>
	[DataMember(Name = "version")]
	public string Version { get; set; } = string.Empty;

	/// <summary>
	/// The view config
	/// </summary>
	[DataMember(Name = "viewConfig")]
	public LogQueryViewConfig? ViewConfig { get; set; }
}
