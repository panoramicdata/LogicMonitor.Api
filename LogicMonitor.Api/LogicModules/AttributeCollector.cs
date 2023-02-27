namespace LogicMonitor.Api.LogicModules;

/// <summary>
///     A collector attribute
/// </summary>
[DataContract]
public class AttributeCollector
{
	/// <summary>
	///     The AWS range value
	/// </summary>
	[DataMember(Name = "awsRangeValue")]
	public string AwsRangeValue { get; set; }

	/// <summary>
	///     The AWS attribute name
	/// </summary>
	[DataMember(Name = "awsAttributeName")]
	public string AwsAttributeName { get; set; }

	/// <summary>
	///     The AWS attribute type
	/// </summary>
	[DataMember(Name = "awsDynamodbAttrType")]
	public string AwsDynamoDbAttrType { get; set; }

	/// <summary>
	///     The AWS key value
	/// </summary>
	[DataMember(Name = "awsKeyValue")]
	public string AwsKeyValue { get; set; }

	/// <summary>
	///     The AWS awsQueryIndexType
	/// </summary>
	[DataMember(Name = "awsQueryIndexType")]
	public string AwsQueryIndexType { get; set; }

	/// <summary>
	///     The AWS Query Index Name
	/// </summary>
	[DataMember(Name = "awsQueryIndexName")]
	public string AwsQueryIndexName { get; set; }

	/// <summary>
	///     The AWS Query Key Value
	/// </summary>
	[DataMember(Name = "awsQueryKeyValue")]
	public string AwsQueryKeyValue { get; set; }

	/// <summary>
	///     The AWS Query Range Op
	/// </summary>
	[DataMember(Name = "awsQueryRangeOp")]
	public string AwsQueryRangeOp { get; set; }

	/// <summary>
	///     The AWS Query Range Value
	/// </summary>
	[DataMember(Name = "awsQueryRangeValue")]
	public string AwsQueryRangeValue { get; set; }

	/// <summary>
	///     The AWS SQS message size in bytes
	/// </summary>
	[DataMember(Name = "awsSqsMessageSize")]
	public int AwsSqsMessageSizeBytes { get; set; }

	/// <summary>
	///     The AWS SQS message size in bytes
	/// </summary>
	[DataMember(Name = "awsSqsMessageNum")]
	public int AwsSqsMessageNumber { get; set; }

	/// <summary>
	///     The AWS SQS message size in bytes
	/// </summary>
	[DataMember(Name = "awsServiceName")]
	public string AwsServiceName { get; set; }

	/// <summary>
	///     The connection timeout in ms
	/// </summary>
	[DataMember(Name = "connectTimeout")]
	public int ConnectTimeoutMs { get; set; }

	/// <summary>
	///     The counters
	/// </summary>
	[DataMember(Name = "counters")]
	public List<CollectorAttributeCounter> Counters { get; set; }

	/// <summary>
	/// data collector\u0027s name
	/// </summary>
	[DataMember(Name = "name")]
	public string Name { get; set; }

	/// <summary>
	///     Encoding
	/// </summary>
	[DataMember(Name = "encoding")]
	public string Encoding { get; set; }

	/// <summary>
	///     Check Type
	/// </summary>
	[DataMember(Name = "checkType")]
	public CheckType CheckType { get; set; }

	/// <summary>
	///     Configs
	/// </summary>
	[DataMember(Name = "configs")]
	public string? Configs { get; set; }

	/// <summary>
	///     The entity
	/// </summary>
	[DataMember(Name = "entity")]
	public string Entity { get; set; }

	/// <summary>
	///     The fields
	/// </summary>
	[DataMember(Name = "fields")]
	public List<string> Fields { get; set; }

	/// <summary>
	///     Whether to follow redirect
	/// </summary>
	[DataMember(Name = "followRedirect")]
	public string FollowRedirect { get; set; }

	/// <summary>
	///     Headers
	/// </summary>
	[DataMember(Name = "headers")]
	public string Headers { get; set; }

	/// <summary>
	///     The instance column name
	/// </summary>
	[DataMember(Name = "instanceColumnName")]
	public string InstanceColumnName { get; set; }

	/// <summary>
	///     The IP address
	/// </summary>
	[DataMember(Name = "ip")]
	public string IpAddress { get; set; }

	/// <summary>
	///     An IPMI Sensor
	/// </summary>
	[DataMember(Name = "ipmiSensor")]
	public string IpmiSensor { get; set; }

	/// <summary>
	///     The Type
	/// </summary>
	[DataMember(Name = "type")]
	public string Type { get; set; }

	/// <summary>
	///     The method Name
	/// </summary>
	[DataMember(Name = "methodName")]
	public string MethodName { get; set; }

	/// <summary>
	///     The namespace
	/// </summary>
	[DataMember(Name = "namespace")]
	public string Namespace { get; set; }

	/// <summary>
	///     The NetApp aggregate
	/// </summary>
	[DataMember(Name = "netAppAggregate")]
	public string NetAppAggregate { get; set; }

	/// <summary>
	///     The NetApp API
	/// </summary>
	[DataMember(Name = "netAppAPI")]
	public string NetAppApi { get; set; }

	/// <summary>
	///     The NetApp index
	/// </summary>
	[DataMember(Name = "netAppIndex")]
	public int? NetAppIndex { get; set; }

	/// <summary>
	///     The NetApp instance
	/// </summary>
	[DataMember(Name = "netAppInstance")]
	public string NetAppInstance { get; set; }

	/// <summary>
	///     The NetApp object
	/// </summary>
	[DataMember(Name = "netAppObject")]
	public string NetAppObject { get; set; }

	/// <summary>
	///     The NetApp type
	/// </summary>
	[DataMember(Name = "netAppType")]
	public string NetAppType { get; set; }

	/// <summary>
	///     The NetApp XML
	/// </summary>
	[DataMember(Name = "netAppXML")]
	public string NetAppXml { get; set; }

	/// <summary>
	///     The NetApp XML index
	/// </summary>
	[DataMember(Name = "netAppXMLIndex")]
	public string NetAppXmlIndex { get; set; }

	/// <summary>
	///     The NetApp XML instance
	/// </summary>
	[DataMember(Name = "netAppXMLInstance")]
	public string NetAppXmlInstance { get; set; }

	/// <summary>
	///     The NetApp XML bulk
	/// </summary>
	[DataMember(Name = "netAppXMLBulk")]
	public string NetAppXmlBulk { get; set; }

	/// <summary>
	///     The NetApp XML bulk locator
	/// </summary>
	[DataMember(Name = "netAppXMLBulkLocator")]
	public string NetAppXmlBulkLocator { get; set; }

	/// <summary>
	///     The NetApp XML locator
	/// </summary>
	[DataMember(Name = "netAppXMLLocator")]
	public string NetAppXmlLocator { get; set; }

	/// <summary>
	///     The parameters
	/// </summary>
	[DataMember(Name = "params")]
	public List<string> Parameters { get; set; }

	/// <summary>
	///     Payload
	/// </summary>
	[DataMember(Name = "payload")]
	public string Payload { get; set; }

	/// <summary>
	/// Plan type
	/// </summary>
	[DataMember(Name = "planType")]
	public string PlanType { get; set; }

	/// <summary>
	///     The port
	/// </summary>
	[DataMember(Name = "port")]
	public string Port { get; set; }

	/// <summary>
	///     The properties
	/// </summary>
	[DataMember(Name = "properties")]
	public string Properties { get; set; }

	/// <summary>
	///     The query
	/// </summary>
	[DataMember(Name = "query")]
	public string Query { get; set; }

	/// <summary>
	///     The query class
	/// </summary>
	[DataMember(Name = "queryClass")]
	public string QueryClass { get; set; }

	/// <summary>
	///     The query index
	/// </summary>
	[DataMember(Name = "queryIndex")]
	public string QueryIndex { get; set; }

	/// <summary>
	///     The query URL
	/// </summary>
	[DataMember(Name = "queryUrl")]
	public string QueryUrl { get; set; }

	/// <summary>
	///     The query value
	/// </summary>
	[DataMember(Name = "queryValue")]
	public string QueryValue { get; set; }

	/// <summary>
	///     The read timeout in ms
	/// </summary>
	[DataMember(Name = "readTimeout")]
	public int ReadTimeoutMs { get; set; }

	/// <summary>
	///     The request
	/// </summary>
	[DataMember(Name = "request")]
	public string Request { get; set; }

	/// <summary>
	///     The number of packets to send
	/// </summary>
	[DataMember(Name = "sendPkts")]
	public int SendPacketCount { get; set; }

	/// <summary>
	///     The target path
	/// </summary>
	[DataMember(Name = "targetPath")]
	public string TargetPath { get; set; }

	/// <summary>
	///     The method inputs
	/// </summary>
	[DataMember(Name = "methodInputs")]
	public string MethodInputs { get; set; }

	/// <summary>
	///     The linuxCmdline
	/// </summary>
	[DataMember(Name = "linuxCmdline")]
	public string LinuxCommandLine { get; set; }

	/// <summary>
	///     The linuxScript
	/// </summary>
	[DataMember(Name = "linuxScript")]
	public string LinuxScript { get; set; }

	/// <summary>
	///     The groovyScript
	/// </summary>
	[DataMember(Name = "groovyScript")]
	public string GroovyScript { get; set; }

	/// <summary>
	///     The password
	/// </summary>
	[DataMember(Name = "password")]
	public string Password { get; set; }

	/// <summary>
	///     The period
	/// </summary>
	[DataMember(Name = "period")]
	public string Period { get; set; }

	/// <summary>
	///     The scriptType
	/// </summary>
	[DataMember(Name = "scriptType")]
	public string ScriptType { get; set; }

	/// <summary>
	///     The report name
	/// </summary>
	[DataMember(Name = "reportName")]
	public string ReportName { get; set; }

	/// <summary>
	///     The URL
	/// </summary>
	[DataMember(Name = "url")]
	public string Url { get; set; }

	/// <summary>
	///     Whether to use SSL
	/// </summary>
	[DataMember(Name = "useSSL")]
	public bool UseSsl { get; set; }

	/// <summary>
	///     The username
	/// </summary>
	[DataMember(Name = "username")]
	public string Username { get; set; }

	/// <summary>
	///     The windowsCmdline
	/// </summary>
	[DataMember(Name = "windowsCmdline")]
	public string WindowsCommandLine { get; set; }

	/// <summary>
	///     The windowsScript
	/// </summary>
	[DataMember(Name = "windowsScript")]
	public string WindowsScript { get; set; }

	/// <summary>
	///     The xenEntity
	/// </summary>
	[DataMember(Name = "xenEntity")]
	public string XenEntity { get; set; }

	/// <summary>
	///     Timeout
	/// </summary>
	[DataMember(Name = "timeout")]
	public int Timeout { get; set; }

	/// <summary>
	/// syntheticScript
	/// </summary>
	[DataMember(Name = "syntheticScript")]
	public string? SyntheticScript { get; set; }

	/// <summary>
	/// dateColumn
	/// </summary>
	[DataMember(Name = "dateColumn")]
	public string DateColumns { get; set; }

	/// <summary>
	/// dateColumn
	/// </summary>
	[DataMember(Name = "soqlEntity")]
	public string SoqlEntity { get; set; }

	/// <summary>
	/// whereClause
	/// </summary>
	[DataMember(Name = "whereClause")]
	public string WhereClause { get; set; }

	/// <summary>
	/// groupByClause
	/// </summary>
	[DataMember(Name = "groupByClause")]
	public string GroupByClause { get; set; }

	/// <summary>
	/// endpointUrlSuffix
	/// </summary>
	[DataMember(Name = "endpointUrlSuffix")]
	public string EndpointUrlSuffix { get; set; }
}
