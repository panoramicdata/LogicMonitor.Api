using System.Runtime.Serialization;

namespace LogicMonitor.Api.Settings
{
	/// <summary>
	/// A message template set
	/// </summary>
	[DataContract]
	public class MessageTemplateSet
	{
		/// <summary>
		/// The DataSource alert message template
		/// </summary>
		[DataMember(Name = "dataSourceAlertMessageTemplate")]
		public AlertMessageTemplate DataSourceAlertMessageTemplate { get; set; }

		/// <summary>
		/// The DataSource clear message template
		/// </summary>
		[DataMember(Name = "dataSourceClearMessageTemplate")]
		public AlertMessageTemplate DataSourceClearMessageTemplate { get; set; }

		/// <summary>
		/// The EventSource alert message template
		/// </summary>
		[DataMember(Name = "eventSourceAlertMessageTemplate")]
		public AlertMessageTemplate EventSourceAlertMessageTemplate { get; set; }

		/// <summary>
		/// The EventSource clear message template
		/// </summary>
		[DataMember(Name = "eventSourceClearMessageTemplate")]
		public AlertMessageTemplate EventSourceClearMessageTemplate { get; set; }

		/// <summary>
		/// The JobMonitor alert message template
		/// </summary>
		[DataMember(Name = "JobMonitorAlertMessageTemplate")]
		public AlertMessageTemplate JobMonitorAlertMessageTemplate { get; set; }

		/// <summary>
		/// The JobMonitor clear message template
		/// </summary>
		[DataMember(Name = "JobMonitorClearMessageTemplate")]
		public AlertMessageTemplate JobMonitorClearMessageTemplate { get; set; }

		/// <summary>
		/// The HostCluster alert message template
		/// </summary>
		[DataMember(Name = "HostClusterAlertMessageTemplate")]
		public AlertMessageTemplate HostClusterAlertMessageTemplate { get; set; }

		/// <summary>
		/// The HostCluster clear message template
		/// </summary>
		[DataMember(Name = "HostClusterClearMessageTemplate")]
		public AlertMessageTemplate HostClusterClearMessageTemplate { get; set; }

		/// <summary>
		/// The Website alert message template
		/// </summary>
		[DataMember(Name = "WebsiteAlertMessageTemplate")]
		public AlertMessageTemplate WebsiteAlertMessageTemplate { get; set; }

		/// <summary>
		/// The Website clear message template
		/// </summary>
		[DataMember(Name = "WebsiteClearMessageTemplate")]
		public AlertMessageTemplate WebsiteClearMessageTemplate { get; set; }

		/// <summary>
		/// The WebsiteOverall alert message template
		/// </summary>
		[DataMember(Name = "WebsiteOverallAlertMessageTemplate")]
		public AlertMessageTemplate WebsitesOverallAlertMessageTemplate { get; set; }

		/// <summary>
		/// The WegbsitesOverall clear message template
		/// </summary>
		[DataMember(Name = "WebsitesOverallClearMessageTemplate")]
		public AlertMessageTemplate WebsitesOverallClearMessageTemplate { get; set; }

		/// <summary>
		/// The CollectorDown alert message template
		/// </summary>
		[DataMember(Name = "CollectorDownAlertMessageTemplate")]
		public AlertMessageTemplate CollectorDownAlertMessageTemplate { get; set; }

		/// <summary>
		/// The CollectorDown clear message template
		/// </summary>
		[DataMember(Name = "CollectorDownClearMessageTemplate")]
		public AlertMessageTemplate CollectorDownClearMessageTemplate { get; set; }

		/// <summary>
		/// The CollectorFailover alert message template
		/// </summary>
		[DataMember(Name = "CollectorFailoverAlertMessageTemplate")]
		public AlertMessageTemplate CollectorFailoverAlertMessageTemplate { get; set; }

		/// <summary>
		/// The CollectorFailover clear message template
		/// </summary>
		[DataMember(Name = "CollectorFailoverClearMessageTemplate")]
		public AlertMessageTemplate CollectorFailoverClearMessageTemplate { get; set; }

		/// <summary>
		/// The AlertThrottled alert message template
		/// </summary>
		[DataMember(Name = "AlertThrottledAlertMessageTemplate")]
		public AlertMessageTemplate AlertThrottledAlertMessageTemplate { get; set; }

		/// <summary>
		/// The AlertThrottled clear message template
		/// </summary>
		[DataMember(Name = "AlertThrottledClearMessageTemplate")]
		public AlertMessageTemplate AlertThrottledClearMessageTemplate { get; set; }

		/// <summary>
		/// The CollectorFailover alert message template
		/// </summary>
		[DataMember(Name = "CollectorFailbackAlertMessageTemplate")]
		public AlertMessageTemplate CollectorFailbackAlertMessageTemplate { get; set; }
	}
}