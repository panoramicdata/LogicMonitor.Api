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
	[DataMember(Name = "name")]
	public string? Name { get; set; }

	/// <summary>
	/// The AWS billing reporting attribute
	/// </summary>
	[DataMember(Name = "awsBillingReportAttribute")]
	public string? AwsBillingReportAttribute { get; set; }

	/// <summary>
	/// The AWS service name
	/// </summary>
	[DataMember(Name = "awsServiceName")]
	public string? AwsServiceName { get; set; }

	/// <summary>
	/// Azure Billing Type
	/// </summary>
	[DataMember(Name = "azureBillingType")]
	public string? AzureBillingType { get; set; }

	/// <summary>
	/// azureCostManagementType
	/// </summary>
	[DataMember(Name = "azureCostManagementType")]
	public string? AzureCostManagementType { get; set; }

	/// <summary>
	/// azureSynapseType
	/// </summary>
	[DataMember(Name = "azureSynapseType")]
	public string? AzureSynapseType { get; set; }

	/// <summary>
	/// The CIM class
	/// </summary>
	[DataMember(Name = "cimClass")]
	public string? CimClass { get; set; }

	/// <summary>
	/// columnInstanceName
	/// </summary>
	[DataMember(Name = "columnInstanceName")]
	public string? ColumnInstanceName { get; set; }

	/// <summary>
	/// The property
	/// </summary>
	[DataMember(Name = "property")]
	public string? Property { get; set; }

	/// <summary>
	/// The namespace
	/// </summary>
	[DataMember(Name = "namespace")]
	public string? Namespace { get; set; }

	/// <summary>
	/// The cluster dimension
	/// </summary>
	[DataMember(Name = "clusterDimension")]
	public string? ClusterDimension { get; set; }

	/// <summary>
	/// The period
	/// </summary>
	[DataMember(Name = "period")]
	public string? Period { get; set; }

	/// <summary>
	/// Metric Name
	/// </summary>
	[DataMember(Name = "metricName")]
	public string? MetricName { get; set; }

	/// <summary>
	/// Node dimension
	/// </summary>
	[DataMember(Name = "nodeDimension")]
	public string? NodeDimension { get; set; }

	/// <summary>
	/// The cluster dimension value
	/// </summary>
	[DataMember(Name = "clusterDimensionValue")]
	public string? ClusterDimensionValue { get; set; }

	/// <summary>
	/// The entity
	/// </summary>
	[DataMember(Name = "entity")]
	public string? Entity { get; set; }

	/// <summary>
	/// GCP Billing Period
	/// </summary>
	[DataMember(Name = "gcpBillingPeriodType")]
	public string? GcpBillingPeriod { get; set; }

	/// <summary>
	/// GCP Billing Type
	/// </summary>
	[DataMember(Name = "gcpBillingType")]
	public string? GcpBillingType { get; set; }

	/// <summary>
	/// The regular expression
	/// </summary>
	[DataMember(Name = "regex")]
	public string? Regex { get; set; }

	/// <summary>
	/// Whether it is case sensitive
	/// </summary>
	[DataMember(Name = "caseSensitive")]
	public bool IsCaseSensitive { get; set; }

	/// <summary>
	/// followRedirect
	/// </summary>
	[DataMember(Name = "followRedirect")]
	public string? FollowRedirect { get; set; }

	/// <summary>
	/// The ports
	/// </summary>
	[DataMember(Name = "ports")]
	public string? Ports { get; set; }

	/// <summary>
	/// The URI
	/// </summary>
	[DataMember(Name = "uri")]
	public string? Uri { get; set; }

	/// <summary>
	/// Whether to use SSL
	/// </summary>
	[DataMember(Name = "useSSL")]
	public bool UseSsl { get; set; }

	/// <summary>
	/// The method
	/// </summary>
	[DataMember(Name = "method")]
	public string? Method { get; set; }

	/// <summary>
	/// The query
	/// </summary>
	[DataMember(Name = "query")]
	public string? Query { get; set; }

	/// <summary>
	/// The type
	/// </summary>
	[DataMember(Name = "type")]
	public string? Type { get; set; }

	/// <summary>
	/// The separator
	/// </summary>
	[DataMember(Name = "separator")]
	public string? Separator { get; set; }

	/// <summary>
	/// The url
	/// </summary>
	[DataMember(Name = "url")]
	public string? Url { get; set; }

	/// <summary>
	/// The sid
	/// </summary>
	[DataMember(Name = "sid")]
	public string? Sid { get; set; }

	/// <summary>
	/// The path
	/// </summary>
	[DataMember(Name = "path")]
	public string? Path { get; set; }

	/// <summary>
	/// The request
	/// </summary>
	[DataMember(Name = "request")]
	public string? Request { get; set; }

	/// <summary>
	/// The instance name
	/// </summary>
	[DataMember(Name = "instanceName")]
	public string? InstanceName { get; set; }

	/// <summary>
	/// The instance group name
	/// </summary>
	[DataMember(Name = "instanceGroupName")]
	public string? InstanceGroupName { get; set; }

	/// <summary>
	/// The instance value
	/// </summary>
	[DataMember(Name = "instanceValue")]
	public string? InstanceValue { get; set; }

	/// <summary>
	/// The instance description
	/// </summary>
	[DataMember(Name = "instanceDescription")]
	public string? InstanceDescription { get; set; }

	/// <summary>
	/// The instance locator
	/// </summary>
	[DataMember(Name = "instanceLocator")]
	public string? InstanceLocator { get; set; }

	/// <summary>
	/// Headers
	/// </summary>
	[DataMember(Name = "headers")]
	public string? Headers { get; set; }

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
	public string? GroupLabel { get; set; }

	/// <summary>
	/// Instance Label
	/// </summary>
	[DataMember(Name = "instanceLabel")]
	public string? InstanceLabel { get; set; }

	/// <summary>
	/// Instance Property Tags
	/// </summary>
	[DataMember(Name = "instancePropertyTags")]
	public string? InstancePropertyTags { get; set; }

	/// <summary>
	/// The object regex
	/// </summary>
	[DataMember(Name = "objRegex")]
	public string? ObjectRegex { get; set; }

	/// <summary>
	/// The category
	/// </summary>
	[DataMember(Name = "category")]
	public string? Category { get; set; }

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
	public string? LookupOid { get; set; }

	/// <summary>
	/// The external resource id
	/// </summary>
	[DataMember(Name = "externalResourceID")]
	public string? ExternalResourceId { get; set; }

	/// <summary>
	/// Description OID
	/// </summary>
	[DataMember(Name = "descriptionOID")]
	public string? DescriptionOid { get; set; }

	/// <summary>
	/// The external resource type
	/// </summary>
	[DataMember(Name = "externalResourceType")]
	public string? ExternalResourceType { get; set; }

	/// <summary>
	/// OID
	/// </summary>
	[DataMember(Name = "OID")]
	public string? Oid { get; set; }

	/// <summary>
	/// Object
	/// </summary>
	[DataMember(Name = "object")]
	public string? Object { get; set; }

	/// <summary>
	/// The instance level properties
	/// </summary>
	[DataMember(Name = "ILP")]
	public List<InstanceLevelProperty>? InstanceLevelProperties { get; set; }

	/// <summary>
	/// Discovery Type
	/// </summary>
	[DataMember(Name = "discoveryType")]
	public string? DiscoveryType { get; set; }

	/// <summary>
	/// Zoom plan usage type
	/// </summary>
	[DataMember(Name = "zoomPlanUsageType")]
	public ZoomPlanUsageType ZoomPlanUsageType { get; set; }

	/// <summary>
	/// The Windows script
	/// </summary>
	[DataMember(Name = "winScript")]
	public string? WindowsScript { get; set; }

	/// <summary>
	/// The groovyScript
	/// </summary>
	[DataMember(Name = "groovyScript")]
	public string? GroovyScript { get; set; }

	/// <summary>
	/// The Linux command line
	/// </summary>
	[DataMember(Name = "linuxCmdLine")]
	public string? LinuxCommandLine { get; set; }

	/// <summary>
	/// The Linux script
	/// </summary>
	[DataMember(Name = "linuxScript")]
	public string? LinuxScript { get; set; }

	/// <summary>
	/// The Windows command line
	/// </summary>
	[DataMember(Name = "winCmdLine")]
	public string? WindowsCommandLine { get; set; }

	/// <summary>
	/// The linked classes
	/// </summary>
	[DataMember(Name = "linkedClasses")]
	public List<LinkedWmiClass>? LinkedClasses { get; set; }

	/// <summary>
	/// wmiClass
	/// </summary>
	[DataMember(Name = "wmiClass")]
	public string? WmiClass { get; set; }

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
	public string? CollectorId { get; set; }

	/// <summary>
	/// zoomRoomIssueType
	/// </summary>
	[DataMember(Name = "zoomRoomIssueType")]
	public string? ZoomRoomIssueType { get; set; }
}
