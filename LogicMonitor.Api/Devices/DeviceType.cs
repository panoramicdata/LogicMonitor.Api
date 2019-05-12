using System.Runtime.Serialization;

namespace LogicMonitor.Api.Devices
{
	/// <summary>
	/// A device type
	/// </summary>
	[DataContract]
	public enum DeviceType
	{
		/// <summary>
		/// Regular device
		/// </summary>
		Regular = 0,

		/// <summary>
		/// Regular device
		/// </summary>
		Aws = 2,

		/// <summary>
		/// Regular device
		/// </summary>
		Azure = 4,
	}
}