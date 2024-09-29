namespace LogicMonitor.Api.Dashboards;

/// <summary>
/// A Group Netflow widget
/// </summary>
[DataContract]
public class GroupNetflowWidget : Widget, IWidget
{
	/// <summary>
	///     The data type
	/// </summary>
	[DataMember(Name = "dataType")]
	public string DataType { get; set; } = string.Empty;

	/// <summary>
	///     The QoS type
	/// </summary>
	[DataMember(Name = "qosType")]
	public string QosType { get; set; } = string.Empty;

	/// <summary>
	/// The ResourceGroup id
	/// </summary>
	[DataMember(Name = "hostGroupId")]
	public int ResourceGroupId { get; set; }

	/// <summary>
	/// Obsolete
	/// </summary>
	[JsonIgnore, IgnoreDataMember]
	[Obsolete("Use ResourceGroupId", true)]
	public int DeviceGroupId => ResourceGroupId;

	/// <summary>
	///     The ResourceGroup name
	/// </summary>
	[DataMember(Name = "deviceGroupName")]
	public string ResourceGroupName { get; set; } = string.Empty;

	/// <summary>
	///	Obsolete
	/// </summary>
	[Obsolete("Use ResourceGroupName", true)]
	public string DeviceGroupName => ResourceGroupName;

	/// <summary>
	///     The row filters
	/// </summary>
	[DataMember(Name = "rowFilters")]
	public string RowFilters { get; set; } = string.Empty;
}
