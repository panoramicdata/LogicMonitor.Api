namespace LogicMonitor.Api.Topologies;

/// <summary>
/// A Topology
/// </summary>
[DataContract]
public class Topology : NamedItem, IHasEndpoint
{
	/// <summary>
	///    Whether to collapse edges
	/// </summary>
	[DataMember(Name = "isCollapseEdges")]
	public bool CollapseEdges { get; set; }

	/// <summary>
	///    Whether to show related vertices
	/// </summary>
	[DataMember(Name = "isDisplayRelatedVertices")]
	public bool DisplayRelatedVertices { get; set; }

	/// <summary>
	///    The group id
	/// </summary>
	[DataMember(Name = "groupId")]
	public int GroupId { get; set; }

	/// <summary>
	///    The group by
	/// </summary>
	[DataMember(Name = "groupBy")]
	public object? GroupBy { get; set; }

	/// <summary>
	///    The hidden vertices
	/// </summary>
	[DataMember(Name = "hiddenVertices")]
	public List<Vertex>? HiddenVertices { get; set; }

	/// <summary>
	///    The layout
	/// </summary>
	[DataMember(Name = "layout")]
	public TopologyLayout Layout { get; set; } = new();

	/// <summary>
	///    The owner
	/// </summary>
	[DataMember(Name = "owner")]
	public string Owner { get; set; } = string.Empty;

	/// <summary>
	///    Whether the item is shareable
	/// </summary>
	[DataMember(Name = "sharable")]
	public bool Shareable { get; set; }

	/// <summary>
	///    Whether to show the alert status
	/// </summary>
	[DataMember(Name = "showAlertStatus")]
	public bool ShowAlertStatus { get; set; }

	/// <summary>
	///    Whether to show the edge status
	/// </summary>
	[DataMember(Name = "showEdgeStatus")]
	public bool ShowEdgeStatus { get; set; }

	/// <summary>
	///    Whether to show the alert status
	/// </summary>
	[DataMember(Name = "showUndiscovered")]
	public bool ShowUndiscovered { get; set; }

	/// <summary>
	///    The views
	/// </summary>
	[DataMember(Name = "views")]
	public List<TopologyView> Views { get; set; } = [];

	/// <summary>
	///    The connections
	/// </summary>
	[DataMember(Name = "connections")]
	public List<TopologyConnection> Connection { get; set; } = [];

	/// <summary>
	/// The REST API endpoint
	/// </summary>
	public string Endpoint() => "topology/topologies";
}
