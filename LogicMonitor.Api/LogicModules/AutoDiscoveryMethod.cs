namespace LogicMonitor.Api.LogicModules;

/// <summary>
/// An autodiscovery method
/// </summary>
[DataContract]
public class AutoDiscoveryMethod
{
	/// <summary>
	/// The auto discovery method name values can be : ad_cim|ad_cloudwatch|ad_collector|ad_dummy|ad_ec2|ad_esx|ad_http|ad_ipmi|ad_jdbc|ad_jmx|ad_netapp|ad_pdh|ad_port|ad_script|ad_snmp|ad_wmi|ad_xen|ad_azurerediscache|ad_awsserviceregion|ad_awsec2reservedinstance|ad_awsec2reservedinstancecoverage|ad_awsecsservice|ad_awsec2scheduledevents|ad_azureserviceregion|ad_azuresubscription|ad_azurebackupjob|ad_azuresdk|ad_azurewebjob|ad_awsbillingreport|ad_awselasticache|ad_awsredshift|ad_azurebilling|ad_awslbtargetgroups|ad_gcpappengine|ad_gcpbilling|ad_awsvpntunnel|ad_gcpvpntunnel|ad_awsglobalwebacl|ad_gcplbbackendservice|ad_gcppubsubsubscription|ad_gcppubsubsnapshot|ad_azurereplicationjob|ad_azureexpressroutecircuitpeering|ad_awsapigatewaystage|ad_azureautomationaccountcertificate|ad_azurevngconnection|ad_azurewebappinstance|ad_azureappserviceenvironmentmultirolepool|ad_openmetrics|ad_awsmediaconnectoutput|ad_awsmediaconnectsource|ad_awswebaclwafv2|ad_saaso365sharepointsite|ad_awscognitoidentityproviders|ad_azureeabilling|ad_saaszoomplanusage|ad_saasstatus|ad_azuresynapse|ad_saasairbrake|ad_syntheticsselenium|ad_azurevirtualdesktopsessionhosts|ad_saaso365subscribedsku|ad_azuredimension|ad_azurecostmanagementdimensions|ad_saaso365servicehealth|ad_saaso365mailbox|ad_azurenetappvolumes|ad_azureloganalyticsworkspaces|ad_saaszoomstatus|ad_saassalesforcelicense|ad_saaszoomroom|ad_saaswebexlicenseusage|ad_azureloganalyticsreplicationjob|ad_paasjsonpath
	/// </summary>
	[DataMember(Name = "name", IsRequired = false)]
	public string? Name { get; set; }

	/// <summary>
	/// The AWS billing reporting attribute
	/// </summary>
	[DataMember(Name = "awsBillingReportAttribute", IsRequired = false)]
	public string? AwsBillingReportAttribute { get; set; }

	/// <summary>
	/// The AWS service name
	/// </summary>
	[DataMember(Name = "awsServiceName", IsRequired = false)]
	public string? AwsServiceName { get; set; }

	/// <summary>
	/// Azure Billing Type
	/// </summary>
	[DataMember(Name = "azureBillingType", IsRequired = false)]
	public string? AzureBillingType { get; set; }

	/// <summary>
	/// The CIM class
	/// </summary>
	[DataMember(Name = "cimClass", IsRequired = false)]
	public string? CimClass { get; set; }

	/// <summary>
	/// The property
	/// </summary>
	[DataMember(Name = "property", IsRequired = false)]
	public string? Property { get; set; }

	/// <summary>
	/// The namespace
	/// </summary>
	[DataMember(Name = "namespace", IsRequired = false)]
	public string? Namespace { get; set; }

	/// <summary>
	/// The cluster dimension
	/// </summary>
	[DataMember(Name = "clusterDimension", IsRequired = false)]
	public string? ClusterDimension { get; set; }

	/// <summary>
	/// The period
	/// </summary>
	[DataMember(Name = "period", IsRequired = false)]
	public string? Period { get; set; }

	/// <summary>
	/// Metric Name
	/// </summary>
	[DataMember(Name = "metricName", IsRequired = false)]
	public string? MetricName { get; set; }

	/// <summary>
	/// Node dimension
	/// </summary>
	[DataMember(Name = "nodeDimension", IsRequired = false)]
	public string? NodeDimension { get; set; }

	/// <summary>
	/// The cluster dimension value
	/// </summary>
	[DataMember(Name = "clusterDimensionValue", IsRequired = false)]
	public string? ClusterDimensionValue { get; set; }

	/// <summary>
	/// The entity
	/// </summary>
	[DataMember(Name = "entity", IsRequired = false)]
	public string? Entity { get; set; }

	/// <summary>
	/// GCP Billing Period
	/// </summary>
	[DataMember(Name = "gcpBillingPeriodType", IsRequired = false)]
	public string? GcpBillingPeriod { get; set; }

	/// <summary>
	/// GCP Billing Type
	/// </summary>
	[DataMember(Name = "gcpBillingType", IsRequired = false)]
	public string? GcpBillingType { get; set; }

	/// <summary>
	/// The regular expression
	/// </summary>
	[DataMember(Name = "regex", IsRequired = false)]
	public string? Regex { get; set; }

	/// <summary>
	/// Whether it is case sensitive
	/// </summary>
	[DataMember(Name = "caseSensitive", IsRequired = false)]
	public bool IsCaseSensitive { get; set; }

	/// <summary>
	/// followRedirect
	/// </summary>
	[DataMember(Name = "followRedirect", IsRequired = false)]
	public string? FollowRedirect { get; set; }

	/// <summary>
	/// The ports
	/// </summary>
	[DataMember(Name = "ports", IsRequired = false)]
	public string? Ports { get; set; }

	/// <summary>
	/// The URI
	/// </summary>
	[DataMember(Name = "uri", IsRequired = false)]
	public string? Uri { get; set; }

	/// <summary>
	/// Whether to use SSL
	/// </summary>
	[DataMember(Name = "useSSL", IsRequired = false)]
	public bool UseSsl { get; set; }

	/// <summary>
	/// The method
	/// </summary>
	[DataMember(Name = "method", IsRequired = false)]
	public string? Method { get; set; }

	/// <summary>
	/// The query
	/// </summary>
	[DataMember(Name = "query", IsRequired = false)]
	public string? Query { get; set; }

	/// <summary>
	/// The type
	/// </summary>
	[DataMember(Name = "type", IsRequired = false)]
	public string? Type { get; set; }

	/// <summary>
	/// The separator
	/// </summary>
	[DataMember(Name = "separator", IsRequired = false)]
	public string? Separator { get; set; }

	/// <summary>
	/// The url
	/// </summary>
	[DataMember(Name = "url", IsRequired = false)]
	public string? Url { get; set; }

	/// <summary>
	/// The sid
	/// </summary>
	[DataMember(Name = "sid", IsRequired = false)]
	public string? Sid { get; set; }

	/// <summary>
	/// The path
	/// </summary>
	[DataMember(Name = "path", IsRequired = false)]
	public string? Path { get; set; }

	/// <summary>
	/// The request
	/// </summary>
	[DataMember(Name = "request", IsRequired = false)]
	public string? Request { get; set; }

	/// <summary>
	/// The instance name
	/// </summary>
	[DataMember(Name = "instanceName", IsRequired = false)]
	public string? InstanceName { get; set; }

	/// <summary>
	/// The instance group name
	/// </summary>
	[DataMember(Name = "instanceGroupName", IsRequired = false)]
	public string? InstanceGroupName { get; set; }

	/// <summary>
	/// The instance value
	/// </summary>
	[DataMember(Name = "instanceValue", IsRequired = false)]
	public string? InstanceValue { get; set; }

	/// <summary>
	/// The instance description
	/// </summary>
	[DataMember(Name = "instanceDescription", IsRequired = false)]
	public string? InstanceDescription { get; set; }

	/// <summary>
	/// The instance locator
	/// </summary>
	[DataMember(Name = "instanceLocator", IsRequired = false)]
	public string? InstanceLocator { get; set; }

	/// <summary>
	/// Headers
	/// </summary>
	[DataMember(Name = "headers", IsRequired = false)]
	public string? Headers { get; set; }

	/// <summary>
	/// Read Timeout ms
	/// </summary>
	[DataMember(Name = "readTimeout", IsRequired = false)]
	public int ReadTimeoutMs { get; set; }

	/// <summary>
	/// Connect Timeout ms
	/// </summary>
	[DataMember(Name = "connectTimeout", IsRequired = false)]
	public int ConnectTimeoutMs { get; set; }

	/// <summary>
	/// Group Label
	/// </summary>
	[DataMember(Name = "groupLabel", IsRequired = false)]
	public string? GroupLabel { get; set; }

	/// <summary>
	/// Instance Label
	/// </summary>
	[DataMember(Name = "instanceLabel", IsRequired = false)]
	public string? InstanceLabel { get; set; }

	/// <summary>
	/// Instance Property Tags
	/// </summary>
	[DataMember(Name = "instancePropertyTags", IsRequired = false)]
	public string? InstancePropertyTags { get; set; }

	/// <summary>
	/// The object regex
	/// </summary>
	[DataMember(Name = "objRegex", IsRequired = false)]
	public string? ObjectRegex { get; set; }

	/// <summary>
	/// The category
	/// </summary>
	[DataMember(Name = "category", IsRequired = false)]
	public string? Category { get; set; }

	/// <summary>
	/// The timeout in ms
	/// </summary>
	[DataMember(Name = "timeout", IsRequired = false)]
	public int Timeout { get; set; }

	/// <summary>
	/// Whether SNMP instance level properties are enabled
	/// </summary>
	[DataMember(Name = "enableSNMPILP", IsRequired = false)]
	public bool EnableSNMPILP { get; set; }

	/// <summary>
	/// Lookup OID
	/// </summary>
	[DataMember(Name = "lookupOID", IsRequired = false)]
	public string? LookupOid { get; set; }

	/// <summary>
	/// The external resource id
	/// </summary>
	[DataMember(Name = "externalResourceID", IsRequired = false)]
	public string? ExternalResourceId { get; set; }

	/// <summary>
	/// Description OID
	/// </summary>
	[DataMember(Name = "descriptionOID", IsRequired = false)]
	public string? DescriptionOid { get; set; }

	/// <summary>
	/// The external resource type
	/// </summary>
	[DataMember(Name = "externalResourceType", IsRequired = false)]
	public string? ExternalResourceType { get; set; }

	/// <summary>
	/// OID
	/// </summary>
	[DataMember(Name = "OID", IsRequired = false)]
	public string? Oid { get; set; }

	/// <summary>
	/// Object
	/// </summary>
	[DataMember(Name = "object", IsRequired = false)]
	public string? Object { get; set; }

	/// <summary>
	/// The instance level properties
	/// </summary>
	[DataMember(Name = "ILP", IsRequired = false)]
	public List<InstanceLevelProperty>? InstanceLevelProperties { get; set; }

	/// <summary>
	/// Discovery Type
	/// </summary>
	[DataMember(Name = "discoveryType", IsRequired = false)]
	public string? DiscoveryType { get; set; }

	/// <summary>
	/// Zoom plan usage type
	/// </summary>
	[DataMember(Name = "zoomPlanUsageType", IsRequired = false)]
	public ZoomPlanUsageType ZoomPlanUsageType { get; set; }

	/// <summary>
	/// The Windows script
	/// </summary>
	[DataMember(Name = "winScript", IsRequired = false)]
	public string? WindowsScript { get; set; }

	/// <summary>
	/// The groovyScript
	/// </summary>
	[DataMember(Name = "groovyScript", IsRequired = false)]
	public string? GroovyScript { get; set; }

	/// <summary>
	/// The Linux command line
	/// </summary>
	[DataMember(Name = "linuxCmdLine", IsRequired = false)]
	public string? LinuxCommandLine { get; set; }

	/// <summary>
	/// The Linux script
	/// </summary>
	[DataMember(Name = "linuxScript", IsRequired = false)]
	public string? LinuxScript { get; set; }

	/// <summary>
	/// The Windows command line
	/// </summary>
	[DataMember(Name = "winCmdLine", IsRequired = false)]
	public string? WindowsCommandLine { get; set; }

	/// <summary>
	/// The linked classes
	/// </summary>
	[DataMember(Name = "linkedClasses", IsRequired = false)]
	public List<LinkedWmiClass>? LinkedClasses { get; set; }

	/// <summary>
	/// wmiClass
	/// </summary>
	[DataMember(Name = "wmiClass", IsRequired = false)]
	public string? WmiClass { get; set; }

	/// <summary>
	/// Whether to enabled linked class instance level properties
	/// </summary>
	[DataMember(Name = "enableLinkedClassILP", IsRequired = false)]
	public bool AreLinkedClassInstanceLevelPropertiesEnabled { get; set; }

	/// <summary>
	/// Whether SNMP instance level properties are enabled
	/// </summary>
	[DataMember(Name = "enableWmiClassILP", IsRequired = false)]
	public bool AreWmiClassInstanceLevelPropertiesEnabled { get; set; }

	/// <summary>
	/// Collector id
	/// </summary>
	[DataMember(Name = "collectorId", IsRequired = false)]
	public string? CollectorId { get; set; }
}
