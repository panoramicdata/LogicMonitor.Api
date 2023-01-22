using System;
using System.Collections.Generic;
using System.Text;

namespace LogicMonitor.Api.Data;

/// <summary>
/// Raw data values
/// </summary>

[DataContract]
public class RawDataValues
{
	/// <summary>
	/// datapoint values 2-D list
	/// </summary>
	[DataMember(Name = "values")]
	public object[][] Values { get; set; }

	/// <summary>
	/// timestamp list
	/// </summary>
	[DataMember(Name = "time")]
	public int[] Time { get; set; }

	/// <summary>
	/// the next page parameters
	/// </summary>
	[DataMember(Name = "nextPageParams")]
	public string NextPageParams { get; set; }
}
