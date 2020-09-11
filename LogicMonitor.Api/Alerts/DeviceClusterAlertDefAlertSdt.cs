using System.Runtime.Serialization;

namespace LogicMonitor.Api.Alerts
{
	/// <summary>
	/// A Device Cluster Alert Def SDT
	/// </summary>
	[DataContract]
	public class DeviceClusterAlertDefSdt : AlertSdt
	{
		/// <summary>
		/// The DataSource name
		/// </summary>
		[DataMember(Name = "dataSourceName")]
		public string DataSourceName { get; set; }

		/// <summary>
		/// The Device Cluster Alert Def ID
		/// </summary>
		[DataMember(Name = "deviceClusterAlertDefId")]
		public int DeviceClusterAlertDefId { get; set; }

		/// <summary>
		/// The Device Group full path
		/// </summary>
		[DataMember(Name = "deviceGroupFullPath")]
		public string DeviceGroupFullPath { get; set; }

		/// <summary>
		/// The Device Group ID
		/// </summary>
		[DataMember(Name = "deviceGroupId")]
		public int DeviceGroupId { get; set; }
	}
}