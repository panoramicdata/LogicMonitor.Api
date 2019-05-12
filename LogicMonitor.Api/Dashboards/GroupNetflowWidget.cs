using System.Runtime.Serialization;

namespace LogicMonitor.Api.Dashboards
{
	/// <summary>
	/// A Group Netflow widget
	/// </summary>
	[DataContract]
	public class GroupNetflowWidget : Widget
	{
		/// <summary>
		///     The data type
		/// </summary>
		[DataMember(Name = "dataType")]
		public string DataType { get; set; }

		/// <summary>
		///     The QoS type
		/// </summary>
		[DataMember(Name = "qosType")]
		public string QosType { get; set; }

		/// <summary>
		///     The device group Id
		/// </summary>
		[DataMember(Name = "deviceGroupId")]
		public int DeviceGroupId { get; set; }

		/// <summary>
		///     The device group name
		/// </summary>
		[DataMember(Name = "deviceGroupName")]
		public string DeviceGroupName { get; set; }

		/// <summary>
		///     The row filters
		/// </summary>
		[DataMember(Name = "rowFilters")]
		public string RowFilters { get; set; }
	}
}