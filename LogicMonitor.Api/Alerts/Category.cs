using System.Runtime.Serialization;

namespace LogicMonitor.Api.Alerts
{
	/// <summary>
	/// A Category
	/// </summary>
	[DataContract]
	public class Category : NamedItem
	{
		/// <summary>
		/// The device group id
		/// </summary>
		[DataMember(Name = "hostGroupId")]
		public int DeviceGroupId { get; set; }

		/// <summary>
		/// The DataSource id
		/// </summary>
		[DataMember(Name = "dataSourceId")]
		public int? DataSourceId { get; set; }
	}
}