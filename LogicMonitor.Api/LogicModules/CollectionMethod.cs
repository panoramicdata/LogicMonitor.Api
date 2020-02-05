using LogicMonitor.Api.Converters;
using Newtonsoft.Json;
using System.Runtime.Serialization;

namespace LogicMonitor.Api.LogicModules
{
	/// <summary>
	/// A collection method
	/// </summary>
	[DataContract]
	[JsonConverter(typeof(TolerantStringEnumConverter))]
	public enum CollectionMethod
	{
		/// <summary>
		/// Unknown
		/// </summary>
		[EnumMember]
		Unknown = 0,

		/// <summary>
		/// Aggregate
		/// </summary>
		[EnumMember(Value = "aggregate")]
		Aggregate,

		/// <summary>
		/// AWS Billing
		/// </summary>
		[EnumMember(Value = "awsbilling")]
		AwsBilling,

		/// <summary>
		/// AWS Billing Report
		/// </summary>
		[EnumMember(Value = "awsbillingreport")]
		AwsBillingReport,

		/// <summary>
		/// AwsClassicElbServiceLimits
		/// </summary>
		[EnumMember(Value = "awsclassicelbservicelimits")]
		AwsClassicElbServiceLimits,

		/// <summary>
		/// AWS Cloud watch
		/// </summary>
		[EnumMember(Value = "awscloudwatch")]
		AwsCloudWatch,

		/// <summary>
		/// AWS Dynamo DB
		/// </summary>
		[EnumMember(Value = "awsdynamodb")]
		AwsDynamoDb,

		/// <summary>
		/// AWS EC2 Reserved Instance
		/// </summary>
		[EnumMember(Value = "awsec2reservedinstance")]
		AwsEc2ReservedInstance,

		/// <summary>
		/// AWS EC2 Reserved Instance Coverage
		/// </summary>
		[EnumMember(Value = "awsec2reservedinstancecoverage")]
		AwsEc2ReservedInstanceCoverage,

		/// <summary>
		/// AWS EC2 Service Limits
		/// </summary>
		[EnumMember(Value = "awsec2servicelimits")]
		AwsEc2ServiceLimits,

		/// <summary>
		/// AWS S3
		/// </summary>
		[EnumMember(Value = "awss3")]
		AwsS3,

		/// <summary>
		/// AwsServiceLimitsFromTrustedAdvisor
		/// </summary>
		[EnumMember(Value = "awsservicelimitsfromtrustedadvisor")]
		AwsServiceLimitsFromTrustedAdvisor,

		/// <summary>
		/// AWS SQS
		/// </summary>
		[EnumMember(Value = "awssqs")]
		AwsSqs,

		/// <summary>
		/// AWS Auto Scaling Service Limits
		/// </summary>
		[EnumMember(Value = "awsautoscalingservicelimits")]
		AwsAutoScalingServiceLimits,

		/// <summary>
		/// AWS EC2 Scheduled Events
		/// </summary>
		[EnumMember(Value = "awsec2scheduledevents")]
		AwsEc2ScheduledEvents,

		/// <summary>
		/// AWS ECS Service Details
		/// </summary>
		[EnumMember(Value = "awsecsservicedetails")]
		AwsEcsServiceDetails,

		/// <summary>
		/// AWS SES Service Limits
		/// </summary>
		[EnumMember(Value = "awssesservicelimits")]
		AwsSesServiceLimits,

		/// <summary>
		/// Azure Billing
		/// </summary>
		[EnumMember(Value = "azurebilling")]
		AzureBilling,

		/// <summary>
		/// Azure Insights
		/// </summary>
		[EnumMember(Value = "azureinsights")]
		AzureInsights,

		/// <summary>
		/// AWS Network Service Limits
		/// </summary>
		[EnumMember(Value = "azurenetworkservicelimits")]
		AzureNetworkServiceLimits,

		/// <summary>
		/// Azure Resource Health
		/// </summary>
		[EnumMember(Value = "azureresourcehealth")]
		AzureResourceHealth,

		/// <summary>
		/// Azure VM Backup Status
		/// </summary>
		[EnumMember(Value = "azurevmbackupstatus")]
		AzureVmBackupStatus,

		/// <summary>
		/// Azure SDK
		/// </summary>
		[EnumMember(Value = "azuresdk")]
		AzureSdk,

		/// <summary>
		/// Azure Storage Service Limits
		/// </summary>
		[EnumMember(Value = "azurestorageservicelimits")]
		AzureStorageServiceLimits,

		/// <summary>
		/// Azure VM Service Limits
		/// </summary>
		[EnumMember(Value = "azurevmservicelimits")]
		AzureVmServiceLimits,

		/// <summary>
		/// AWS Cloud watch
		/// </summary>
		[EnumMember(Value = "batchscript")]
		Batchscript,

		/// <summary>
		/// AWS Cloud watch
		/// </summary>
		[EnumMember(Value = "cim")]
		Cim,

		/// <summary>
		/// Datapump
		/// </summary>
		[EnumMember(Value = "datapump")]
		Datapump,

		/// <summary>
		/// DNS
		/// </summary>
		[EnumMember(Value = "dns")]
		Dns,

		/// <summary>
		/// ESX
		/// </summary>
		[EnumMember(Value = "esx")]
		Esx,

		/// <summary>
		/// GCP Billing
		/// </summary>
		[EnumMember(Value = "gcpbilling")]
		GcpBilling,

		/// <summary>
		/// GCP Compute Service Limits
		/// </summary>
		[EnumMember(Value = "gcpcomputeservicelimits")]
		GcpComputeServiceLimits,

		/// <summary>
		/// GCP Stack Driver
		/// </summary>
		[EnumMember(Value = "gcpstackdriver")]
		GcpStackDriver,

		/// <summary>
		/// Internal
		/// </summary>
		[EnumMember(Value = "internal")]
		Internal,

		/// <summary>
		/// Internal
		/// </summary>
		[EnumMember(Value = "ipmi")]
		Ipmi,

		/// <summary>
		/// JDBC
		/// </summary>
		[EnumMember(Value = "jdbc")]
		Jdbc,

		/// <summary>
		/// JMX
		/// </summary>
		[EnumMember(Value = "jmx")]
		Jmx,

		/// <summary>
		/// Memcached
		/// </summary>
		[EnumMember(Value = "memcached")]
		Memcached,

		/// <summary>
		/// AWS Cloud watch
		/// </summary>
		[EnumMember(Value = "mongo")]
		Mongo,

		/// <summary>
		/// Netapp
		/// </summary>
		[EnumMember(Value = "netapp")]
		Netapp,

		/// <summary>
		/// Perfmon
		/// </summary>
		[EnumMember(Value = "perfmon")]
		Perfmon,

		/// <summary>
		/// Ping
		/// </summary>
		[EnumMember(Value = "ping")]
		Ping,

		/// <summary>
		/// script
		/// </summary>
		[EnumMember(Value = "script")]
		Script,

		/// <summary>
		/// Script
		/// </summary>
		[EnumMember(Value = "Script")]
		Script2,

		/// <summary>
		/// SNMP
		/// </summary>
		[EnumMember(Value = "snmp")]
		Snmp,

		/// <summary>
		/// TCP
		/// </summary>
		[EnumMember(Value = "tcp")]
		Tcp,

		/// <summary>
		/// UDP
		/// </summary>
		[EnumMember(Value = "udp")]
		Udp,

		/// <summary>
		/// WMI
		/// </summary>
		[EnumMember(Value = "wmi")]
		Wmi,

		/// <summary>
		/// Webpage
		/// </summary>
		[EnumMember(Value = "webpage")]
		Webpage,

		/// <summary>
		/// Xen
		/// </summary>
		[EnumMember(Value = "xen")]
		Xen
	}
}