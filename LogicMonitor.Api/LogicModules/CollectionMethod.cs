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
	Aggregate = 1,

	/// <summary>
	/// AWS Billing
	/// </summary>
	[EnumMember(Value = "awsbilling")]
	AwsBilling = 2,

	/// <summary>
	/// AWS Billing Report
	/// </summary>
	[EnumMember(Value = "awsbillingreport")]
	AwsBillingReport = 3,

	/// <summary>
	/// AwsClassicElbServiceLimits
	/// </summary>
	[EnumMember(Value = "awsclassicelbservicelimits")]
	AwsClassicElbServiceLimits = 4,

	/// <summary>
	/// AWS Cloud watch
	/// </summary>
	[EnumMember(Value = "awscloudwatch")]
	AwsCloudWatch = 5,

	/// <summary>
	/// AWS Dynamo DB
	/// </summary>
	[EnumMember(Value = "awsdynamodb")]
	AwsDynamoDb = 6,

	/// <summary>
	/// AWS EC2 Reserved Instance
	/// </summary>
	[EnumMember(Value = "awsec2reservedinstance")]
	AwsEc2ReservedInstance = 7,

	/// <summary>
	/// AWS EC2 Reserved Instance Coverage
	/// </summary>
	[EnumMember(Value = "awsec2reservedinstancecoverage")]
	AwsEc2ReservedInstanceCoverage = 8,

	/// <summary>
	/// AWS EC2 Service Limits
	/// </summary>
	[EnumMember(Value = "awsec2servicelimits")]
	AwsEc2ServiceLimits = 9,

	/// <summary>
	/// AWS S3
	/// </summary>
	[EnumMember(Value = "awss3")]
	AwsS3 = 10,

	/// <summary>
	/// AwsServiceLimitsFromTrustedAdvisor
	/// </summary>
	[EnumMember(Value = "awsservicelimitsfromtrustedadvisor")]
	AwsServiceLimitsFromTrustedAdvisor = 11,

	/// <summary>
	/// AWS SQS
	/// </summary>
	[EnumMember(Value = "awssqs")]
	AwsSqs = 12,

	/// <summary>
	/// AWS Auto Scaling Service Limits
	/// </summary>
	[EnumMember(Value = "awsautoscalingservicelimits")]
	AwsAutoScalingServiceLimits = 13,

	/// <summary>
	/// AWS EBS Volumes Snapshot
	/// </summary>
	[EnumMember(Value = "awsebsvolumesnapshot")]
	AwsEbsVolumesSnapshot = 14,

	/// <summary>
	/// AWS EC2 Scheduled Events
	/// </summary>
	[EnumMember(Value = "awsec2scheduledevents")]
	AwsEc2ScheduledEvents = 15,

	/// <summary>
	/// AWS ECS Service Details
	/// </summary>
	[EnumMember(Value = "awsecsservicedetails")]
	AwsEcsServiceDetails = 16,

	/// <summary>
	/// AWS RDS Performance Insights
	/// </summary>
	[EnumMember(Value = "awsrdsperformanceinsights")]
	AwsRdsPerformanceInsights = 17,

	/// <summary>
	/// AWS RDS Service Limits
	/// </summary>
	[EnumMember(Value = "awsrdsservicelimits")]
	AwsRdsServiceLimits = 18,

	/// <summary>
	/// AWS SES Service Limits
	/// </summary>
	[EnumMember(Value = "awssesservicelimits")]
	AwsSesServiceLimits = 19,

	/// <summary>
	/// Azure Automation Account Certificate
	/// </summary>
	[EnumMember(Value = "azureautomationaccountcertificate")]
	AzureAutomationAccountCertificate = 20,

	/// <summary>
	/// Azure App Service Environment Multi Role Pool
	/// </summary>
	[EnumMember(Value = "azureappserviceenvironmentmultirolepool")]
	AzureAppServiceEnvironmentMultiRolePool = 21,

	/// <summary>
	/// Azure express route circuit peering
	/// </summary>
	[EnumMember(Value = "azureexpressroutecircuitpeering")]
	AzureExpressRouteCircuitPeering = 22,

	/// <summary>
	/// Azure replication job
	/// </summary>
	[EnumMember(Value = "azurereplicationjob")]
	AzureReplicationJob = 23,

	/// <summary>
	/// Azure backup job
	/// </summary>
	[EnumMember(Value = "azurebackupjob")]
	AzureBackupJob = 24,

	/// <summary>
	/// Azure Billing
	/// </summary>
	[EnumMember(Value = "azurebilling")]
	AzureBilling = 25,

	/// <summary>
	/// Azure Insights
	/// </summary>
	[EnumMember(Value = "azureinsights")]
	AzureInsights = 26,

	/// <summary>
	/// AWS Network Service Limits
	/// </summary>
	[EnumMember(Value = "azurenetworkservicelimits")]
	AzureNetworkServiceLimits = 27,

	/// <summary>
	/// Azure Resource Health
	/// </summary>
	[EnumMember(Value = "azureresourcehealth")]
	AzureResourceHealth = 28,

	/// <summary>
	/// Azure Virtual Desktop Host Pools
	/// </summary>
	[EnumMember(Value = "azurevirtualdesktophostpools")]
	AzureVirtualDesktopHostPools = 29,

	/// <summary>
	/// Azure Virtual Desktop Session Hosts
	/// </summary>
	[EnumMember(Value = "azurevirtualdesktopsessionhosts")]
	AzureVirtualDesktopSessionHosts = 30,

	/// <summary>
	/// Azure VM Backup Status
	/// </summary>
	[EnumMember(Value = "azurevmbackupstatus")]
	AzureVmBackupStatus = 31,

	/// <summary>
	/// Azure VNG Connection
	/// </summary>
	[EnumMember(Value = "azurevngconnection")]
	AzureVngConnection = 32,

	/// <summary>
	/// Azure SDK
	/// </summary>
	[EnumMember(Value = "azuresdk")]
	AzureSdk = 33,

	/// <summary>
	/// Azure Storage Service Limits
	/// </summary>
	[EnumMember(Value = "azurestorageservicelimits")]
	AzureStorageServiceLimits = 34,

	/// <summary>
	/// Azure VM Service Limits
	/// </summary>
	[EnumMember(Value = "azurevmservicelimits")]
	AzureVmServiceLimits = 35,

	/// <summary>
	/// Azure Web Job
	/// </summary>
	[EnumMember(Value = "azurewebjob")]
	AzureWebJob = 36,

	/// <summary>
	/// AWS Cloud watch
	/// </summary>
	[EnumMember(Value = "batchscript")]
	Batchscript = 37,

	/// <summary>
	/// AWS Cloud watch
	/// </summary>
	[EnumMember(Value = "cim")]
	Cim = 38,

	/// <summary>
	/// Datapump
	/// </summary>
	[EnumMember(Value = "datapump")]
	Datapump = 39,

	/// <summary>
	/// DNS
	/// </summary>
	[EnumMember(Value = "dns")]
	Dns = 40,

	/// <summary>
	/// ESX
	/// </summary>
	[EnumMember(Value = "esx")]
	Esx = 41,

	/// <summary>
	/// GCP Billing
	/// </summary>
	[EnumMember(Value = "gcpbilling")]
	GcpBilling = 42,

	/// <summary>
	/// GCP Billing Big Query
	/// </summary>
	[EnumMember(Value = "gcpbillingbigquery")]
	GcpBillingBigQuery = 43,

	/// <summary>
	/// GCP Compute Service Limits
	/// </summary>
	[EnumMember(Value = "gcpcomputeservicelimits")]
	GcpComputeServiceLimits = 44,

	/// <summary>
	/// GCP Stack Driver
	/// </summary>
	[EnumMember(Value = "gcpstackdriver")]
	GcpStackDriver = 45,

	/// <summary>
	/// GCP Back-end Service Health
	/// </summary>
	[EnumMember(Value = "gcpbackendservicehealth")]
	GcpBackendServiceHealth = 46,

	/// <summary>
	/// Internal
	/// </summary>
	[EnumMember(Value = "internal")]
	Internal = 47,

	/// <summary>
	/// Internal
	/// </summary>
	[EnumMember(Value = "ipmi")]
	Ipmi = 48,

	/// <summary>
	/// JDBC
	/// </summary>
	[EnumMember(Value = "jdbc")]
	Jdbc = 49,

	/// <summary>
	/// JMX
	/// </summary>
	[EnumMember(Value = "jmx")]
	Jmx = 50,

	/// <summary>
	/// Memcached
	/// </summary>
	[EnumMember(Value = "memcached")]
	Memcached = 51,

	/// <summary>
	/// AWS Cloud watch
	/// </summary>
	[EnumMember(Value = "mongo")]
	Mongo = 52,

	/// <summary>
	/// Netapp
	/// </summary>
	[EnumMember(Value = "netapp")]
	Netapp = 53,

	/// <summary>
	/// Openmetrics
	/// </summary>
	[EnumMember(Value = "openmetrics")]
	OpenMetrics = 54,

	/// <summary>
	/// Perfmon
	/// </summary>
	[EnumMember(Value = "perfmon")]
	Perfmon = 55,

	/// <summary>
	/// Ping
	/// </summary>
	[EnumMember(Value = "ping")]
	Ping = 56,

	/// <summary>
	/// Property
	/// </summary>
	[EnumMember(Value = "property")]
	Property = 57,

	/// <summary>
	/// pushmodules
	/// </summary>
	[EnumMember(Value = "pushmodules")]
	PushModules = 58,

	/// <summary>
	/// saas airbrake
	/// </summary>
	[EnumMember(Value = "saasairbrake")]
	SaasAirbrake = 59,

	/// <summary>
	/// saasoffice365csvreport
	/// </summary>
	[EnumMember(Value = "saasoffice365csvreport")]
	SaasOffice365CsvReport = 60,

	/// <summary>
	/// saasoffice365sharepointreport
	/// </summary>
	[EnumMember(Value = "saasoffice365sharepointreport")]
	SaasOffice365SharepointReport = 61,

	/// <summary>
	/// saasoffice365subscribedskus
	/// </summary>
	[EnumMember(Value = "saasoffice365subscribedskus")]
	Saasoffice365SubscribedSkus = 62,

	/// <summary>
	/// saassalesforcejson
	/// </summary>
	[EnumMember(Value = "saassalesforcejson")]
	SaasSalesforceJson = 63,

	/// <summary>
	/// saassalesforcesoqlquery
	/// </summary>
	[EnumMember(Value = "saassalesforcesoqlquery")]
	SaasSalesforceSoqlQuery = 64,

	/// <summary>
	/// saas status
	/// </summary>
	[EnumMember(Value = "saasstatus")]
	SaasStatus = 65,

	/// <summary>
	/// saas zoom json
	/// </summary>
	[EnumMember(Value = "saaszoomjson")]
	SaasZoomJson = 66,

	/// <summary>
	/// saas zoom plan usage
	/// </summary>
	[EnumMember(Value = "saaszoomplanusage")]
	SaasZoomPlanUsage = 67,

	/// <summary>
	/// script
	/// </summary>
	[EnumMember(Value = "script")]
	Script = 68,

	/// <summary>
	/// Script
	/// </summary>
	[EnumMember(Value = "Script")]
	Script2 = 69,

	/// <summary>
	/// Script
	/// </summary>
	[EnumMember(Value = "syntheticsselenium")]
	SyntheticsSelenium = 70,

	/// <summary>
	/// SNMP
	/// </summary>
	[EnumMember(Value = "snmp")]
	Snmp = 71,

	/// <summary>
	/// TCP
	/// </summary>
	[EnumMember(Value = "tcp")]
	Tcp = 72,

	/// <summary>
	/// UDP
	/// </summary>
	[EnumMember(Value = "udp")]
	Udp = 73,

	/// <summary>
	/// WMI
	/// </summary>
	[EnumMember(Value = "wmi")]
	Wmi = 74,

	/// <summary>
	/// Webpage
	/// </summary>
	[EnumMember(Value = "webpage")]
	Webpage = 75,

	/// <summary>
	/// Webpage (different spelling of property in the JSON!)
	/// </summary>
	[EnumMember(Value = "Webpage")] // version 136 introduced this issue!
	Webpage1 = 76,

	/// <summary>
	/// Xen
	/// </summary>
	[EnumMember(Value = "xen")]
	Xen = 77,

	/// <summary>
	/// Azure backup protected item backup job
	/// </summary>
	[EnumMember(Value = "azurebackupprotecteditembackupjob")]
	AzureBackupProtectedItemBackupJob = 101,

	/// <summary>
	/// Azure backup protected item health
	/// </summary>
	[EnumMember(Value = "azurebackupprotecteditemhealth")]
	AzureBackupProtectedItemHealth = 102,

	/// <summary>
	/// Azure cost management
	/// </summary>
	[EnumMember(Value = "azurecostmanagement")]
	AzureCostManagement = 103,

	/// <summary>
	/// Azure log analytics replication job
	/// </summary>
	[EnumMember(Value = "azureloganalyticsreplicationjob")]
	AzureLogAnalyticsReplicationJob = 104,

	/// <summary>
	/// Azure log analytics workspaces
	/// </summary>
	[EnumMember(Value = "azureloganalyticsworkspaces")]
	AzureLogAnalyticsWorkspaces = 105,

	/// <summary>
	/// Azure recovery service vault sr
	/// </summary>
	[EnumMember(Value = "azurerecoveryservicevaultsr")]
	AzureRecoveryServiceVaultSR = 106,

	/// <summary>
	/// Azure replication disaster recovery
	/// </summary>
	[EnumMember(Value = "azurereplicationdisasterrecovery")]
	AzureReplicationDisasterRecovery = 107,

	/// <summary>
	/// SASS Office 365 service health
	/// </summary>
	[EnumMember(Value = "saasoffice365servicehealth")]
	SaasOffice365ServiceHealth = 201,

	/// <summary>
	/// SASS Salesforce instance status
	/// </summary>
	[EnumMember(Value = "saassalesforceinstancestatus")]
	SaasSalesforceInstanceStatus = 202,
}
