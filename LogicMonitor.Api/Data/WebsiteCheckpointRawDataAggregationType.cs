namespace LogicMonitor.Api.Data;

/// <summary>
/// An aggregation type used by Website Checkpoint raw data requests
/// </summary>
[DataContract]
public enum WebsiteCheckpointRawDataAggregationType
{
	/// <summary>
	/// None
	/// </summary>
	None,

	/// <summary>
	/// First
	/// </summary>
	First,

	/// <summary>
	/// Last
	/// </summary>
	Last,

	/// <summary>
	/// Min
	/// </summary>
	Min,

	/// <summary>
	/// Max
	/// </summary>
	Max,

	/// <summary>
	/// Sum
	/// </summary>
	Sum,

	/// <summary>
	/// Average
	/// </summary>
	Average
}
