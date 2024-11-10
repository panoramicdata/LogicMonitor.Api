namespace LogicMonitor.Api.Alerts;

/// <summary>
/// A Category
/// </summary>
[DataContract]
public class Category : NamedItem
{
	/// <summary>
	/// The ResourceGroup id
	/// </summary>
	[DataMember(Name = "hostGroupId")]
	public int ResourceGroupId { get; set; }

	/// <summary>
	/// The DataSource id
	/// </summary>
	[DataMember(Name = "dataSourceId")]
	public int? DataSourceId { get; set; }
}
