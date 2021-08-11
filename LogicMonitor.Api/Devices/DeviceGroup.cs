using LogicMonitor.Api.Alerts;
using LogicMonitor.Api.Attributes;
using LogicMonitor.Api.Extensions;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace LogicMonitor.Api.Devices
{
	/// <summary>
	///    A device group
	/// </summary>
	[DataContract]
	public class DeviceGroup : NamedItem, IHasCustomProperties, IPatchable
	{
		/// <summary>
		///    The Alert disable status
		/// </summary>
		[SantabaReadOnly]
		[DataMember(Name = "alertDisableStatus")]
		public AlertDisableStatus AlertDisableStatus { get; set; }

		/// <summary>
		///    Whether alerting is enabled
		/// </summary>
		[Obsolete("Use !IsAlertingDisabled instead", true)]
		public bool AlertEnable => !IsAlertingDisabled;

		/// <summary>
		///    The alert status
		/// </summary>
		[SantabaReadOnly]
		[DataMember(Name = "alertStatus", IsRequired = false)]
		public AlertStatus AlertStatus { get; set; }

		/// <summary>
		///    What the DeviceGroup applies to
		/// </summary>
		[DataMember(Name = "appliesTo")]
		public string AppliesTo { get; set; }

		/// <summary>
		///    The auto visual result
		/// </summary>
		[SantabaReadOnly]
		[DataMember(Name = "autoVisualResult")]
		public string AutoVisualResult { get; set; }

		/// <summary>
		///    The clusterAlertStatus
		/// </summary>
		[SantabaReadOnly]
		[DataMember(Name = "clusterAlertStatus")]
		public string ClusterAlertStatus { get; set; }

		/// <summary>
		///    The cluster alert status priority
		/// </summary>
		[SantabaReadOnly]
		[DataMember(Name = "clusterAlertStatusPriority")]
		public int ClusterAlertStatusPriority { get; set; }

		/// <summary>
		///    The default autoBalanced CollectorGroup id
		/// </summary>
		[DataMember(Name = "defaultAutoBalancedCollectorGroupId")]
		public int DefaultAutoBalancedCollectorGroupId { get; set; }

		/// <summary>
		///    The default Collector description
		/// </summary>
		[SantabaReadOnly]
		[DataMember(Name = "defaultCollectorDescription")]
		public string DefaultCollectorDescription { get; set; }

		/// <summary>
		///    The default Collector Id
		/// </summary>
		[DataMember(Name = "defaultCollectorId", IsRequired = false)]
		public int DefaultCollectorId { get; set; }

		/// <summary>
		///    The default Collector Id (used by MonitorObjectGroups only)
		/// </summary>
		[DataMember(Name = "defaultAgentId", IsRequired = false)]
		public int DefaultAgentId { get; set; }

		/// <summary>
		///    AWS Device count
		/// </summary>
		[SantabaReadOnly]
		[DataMember(Name = "numOfAWSDevices")]
		public int AwsDeviceCount { get; set; }

		/// <summary>
		///    AWS Regions info
		/// </summary>
		[SantabaReadOnly]
		[DataMember(Name = "awsRegionsInfo")]
		public string AwsRegionsInfo { get; set; }

		/// <summary>
		///    AWS Test result
		/// </summary>
		[SantabaReadOnly]
		[DataMember(Name = "awsTestResult")]
		public string AwsTestResult { get; set; }

		/// <summary>
		///    AWS Test result code
		/// </summary>
		[SantabaReadOnly]
		[DataMember(Name = "awsTestResultCode")]
		public int AwsTestResultCode { get; set; }

		/// <summary>
		///    Azure Device count
		/// </summary>
		[SantabaReadOnly]
		[DataMember(Name = "numOfAzureDevices")]
		public int AzureDeviceCount { get; set; }

		/// <summary>
		///    Azure regions info
		/// </summary>
		[SantabaReadOnly]
		[DataMember(Name = "azureRegionsInfo")]
		public string AzureRegionsInfo { get; set; }

		/// <summary>
		///    Azure test result
		/// </summary>
		[SantabaReadOnly]
		[DataMember(Name = "azureTestResult")]
		public string AzureTestResult { get; set; }

		/// <summary>
		///    Azure Test result code
		/// </summary>
		[SantabaReadOnly]
		[DataMember(Name = "azureTestResultCode")]
		public int AzureTestResultCode { get; set; }

		/// <summary>
		///    GCP Device count
		/// </summary>
		[SantabaReadOnly]
		[DataMember(Name = "numOfGcpDevices")]
		public int GcpDeviceCount { get; set; }

		/// <summary>
		///    GCP regions info
		/// </summary>
		[SantabaReadOnly]
		[DataMember(Name = "gcpRegionsInfo")]
		public string GcpRegionsInfo { get; set; }

		/// <summary>
		///    GCP test result
		/// </summary>
		[SantabaReadOnly]
		[DataMember(Name = "gcpTestResult")]
		public string GcpTestResult { get; set; }

		/// <summary>
		///    GCP Test result code
		/// </summary>
		[SantabaReadOnly]
		[DataMember(Name = "gcpTestResultCode")]
		public int GcpTestResultCode { get; set; }

		/// <summary>
		///    Whether netflow is enabled
		/// </summary>
		[DataMember(Name = "enableNetflow")]
		public bool IsNetflowEnabled { get; set; }

		/// <summary>
		///    Whether it has Netflow enabled devices
		/// </summary>
		[SantabaReadOnly]
		[DataMember(Name = "hasNetflowEnabledDevices")]
		public bool HasNetflowEnabledDevices { get; set; }

		/// <summary>
		///    The number of seconds since the Epoch of the device creation in the system.
		/// </summary>
		[SantabaReadOnly]
		[DataMember(Name = "createdOn")]
		public int? CreatedOnTimestampUtc { get; set; }

		/// <summary>
		///    The UTC DateTime that the device was created in the system
		/// </summary>
		[IgnoreDataMember]
		public DateTime? CreatedOnUtc => CreatedOnTimestampUtc?.ToNullableDateTimeUtc();

		/// <summary>
		///    Custom DeviceGroup properties
		/// </summary>
		[DataMember(Name = "customProperties")]
		public List<Property> CustomProperties { get; set; }

		/// <summary>
		///    Device count
		/// </summary>
		[SantabaReadOnly]
		[DataMember(Name = "numOfHosts")]
		public int DeviceCount { get; set; }

		/// <summary>
		///    The default collector group id
		/// </summary>
		[DataMember(Name = "defaultCollectorGroupId")]
		public int DefaultCollectorGroupId { get; set; }

		/// <summary>
		///    The default collector group description
		/// </summary>
		[DataMember(Name = "defaultCollectorGroupDescription")]
		public string DefaultCollectorGroupDescription { get; set; }

		/// <summary>
		///    The default load balance collector group id
		/// </summary>
		[DataMember(Name = "defaultLoadBalanceCollectorGroupId")]
		public int DefaultLoadBalanceCollectorGroupId { get; set; }

		/// <summary>
		///    The DeviceGroupType
		/// </summary>
		[SantabaReadOnly]
		[DataMember(Name = "groupType")]
		public DeviceGroupType DeviceGroupType { get; set; }

		/// <summary>
		///    The Devices in this DeviceGroup.
		///    This information does not always come from the API, so you may need to populate it yourself.
		/// </summary>
		[SantabaReadOnly]
		[DataMember(IsRequired = false)]
		public List<Device> Devices { get; set; }

		/// <summary>
		///    Direct Device count
		/// </summary>
		[SantabaReadOnly]
		[DataMember(Name = "numOfDirectDevices")]
		public int DirectDeviceCount { get; set; }

		/// <summary>
		///    Direct SubGroups count
		/// </summary>
		[SantabaReadOnly]
		[DataMember(Name = "numOfDirectSubGroups")]
		public int DirectSubGroupCount { get; set; }

		/// <summary>
		///    The alert status Priority
		/// </summary>
		[SantabaReadOnly]
		[DataMember(Name = "alertStatusPriority")]
		public int AlertStatusPriority { get; set; }

		/// <summary>
		///    Whether alerting is effectively enabled (e.g. this may be overridden at a higher level)
		/// </summary>
		[SantabaReadOnly]
		[DataMember(Name = "effectiveAlertEnabled")]
		public bool EffectiveAlertEnabled { get; set; }

		/// <summary>
		///    Any extra information
		/// </summary>
		[SantabaReadOnly]
		[DataMember(Name = "extra")]
		public object Extra { get; set; }

		/// <summary>
		///    The DeviceGroup full path
		/// </summary>
		[SantabaReadOnly]
		[DataMember(Name = "fullPath")]
		public string FullPath { get; set; }

		/// <summary>
		///    The Group status
		/// </summary>
		[SantabaReadOnly]
		[DataMember(Name = "groupStatus")]
		public string GroupStatus { get; set; }

		/// <summary>
		///    Disable Alerting
		/// </summary>
		[DataMember(Name = "disableAlerting")]
		public bool IsAlertingDisabled { get; set; }

		/// <summary>
		///     The Alerting disabled on
		/// </summary>
		[SantabaReadOnly]
		[DataMember(Name = "alertingDisabledOn")]
		public object AlertingDisabledOn { get; set; }
		// LogicMonitor sometimes returns a string, so the following cannot be used
		// public AlertingDisabledOn AlertingDisabledOn { get;set; }

		/// <summary>
		///    The Parent Group Type
		/// </summary>
		[DataMember(Name = "parentId")]
		public int ParentId { get; set; }

		/// <summary>
		///    The SDT status
		/// </summary>
		[SantabaReadOnly]
		[DataMember(Name = "sdtStatus")]
		public SdtStatus SdtStatus { get; set; }

		/// <summary>
		///    Child DeviceGroups
		/// </summary>
		[SantabaReadOnly]
		[DataMember(Name = "subGroups")]
		public List<DeviceGroup> SubGroups { get; set; }

		/// <summary>
		///    The Group status
		/// </summary>
		[SantabaReadOnly]
		[DataMember(Name = "userPermission")]
		public UserPermission UserPermission { get; set; }

		/// <summary>
		///    The endpoint
		/// </summary>
		/// <returns></returns>
		public string Endpoint() => "device/groups";

		/// <summary>
		///    ToString override
		/// </summary>
		/// <returns>FullPath</returns>
		public override string ToString() => FullPath;
	}
}