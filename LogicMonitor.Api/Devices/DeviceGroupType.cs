using LogicMonitor.Api.Converters;
using Newtonsoft.Json;
using System.Runtime.Serialization;

namespace LogicMonitor.Api.Devices
{
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
		///     AWS/AwsRoot
		/// </summary>
		[EnumMember(Value = "AWS/AwsRoot")]
		AwsRoot = 3,

		/// <summary>
		///     AWS/EC2
		/// </summary>
		[EnumMember(Value = "AWS/EC2")]
		AwsEc2 = 4,

		/// <summary>
		///     AWS/DynamoDB
		/// </summary>
		[EnumMember(Value = "AWS/DynamoDB")]
		AwsDynamoDb = 5,

		/// <summary>
		///     AWS/ElastiCache
		/// </summary>
		[EnumMember(Value = "AWS/ElastiCache")]
		AwsElastiCache = 6,

		/// <summary>
		///     AWS/ELB
		/// </summary>
		[EnumMember(Value = "AWS/ELB")]
		AwsElb = 7,

		/// <summary>
		///     AWS/EBS
		/// </summary>
		[EnumMember(Value = "AWS/EBS")]
		AwsEbs = 8,

		/// <summary>
		///     AWS/SQS
		/// </summary>
		[EnumMember(Value = "AWS/SQS")]
		AwsSqs = 9,

		/// <summary>
		///     AWS/RDS
		/// </summary>
		[EnumMember(Value = "AWS/RDS")]
		AwsRds = 10,

		/// <summary>
		///     AWS/Route53
		/// </summary>
		[EnumMember(Value = "AWS/Route53")]
		AwsRoute53 = 11,

		/// <summary>
		///     AWS/S3
		/// </summary>
		[EnumMember(Value = "AWS/S3")]
		AwsS3 = 12,

		/// <summary>
		///     AWS/SNS
		/// </summary>
		[EnumMember(Value = "AWS/SNS")]
		AwsSns = 13,

		/// <summary>
		///     AWS/AutoScaling
		/// </summary>
		[EnumMember(Value = "AWS/AutoScaling")]
		AwsAutoScaling = 14,

		/// <summary>
		///     AWS/APIGateway
		/// </summary>
		[EnumMember(Value = "AWS/APIGateway")]
		AwsApiGateway = 15,

		/// <summary>
		///     AWS/ApplicationELB
		/// </summary>
		[EnumMember(Value = "AWS/ApplicationELB")]
		AwsApplicationElb = 16,

		/// <summary>
		///     AWS/Kinesis
		/// </summary>
		[EnumMember(Value = "AWS/Kinesis")]
		AwsKinesis = 17,

		/// <summary>
		///     AWS/RedShift
		/// </summary>
		[EnumMember(Value = "AWS/RedShift")]
		AwsRedShift = 18,

		/// <summary>
		///     Azure/AzureRoot
		/// </summary>
		[EnumMember(Value = "Azure/AzureRoot")]
		AzureAzureRoot = 19,

		/// <summary>
		///     Azure/AppService
		/// </summary>
		[EnumMember(Value = "Azure/AppService")]
		AzureAppService = 20,

		/// <summary>
		///     Azure/SQLDatabase
		/// </summary>
		[EnumMember(Value = "Azure/SQLDatabase")]
		AzureSqlDatabase = 21,

		/// <summary>
		///     Azure/VirtualMachine
		/// </summary>
		[EnumMember(Value = "Azure/VirtualMachine")]
		AzureVirtualMachine = 22,

		/// <summary>
		///     AWS/CloudFront
		/// </summary>
		[EnumMember(Value = "AWS/CloudFront")]
		AwsCloudFront = 23,

		/// <summary>
		///     Azure/EventHub
		/// </summary>
		[EnumMember(Value = "Azure/EventHub")]
		AzureEventHub = 24,

		/// <summary>
		///     Azure/RedisCache
		/// </summary>
		[EnumMember(Value = "Azure/RedisCache")]
		AzureRedisCache = 25,

		/// <summary>
		///     Azure/VirtualMachineScaleSet
		/// </summary>
		[EnumMember(Value = "Azure/VirtualMachineScaleSet")]
		VirtualMachineScaleSet = 26,

		/// <summary>
		///     Azure/VirtualMachineScaleSetVm
		/// </summary>
		[EnumMember(Value = "Azure/VirtualMachineScaleSetVM")]
		VirtualMachineScaleSetVm = 27,

		/// <summary>
		///     AWS/Lambda
		/// </summary>
		[EnumMember(Value = "AWS/Lambda")]
		AwsLambda = 28,

		/// <summary>
		///     AWS/ECS
		/// </summary>
		[EnumMember(Value = "AWS/ECS")]
		AwsEcs = 29,

		/// <summary>
		///     AWS/EFS
		/// </summary>
		[EnumMember(Value = "AWS/EFS")]
		AwsEfs = 30,

		/// <summary>
		///     AWS/Elasticsearch
		/// </summary>
		[EnumMember(Value = "AWS/Elasticsearch")]
		AwsElasticsearch = 31,

		/// <summary>
		///     AWS/SWF-ActivityType
		/// </summary>
		[EnumMember(Value = "AWS/SWF-ActivityType")]
		AwsSwfActivityType = 32,

		/// <summary>
		///     AWS/SWF-Workflows
		/// </summary>
		[EnumMember(Value = "AWS/SWF-WorkflowType")]
		AwsSwfWorkflows = 33,

		/// <summary>
		///     AWS/EMR
		/// </summary>
		[EnumMember(Value = "AWS/EMR")]
		AwsEmr = 34,

		/// <summary>
		///     Azure/ApplicationGateway
		/// </summary>
		[EnumMember(Value = "Azure/ApplicationGateway")]
		AzureApplicationGateway = 35,

		/// <summary>
		///     Azure/IoTHub
		/// </summary>
		[EnumMember(Value = "Azure/IoTHub")]
		AzureIotHub = 36,

		/// <summary>
		///     Azure/ServiceBus
		/// </summary>
		[EnumMember(Value = "Azure/ServiceBus")]
		AzureServiceBus = 37,

		/// <summary>
		///     Azure/Function
		/// </summary>
		[EnumMember(Value = "Azure/Function")]
		AzureFunction = 38,

		/// <summary>
		///     Azure/VirtualMachineScaleSet
		/// </summary>
		[EnumMember(Value = "Azure/VirtualMachineScaleSet")]
		AzureVirtualMachineScaleSet = 39,

		/// <summary>
		///     Azure/VirtualMachineScaleSetVM
		/// </summary>
		[EnumMember(Value = "Azure/VirtualMachineScaleSetVM")]
		AzureVirtualMachineScaleSetVm = 40,

		/// <summary>
		///     Azure/VirtualMachineScaleSetVM
		/// </summary>
		[EnumMember(Value = "AzureVmScaleSetSubgroup")]
		AzureVmScaleSetSubgroup = 41,

		/// <summary>
		///     AwsAutoscalingSubgroup
		/// </summary>
		[EnumMember(Value = "AwsAutoscalingSubgroup")]
		AwsAutoscalingSubgroup = 42,

		/// <summary>
		///     AWS/VPN
		/// </summary>
		[EnumMember(Value = "AWS/VPN")]
		AwsVpn = 43,

		/// <summary>
		///     AWS/Firehose
		/// </summary>
		[EnumMember(Value = "AWS/Firehose")]
		AwsFirehose = 44,

		/// <summary>
		///     AWS/SES
		/// </summary>
		[EnumMember(Value = "AWS/SES")]
		AwsSes = 45,

		/// <summary>
		///     AWS/NetworkELB
		/// </summary>
		[EnumMember(Value = "AWS/NetworkELB")]
		AwsNetworkElb = 46,

		/// <summary>
		///     AWS/WorkSpace
		/// </summary>
		[EnumMember(Value = "AWS/WorkSpace")]
		AwsWorkSpace = 47,

		/// <summary>
		///     AWS/DirectConnect
		/// </summary>
		[EnumMember(Value = "AWS/DirectConnect")]
		AwsDirectConnect = 48,

		/// <summary>
		///     Azure/AnalysisService
		/// </summary>
		[EnumMember(Value = "Azure/AnalysisService")]
		AzureAnalysisService = 49,

		/// <summary>
		///     Azure/MySQL
		/// </summary>
		[EnumMember(Value = "Azure/MySQL")]
		AzureMySql = 50,

		/// <summary>
		///     Azure/PostgreSQL
		/// </summary>
		[EnumMember(Value = "Azure/PostgreSQL")]
		AzurePostgreSql = 51,

		/// <summary>
		///     Azure/StorageAccount
		/// </summary>
		[EnumMember(Value = "Azure/StorageAccount")]
		AzureStorageAccount = 52,

		/// <summary>
		///     Azure/StorageAccountSubGroup
		/// </summary>
		[EnumMember(Value = "AzureStorageAccountSubgroup")]
		AzureStorageAccountSubGroup = 53,

		/// <summary>
		///     AWS/NATGateway
		/// </summary>
		[EnumMember(Value = "AWS/NATGateway")]
		AwsNatGateway = 54,

		/// <summary>
		///     Azure/BlobStorage
		/// </summary>
		[EnumMember(Value = "Azure/BlobStorage")]
		AzureBlobStorage = 55,

		/// <summary>
		///     Azure/FileStorage
		/// </summary>
		[EnumMember(Value = "Azure/FileStorage")]
		AzureFileStorage = 56,

		/// <summary>
		///     Azure/QueueStorage
		/// </summary>
		[EnumMember(Value = "Azure/QueueStorage")]
		AzureQueueStorage = 57,

		/// <summary>
		///     Azure/TableStorage
		/// </summary>
		[EnumMember(Value = "Azure/TableStorage")]
		AzureTableStorage = 58,

		/// <summary>
		///     Azure/ApiManagement
		/// </summary>
		[EnumMember(Value = "Azure/ApiManagement")]
		AzureApiManagement = 59,

		/// <summary>
		///     AWS/ElasticBeanstalk
		/// </summary>
		[EnumMember(Value = "AWS/ElasticBeanstalk")]
		AwsElasticBeanstalk = 60,

		/// <summary>
		///     AWS/WorkSpaceDirectory
		/// </summary>
		[EnumMember(Value = "AWS/WorkSpaceDirectory")]
		AwsWorkSpaceDirectory = 61,

		/// <summary>
		///     Azure/AppServicePlan
		/// </summary>
		[EnumMember(Value = "Azure/AppServicePlan")]
		AzureAppServicePlan = 62,

		/// <summary>
		///     Azure/AutomationAccount
		/// </summary>
		[EnumMember(Value = "Azure/AutomationAccount")]
		AzureAutomationAccount = 63,

		/// <summary>
		///     Azure/CosmosDB
		/// </summary>
		[EnumMember(Value = "Azure/CosmosDB")]
		AzureCosmosDb = 64,

		/// <summary>
		///     Azure/DataLakeAnalytics
		/// </summary>
		[EnumMember(Value = "Azure/DataLakeAnalytics")]
		AzureDataLakeAnalytics = 65,

		/// <summary>
		///     Azure/DataLakeStore
		/// </summary>
		[EnumMember(Value = "Azure/DataLakeStore")]
		AzureDataLakeStore = 66,

		/// <summary>
		///     Azure/ExpressRouteCircuit
		/// </summary>
		[EnumMember(Value = "Azure/ExpressRouteCircuit")]
		AzureExpressRouteCircuit = 67,

		/// <summary>
		///     Azure/VirtualNetworkGateway
		/// </summary>
		[EnumMember(Value = "Azure/VirtualNetworkGateway")]
		AzureVirtualNetworkGateway = 68,

		/// <summary>
		///     Azure/AppServicePlanSubgroup
		/// </summary>
		[EnumMember(Value = "AzureAppServicePlanSubgroup")]
		AzureAppServicePlanSubgroup = 69,

		/// <summary>
		///     Azure/AppServicePlanSubgroup
		/// </summary>
		[EnumMember(Value = "BizService")]
		Service = 70,

		/// <summary>
		///     AWS/DmsReplication
		/// </summary>
		[EnumMember(Value = "AWS/DmsReplication")]
		AwsDmsReplication = 71,

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
}