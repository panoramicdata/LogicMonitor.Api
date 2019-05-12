using System.Runtime.Serialization;

namespace LogicMonitor.Api.Dashboards
{
	/// <summary>
	/// A netflow graph widget
	/// </summary>
	public class NetflowGraphWidget : Widget
	{
		/// <summary>
		/// The graph type
		/// </summary>
		[DataMember(Name = "graph")]
		public string GraphType { get; set; }

		/// <summary>
		/// The interface index
		/// </summary>
		[DataMember(Name = "interfaceIndex")]
		public int InterfaceIndex { get; set; }

		/// <summary>
		/// The device id
		/// </summary>
		[DataMember(Name = "deviceId")]
		public int DeviceId { get; set; }

		/// <summary>
		/// The device name
		/// </summary>
		[DataMember(Name = "deviceDisplayName")]
		public string DeviceName { get; set; }

		/// <summary>
		/// The row filters
		/// </summary>
		[DataMember(Name = "rowFilters")]
		public string RowFilters { get; set; }

		/// <summary>
		/// The direction
		/// </summary>
		[DataMember(Name = "direction")]
		public string Direction { get; set; }

		/// <summary>
		/// The QoS type
		/// </summary>
		[DataMember(Name = "qosType")]
		public string QosType { get; set; }

		/// <summary>
		/// The interface name
		/// </summary>
		[DataMember(Name = "interfaceName")]
		public string InterfaceName { get; set; }

		/// <summary>
		/// The netflow filter
		/// </summary>
		[DataMember(Name = "netflowFilter")]
		public object NetflowFilter { get; set; }

		/// <summary>
		///     The display settings
		/// </summary>
		[DataMember(Name = "displaySettings")]
		public object DisplaySettings { get; set; }
	}
}