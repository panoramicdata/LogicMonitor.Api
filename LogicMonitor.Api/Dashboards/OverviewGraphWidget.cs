using System.Runtime.Serialization;

namespace LogicMonitor.Api.Dashboards
{
	/// <summary>
	/// An overview graph widget
	/// </summary>
	[DataContract]
	public class OverviewGraphWidget : GraphWidget
	{
		/// <summary>
		/// The device id
		/// </summary>
		[DataMember(Name = "hId")]
		public int DeviceId { get; set; }

		/// <summary>
		/// The datasource id
		/// </summary>
		[DataMember(Name = "dsId")]
		public int DataSourceId { get; set; }

		/// <summary>
		/// The datasource name
		/// </summary>
		[DataMember(Name = "dsName")]
		public string DataSourceName { get; set; }

		/// <summary>
		/// The datasource instance group id
		/// </summary>
		[DataMember(Name = "dsigId")]
		public int DataSourceInstanceGroupId { get; set; }

		/// <summary>
		/// The datasource instance group name
		/// </summary>
		[DataMember(Name = "dsigName")]
		public string DataSourceInstanceGroupName { get; set; }

		/// <summary>
		/// The device name
		/// </summary>
		[DataMember(Name = "hostName")]
		public string DeviceName { get; set; }

		/// <summary>
		/// The graph id
		/// </summary>
		[DataMember(Name = "graphId")]
		public int GraphId { get; set; }

		/// <summary>
		/// The graph name
		/// </summary>
		[DataMember(Name = "graphName")]
		public string GraphName { get; set; }

		/// <summary>
		///     The display settings
		/// </summary>
		[DataMember(Name = "displaySettings")]
		public object DisplaySettings { get; set; }
	}
}