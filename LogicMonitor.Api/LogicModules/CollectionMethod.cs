namespace LogicMonitor.Api.LogicModules;

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
	/// AWS EBS Volumes Snapshot
	/// </summary>
	[EnumMember(Value = "awsebsvolumesnapshot")]
	AwsEbsVolumesSnapshot,

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
	/// AWS RDS Performance Insights
	/// </summary>
	[EnumMember(Value = "awsrdsperformanceinsights")]
	AwsRdsPerformanceInsights,

	/// <summary>
	/// AWS RDS Service Limits
	/// </summary>
	[EnumMember(Value = "awsrdsservicelimits")]
	AwsRdsServiceLimits,

	/// <summary>
	/// AWS SES Service Limits
	/// </summary>
	[EnumMember(Value = "awssesservicelimits")]
	AwsSesServiceLimits,

	/// <summary>
	/// Azure Automation Account Certificate
	/// </summary>
	[EnumMember(Value = "azureautomationaccountcertificate")]
	AzureAutomationAccountCertificate,

	/// <summary>
	/// Azure App Service Environment Multi Role Pool
	/// </summary>
	[EnumMember(Value = "azureappserviceenvironmentmultirolepool")]
	AzureAppServiceEnvironmentMultiRolePool,

	/// <summary>
	/// Azure express route circuit peering
	/// </summary>
	[EnumMember(Value = "azureexpressroutecircuitpeering")]
	AzureExpressRouteCircuitPeering,

	/// <summary>
	/// Azure replication job
	/// </summary>
	[EnumMember(Value = "azurereplicationjob")]
	AzureReplicationJob,

	/// <summary>
	/// Azure backup job
	/// </summary>
	[EnumMember(Value = "azurebackupjob")]
	AzureBackupJob,

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
	/// Azure Virtual Desktop Host Pools
	/// </summary>
	[EnumMember(Value = "azurevirtualdesktophostpools")]
	AzureVirtualDesktopHostPools,

	/// <summary>
	/// Azure Virtual Desktop Session Hosts
	/// </summary>
	[EnumMember(Value = "azurevirtualdesktopsessionhosts")]
	AzureVirtualDesktopSessionHosts,

	/// <summary>
	/// Azure VM Backup Status
	/// </summary>
	[EnumMember(Value = "azurevmbackupstatus")]
	AzureVmBackupStatus,

	/// <summary>
	/// Azure VNG Connection
	/// </summary>
	[EnumMember(Value = "azurevngconnection")]
	AzureVngConnection,

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
	/// Azure Web Job
	/// </summary>
	[EnumMember(Value = "azurewebjob")]
	AzureWebJob,

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
	/// GCP Billing Big Query
	/// </summary>
	[EnumMember(Value = "gcpbillingbigquery")]
	GcpBillingBigQuery,

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
	/// GCP Back-end Service Health
	/// </summary>
	[EnumMember(Value = "gcpbackendservicehealth")]
	GcpBackendServiceHealth,

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
	/// Property
	/// </summary>
	[EnumMember(Value = "property")]
	Property,

	/// <summary>
	/// pushmodules
	/// </summary>
	[EnumMember(Value = "pushmodules")]
	PushModules,

	/// <summary>
	/// saas airbrake
	/// </summary>
	[EnumMember(Value = "saasairbrake")]
	SaasAirbrake,

	/// <summary>
	/// saasoffice365csvreport
	/// </summary>
	[EnumMember(Value = "saasoffice365csvreport")]
	SaasOffice365CsvReport,

	/// <summary>
	/// saasoffice365sharepointreport
	/// </summary>
	[EnumMember(Value = "saasoffice365sharepointreport")]
	SaasOffice365SharepointReport,

	/// <summary>
	/// saasoffice365subscribedskus
	/// </summary>
	[EnumMember(Value = "saasoffice365subscribedskus")]
	Saasoffice365SubscribedSkus,

	/// <summary>
	/// saassalesforcejson
	/// </summary>
	[EnumMember(Value = "saassalesforcejson")]
	SaasSalesforceJson,

	/// <summary>
	/// saassalesforcesoqlquery
	/// </summary>
	[EnumMember(Value = "saassalesforcesoqlquery")]
	SaasSalesforceSoqlQuery,

	/// <summary>
	/// saas status
	/// </summary>
	[EnumMember(Value = "saasstatus")]
	SaasStatus,

	/// <summary>
	/// saas zoom json
	/// </summary>
	[EnumMember(Value = "saaszoomjson")]
	SaasZoomJson,

	/// <summary>
	/// saas zoom plan usage
	/// </summary>
	[EnumMember(Value = "saaszoomplanusage")]
	SaasZoomPlanUsage,

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
	/// Script
	/// </summary>
	[EnumMember(Value = "syntheticsselenium")]
	SyntheticsSelenium,

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
	/// Webpage (different spelling of property in the JSON!)
	/// </summary>
	[EnumMember(Value = "Webpage")] // version 136 introduced this issue!
	Webpage1,

	/// <summary>
	/// Xen
	/// </summary>
	[EnumMember(Value = "xen")]
	Xen
}
