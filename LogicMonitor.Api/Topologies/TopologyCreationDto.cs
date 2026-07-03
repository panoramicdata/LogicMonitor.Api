namespace LogicMonitor.Api.Topologies;

/// <summary>
/// Topology (map) creation DTO
/// </summary>
[DataContract]
public class TopologyCreationDto : CreationDto<Topology>, IHasName
{
	/// <summary>
	/// Name
	/// </summary>
	[DataMember(Name = "name")]
	public string Name { get; set; } = string.Empty;

	/// <summary>
	/// The id of the topology group the map belongs to
	/// </summary>
	[DataMember(Name = "groupId")]
	public int GroupId { get; set; }

	/// <summary>
	/// The views (one per seed resource) that make up the map
	/// </summary>
	[DataMember(Name = "views")]
	public List<TopologyView> Views { get; set; } = [];

	/// <summary>
	/// The layout
	/// </summary>
	[DataMember(Name = "layout")]
	public TopologyLayout Layout { get; set; } = new();

	/// <summary>
	/// Manual connections (usually empty; edges are derived from topology data)
	/// </summary>
	[DataMember(Name = "connections")]
	public List<string> Connections { get; set; } = [];

	/// <summary>
	/// Whether to collapse edges
	/// </summary>
	[DataMember(Name = "isCollapseEdges")]
	public bool CollapseEdges { get; set; } = true;

	/// <summary>
	/// Whether to show related vertices
	/// </summary>
	[DataMember(Name = "isDisplayRelatedVertices")]
	public bool DisplayRelatedVertices { get; set; }

	/// <summary>
	/// Whether to show the alert status
	/// </summary>
	[DataMember(Name = "showAlertStatus")]
	public bool ShowAlertStatus { get; set; } = true;

	/// <summary>
	/// Whether to show the edge status
	/// </summary>
	[DataMember(Name = "showEdgeStatus")]
	public bool ShowEdgeStatus { get; set; }

	/// <summary>
	/// Whether to show undiscovered vertices
	/// </summary>
	[DataMember(Name = "showUndiscovered")]
	public bool ShowUndiscovered { get; set; }

	/// <summary>
	/// Whether the map is shareable. API-only accounts cannot own private items, so this defaults to true.
	/// </summary>
	[DataMember(Name = "sharable")]
	public bool Shareable { get; set; } = true;

	/// <summary>
	/// The hidden vertices
	/// </summary>
	[DataMember(Name = "hiddenVertices")]
	public List<Vertex>? HiddenVertices { get; set; }

	/// <summary>
	/// The group by
	/// </summary>
	[DataMember(Name = "groupBy")]
	public object? GroupBy { get; set; }

	/// <summary>
	/// The edge status configuration
	/// </summary>
	[DataMember(Name = "edgeStatusConfig")]
	public object? EdgeStatusConfig { get; set; }
}
