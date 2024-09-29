namespace LogicMonitor.Api.LogicModules;

/// <summary>
/// An autodiscovery method
/// </summary>
[DataContract]
public class AutoDiscoveryMethod
{
	/// <summary>
	/// The auto discovery method name
	/// </summary>
	[DataMember(Name = "name")]
	public string Name { get; set; } = string.Empty;

	/// <summary>
	/// The AWS billing reporting attribute
	/// </summary>
	[DataMember(Name = "awsBillingReportAttribute")]
	public string AwsBillingReportAttribute { get; set; } = string.Empty;

	/// <summary>
	/// The AWS service name
	/// </summary>
	[DataMember(Name = "awsServiceName")]
	public string AwsServiceName { get; set; } = string.Empty;

	/// <summary>
	/// Azure Billing Type
	/// </summary>
	[DataMember(Name = "azureBillingType")]
	public string AzureBillingType { get; set; } = string.Empty;

	/// <summary>
	/// azureCostManagementType
	/// </summary>
	[DataMember(Name = "azureCostManagementType")]
	public string AzureCostManagementType { get; set; } = string.Empty;

	/// <summary>
	/// azureSynapseType
	/// </summary>
	[DataMember(Name = "azureSynapseType")]
	public string AzureSynapseType { get; set; } = string.Empty;

	/// <summary>
	/// The CIM class
	/// </summary>
	[DataMember(Name = "cimClass")]
	public string CimClass { get; set; } = string.Empty;

	/// <summary>
	/// columnInstanceName
	/// </summary>
	[DataMember(Name = "columnInstanceName")]
	public string ColumnInstanceName { get; set; } = string.Empty;

	/// <summary>
	/// The property
	/// </summary>
	[DataMember(Name = "property")]
	public string Property { get; set; } = string.Empty;

	/// <summary>
	/// The namespace
	/// </summary>
	[DataMember(Name = "namespace")]
	public string Namespace { get; set; } = string.Empty;

	/// <summary>
	/// The cluster dimension
	/// </summary>
	[DataMember(Name = "clusterDimension")]
	public string ClusterDimension { get; set; } = string.Empty;

	/// <summary>
	/// The instance path
	/// </summary>
	[DataMember(Name = "instancePath")]
	public string? InstancePath { get; set; }

	/// <summary>
	/// The period
	/// </summary>
	[DataMember(Name = "period")]
	public string Period { get; set; } = string.Empty;

	/// <summary>
	/// Metric Name
	/// </summary>
	[DataMember(Name = "metricName")]
	public string MetricName { get; set; } = string.Empty;

	/// <summary>
	/// Node dimension
	/// </summary>
	[DataMember(Name = "nodeDimension")]
	public string NodeDimension { get; set; } = string.Empty;

	/// <summary>
	/// The cluster dimension value
	/// </summary>
	[DataMember(Name = "clusterDimensionValue")]
	public string ClusterDimensionValue { get; set; } = string.Empty;

	/// <summary>
	/// The entity
	/// </summary>
	[DataMember(Name = "entity")]
	public string Entity { get; set; } = string.Empty;

	/// <summary>
	/// GCP Billing Period
	/// </summary>
	[DataMember(Name = "gcpBillingPeriodType")]
	public string GcpBillingPeriod { get; set; } = string.Empty;

	/// <summary>
	/// GCP Billing Type
	/// </summary>
	[DataMember(Name = "gcpBillingType")]
	public string GcpBillingType { get; set; } = string.Empty;

	/// <summary>
	/// The regular expression
	/// </summary>
	[DataMember(Name = "regex")]
	public string Regex { get; set; } = string.Empty;

	/// <summary>
	/// Whether it is case sensitive
	/// </summary>
	[DataMember(Name = "caseSensitive")]
	public bool IsCaseSensitive { get; set; }

	/// <summary>
	/// followRedirect
	/// </summary>
	[DataMember(Name = "followRedirect")]
	public string FollowRedirect { get; set; } = string.Empty;

	/// <summary>
	/// The ports
	/// </summary>
	[DataMember(Name = "ports")]
	public string Ports { get; set; } = string.Empty;

	/// <summary>
	/// The URI
	/// </summary>
	[DataMember(Name = "uri")]
	public string Uri { get; set; } = string.Empty;

	/// <summary>
	/// Whether to use SSL
	/// </summary>
	[DataMember(Name = "useSSL")]
	public bool UseSsl { get; set; }

	/// <summary>
	/// The method
	/// </summary>
	[DataMember(Name = "method")]
	public string Method { get; set; } = string.Empty;

	/// <summary>
	/// The query
	/// </summary>
	[DataMember(Name = "query")]
	public string Query { get; set; } = string.Empty;

	/// <summary>
	/// The type
	/// </summary>
	[DataMember(Name = "type")]
	public string Type { get; set; } = string.Empty;

	/// <summary>
	/// The separator
	/// </summary>
	[DataMember(Name = "separator")]
	public string Separator { get; set; } = string.Empty;

	/// <summary>
	/// The url
	/// </summary>
	[DataMember(Name = "url")]
	public string Url { get; set; } = string.Empty;

	/// <summary>
	/// The sid
	/// </summary>
	[DataMember(Name = "sid")]
	public string Sid { get; set; } = string.Empty;

	/// <summary>
	/// The path
	/// </summary>
	[DataMember(Name = "path")]
	public string Path { get; set; } = string.Empty;

	/// <summary>
	/// The request
	/// </summary>
	[DataMember(Name = "request")]
	public string Request { get; set; } = string.Empty;

	/// <summary>
	/// The instance name
	/// </summary>
	[DataMember(Name = "instanceName")]
	public string InstanceName { get; set; } = string.Empty;

	/// <summary>
	/// The instance group name
	/// </summary>
	[DataMember(Name = "instanceGroupName")]
	public string InstanceGroupName { get; set; } = string.Empty;

	/// <summary>
	/// The instance value
	/// </summary>
	[DataMember(Name = "instanceValue")]
	public string InstanceValue { get; set; } = string.Empty;

	/// <summary>
	/// The instance description
	/// </summary>
	[DataMember(Name = "instanceDescription")]
	public string InstanceDescription { get; set; } = string.Empty;

	/// <summary>
	/// The instance locator
	/// </summary>
	[DataMember(Name = "instanceLocator")]
	public string InstanceLocator { get; set; } = string.Empty;

	/// <summary>
	/// Headers
	/// </summary>
	[DataMember(Name = "headers")]
	public string Headers { get; set; } = string.Empty;

	/// <summary>
	/// Read Timeout ms
	/// </summary>
	[DataMember(Name = "readTimeout")]
	public int ReadTimeoutMs { get; set; }

	/// <summary>
	/// Connect Timeout ms
	/// </summary>
	[DataMember(Name = "connectTimeout")]
	public int ConnectTimeoutMs { get; set; }

	/// <summary>
	/// Group Label
	/// </summary>
	[DataMember(Name = "groupLabel")]
	public string GroupLabel { get; set; } = string.Empty;

	/// <summary>
	/// Instance Label
	/// </summary>
	[DataMember(Name = "instanceLabel")]
	public string InstanceLabel { get; set; } = string.Empty;

	/// <summary>
	/// Instance Property Tags
	/// </summary>
	[DataMember(Name = "instancePropertyTags")]
	public string InstancePropertyTags { get; set; } = string.Empty;

	/// <summary>
	/// The object regex
	/// </summary>
	[DataMember(Name = "objRegex")]
	public string ObjectRegex { get; set; } = string.Empty;

	/// <summary>
	/// The category
	/// </summary>
	[DataMember(Name = "category")]
	public string Category { get; set; } = string.Empty;

	/// <summary>
	/// The timeout in ms
	/// </summary>
	[DataMember(Name = "timeout")]
	public int Timeout { get; set; }

	/// <summary>
	/// Whether SNMP instance level properties are enabled
	/// </summary>
	[DataMember(Name = "enableSNMPILP")]
	public bool EnableSNMPILP { get; set; }

	/// <summary>
	/// Lookup OID
	/// </summary>
	[DataMember(Name = "lookupOID")]
	public string LookupOid { get; set; } = string.Empty;

	/// <summary>
	/// The external resource id
	/// </summary>
	[DataMember(Name = "externalResourceID")]
	public string ExternalResourceId { get; set; } = string.Empty;

	/// <summary>
	/// Description OID
	/// </summary>
	[DataMember(Name = "descriptionOID")]
	public string DescriptionOid { get; set; } = string.Empty;

	/// <summary>
	/// The external resource type
	/// </summary>
	[DataMember(Name = "externalResourceType")]
	public string ExternalResourceType { get; set; } = string.Empty;

	/// <summary>
	/// OID
	/// </summary>
	[DataMember(Name = "OID")]
	public string Oid { get; set; } = string.Empty;

	/// <summary>
	/// Object
	/// </summary>
	[DataMember(Name = "object")]
	public string ADOObject { get; set; } = string.Empty;

	/// <summary>
	/// The instance level properties
	/// </summary>
	[DataMember(Name = "ILP")]
	public List<InstanceLevelProperty> InstanceLevelProperties { get; set; } = [];

	/// <summary>
	/// Discovery Type
	/// </summary>
	[DataMember(Name = "discoveryType")]
	public string DiscoveryType { get; set; } = string.Empty;

	/// <summary>
	/// Resource URL
	/// </summary>
	[DataMember(Name = "resourceUrl")]
	public string? ResourceUrl { get; set; }

	/// <summary>
	/// Zoom plan usage type
	/// </summary>
	[DataMember(Name = "zoomPlanUsageType")]
	public ZoomPlanUsageType ZoomPlanUsageType { get; set; }

	/// <summary>
	/// The Windows script
	/// </summary>
	[DataMember(Name = "winScript")]
	public string WindowsScript { get; set; } = string.Empty;

	/// <summary>
	/// The groovyScript
	/// </summary>
	[DataMember(Name = "groovyScript")]
	public string GroovyScript { get; set; } = string.Empty;

	/// <summary>
	/// The Linux command line
	/// </summary>
	[DataMember(Name = "linuxCmdLine")]
	public string LinuxCommandLine { get; set; } = string.Empty;

	/// <summary>
	/// The Linux script
	/// </summary>
	[DataMember(Name = "linuxScript")]
	public string LinuxScript { get; set; } = string.Empty;

	/// <summary>
	/// The Windows command line
	/// </summary>
	[DataMember(Name = "winCmdLine")]
	public string WindowsCommandLine { get; set; } = string.Empty;

	/// <summary>
	/// The linked classes
	/// </summary>
	[DataMember(Name = "linkedClasses")]
	public List<LinkedWmiClass> LinkedClasses { get; set; } = [];

	/// <summary>
	/// wmiClass
	/// </summary>
	[DataMember(Name = "wmiClass")]
	public string WmiClass { get; set; } = string.Empty;

	/// <summary>
	/// Whether to enabled linked class instance level properties
	/// </summary>
	[DataMember(Name = "enableLinkedClassILP")]
	public bool AreLinkedClassInstanceLevelPropertiesEnabled { get; set; }

	/// <summary>
	/// Whether SNMP instance level properties are enabled
	/// </summary>
	[DataMember(Name = "enableWmiClassILP")]
	public bool AreWmiClassInstanceLevelPropertiesEnabled { get; set; }

	/// <summary>
	/// Collector id
	/// </summary>
	[DataMember(Name = "collectorId")]
	public string CollectorId { get; set; } = string.Empty;

	/// <summary>
	/// zoomRoomIssueType
	/// </summary>
	[DataMember(Name = "zoomRoomIssueType")]
	public string ZoomRoomIssueType { get; set; } = string.Empty;

	/// <summary>
	/// azureTagKeys
	/// </summary>
	[DataMember(Name = "azureTagKeys")]
	public string AzureTagKeys { get; set; } = string.Empty;

	/// <summary>
	/// Value Attribute
	/// </summary>
	[DataMember(Name = "valueAttribute")]
	public string? ValueAttribute { get; set; } = string.Empty;

	/// <summary>
	/// Value Attribute
	/// </summary>
	[DataMember(Name = "value2Attribute")]
	public string? Value2Attribute { get; set; } = string.Empty;

	/// <summary>
	/// aliasAttribute
	/// </summary>
	[DataMember(Name = "aliasAttribute")]
	public string AliasAttribute { get; set; } = string.Empty;

	/// <summary>
	/// descriptionAttribute
	/// </summary>
	[DataMember(Name = "descriptionAttribute")]
	public string DescriptionAttribute { get; set; } = string.Empty;

	/// <summary>
	/// propertyAttributes
	/// </summary>
	[DataMember(Name = "propertyAttributes")]
	public string PropertyAttributes { get; set; } = string.Empty;
}
