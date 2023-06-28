using System;
using System.Collections.Generic;
using System.Text;

namespace LogicMonitor.Api.Settings;

/// <summary>
/// External API statistic information
/// </summary>
[DataContract]
    public class ExternalApiStats
    {
	/// <summary>
	/// Summary
	/// </summary>
	[DataMember(Name = "summary")]
	public string Summary { get; set; } = string.Empty;

	/// <summary>
	/// Total requests
	/// </summary>
	[DataMember(Name = "totalRequests")]
	public int TotalRequests { get; set; }

	/// <summary>
	/// Total waiting requests
	/// </summary>
	[DataMember(Name = "totalWaitingRequests")]
	public int TotalWaitingRequests { get; set; }

	/// <summary>
	/// Total nano time
	/// </summary>
	[DataMember(Name = "totNanoTime")]
	public int TotNanoTime { get; set; }

	/// <summary>
	/// Total processed requests
	/// </summary>
	[DataMember(Name = "totalProcessedRequests")]
	public int TotalProcessedRequests { get; set; }

	/// <summary>
	/// API
	/// </summary>
	[DataMember(Name = "api")]
	public string Api { get; set; } = string.Empty;

	/// <summary>
	/// Max nano time
	/// </summary>
	[DataMember(Name = "maxNanoTime")]
	public int MaxNanoTime { get; set; }

	/// <summary>
	/// Tags
	/// </summary>
	[DataMember(Name = "tags")]
	public List<string> Tags { get; set; } = new();
    }
