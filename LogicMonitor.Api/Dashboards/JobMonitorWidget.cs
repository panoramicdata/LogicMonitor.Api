using System.Runtime.Serialization;

namespace LogicMonitor.Api.Dashboards
{
	/// <summary>
	///     A job monitor widget
	/// </summary>
	[DataContract]
	public class JobMonitorWidget : Widget
	{
		/// <summary>
		///     The device display name
		/// </summary>
		[DataMember(Name = "deviceDisplayName")]
		public string DeviceDisplayName { get; set; }

		/// <summary>
		///     The device group display name
		/// </summary>
		[DataMember(Name = "groupDisplayName")]
		public string DeviceGroupDisplayName { get; set; }

		/// <summary>
		///     The batch job name
		/// </summary>
		[DataMember(Name = "batchJobName")]
		public string BatchJobName { get; set; }

		/// <summary>
		///     The BatchJob Id
		/// </summary>
		[DataMember(Name = "batchJobId")]
		public string BatchJobId { get; set; }

		/// <summary>
		///     The display settings
		/// </summary>
		[DataMember(Name = "displaySettings")]
		public object DisplaySettings { get; set; }
	}
}