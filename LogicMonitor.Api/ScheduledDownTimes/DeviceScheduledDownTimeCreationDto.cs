using System.Runtime.Serialization;

namespace LogicMonitor.Api.ScheduledDownTimes
{

	/// <summary>
	///    Device SDT creation DTO
	/// </summary>
	public class DeviceScheduledDownTimeCreationDto : ScheduledDownTimeCreationDto
	{
		/// <summary>
		///    Device
		/// </summary>
		/// <param name="deviceId"></param>
		public DeviceScheduledDownTimeCreationDto(int deviceId) : base(ScheduledDownTimeType.Device)
			=> DeviceId = deviceId;

		/// <summary>
		///    The collector id
		/// </summary>
		[DataMember(Name = "deviceId")]
		public int DeviceId { get; set; }

	}
}