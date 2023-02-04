namespace LogicMonitor.Api.LogicModules;

/// <summary>
/// An autodiscovery method
/// </summary>
[DataContract]
public class AutoDiscoveryMethod
{
	/// <summary>
	/// Whether SNMP instance level properties are enabled
	/// </summary>
	[DataMember(Name = "enableSNMPILP")]
	public string AreSnmpInstanceLevelPropertiesEnabled { get; set; }

	/// <summary>
	/// The AWS billing reporting attribute
	/// </summary>
	[DataMember(Name = "awsBillingReportAttribute")]
	public string AwsBillingReportAttribute { get; set; }

	/// <summary>
	/// The AWS service name
	/// </summary>
	[DataMember(Name = "awsServiceName")]
	public string AwsServiceName { get; set; }

	/// <summary>
	/// Azure Billing Type
	/// </summary>
	[DataMember(Name = "azureBillingType")]
	public string AzureBillingType { get; set; }

	/// <summary>
	/// The cluster dimension
	/// </summary>
	[DataMember(Name = "clusterDimension")]
	public string ClusterDimension { get; set; }

	/// <summary>
	/// The cluster dimension value
	/// </summary>
	[DataMember(Name = "clusterDimensionValue")]
	public string ClusterDimensionValue { get; set; }

	/// <summary>
	/// The entity
	/// </summary>
	[DataMember(Name = "entity")]
	public string Entity { get; set; }

	/// <summary>
	/// The object regex
	/// </summary>
	[DataMember(Name = "objRegex")]
	public string ObjectRegex { get; set; }

	/// <summary>
	/// The category
	/// </summary>
	[DataMember(Name = "category")]
	public string Category { get; set; }

	/// <summary>
	/// The CIM class
	/// </summary>
	[DataMember(Name = "cimClass")]
	public string CimClass { get; set; }

	/// <summary>
	/// The collector id
	/// </summary>
	[DataMember(Name = "collectorId")]
	public string CollectorId { get; set; }

	/// <summary>
	/// Name
	/// </summary>
	[DataMember(Name = "name")]
	public string Name { get; set; }

	/// <summary>
	/// Node dimension
	/// </summary>
	[DataMember(Name = "nodeDimension")]
	public string NodeDimension { get; set; }

	/// <summary>
	/// Discovery Type
	/// </summary>
	[DataMember(Name = "discoveryType")]
	public string DiscoveryType { get; set; }

	/// <summary>
	/// Whether to follow redirect
	/// </summary>
	[DataMember(Name = "followRedirect")]
	public bool FollowRedirect { get; set; }

	/// <summary>
	/// GCP Billing Type
	/// </summary>
	[DataMember(Name = "gcpBillingType")]
	public string GcpBillingType { get; set; }

	/// <summary>
	/// The groovyScript
	/// </summary>
	[DataMember(Name = "groovyScript")]
	public string GroovyScript { get; set; }

	/// <summary>
	/// The instance description
	/// </summary>
	[DataMember(Name = "instanceDescription")]
	public string InstanceDescription { get; set; }

	/// <summary>
	/// The instance locator
	/// </summary>
	[DataMember(Name = "instanceLocator")]
	public string InstanceLocator { get; set; }

	/// <summary>
	/// The instance group name
	/// </summary>
	[DataMember(Name = "instanceGroupName")]
	public string InstanceGroupName { get; set; }

	/// <summary>
	/// The instance name
	/// </summary>
	[DataMember(Name = "instanceName")]
	public string InstanceName { get; set; }

	/// <summary>
	/// The instance value
	/// </summary>
	[DataMember(Name = "instanceValue")]
	public string InstanceValue { get; set; }

	/// <summary>
	/// Whether it is case sensitive
	/// </summary>
	[DataMember(Name = "caseSensitive")]
	public bool IsCaseSensitive { get; set; }

	/// <summary>
	/// The method
	/// </summary>
	[DataMember(Name = "method")]
	public string Method { get; set; }

	/// <summary>
	/// OID (SNMP only)
	/// </summary>
	[DataMember(Name = "OID")]
	public string Oid { get; set; }

	/// <summary>
	/// The sid
	/// </summary>
	[DataMember(Name = "sid")]
	public string Sid { get; set; }

	/// <summary>
	/// Name
	/// </summary>
	[DataMember(Name = "type")]
	public string Type { get; set; }

	/// <summary>
	/// Description OID (SNMP only)
	/// </summary>
	[DataMember(Name = "descriptionOID")]
	public string DescriptionOid { get; set; }

	/// <summary>
	/// Lookup OID (SNMP only)
	/// </summary>
	[DataMember(Name = "lookupOID")]
	public string LookupOid { get; set; }

	/// <summary>
	/// namespace
	/// </summary>
	[DataMember(Name = "namespace")]
	public string Namespace { get; set; }

	/// <summary>
	/// The object
	/// </summary>
	[DataMember(Name = "object")]
	public string Object { get; set; }

	/// <summary>
	/// The path
	/// </summary>
	[DataMember(Name = "path")]
	public string Path { get; set; }

	/// <summary>
	/// The period
	/// </summary>
	[DataMember(Name = "period")]
	public string Period { get; set; }

	/// <summary>
	/// The property
	/// </summary>
	[DataMember(Name = "property")]
	public string Property { get; set; }

	/// <summary>
	/// The query
	/// </summary>
	[DataMember(Name = "query")]
	public string Query { get; set; }

	/// <summary>
	/// The regular expression
	/// </summary>
	[DataMember(Name = "regex")]
	public string Regex { get; set; }

	/// <summary>
	/// The request
	/// </summary>
	[DataMember(Name = "request")]
	public string Request { get; set; }

	/// <summary>
	/// The separator
	/// </summary>
	[DataMember(Name = "separator")]
	public string Separator { get; set; }

	/// <summary>
	/// The timeout in ms
	/// </summary>
	[DataMember(Name = "timeout")]
	public string TimeoutMs { get; set; }

	/// <summary>
	/// The URI
	/// </summary>
	[DataMember(Name = "uri")]
	public string Uri { get; set; }

	/// <summary>
	/// wmiClass
	/// </summary>
	[DataMember(Name = "wmiClass")]
	public string WmiClass { get; set; }

	/// <summary>
	/// Whether SNMP instance level properties are enabled
	/// </summary>
	[DataMember(Name = "enableWmiClassILP")]
	public bool AreWmiClassInstanceLevelPropertiesEnabled { get; set; }

	/// <summary>
	/// The instance level properties
	/// </summary>
	[DataMember(Name = "ILP")]
	public List<InstanceLevelProperty> InstanceLevelProperties { get; set; }

	/// <summary>
	/// Whether to enabled linked class instance level properties
	/// </summary>
	[DataMember(Name = "enableLinkedClassILP")]
	public bool AreLinkedClassInstanceLevelPropertiesEnabled { get; set; }

	/// <summary>
	/// The linked classes
	/// </summary>
	[DataMember(Name = "linkedClasses")]
	public List<string> LinkedClasses { get; set; }

	/// <summary>
	/// The Linux command line
	/// </summary>
	[DataMember(Name = "linuxCmdLine")]
	public string LinuxCommandLine { get; set; }

	/// <summary>
	/// The Linux script
	/// </summary>
	[DataMember(Name = "linuxScript")]
	public string LinuxScript { get; set; }

	/// <summary>
	/// The ports
	/// </summary>
	[DataMember(Name = "ports")]
	public string Ports { get; set; }

	/// <summary>
	/// The URL
	/// </summary>
	[DataMember(Name = "url")]
	public string Url { get; set; }

	/// <summary>
	/// Whether to use SSL
	/// </summary>
	[DataMember(Name = "useSsl")]
	public bool UseSsl { get; set; }

	/// <summary>
	/// The Windows command line
	/// </summary>
	[DataMember(Name = "winCmdLine")]
	public string WindowsCommandLine { get; set; }

	/// <summary>
	/// The Windows script
	/// </summary>
	[DataMember(Name = "winScript")]
	public string WindowsScript { get; set; }

	/// <summary>
	/// The external resource id
	/// </summary>
	[DataMember(Name = "externalResourceID")]
	public string ExternalResourceId { get; set; }

	/// <summary>
	/// The external resource type
	/// </summary>
	[DataMember(Name = "externalResourceType")]
	public string ExternalResourceType { get; set; }

	/// <summary>
	/// Connect Timeout ms
	/// </summary>
	[DataMember(Name = "connectTimeout")]
	public int ConnectTimeoutMs { get; set; }

	/// <summary>
	/// Read Timeout ms
	/// </summary>
	[DataMember(Name = "readTimeout")]
	public int ReadTimeoutMs { get; set; }

	/// <summary>
	/// Group Label
	/// </summary>
	[DataMember(Name = "groupLabel")]
	public string GroupLabel { get; set; }

	/// <summary>
	/// Metric Name
	/// </summary>
	[DataMember(Name = "metricName")]
	public string MetricName { get; set; }

	/// <summary>
	/// Instance Label
	/// </summary>
	[DataMember(Name = "instanceLabel")]
	public string InstanceLabel { get; set; }

	/// <summary>
	/// Headers
	/// </summary>
	[DataMember(Name = "headers")]
	public string Headers { get; set; }

	/// <summary>
	/// Instance Property Tags
	/// </summary>
	[DataMember(Name = "instancePropertyTags")]
	public string InstancePropertyTags { get; set; }

	/// <summary>
	/// GCP Billing Period
	/// </summary>
	[DataMember(Name = "gcpBillingPeriodType")]
	public string GcpBillingPeriod { get; set; }

	/// <summary>
	/// Zoom plan usage type
	/// </summary>
	[DataMember(Name = "zoomPlanUsageType")]
	public ZoomPlanUsageType ZoomPlanUsageType { get; set; }
}
