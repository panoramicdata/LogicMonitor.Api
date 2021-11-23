using System.Collections.Generic;
using System.Runtime.Serialization;

namespace LogicMonitor.Api.LogicModules;

/// <summary>
/// The DataPoint configuration
/// </summary>
[DataContract]
public class DataPointConfigurationCollection
{
	/// <summary>
	/// The DataSource type
	/// </summary>
	[DataMember(Name = "datasourceType")]
	public string DataSourceType { get; set; }

	/// <summary>
	/// The items
	/// </summary>
	[DataMember(Name = "dpConfig")]
	public List<DataPointConfiguration> Items { get; set; }
}
