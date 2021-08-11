using System.Runtime.Serialization;

namespace LogicMonitor.Api.Alerts
{
	/// <summary>
	/// A device alert SDT
	/// </summary>
	[DataContract]
	public class DeviceAlertSdt : AlertSdt
	{
		/// <summary>
		/// The device id
		/// </summary>
		[DataMember(Name = "hostId")]
		public int DeviceId { get; set; }
	}
}