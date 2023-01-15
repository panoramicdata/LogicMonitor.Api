using System;
using System.Collections.Generic;
using System.Text;

namespace LogicMonitor.Api.Collectors;

/// <summary>
/// PaaSMongoDbCollectorAttribute
/// </summary>

[DataContract]
public class PaaSMongoDbCollectorAttribute : CollectorAttribute
{
	/// <summary>
	/// Period
	/// </summary>
	[DataMember(Name = "resourceUrl")]
	public string ResourceUrl { get; set;  }
}
