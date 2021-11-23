namespace LogicMonitor.Api.Data;

/// <summary>
/// An instance FetchData response
/// </summary>
[DataContract]
public class FetchDataResponse
{
	/// <summary>
	/// The total count
	/// </summary>
	[DataMember(Name = "total")]
	public int TotalCount { get; set; }

	/// <summary>
	/// The instance FetchData responses
	/// </summary>
	[DataMember(Name = "items")]
	public List<InstanceFetchDataResponse> InstanceFetchDataResponses { get; set; }

	/// <summary>
	/// The searh id
	/// </summary>
	[DataMember(Name = "searchId")]
	public object SearchId { get; set; }

	/// <summary>
	/// Whether it is min
	/// </summary>
	[DataMember(Name = "isMin")]
	public bool IsMin { get; set; }
}
