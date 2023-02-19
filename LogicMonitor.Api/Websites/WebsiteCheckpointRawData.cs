using System;
using System.Collections.Generic;
using System.Text;

namespace LogicMonitor.Api.Websites;

/// <summary>
/// WebsiteCheckpointRawData
/// </summary>

[DataContract]
public class WebsiteCheckpointRawData
{
	/// <summary>
	/// datapoint values 2-D list
	/// </summary>
	[DataMember(Name = "values", IsRequired = false)]
	public object[]? Values { get; set; }

	/// <summary>
	/// timestamp list
	/// </summary>
	[DataMember(Name = "time", IsRequired = false)]
	public long[]? Time { get; set; }

	/// <summary>
	/// the next page parameters
	/// </summary>
	[DataMember(Name = "nextPageParams", IsRequired = false)]
	public string? NextPageParams { get; set; }
}
