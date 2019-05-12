using LogicMonitor.Api.Alerts;
using LogicMonitor.Api.Attributes;
using LogicMonitor.Api.Extensions;
using LogicMonitor.Api.LogicModules;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace LogicMonitor.Api.Devices
{
	/// <summary>
	///    A Device (also known as a Host)
	/// </summary>
	[DataContract]
	public class Device : NamedItem, IHasCustomProperties, IPatchable
	{
		/// <summary>
		///    The autoBalanced CollectorGroup id
		/// </summary>
		[DataMember(Name = "autoBalancedCollectorGroupId")]
		public int AutoBalancedCollectorGroupId { get; set; }

		/// <summary>
		///    The alert disable status
		/// </summary>
		[DataMember(Name = "alertDisableStatus")]
		public AlertDisableStatus AlertDisableStatus { get; set; }

		/// <summary>
		///    The alert disable status
		/// </summary>
		[SantabaReadOnly]
		[DataMember(Name = "alertingDisabledOn")]
		public AlertingDisabledOn AlertingDisabledOn { get; set; }

		/// <summary>
		///    The alert status
		/// </summary>
		[DataMember(Name = "alertStatusPriority")]
		public int AlertStatusPriority { get; set; }

		/// <summary>
		///    The alert status
		/// </summary>
		[SantabaReadOnly]
		[DataMember(Name = "alertStatus")]
		public AlertStatus AlertStatus { get; set; }

		/// <summary>
		///    Whether the ancestorHasDisabledLogicModule
		/// </summary>
		[SantabaReadOnly]
		[DataMember(Name = "ancestorHasDisabledLogicModule")]
		public bool AncestorHasDisabledLogicModule { get; set; }

		/// <summary>
		/// The auto properties
		/// </summary>
		[SantabaReadOnly]
		[DataMember(Name = "autoProperties")]
		public List<Property> AutoProperties { get; set; }

		/// <summary>
		///    The time that the auto-properties were assigned in seconds since the Epoch
		/// </summary>
		[SantabaReadOnly]
		[DataMember(Name = "autoPropsAssignedOn")]
		public long? AutoPropertiesAssignedOnSeconds { get; set; }

		/// <summary>
		///    The time that the auto-properties were updated
		/// </summary>
		[SantabaReadOnly]
		[DataMember(Name = "autoPropsUpdatedOn")]
		public long? AutoPropertiesUpdatedOnSeconds { get; set; }

		/// <summary>
		///    The device AWS status
		/// </summary>
		[SantabaReadOnly]
		[DataMember(Name = "awsState")]
		public AwsState AwsState { get; set; }

		/// <summary>
		///    The device Azure status
		/// </summary>
		[SantabaReadOnly]
		[DataMember(Name = "azureState")]
		public AzureState AzureState { get; set; }

		/// <summary>
		///    The device GCP status
		/// </summary>
		[SantabaReadOnly]
		[DataMember(Name = "gcpState")]
		public AzureState GcpState { get; set; }

		/// <summary>
		///    Whether the device can use remote session
		/// </summary>
		[DataMember(Name = "canUseRemoteSession")]
		public bool CanUseRemoteSession { get; set; }

		/// <summary>
		///    The Collector description
		/// </summary>
		[SantabaReadOnly]
		[DataMember(Name = "collectorDescription")]
		public string CollectorDescription { get; set; }

		/// <summary>
		///    When the device was created
		/// </summary>
		[SantabaReadOnly]
		[DataMember(Name = "createdOn")]
		public long? CreatedOnSeconds { get; set; }

		/// <summary>
		///    The Current Collector's ID
		/// </summary>
		[SantabaReadOnly]
		[DataMember(Name = "currentCollectorId")]
		public int CurrentCollectorId { get; set; }

		/// <summary>
		///    Custom properties
		/// </summary>
		[DataMember(Name = "customProperties")]
		public List<Property> CustomProperties { get; set; }

		/// <summary>
		///    The device status
		/// </summary>
		[SantabaReadOnly]
		[DataMember(Name = "deletedTimeInMs")]
		public long DeletedTimeinMs { get; set; }

		/// <summary>
		///    The device type
		/// </summary>
		[SantabaReadOnly]
		[DataMember(Name = "deviceType")]
		public DeviceType DeviceType { get; set; }

		/// <summary>
		///    Whether alerting is effectively enabled
		/// </summary>
		[DataMember(Name = "disableAlerting")]
		public bool IsAlertingDisabled { get; set; }

		/// <summary>
		///    The display name
		/// </summary>
		[DataMember(Name = "displayName")]
		public string DisplayName { get; set; }

		/// <summary>
		///    Whether alerting is effectively enabled
		/// </summary>
		[SantabaReadOnly]
		[DataMember(Name = "effectiveAlertEnabled")]
		public bool EffectiveAlertEnabled { get; set; }

		/// <summary>
		///    Whether Netflow is enabled
		/// </summary>
		[DataMember(Name = "enableNetflow")]
		public bool EnableNetflow { get; set; }

		/// <summary>
		///    Whether the device has an active instance
		/// </summary>
		[SantabaReadOnly]
		[DataMember(Name = "hasActiveInstance")]
		public bool HasActiveInstance { get; set; }

		/// <summary>
		///    Whether the device has a disabled sub-resource
		/// </summary>
		[SantabaReadOnly]
		[DataMember(Name = "hasDisabledSubResource")]
		public bool HasDisabledSubResource { get; set; }

		/// <summary>
		///    Whether the device has more (?)
		/// </summary>
		[SantabaReadOnly]
		[DataMember(Name = "hasMore")]
		public bool HasMore { get; set; }

		/// <summary>
		///    The device type (usually Host)
		/// </summary>
		[DataMember(Name = "hostGroupIds")]
		public string DeviceGroupIdsString { get; set; }

		/// <summary>
		///    The device status
		/// </summary>
		[SantabaReadOnly]
		[DataMember(Name = "hostStatus")]
		public Level DeviceStatus { get; set; }

		/// <summary>
		///    Inherited properties
		/// </summary>
		[SantabaReadOnly]
		[DataMember(Name = "inheritedProperties")]
		public List<Property> InheritedProperties { get; set; }

		/// <summary>
		///    The instances
		/// </summary>
		[SantabaReadOnly]
		[DataMember(Name = "instance")]
		public List<DeviceDataSourceInstanceSummary> Instances { get; set; }

		/// <summary>
		///    The last time that raw data was received for the device
		/// </summary>
		[SantabaReadOnly]
		[DataMember(Name = "lastDataTime")]
		public long? LastDataTimeSeconds { get; set; }

		/// <summary>
		///    The last time that raw data was received for the device
		/// </summary>
		[SantabaReadOnly]
		[DataMember(Name = "lastRawDataTime")]
		public long? LastRawDataTimeSeconds { get; set; }

		/// <summary>
		///    The device's configured URL
		/// </summary>
		[DataMember(Name = "link")]
		public string Link { get; set; }

		/// <summary>
		///    Whether the device has a disabled sub-resource
		/// </summary>
		[SantabaReadOnly]
		[DataMember(Name = "manualDiscoveryFlags")]
		public ManualDiscoveryFlags ManualDiscoveryFlags { get; set; }

		/// <summary>
		///    The Netflow Collector Id
		/// </summary>
		[SantabaReadOnly]
		[DataMember(Name = "netflowCollectorId")]
		public int NetflowCollectorId { get; set; }

		/// <summary>
		///    The Netflow Collector description
		/// </summary>
		[SantabaReadOnly]
		[DataMember(Name = "netflowCollectorDescription")]
		public string NetflowCollectorDescription { get; set; }

		/// <summary>
		///    The Netflow Collector Group Id
		/// </summary>
		[SantabaReadOnly]
		[DataMember(Name = "netflowCollectorGroupId")]
		public int NetflowCollectorGroupId { get; set; }

		/// <summary>
		///    The Netflow Collector Group name
		/// </summary>
		[SantabaReadOnly]
		[DataMember(Name = "netflowCollectorGroupName")]
		public string NetflowCollectorGroupName { get; set; }

		/// <summary>
		///    The preferred Collector Id
		/// </summary>
		[DataMember(Name = "preferredCollectorId")]
		public int PreferredCollectorId { get; set; }

		/// <summary>
		///    The preferred CollectorGroup Id
		/// </summary>
		[SantabaReadOnly]
		[DataMember(Name = "preferredCollectorGroupId")]
		public int PreferredCollectorGroupId { get; set; }

		/// <summary>
		///    The preferred CollectorGroup Id
		/// </summary>
		[SantabaReadOnly]
		[DataMember(Name = "preferredCollectorGroupName")]
		public string PreferredCollectorGroupName { get; set; }

		/// <summary>
		///    The ID of the related device
		/// </summary>
		[DataMember(Name = "relatedDeviceId")]
		public int RelatedDeviceId { get; set; }

		/// <summary>
		///    The Scan config ID
		/// </summary>
		[SantabaReadOnly]
		[DataMember(Name = "scanConfigId")]
		public int ScanConfigId { get; set; }

		/// <summary>
		///    Whether the device is currently in SDT
		/// </summary>
		[SantabaReadOnly]
		[DataMember(Name = "sdtStatus")]
		public SdtStatus SdtStatus { get; set; }

		/// <summary>
		///    The device's system properties
		/// </summary>
		[SantabaReadOnly]
		[DataMember(Name = "systemProperties")]
		public List<Property> SystemProperties { get; set; }

		/// <summary>
		///    The time in Ms before the device will be deleted
		/// </summary>
		[SantabaReadOnly]
		[DataMember(Name = "toDeleteTimeInMs")]
		public long ToDeleteTimeinMs { get; set; }

		/// <summary>
		///    Uptime in seconds
		/// </summary>
		[SantabaReadOnly]
		[DataMember(Name = "upTimeInSeconds")]
		public int UptimeInSeconds { get; set; }

		/// <summary>
		///    The last time that the device was updated in seconds since the Epoch
		/// </summary>
		[SantabaReadOnly]
		[DataMember(Name = "updatedOn")]
		public long? UpdatedOnSeconds { get; set; }

		/// <summary>
		///    LoadBalance CollectorGroupId
		/// </summary>
		[DataMember(Name = "loadBalanceCollectorGroupId")]
		public int LoadBalanceCollectorGroupId { get; set; }

		/// <summary>
		///    User Permission
		/// </summary>
		[SantabaReadOnly]
		[DataMember(Name = "userPermission")]
		public UserPermission UserPermission { get; set; }

		/// <summary>
		///    The UTC DateTime that the auto-properties were assigned
		/// </summary>
		[IgnoreDataMember]
		public DateTime? AutoPropertiesAssignedOnUtc => AutoPropertiesAssignedOnSeconds?.ToNullableDateTimeUtc();

		/// <summary>
		///    The UTC DateTime that the auto-properties were updated
		/// </summary>
		[IgnoreDataMember]
		public DateTime? AutoPropertiesUpdatedOnUtc => AutoPropertiesUpdatedOnSeconds?.ToNullableDateTimeUtc();

		/// <summary>
		///    The UTC DateTime that the device was created in the system
		/// </summary>
		[IgnoreDataMember]
		public DateTime? CreatedOnUtc => CreatedOnSeconds?.ToNullableDateTimeUtc();

		/// <summary>
		///    The UTC DateTime that raw data was last received
		/// </summary>
		[IgnoreDataMember]
		public DateTime? LastRawDataUtc => LastRawDataTimeSeconds?.ToNullableDateTimeUtc();

		/// <summary>
		///    The UTC DateTime that data was last received
		/// </summary>
		[IgnoreDataMember]
		public DateTime? LastDataUtc => LastDataTimeSeconds?.ToNullableDateTimeUtc();

		/// <summary>
		///    The UTC DateTime that the device was updated in the system
		/// </summary>
		[IgnoreDataMember]
		public DateTime? UpdatedOnUtc => UpdatedOnSeconds?.ToNullableDateTimeUtc();

		/// <inheritdoc />
		public string Endpoint() => "device/devices";

		/// <inheritdoc />
		public override string ToString() => $"{Id} : {(!string.IsNullOrWhiteSpace(DisplayName) ? DisplayName : Name)}";
	}
}