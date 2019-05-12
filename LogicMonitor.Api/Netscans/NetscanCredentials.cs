using System.Runtime.Serialization;

namespace LogicMonitor.Api.Netscans
{
	/// <summary>
	///    A netscan policy credential set
	/// </summary>
	[DataContract(Name = "credentials")]
	public class NetscanCredentials
	{
		/// <summary>
		///    The device group id
		/// </summary>
		[DataMember(Name = "deviceGroupId")]
		public int DeviceGroupId { get; set; }

		/// <summary>
		///    The device group name
		/// </summary>
		[DataMember(Name = "deviceGroupName")]
		public string DeviceGroupName { get; set; }

		/// <summary>
		///    The custom
		/// </summary>
		[DataMember(Name = "custom")]
		public object Custom { get; set; }
	}
}