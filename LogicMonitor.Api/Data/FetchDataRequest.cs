namespace LogicMonitor.Api.Data;

/// <summary>
/// A fetch data request
/// </summary>
[DataContract]
public class FetchDataRequest
{
	/// <summary>
	/// The comma-separated instance ids
	/// </summary>
	[DataMember(Name = "instanceIds")]
	public string InstanceIds { get; set; }
}
