namespace LogicMonitor.Api.Devices;

/// <summary>
///     A device group type
/// </summary>
[DataContract]
[JsonConverter(typeof(TolerantStringEnumConverter))]
public enum DeviceGroupType
{
	/// <summary>
	///     Unknown)
	/// </summary>
	[EnumMember(Value = "0")]
	Unknown = 0,

	/// <summary>
	///     A host
	/// </summary>
	[EnumMember(Value = "1")]
	Dynamic = 1,

	/// <summary>
	///     Normal
	/// </summary>
	[EnumMember(Value = "Normal")]
	Normal = 2,

	/// <summary>
	///     AwsWorkSpaceDirectorySubgroup
	/// </summary>
	[EnumMember(Value = "AwsWorkSpaceDirectorySubgroup")]
	AwsWorkSpaceDirectorySubgroup = 3,

	/// <summary>
	///     AppStream
	/// </summary>
	[EnumMember(Value = "AppStream")]
	AppStream = 4,

	/// <summary>
	///     AWS/StepFunctions
	/// </summary>
	[EnumMember(Value = "AWS/StepFunctions")]
	AwsStepFunctions = 5,

	/// <summary>
	///     AWS/Cognito
	/// </summary>
	[EnumMember(Value = "AWS/Cognito")]
	AwsCognito = 6,

	/// <summary>
	///     AwsElasticBeanstalkSubgroup
	/// </summary>
	[EnumMember(Value = "AwsElasticBeanstalkSubgroup")]
	AwsElasticBeanstalkSubgroup = 7,

	/// <summary>
	///     AWS/SageMaker
	/// </summary>
	[EnumMember(Value = "AWS/SageMaker")]
	AwsSageMaker = 8,

	/// <summary>
	///     AWS/ElasticTranscoder
	/// </summary>
	[EnumMember(Value = "AWS/ElasticTranscoder")]
	AwsElasticTranscoder = 9,

	/// <summary>
	///     AWS/Route53Resolver
	/// </summary>
	[EnumMember(Value = "AWS/Route53Resolver")]
	AwsRoute53Resolver = 10,

	/// <summary>
	///     AWS/AppStream
	/// </summary>
	[EnumMember(Value = "AWS/AppStream")]
	AwsAppStream = 11,

	/// <summary>
	///     AWS/Athena
	/// </summary>
	[EnumMember(Value = "AWS/Athena")]
	AwsAthena = 12,

	/// <summary>
	///     AWS/OpsWorks
	/// </summary>
	[EnumMember(Value = "AWS/OpsWorks")]
	AwsOpsWorks = 13,

	/// <summary>
	///     AWS/DocDB
	/// </summary>
	[EnumMember(Value = "AWS/DocDB")]
	AwsDocDB = 14,

	/// <summary>
	///     AWS/EventBridge
	/// </summary>
	[EnumMember(Value = "AWS/EventBridge")]
	AwsEventBridge = 15,

	/// <summary>
	///     AWS/AmazonMQ
	/// </summary>
	[EnumMember(Value = "AWS/AmazonMQ")]
	AwsAmazonMQ = 16,

	/// <summary>
	///     AWS/Glue
	/// </summary>
	[EnumMember(Value = "AWS/Glue")]
	AwsGlue = 17,

	/// <summary>
	///     AWS/MediaConnect
	/// </summary>
	[EnumMember(Value = "AWS/MediaConnect")]
	AwsMediaConnect = 18,

	/// <summary>
	///     AWS/MediaStore
	/// </summary>
	[EnumMember(Value = "AWS/MediaStore")]
	AwsMediaStore = 19,

	/// <summary>
	///     AWS/MediaConvert
	/// </summary>
	[EnumMember(Value = "AWS/MediaConvert")]
	AwsMediaConvert = 20,

	/// <summary>
	///     AWS/MediaPackageLive
	/// </summary>
	[EnumMember(Value = "AWS/MediaPackageLive")]
	AwsMediaPackageLive = 21,

	/// <summary>
	///     AWS/MediaPackageVOD
	/// </summary>
	[EnumMember(Value = "AWS/MediaPackageVOD")]
	AwsMediaPackageVOD = 22,

	/// <summary>
	///     AWS/MediaTailor
	/// </summary>
	[EnumMember(Value = "AWS/MediaTailor")]
	AwsMediaTailor = 23,

	/// <summary>
	///     AWS/AwsRoot
	/// </summary>
	[EnumMember(Value = "AWS/AwsRoot")]
	AwsRoot = 24,

	/// <summary>
	///     AWS/CloudSearch
	/// </summary>
	[EnumMember(Value = "AWS/CloudSearch")]
	AwsCloudSearch = 25,

	/// <summary>
	///     AWS/CodeBuild
	/// </summary>
	[EnumMember(Value = "AWS/CodeBuild")]
	AwsCodeBuild = 26,

	/// <summary>
	///     AWS/EC2
	/// </summary>
	[EnumMember(Value = "AWS/EC2")]
	AwsEc2 = 27,

	/// <summary>
	///     AWS/DmsReplicationTasks
	/// </summary>
	[EnumMember(Value = "AWS/DmsReplicationTasks")]
	AwsDmsReplicationTasks = 28,

	/// <summary>
	///     AWS/DynamoDB
	/// </summary>
	[EnumMember(Value = "AWS/DynamoDB")]
	AwsDynamoDb = 29,

	/// <summary>
	///     AWS/ElastiCache
	/// </summary>
	[EnumMember(Value = "AWS/ElastiCache")]
	AwsElastiCache = 30,

	/// <summary>
	///     AWS/ELB
	/// </summary>
	[EnumMember(Value = "AWS/ELB")]
	AwsElb = 31,

	/// <summary>
	///     AWS/EBS
	/// </summary>
	[EnumMember(Value = "AWS/EBS")]
	AwsEbs = 32,

	/// <summary>
	///     AWS/SQS
	/// </summary>
	[EnumMember(Value = "AWS/SQS")]
	AwsSqs = 33,

	/// <summary>
	///     AWS/RDS
	/// </summary>
	[EnumMember(Value = "AWS/RDS")]
	AwsRds = 34,

	/// <summary>
	///     AWS/Route53
	/// </summary>
	[EnumMember(Value = "AWS/Route53")]
	AwsRoute53 = 35,

	/// <summary>
	///     AWS/S3
	/// </summary>
	[EnumMember(Value = "AWS/S3")]
	AwsS3 = 36,

	/// <summary>
	///     AWS/SNS
	/// </summary>
	[EnumMember(Value = "AWS/SNS")]
	AwsSns = 37,

	/// <summary>
	///     AWS/AutoScaling
	/// </summary>
	[EnumMember(Value = "AWS/AutoScaling")]
	AwsAutoScaling = 38,

	/// <summary>
	///     AWS/APIGateway
	/// </summary>
	[EnumMember(Value = "AWS/APIGateway")]
	AwsApiGateway = 39,

	/// <summary>
	///     AWS/ApplicationELB
	/// </summary>
	[EnumMember(Value = "AWS/ApplicationELB")]
	AwsApplicationElb = 40,

	/// <summary>
	///     AWS/FSx
	/// </summary>
	[EnumMember(Value = "AWS/FSx")]
	AwsFSx = 41,

	/// <summary>
	///     AWS/Kinesis
	/// </summary>
	[EnumMember(Value = "AWS/Kinesis")]
	AwsKinesis = 42,

	/// <summary>
	///     AWS/KinesisVideo
	/// </summary>
	[EnumMember(Value = "AWS/KinesisVideo")]
	AwsKinesisVideo = 43,

	/// <summary>
	///     AWS/MSKBroker
	/// </summary>
	[EnumMember(Value = "AWS/MSKBroker")]
	AwsMSKBroker = 44,

	/// <summary>
	///     AWS/MSKCluster
	/// </summary>
	[EnumMember(Value = "AWS/MSKCluster")]
	AwsMSKCluster = 45,

	/// <summary>
	///     AWS/RedShift
	/// </summary>
	[EnumMember(Value = "AWS/RedShift")]
	AwsRedShift = 46,

	/// <summary>
	///     AWS/TransitGateway
	/// </summary>
	[EnumMember(Value = "AWS/TransitGateway")]
	AwsTransitGateway = 47,

	/// <summary>
	///     Azure/AzureRoot
	/// </summary>
	[EnumMember(Value = "Azure/AzureRoot")]
	AzureAzureRoot = 48,

	/// <summary>
	///     Azure/AppService
	/// </summary>
	[EnumMember(Value = "Azure/AppService")]
	AzureAppService = 49,

	/// <summary>
	///     Azure/SQLDatabase
	/// </summary>
	[EnumMember(Value = "Azure/SQLDatabase")]
	AzureSqlDatabase = 50,

	/// <summary>
	///     Azure/VirtualMachine
	/// </summary>
	[EnumMember(Value = "Azure/VirtualMachine")]
	AzureVirtualMachine = 51,

	/// <summary>
	///     AWS/CloudFront
	/// </summary>
	[EnumMember(Value = "AWS/CloudFront")]
	AwsCloudFront = 52,

	/// <summary>
	///     Azure/EventHub
	/// </summary>
	[EnumMember(Value = "Azure/EventHub")]
	AzureEventHub = 53,

	/// <summary>
	///     Azure/RedisCache
	/// </summary>
	[EnumMember(Value = "Azure/RedisCache")]
	AzureRedisCache = 54,

	/// <summary>
	///     Azure/VirtualMachineScaleSet
	/// </summary>
	[EnumMember(Value = "Azure/VirtualMachineScaleSet")]
	VirtualMachineScaleSet = 55,

	/// <summary>
	///     Azure/VirtualMachineScaleSetVm
	/// </summary>
	[EnumMember(Value = "Azure/VirtualMachineScaleSetVM")]
	VirtualMachineScaleSetVm = 56,

	/// <summary>
	///     AWS/Lambda
	/// </summary>
	[EnumMember(Value = "AWS/Lambda")]
	AwsLambda = 57,

	/// <summary>
	///     AWS/ECS
	/// </summary>
	[EnumMember(Value = "AWS/ECS")]
	AwsEcs = 58,

	/// <summary>
	///     AWS/EFS
	/// </summary>
	[EnumMember(Value = "AWS/EFS")]
	AwsEfs = 59,

	/// <summary>
	///     AWS/Elasticsearch
	/// </summary>
	[EnumMember(Value = "AWS/Elasticsearch")]
	AwsElasticsearch = 60,

	/// <summary>
	///     AWS/SWF-ActivityType
	/// </summary>
	[EnumMember(Value = "AWS/SWF-ActivityType")]
	AwsSwfActivityType = 61,

	/// <summary>
	///     AWS/SWF-Workflows
	/// </summary>
	[EnumMember(Value = "AWS/SWF-WorkflowType")]
	AwsSwfWorkflows = 62,

	/// <summary>
	///     AWS/EMR
	/// </summary>
	[EnumMember(Value = "AWS/EMR")]
	AwsEmr = 63,

	/// <summary>
	///     Azure/ApplicationGateway
	/// </summary>
	[EnumMember(Value = "Azure/ApplicationGateway")]
	AzureApplicationGateway = 64,

	/// <summary>
	///     Azure/IoTHub
	/// </summary>
	[EnumMember(Value = "Azure/IoTHub")]
	AzureIotHub = 65,

	/// <summary>
	///     Azure/ServiceBus
	/// </summary>
	[EnumMember(Value = "Azure/ServiceBus")]
	AzureServiceBus = 66,

	/// <summary>
	///     Azure/Function
	/// </summary>
	[EnumMember(Value = "Azure/Function")]
	AzureFunction = 67,

	/// <summary>
	///     Azure/VirtualMachineScaleSet
	/// </summary>
	[EnumMember(Value = "Azure/VirtualMachineScaleSet")]
	AzureVirtualMachineScaleSet = 68,

	/// <summary>
	///     Azure/VirtualMachineScaleSetVM
	/// </summary>
	[EnumMember(Value = "Azure/VirtualMachineScaleSetVM")]
	AzureVirtualMachineScaleSetVm = 69,

	/// <summary>
	///     Azure/VirtualMachineScaleSetVM
	/// </summary>
	[EnumMember(Value = "AzureVmScaleSetSubgroup")]
	AzureVmScaleSetSubgroup = 70,

	/// <summary>
	///     AwsAutoscalingSubgroup
	/// </summary>
	[EnumMember(Value = "AwsAutoscalingSubgroup")]
	AwsAutoscalingSubgroup = 71,

	/// <summary>
	///     AWS/VPN
	/// </summary>
	[EnumMember(Value = "AWS/VPN")]
	AwsVpn = 72,

	/// <summary>
	///     AWS/Firehose
	/// </summary>
	[EnumMember(Value = "AWS/Firehose")]
	AwsFirehose = 73,

	/// <summary>
	///     AWS/SES
	/// </summary>
	[EnumMember(Value = "AWS/SES")]
	AwsSes = 74,

	/// <summary>
	///     AWS/NetworkELB
	/// </summary>
	[EnumMember(Value = "AWS/NetworkELB")]
	AwsNetworkElb = 75,

	/// <summary>
	///     AWS/WorkSpace
	/// </summary>
	[EnumMember(Value = "AWS/WorkSpace")]
	AwsWorkSpace = 76,

	/// <summary>
	///     AWS/DirectConnect
	/// </summary>
	[EnumMember(Value = "AWS/DirectConnect")]
	AwsDirectConnect = 77,


	/// <summary>
	///     Azure/SignalR
	/// </summary>
	[EnumMember(Value = "Azure/SignalR")]
	AzureSignalR = 78,

	/// <summary>
	///     Azure/AnalysisService
	/// </summary>
	[EnumMember(Value = "Azure/AnalysisService")]
	AzureAnalysisService = 79,

	/// <summary>
	///     Azure/TrafficManager
	/// </summary>
	[EnumMember(Value = "Azure/TrafficManager")]
	AzureTrafficManager = 80,

	/// <summary>
	///     Azure/MySQL
	/// </summary>
	[EnumMember(Value = "Azure/MySQL")]
	AzureMySql = 81,

	/// <summary>
	///     Azure/PostgreSQL
	/// </summary>
	[EnumMember(Value = "Azure/PostgreSQL")]
	AzurePostgreSql = 82,

	/// <summary>
	///     Azure/StorageAccount
	/// </summary>
	[EnumMember(Value = "Azure/StorageAccount")]
	AzureStorageAccount = 83,

	/// <summary>
	///     Azure/StorageAccountSubGroup
	/// </summary>
	[EnumMember(Value = "AzureStorageAccountSubgroup")]
	AzureStorageAccountSubGroup = 84,

	/// <summary>
	///     AWS/NATGateway
	/// </summary>
	[EnumMember(Value = "AWS/NATGateway")]
	AwsNatGateway = 85,

	/// <summary>
	///     Azure/BlobStorage
	/// </summary>
	[EnumMember(Value = "Azure/BlobStorage")]
	AzureBlobStorage = 86,

	/// <summary>
	///     Azure/FileStorage
	/// </summary>
	[EnumMember(Value = "Azure/FileStorage")]
	AzureFileStorage = 87,

	/// <summary>
	///     Azure/QueueStorage
	/// </summary>
	[EnumMember(Value = "Azure/QueueStorage")]
	AzureQueueStorage = 88,

	/// <summary>
	///     Azure/TableStorage
	/// </summary>
	[EnumMember(Value = "Azure/TableStorage")]
	AzureTableStorage = 89,

	/// <summary>
	///     Azure/ApiManagement
	/// </summary>
	[EnumMember(Value = "Azure/ApiManagement")]
	AzureApiManagement = 90,

	/// <summary>
	///     AWS/ElasticBeanstalk
	/// </summary>
	[EnumMember(Value = "AWS/ElasticBeanstalk")]
	AwsElasticBeanstalk = 91,

	/// <summary>
	///     AWS/WorkSpaceDirectory
	/// </summary>
	[EnumMember(Value = "AWS/WorkSpaceDirectory")]
	AwsWorkSpaceDirectory = 92,

	/// <summary>
	///     Azure/AppServicePlan
	/// </summary>
	[EnumMember(Value = "Azure/AppServicePlan")]
	AzureAppServicePlan = 93,

	/// <summary>
	///     Azure/AutomationAccount
	/// </summary>
	[EnumMember(Value = "Azure/AutomationAccount")]
	AzureAutomationAccount = 94,

	/// <summary>
	///     Azure/CosmosDB
	/// </summary>
	[EnumMember(Value = "Azure/CosmosDB")]
	AzureCosmosDb = 95,

	/// <summary>
	///     Azure/DataLakeAnalytics
	/// </summary>
	[EnumMember(Value = "Azure/DataLakeAnalytics")]
	AzureDataLakeAnalytics = 96,

	/// <summary>
	///     Azure/DataLakeStore
	/// </summary>
	[EnumMember(Value = "Azure/DataLakeStore")]
	AzureDataLakeStore = 97,

	/// <summary>
	///     Azure/ExpressRouteCircuit
	/// </summary>
	[EnumMember(Value = "Azure/ExpressRouteCircuit")]
	AzureExpressRouteCircuit = 98,

	/// <summary>
	///     Azure/VirtualNetworkGateway
	/// </summary>
	[EnumMember(Value = "Azure/VirtualNetworkGateway")]
	AzureVirtualNetworkGateway = 99,

	/// <summary>
	///     Azure/AppServicePlanSubgroup
	/// </summary>
	[EnumMember(Value = "AzureAppServicePlanSubgroup")]
	AzureAppServicePlanSubgroup = 100,

	/// <summary>
	///     Azure/AppServicePlanSubgroup
	/// </summary>
	[EnumMember(Value = "BizService")]
	Service = 101,

	/// <summary>
	///     AWS/DmsReplication
	/// </summary>
	[EnumMember(Value = "AWS/DmsReplication")]
	AwsDmsReplication = 102,

	/// <summary>
	///     Azure/MariaDB
	/// </summary>
	[EnumMember(Value = "Azure/MariaDB")]
	AzureMariaDb = 103,

	/// <summary>
	///     Azure/ApplicationInsights
	/// </summary>
	[EnumMember(Value = "Azure/ApplicationInsights")]
	AzureApplicationInsights = 104,

	/// <summary>
	///     Azure/Firewall
	/// </summary>
	[EnumMember(Value = "Azure/Firewall")]
	AzureFirewall = 105,

	/// <summary>
	///     Azure/SQLElasticPool
	/// </summary>
	[EnumMember(Value = "Azure/SQLElasticPool")]
	AzureSQLElasticPool = 106,

	/// <summary>
	///     Azure/SQLManagedInstance
	/// </summary>
	[EnumMember(Value = "Azure/SQLManagedInstance")]
	AzureSQLManagedInstance = 107,

	/// <summary>
	///     Azure/SQLManagedInstance
	/// </summary>
	[EnumMember(Value = "Azure/HDInsight")]
	AzureHDInsight = 108,

	/// <summary>
	///     Azure/RecoveryServices
	/// </summary>
	[EnumMember(Value = "Azure/RecoveryServices")]
	AzureRecoveryServices = 109,

	/// <summary>
	///     Azure/NetworkInterface
	/// </summary>
	[EnumMember(Value = "Azure/NetworkInterface")]
	AzureNetworkInterface = 110,

	/// <summary>
	///     Azure/BatchAccount
	/// </summary>
	[EnumMember(Value = "Azure/BatchAccount")]
	AzureBatchAccount = 111,

	/// <summary>
	///     Azure/LogicApps
	/// </summary>
	[EnumMember(Value = "Azure/LogicApps")]
	AzureLogicApps = 112,

	/// <summary>
	///     Azure/DataFactory
	/// </summary>
	[EnumMember(Value = "Azure/DataFactory")]
	AzureDataFactory = 113,

	/// <summary>
	///     Azure/PublicIP
	/// </summary>
	[EnumMember(Value = "Azure/PublicIP")]
	AzurePublicIP = 114,

	/// <summary>
	///     Azure/StreamAnalytics
	/// </summary>
	[EnumMember(Value = "Azure/StreamAnalytics")]
	AzureStreamAnalytics = 115,

	/// <summary>
	///     Azure/EventGrid
	/// </summary>
	[EnumMember(Value = "Azure/EventGrid")]
	AzureEventGrid = 116,

	/// <summary>
	///     Azure/LoadBalancers
	/// </summary>
	[EnumMember(Value = "Azure/LoadBalancers")]
	AzureLoadBalancers = 117,

	/// <summary>
	///     Azure/ServiceFabricMesh
	/// </summary>
	[EnumMember(Value = "Azure/ServiceFabricMesh")]
	AzureServiceFabricMesh = 118,

	/// <summary>
	///     Azure/CognitiveSearch
	/// </summary>
	[EnumMember(Value = "Azure/CognitiveSearch")]
	AzureCognitiveSearch = 119,

	/// <summary>
	///     Azure/CognitiveServices
	/// </summary>
	[EnumMember(Value = "Azure/CognitiveServices")]
	AzureCognitiveServices = 120,

	/// <summary>
	///     Azure/MLWorkspaces
	/// </summary>
	[EnumMember(Value = "Azure/MLWorkspaces")]
	AzureMLWorkspaces = 121,

	/// <summary>
	///     Azure/FrontDoors
	/// </summary>
	[EnumMember(Value = "Azure/FrontDoors")]
	AzureFrontDoors = 122,

	/// <summary>
	///     Azure/KeyVault
	/// </summary>
	[EnumMember(Value = "Azure/KeyVault")]
	AzureKeyVault = 123,

	/// <summary>
	///     Azure/RelayNamespaces
	/// </summary>
	[EnumMember(Value = "Azure/RelayNamespaces")]
	AzureRelayNamespaces = 124,

	/// <summary>
	///     Azure/NotificationHubs
	/// </summary>
	[EnumMember(Value = "Azure/NotificationHubs")]
	AzureNotificationHubs = 125,

	/// <summary>
	///     Azure/AppServiceEnvironment
	/// </summary>
	[EnumMember(Value = "Azure/AppServiceEnvironment")]
	AzureAppServiceEnvironment = 126,

	/// <summary>
	///     Occasionally, LogicMonitor returns "2001" as the device group type.  It is believed that this is a bug in LogicMonitor.
	///     This entry provides a workaround.
	/// </summary>
	Unknown2001 = 2001,

	/// <summary>
	///     Occasionally, LogicMonitor returns "2003" as the device group type.  It is believed that this is a bug in LogicMonitor.
	///     This entry provides a workaround.
	/// </summary>
	Unknown2003 = 2003
}
