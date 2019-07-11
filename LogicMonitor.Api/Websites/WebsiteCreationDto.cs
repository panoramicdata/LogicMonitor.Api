using System.Collections.Generic;
using System.Runtime.Serialization;

namespace LogicMonitor.Api.Websites
{
	/// <summary>
	///    A Website creation DTO
	/// </summary>
	[DataContract]
	public class WebsiteCreationDto : CreationDto<Website>
	{
		/// <summary>
		///    The website folder id
		/// </summary>
		[DataMember(Name = "websiteFolderId")]
		public string WebsiteGroupId { get; set; }

		/// <summary>
		///    Whether monitoring is disabled
		/// </summary>
		[DataMember(Name = "disableAlerting")]
		public string IsAlertingDisabled { get; set; }

		/// <summary>
		///    The polling interval in minutes
		/// </summary>
		[DataMember(Name = "pollingInterval")]
		public string PollingIntervalMinutes { get; set; }

		/// <summary>
		///    Whether to use the default location setting
		/// </summary>
		[DataMember(Name = "useDefaultLocationSetting")]
		public bool UseDefaultLocationSetting { get; set; }

		/// <summary>
		///    Whether to use the default location setting
		/// </summary>
		[DataMember(Name = "useDefaultAlertSetting")]
		public bool UseDefaultAlertSetting { get; set; }

		/// <summary>
		///    Whether the testing is internal
		/// </summary>
		[DataMember(Name = "isInternal")]
		public bool IsInternal { get; set; }

		/// <summary>
		///    Whether to disable alerting
		/// </summary>
		[DataMember(Name = "type")]
		public WebsiteType Type { get; set; }

		/// <summary>
		///    The name
		/// </summary>
		[DataMember(Name = "name")]
		public string Name { get; set; }

		/// <summary>
		///    The description
		/// </summary>
		[DataMember(Name = "description")]
		public string Description { get; set; }

		/// <summary>
		///    The hostname / IP address
		/// </summary>
		[DataMember(Name = "host")]
		public string HostName { get; set; }

		/// <summary>
		/// The HTTP schema
		/// </summary>
		[DataMember(Name = "schema")]
		public HttpSchema HttpSchema { get; set; }

		/// <summary>
		/// The domain
		/// </summary>
		[DataMember(Name = "domain")]
		public string Domain { get; set; }

		/// <summary>
		/// The steps
		/// </summary>
		[DataMember(Name = "steps")]
		public List<WebsiteStep> Steps { get; set; }

		/// <summary>
		///    The attempt count as a string
		/// </summary>
		[DataMember(Name = "count")]
		public string Count { get; set; }

		/// <summary>
		///    The percentPktsNotReceiveInTime
		/// </summary>
		[DataMember(Name = "percentPktsNotReceiveInTime")]
		public string PercentPktsNotReceiveInTime { get; set; }

		/// <summary>
		///    The timeoutInMSPktsNotReceive
		/// </summary>
		[DataMember(Name = "timeoutInMSPktsNotReceive")]
		public string TimeoutInMsPktsNotReceive { get; set; }

		/// <summary>
		///    The transition
		/// </summary>
		[DataMember(Name = "transition")]
		public string Transition { get; set; }

		/// <summary>
		///    The globalSmAlertCond
		/// </summary>
		[DataMember(Name = "globalSmAlertCond")]
		public string GlobalSmAlertCond { get; set; }

		/// <summary>
		///    The overallAlertLevel
		/// </summary>
		[DataMember(Name = "overallAlertLevel")]
		public string OverallAlertLevel { get; set; }

		/// <summary>
		///    The individualAlertLevel
		/// </summary>
		[DataMember(Name = "individualAlertLevel")]
		public string IndividualAlertLevel { get; set; }

		/// <summary>
		///    The individualSmAlertEnable
		/// </summary>
		[DataMember(Name = "individualSmAlertEnable")]
		public bool IndividualSmAlertEnable { get; set; }

		/// <summary>
		///    The test location.
		/// </summary>
		[DataMember(Name = "testLocation")]
		public TestLocation TestLocation { get; set; }

		/// <summary>
		/// Whether to trigger SSL Status Alerts
		/// </summary>
		[DataMember(Name = "triggerSSLStatusAlert")]
		public bool TriggerSslStatusAlerts { get; set; }

		/// <summary>
		/// Whether to trigger SSL Expiration Alerts
		/// </summary>
		[DataMember(Name = "triggerSSLExpirationAlert")]
		public bool TriggerSslExpirationAlerts { get; set; }

		/// <summary>
		/// The website properties
		/// </summary>
		[DataMember(Name = "websiteProperties")]
		public List<Property> WebsiteProperties { get; set; }
	}
}