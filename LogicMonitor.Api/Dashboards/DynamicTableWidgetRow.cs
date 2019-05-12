using System.Runtime.Serialization;

namespace LogicMonitor.Api.Dashboards
{
	/// <summary>
	/// A DynamicTableWidgetRow
	/// </summary>
	[DataContract]
	public class DynamicTableWidgetRow
	{
		/// <summary>
		///     The label
		/// </summary>
		[DataMember(Name = "label")]
		public string Label { get; set; }

		/// <summary>
		///     The label
		/// </summary>
		[DataMember(Name = "instanceName")]
		public string InstanceName { get; set; }

		/// <summary>
		///     The group FullPath
		/// </summary>
		[DataMember(Name = "groupFullPath")]
		public string GroupFullPath { get; set; }

		/// <summary>
		///     The Device display name
		/// </summary>
		[DataMember(Name = "deviceDisplayName")]
		public string DeviceDisplayName { get; set; }
	}
}