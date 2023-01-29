namespace LogicMonitor.Api.LogicModules;

/// <summary>
/// DeviceDataSourceInstanceConfigDiff
/// </summary>
[DataContract]
public class DeviceDataSourceInstanceConfigDiff
{
	/// <summary>
	/// diff type, values can be : add|remove
	/// </summary>
	[DataMember(Name = "type")]
	public string Type { get; set; }

	/// <summary>
	/// configuration content
	/// </summary>
	[DataMember(Name = "content")]
	public string Content { get; set; }

	/// <summary>
	/// diff row number
	/// </summary>
	[DataMember(Name = "rowNo")]
	public int RowNo { get; set; }
}
