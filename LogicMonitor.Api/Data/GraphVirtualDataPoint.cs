using System;
using System.Collections.Generic;
using System.Text;

namespace LogicMonitor.Api.Data;

/// <summary>
/// Graph virtual data point
/// </summary>

[DataContract]
public class GraphVirtualDataPoint
{
	/// <summary>
	/// the graph virtual data point rpn expression
	/// </summary>
	[DataMember(Name = "rpn")]
	public string Rpn { get; set; }

	/// <summary>
	/// the graph virtual data point name
	/// </summary>
	public string Name { get; set; }
}
