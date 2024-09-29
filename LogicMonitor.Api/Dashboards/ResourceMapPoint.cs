namespace LogicMonitor.Api.Dashboards;

/// <summary>
///    A device map point
/// </summary>
[DataContract]
public class ResourceMapPoint : MapPoint
{
	/// <summary>
	///    Constructor
	/// </summary>
	public ResourceMapPoint()
	{
		Type = "device";
	}

	/// <summary>
	///     The ResourceGroup full path
	/// </summary>
	[DataMember(Name = "deviceGroupFullPath")]
	public string ResourceGroupFullPath { get; set; } = string.Empty;

	/// <summary>
	/// Obsolete
	/// </summary>
	[JsonIgnore, IgnoreDataMember]
	[Obsolete("Use ResourceGroupFullPath instead", true)]
	public string DeviceGroupFullPath => ResourceGroupFullPath;

	/// <summary>
	///    The Resource DisplayName
	/// </summary>
	[DataMember(Name = "deviceDisplayName")]
	public string ResourceDisplayName { get; set; } = string.Empty;

	/// <summary>
	/// Obsolete
	/// </summary>
	[JsonIgnore, IgnoreDataMember]
	[Obsolete("Use ResourceDisplayName instead", true)]
	public string DeviceDisplayName => ResourceDisplayName;

	/// <summary>
	///    Whether it has location
	/// </summary>
	[DataMember(Name = "hasLocation")]
	public bool HasLocation { get; set; }
}
