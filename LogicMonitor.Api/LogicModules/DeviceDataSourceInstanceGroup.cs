using LogicMonitor.Api.Alerts;
using LogicMonitor.Api.Extensions;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace LogicMonitor.Api.LogicModules
{
	/// <summary>
	/// A DeviceDataSourceInstanceGroup
	/// </summary>
	[DataContract]
	public class DeviceDataSourceInstanceGroup : NamedItem
	{
		/// <summary>
		/// The AlertStatus
		/// </summary>
		[DataMember(Name = "alertStatus")]
		public AlertStatus AlertStatus { get; set; }

		/// <summary>
		/// The Alert Disable Status
		/// </summary>
		[DataMember(Name = "alertDisableStatus")]
		public AlertDisableStatus AlertDisableStatus { get; set; }

		/// <summary>
		///     The Alerting disabled on
		/// </summary>
		[DataMember(Name = "alertingDisabledOn")]
		public object AlertingDisabledOn { get; set; }
		// LogicMonitor sometimes returns a string, so the following cannot be used
		// public AlertingDisabledOn AlertingDisabledOn { get;set; }

		/// <summary>
		/// The AlertStatus priority
		/// </summary>
		[DataMember(Name = "alertStatusPriority")]
		public int AlertStatusPriority { get; set; }

		/// <summary>
		/// The SDT Status
		/// </summary>
		[DataMember(Name = "sdtStatus")]
		public SdtStatus SdtStatus { get; set; }

		/// <summary>
		/// The SDT Status priority
		/// </summary>
		[DataMember(Name = "sdtStatusPriority")]
		public int SdtStatusPriority { get; set; }

		/// <summary>
		/// The createdOn
		/// </summary>
		[DataMember(Name = "createOn")]
		public long CreatedOnUtcSeconds { get; set; }

		/// <summary>
		/// The alertingDisableOn as DateTime, nul if not done
		/// </summary>
		[IgnoreDataMember]
		public DateTime? CreatedOnUtc => CreatedOnUtcSeconds.ToDateTimeUtc();

		/// <summary>
		/// The sdtAt
		/// </summary>
		[DataMember(Name = "sdtAt")]
		public string SdtAt { get; set; }

		/// <summary>
		/// The DeviceId
		/// </summary>
		[DataMember(Name = "deviceId")]
		public int DeviceId { get; set; }

		/// <summary>
		/// The deviceDisplayName
		/// </summary>
		[DataMember(Name = "deviceDisplayName")]
		public string DeviceDisplayName { get; set; }

		/// <summary>
		/// groupsDisabledThisSource
		/// </summary>
		[DataMember(Name = "groupsDisabledThisSource")]
		public List<DisabledGroup> GroupsDisabledThisSource { get; set; }

		/// <summary>
		/// instancesNum
		/// </summary>
		[DataMember(Name = "instancesNum")]
		public int InstanceCount { get; set; }

		/// <summary>
		/// disabledInstancesNum
		/// </summary>
		[DataMember(Name = "disabledInstancesNum")]
		public int DisabledInstanceCount { get; set; }

		/// <summary>
		/// deviceDataSourceId
		/// </summary>
		[DataMember(Name = "deviceDataSourceId")]
		public int DeviceDataSourceId { get; set; }
	}
}