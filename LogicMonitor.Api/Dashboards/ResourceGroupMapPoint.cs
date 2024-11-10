namespace LogicMonitor.Api.Dashboards;

/// <summary>
///    A ResourceGroup map point
/// </summary>
[DataContract]
public class ResourceGroupMapPoint : MapPoint
{
	/// <summary>
	///    Constructor
	/// </summary>
	public ResourceGroupMapPoint()
	{
		Type = "group";
	}

	/// <summary>
	///     The ResourceGroup full path
	/// </summary>
	[DataMember(Name = "deviceGroupFullPath")]
	public string ResourceGroupFullPath { get; set; } = string.Empty;

	/// <summary>
	///    The Resource DisplayName
	/// </summary>
	[DataMember(Name = "deviceDisplayName")]
	public string ResourceDisplayName { get; set; } = string.Empty;

	/// <summary>
	///    Whether it has location
	/// </summary>
	[DataMember(Name = "hasLocation")]
	public bool HasLocation { get; set; }
}
