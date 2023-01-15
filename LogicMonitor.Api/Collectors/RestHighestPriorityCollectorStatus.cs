using System;
using System.Collections.Generic;
using System.Text;

namespace LogicMonitor.Api.Collectors;

/// <summary>
/// RestHighestPriorityCollectorStatus
/// </summary>
[DataContract]
public class RestHighestPriorityCollectorStatus
{
	/// <summary>
	/// The SDT status of the highest priority sub collector
	/// </summary>
	[DataMember(Name = "inSDT")]
	public bool InSDT { get; set; }

	/// <summary>
	/// The acked status of the highest priority sub collector
	/// </summary>
	[DataMember(Name = "acked")]
	public bool Acked { get; set; }

	/// <summary>
	/// The down status of the highest priority sub collector
	/// </summary>
	[DataMember(Name = "isDown")]
	public bool IsDown { get; set; }

	/// <summary>
	/// The status of the highest priority sub collector
	/// </summary>
	[DataMember(Name = "status")]
	public int Status { get; set; }
}
