using System.Runtime.Serialization;

namespace LogicMonitor.Api.Alerts
{
	/// <summary>
	/// A Device EventSource SDT
	/// </summary>
	[DataContract]
	public class DeviceEventSourceAlertSdt : AlertSdt
	{
		/// <summary>
		/// The Device display name
		/// </summary>
		[DataMember(Name = "deviceDisplayName")]
		public string DeviceDisplayName { get; set; }

		/// <summary>
		/// The Device EventSource ID
		/// </summary>
		[DataMember(Name = "deviceEventSourceId")]
		public int DeviceEventSourceId { get; set; }

		/// <summary>
		/// The Device ID
		/// </summary>
		[DataMember(Name = "deviceId")]
		public int DeviceId { get; set; }

		/// <summary>
		/// The EventSource name
		/// </summary>
		[DataMember(Name = "eventSourceName")]
		public string EventSourceName { get; set; }
	}
}
