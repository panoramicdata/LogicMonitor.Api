namespace LogicMonitor.Api.Devices;

/// <summary>
/// A Resource DataSource instance
/// </summary>
[DataContract(Name = "deviceDataSourceInstance")]
public class ResourceDataSourceInstanceSummary : NamedItem
{
	/// <summary>
	/// The alias
	/// </summary>
	[DataMember]
	public string Alias { get; set; } = string.Empty;
}
