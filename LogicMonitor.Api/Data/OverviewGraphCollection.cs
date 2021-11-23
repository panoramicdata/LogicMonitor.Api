using LogicMonitor.Api.LogicModules;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace LogicMonitor.Api.Data;

/// <summary>
///    An overview graph collection
/// </summary>
[DataContract]
public class OverviewGraphCollection
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
	public List<DataSourceGraph> OverviewGraphs { get; set; }
}
