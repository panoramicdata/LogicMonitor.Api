using System.Runtime.Serialization;

namespace LogicMonitor.Api.ScheduledDownTimes
{
	/// <summary>
	///    Device SDT creation DTO
	/// </summary>
	public class DeviceDataSourceScheduledDownTimeCreationDto : ScheduledDownTimeCreationDto
	{
		/// <summary>
		///    Device
		/// </summary>
		/// <param name="deviceId"></param>
		/// <param name="dataSourceId"></param>
		public DeviceDataSourceScheduledDownTimeCreationDto(int deviceId, int dataSourceId) : base(ScheduledDownTimeType.DeviceDataSource)
		{
			DeviceId = deviceId;
			DataSourceId = dataSourceId;
		}

		/// <summary>
		///    Device
		/// </summary>
		/// <param name="deviceId"></param>
		/// <param name="dataSourceName"></param>
		public DeviceDataSourceScheduledDownTimeCreationDto(int deviceId, string dataSourceName) : base(ScheduledDownTimeType.DeviceDataSource)
		{
			DeviceId = deviceId;
			DataSourceName = dataSourceName;
		}

		/// <summary>
		///    The Device id
		/// </summary>
		[DataMember(Name = "deviceId")]
		public int DeviceId { get; set; }

		/// <summary>
		///    The data source id
		/// </summary>
		[DataMember(Name = "dataSourceId")]
		public int DataSourceId { get; set; }

		/// <summary>
		///    The datasource name
		/// </summary>
		[DataMember(Name = "dataSourceName")]
		public string DataSourceName { get; set; }
	}
}