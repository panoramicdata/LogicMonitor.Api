using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System.Runtime.Serialization;

namespace LogicMonitor.Api.ScheduledDownTimes
{
	/// <summary>
	/// Scheduled down time type
	/// </summary>
	[DataContract]
	[JsonConverter(typeof(StringEnumConverter))]
	public enum ScheduledDownTimeType
	{
		/// <summary>
		/// Unknown
		/// </summary>
		[EnumMember(Value = "")]
		Unknown,

		/// <summary>
		/// Website
		/// </summary>
		[EnumMember(Value = "WebsiteSDT")]
		Website,

		/// <summary>
		/// Website Group
		/// </summary>
		[EnumMember(Value = "WebsiteGroupSDT")]
		WebsiteGroup,

		/// <summary>
		/// Website checkpoint
		/// </summary>
		[EnumMember(Value = "WebsiteCheckpointSDT")]
		WebsiteCheckpoint,

		/// <summary>
		/// Device
		/// </summary>
		[EnumMember(Value = "DeviceSDT")]
		Device,

		/// <summary>
		/// Device Group
		/// </summary>
		[EnumMember(Value = "DeviceGroupSDT")]
		DeviceGroup,

		/// <summary>
		/// Collector
		/// </summary>
		[EnumMember(Value = "CollectorSDT")]
		Collector,

		/// <summary>
		/// Device Batch Job
		/// </summary>
		[EnumMember(Value = "DeviceBatchJobSDT")]
		DeviceBatchJob,

		/// <summary>
		/// Device Data Source
		/// </summary>
		[EnumMember(Value = "DeviceDataSourceSDT")]
		DeviceDataSource,

		/// <summary>
		/// Device Event Source
		/// </summary>
		[EnumMember(Value = "DeviceEventSourceSDT")]
		DeviceEventSource,

		/// <summary>
		/// Device Data Source Instance
		/// </summary>
		[EnumMember(Value = "DeviceDataSourceInstanceSDT")]
		DeviceDataSourceInstance,

		/// <summary>
		/// Device Data Source Instance Group
		/// </summary>
		[EnumMember(Value = "DeviceDataSourceInstanceGroupSDT")]
		DeviceDataSourceInstanceGroup,

		/// <summary>
		/// Service
		/// </summary>
		[EnumMember(Value = "ServiceSDT")]
		Service,
		// Have not created a CreationDto as adding an SDT to a service in the UI appears
		// to use a resourceSDT, so this may not be required

		/// <summary>
		/// Device Cluster Alert Def
		/// </summary>
		[EnumMember(Value = "DeviceClusterAlertDefSDT")]
		DeviceClusterAlertDefSdt,
		// Have not created a CreationDto yet...
	}
}