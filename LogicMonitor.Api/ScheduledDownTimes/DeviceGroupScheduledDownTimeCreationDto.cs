using System.Runtime.Serialization;

namespace LogicMonitor.Api.ScheduledDownTimes
{

	/// <summary>
	///    DeviceGroup SDT creation DTO
	/// </summary>
	public class DeviceGroupScheduledDownTimeCreationDto : ScheduledDownTimeCreationDto
	{
		/// <summary>
		///    Device
		/// </summary>
		/// <param name="deviceGroupId"></param>
		public DeviceGroupScheduledDownTimeCreationDto(int deviceGroupId) : base(ScheduledDownTimeType.DeviceGroup)
			=> DeviceGroupId = deviceGroupId;

		/// <summary>
		///    The DeviceGroup id
		/// </summary>
		[DataMember(Name = "deviceGroupId")]
		public int DeviceGroupId { get; set; }
	}
}