namespace LogicMonitor.Api.Devices;

/// <summary>
/// NetflowDataBase
/// </summary>

[DataContract]
public class NetflowDataBase
{
	/// <summary>
	/// dataType
	/// </summary>
	[DataMember(Name = "dataType", IsRequired = false)]
	public string DataType { get; set; }
}
