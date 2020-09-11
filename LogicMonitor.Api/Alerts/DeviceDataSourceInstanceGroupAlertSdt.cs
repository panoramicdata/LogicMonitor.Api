using System.Runtime.Serialization;

namespace LogicMonitor.Api.Alerts
{
	/// <summary>
	/// A device data source instance group SDT
	/// </summary>
	[DataContract]
	public class DeviceDataSourceInstanceGroupAlertSdt : AlertSdt
	{
		/// <summary>
		/// The DataSource name
		/// </summary>
		[DataMember(Name = "dataSourceName")]
		public string DataSourceName { get; set; }

		/// <summary>
		/// The Device DataSource ID
		/// </summary>
		[DataMember(Name = "deviceDataSourceId")]
		public int DeviceDataSourceId { get; set; }

		/// <summary>
		/// The Device DataSource Instance Group ID
		/// </summary>
		[DataMember(Name = "deviceDataSourceInstanceGroupId")]
		public int DeviceDataSourceInstanceGroupId { get; set; }

		/// <summary>
		/// The Device DataSource Instance Group name
		/// </summary>
		[DataMember(Name = "deviceDataSourceInstanceGroupName")]
		public string DeviceDataSourceInstanceGroupName { get; set; }

		/// <summary>
		/// The Device display name
		/// </summary>
		[DataMember(Name = "deviceDisplayName")]
		public string DeviceDisplayName { get; set; }

		/// <summary>
		/// The Device ID
		/// </summary>
		[DataMember(Name = "deviceId")]
		public int DeviceId { get; set; }
	}
}