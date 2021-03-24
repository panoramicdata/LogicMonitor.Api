using LogicMonitor.Api.Alerts;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace LogicMonitor.Api.LogicModules
{
	/// <summary>
	///    A Device EventSource
	/// </summary>
	public class DeviceEventSource : IdentifiedItem
	{
		/// <summary>
		///    Alert Disable Status
		/// </summary>
		[DataMember(Name = "alertDisableStatus")]
		public AlertDisableStatus AlertDisableStatus { get; set; }

		/// <summary>
		///    Alert Status
		/// </summary>
		[DataMember(Name = "alertStatus")]
		public AlertStatus AlertStatus { get; set; }

		/// <summary>
		///    Alert Status priority
		/// </summary>
		[DataMember(Name = "alertStatusPriority")]
		public int AlertStatusPriority { get; set; }

		/// <summary>
		///    The time alerting was disabled in seconds since the Epoch
		/// </summary>
		[DataMember(Name = "alertingDisabledOn")]
		public object AlertingDisabledOn { get; set; }
		// LogicMonitor sometimes returns a string, so the following cannot be used
		// public AlertingDisabledOn AlertingDisabledOn { get;set; }

		/// <summary>
		///    The event source type
		/// </summary>
		[DataMember(Name = "eventType")]
		public EventSourceType EventSourceType { get; set; }

		/// <summary>
		///    Description
		/// </summary>
		[DataMember(Name = "eventSourceDescription")]
		public string EventSourceDescription { get; set; }

		/// <summary>
		///    DataSource Id
		/// </summary>
		[DataMember(Name = "eventSourceId")]
		public int EventSourceId { get; set; }

		/// <summary>
		///    DataSource Name
		/// </summary>
		[DataMember(Name = "eventSourceName")]
		public string EventSourceName { get; set; }

		/// <summary>
		///    The device Id
		/// </summary>
		[DataMember(Name = "deviceId")]
		public int DeviceId { get; set; }

		/// <summary>
		///    The device display name
		/// </summary>
		[DataMember(Name = "deviceDisplayName")]
		public string DeviceDisplayName { get; set; }

		/// <summary>
		///    The group name
		/// </summary>
		[DataMember(Name = "eventSourceGroupName")]
		public string GroupName { get; set; }

		/// <summary>
		///    Disabled groups
		/// </summary>
		[DataMember(Name = "groupsDisabledThisSource")]
		public List<object> DisabledGroups { get; set; }

		/// <summary>
		///    Is monitoring disabled
		/// </summary>
		[DataMember(Name = "stopMonitoring")]
		public bool IsMonitoringDisabled { get; set; }

		/// <summary>
		///    Is alerting disabled
		/// </summary>
		[DataMember(Name = "disableAlerting")]
		public bool IsAlertingDisabled { get; set; }

		/// <summary>
		///    SDT Status
		/// </summary>
		[DataMember(Name = "sdtStatus")]
		public SdtStatus SdtStatus { get; set; }

		/// <summary>
		///    SDT at
		/// </summary>
		[DataMember(Name = "sdtAt")]
		public string SdtAt { get; set; }

		/// <inheritdoc />
		public override string ToString() => $"{Id}: {EventSourceName} ({EventSourceId}) on device {DeviceDisplayName} ({DeviceId})";
	}
}