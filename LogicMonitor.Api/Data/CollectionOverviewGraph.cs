namespace LogicMonitor.Api.Data;

/// <summary>
///    An overview graph collection
/// </summary>
[DataContract]
public class CollectionOverviewGraph
{
	/// <summary>
	///    Displayed As
	/// </summary>
	[DataMember(Name = "displayedAs")]
	public string DisplayedAs { get; set; }

	/// <summary>
	///    The DataSource InstanceGroup name
	/// </summary>
	[DataMember(Name = "dsigName")]
	public string DataSourceInstanceGroupName { get; set; }

	/// <summary>
	///    The overview Graphs
	/// </summary>
	[DataMember(Name = "ographs")]
	public List<DataSourceOverviewGraph> OverviewGraphs { get; set; }
}
