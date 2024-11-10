namespace LogicMonitor.Api.LogicModules;

/// <summary>
/// A collection of devices to which the DataSource applies
/// </summary>
[DataContract]
public class CollectionDataSourceAppliesTo : IdentifiedItem
{
	/// <summary>
	/// The DataSource Id
	/// </summary>
	[DataMember(Name = "dataSourceId")]
	public int DataSourceId { get; set; }

	/// <summary>
	/// The dataSourceDisplayName
	/// </summary>
	[DataMember(Name = "dataSourceDisplayName")]
	public string DataSourceDisplayName { get; set; } = string.Empty;

	/// <summary>
	/// The dataSourceGroupName
	/// </summary>
	[DataMember(Name = "dataSourceGroupName")]
	public string DataSourceGroupName { get; set; } = string.Empty;

	/// <summary>
	/// The ResourceGroup id
	/// </summary>
	[DataMember(Name = "deviceGroupId")]
	public int ResourceGroupId { get; set; }

	/// <summary>
	/// The stopMonitoringstopMonitoring
	/// </summary>
	[DataMember(Name = "stopMonitoring")]
	public bool StopMonitoring { get; set; }

	/// <summary>
	/// The disableAlerting
	/// </summary>
	[DataMember(Name = "disableAlerting")]
	public bool DisableAlerting { get; set; }

	/// <summary>
	/// The DataSource Id
	/// </summary>
	[DataMember(Name = "dataSourceDeviceList")]
	public List<DataSourceResource> DataSourceResources { get; set; } = [];

	/// <summary>
	/// Returns a string that represents the current object.
	/// </summary>
	public override string ToString()
		=> $"{DataSourceDisplayName} ({DataSourceId}) with {DataSourceResources?.Count.ToString(CultureInfo.InvariantCulture) ?? "0"} devices";
}
