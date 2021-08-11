using System.Collections.Generic;
using System.Runtime.Serialization;

namespace LogicMonitor.Api.Devices
{
	/// <summary>
	///    A Device (also known as a Host)
	/// </summary>
	[DataContract]
	public class DeviceCreationDto : CreationDto<Device>
	{
		/// <summary>
		///    A comma-separated list of device group ids
		/// </summary>
		[DataMember(Name = "hostGroupIds")]
		public string DeviceGroupIds { get; set; }

		/// <summary>
		///    The device name
		/// </summary>
		[DataMember(Name = "name")]
		public string Name { get; set; }

		/// <summary>
		///    The display name
		/// </summary>
		[DataMember(Name = "displayName")]
		public string DisplayName { get; set; }

		/// <summary>
		///    The device name
		/// </summary>
		[DataMember(Name = "description")]
		public string Description { get; set; }

		/// <summary>
		///    The device link
		/// </summary>
		[DataMember(Name = "link")]
		public string Link { get; set; }

		/// <summary>
		///    Whether to disable alerting
		/// </summary>
		[DataMember(Name = "disableAlerting")]
		public bool DisableAlerting { get; set; }

		/// <summary>
		///    Whether Netflow is enabled
		/// </summary>
		[DataMember(Name = "enableNetflow")]
		public bool EnableNetflow { get; set; }

		/// <summary>
		///    The Netflow Collector Id as a string
		/// </summary>
		[DataMember(Name = "netflowCollectorId")]
		public string NetflowCollectorId { get; set; }

		/// <summary>
		///    The Preferred Collector's Id
		/// </summary>
		[DataMember(Name = "preferredCollectorId")]
		public int PreferredCollectorId { get; set; }

		/// <summary>
		///    Custom properties
		/// </summary>
		[DataMember(Name = "customProperties")]
		public List<Property> CustomProperties { get; set; }

		/// <summary>
		///    ToString override
		/// </summary>
		public override string ToString() => !string.IsNullOrWhiteSpace(DisplayName) ? DisplayName : Name;
	}
}