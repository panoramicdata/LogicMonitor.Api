namespace LogicMonitor.Api.Resources;

/// <summary>
/// NetflowDataBase
/// </summary>

[DataContract]
public class NetflowDataBase
{
	/// <summary>
	/// dataType
	/// </summary>
	[DataMember(Name = "dataType")]
	public string DataType { get; set; } = string.Empty;
}
