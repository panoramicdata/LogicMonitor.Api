namespace LogicMonitor.Api.Usage;

/// <summary>
/// A summary of portal usage metrics
/// </summary>
[DataContract]
public class CompanyMetricsSummary
{
	/// <summary>
	/// The destroy account
	/// </summary>
	[DataMember(Name = "destroyAccount")]
	public bool DestroyAccount { get; set; }

	/// <summary>
	/// The destroy on local
	/// </summary>
	[DataMember(Name = "destroyOnLocal")]
	public string DestroyOnLocal { get; set; } = string.Empty;

	/// <summary>
	/// The host groups info
	/// </summary>
	[DataMember(Name = "hostGroupsInfo")]
	public JObject? HostGroupsInfo { get; set; }

	/// <summary>
	/// The number of alert rules
	/// </summary>
	[DataMember(Name = "numberOfAlertRules")]
	public int NumberOfAlertRules { get; set; }

	/// <summary>
	/// The number of allowed dtconfigs
	/// </summary>
	[DataMember(Name = "numberOfAllowedDTConfigs")]
	public long NumberOfAllowedDTConfigs { get; set; }

	/// <summary>
	/// The number of api users
	/// </summary>
	[DataMember(Name = "numberOfApiUsers")]
	public int NumberOfApiUsers { get; set; }

	/// <summary>
	/// The number of complex data points
	/// </summary>
	[DataMember(Name = "numberOfComplexDataPoints")]
	public long NumberOfComplexDataPoints { get; set; }

	/// <summary>
	/// The number of dashboards
	/// </summary>
	[DataMember(Name = "numberOfDashboards")]
	public int NumberOfDashboards { get; set; }

	/// <summary>
	/// The number of data points
	/// </summary>
	[DataMember(Name = "numberOfDataPoints")]
	public long NumberOfDataPoints { get; set; }

	/// <summary>
	/// The number of datasource instances
	/// </summary>
	[DataMember(Name = "numberOfDatasourceInstances")]
	public long NumberOfDatasourceInstances { get; set; }

	/// <summary>
	/// Sum of numOfStandardDevices, numOfCombinedAWSDevices, numOfCombinedAzureDevices, and numOfCombinedGCPDevices
	/// </summary>
	[SantabaReadOnly]
	[DataMember(Name = "numberOfDevices")]
	public int NumberOfDevices { get; set; }

	/// <summary>
	/// The number of dynamic thresholds
	/// </summary>
	[DataMember(Name = "numberOfDynamicThresholds")]
	public long NumberOfDynamicThresholds { get; set; }

	/// <summary>
	/// The number of instances per ds
	/// </summary>
	[DataMember(Name = "numberOfInstancesPerDS")]
	public JObject? NumberOfInstancesPerDS { get; set; }

	/// <summary>
	/// Number of monitored Kubernetes Nodes, Pods, and Services
	/// </summary>
	[SantabaReadOnly]
	[DataMember(Name = "numberOfKubernetesDevices")]
	public int NumberOfKubernetesDevices { get; set; }

	/// <summary>
	/// The number of light devices
	/// </summary>
	[DataMember(Name = "numberOfLightDevices")]
	public int NumberOfLightDevices { get; set; }

	/// <summary>
	/// The number of open alerts
	/// </summary>
	[DataMember(Name = "numberOfOpenAlerts")]
	public int NumberOfOpenAlerts { get; set; }

	/// <summary>
	/// The number of previous day alerts
	/// </summary>
	[DataMember(Name = "numberOfPreviousDayAlerts")]
	public int NumberOfPreviousDayAlerts { get; set; }

	/// <summary>
	/// The number of reports in last24 hrs
	/// </summary>
	[DataMember(Name = "numberOfReportsInLast24Hrs")]
	public int NumberOfReportsInLast24Hrs { get; set; }

	/// <summary>
	/// The number of root cause analysis rules
	/// </summary>
	[DataMember(Name = "numberOfRootCauseAnalysisRules")]
	public int NumberOfRootCauseAnalysisRules { get; set; }

	/// <summary>
	/// The number of session users
	/// </summary>
	[DataMember(Name = "numberOfSessionUsers")]
	public int NumberOfSessionUsers { get; set; }

	/// <summary>
	/// Number of standard devices
	/// </summary>
	[SantabaReadOnly]
	[DataMember(Name = "numberOfStandardDevices")]
	public int NumberOfStandardDevices { get; set; }

	/// <summary>
	/// The number of user roles
	/// </summary>
	[DataMember(Name = "numberOfUserRoles")]
	public int NumberOfUserRoles { get; set; }

	/// <summary>
	/// The number of widgets
	/// </summary>
	[DataMember(Name = "numberOfWidgets")]
	public int NumberOfWidgets { get; set; }

	/// <summary>
	/// Number of AWS resources not monitored with a local Collector
	/// </summary>
	[SantabaReadOnly]
	[DataMember(Name = "numOfAWSDevices")]
	public int NumOfAWSDevices { get; set; }

	/// <summary>
	/// Number of Azure resources not monitored with a local Collector
	/// </summary>
	[SantabaReadOnly]
	[DataMember(Name = "numOfAzureDevices")]
	public int NumOfAzureDevices { get; set; }

	/// <summary>
	/// Number of AWS resources monitored with a local Collector
	/// </summary>
	[SantabaReadOnly]
	[DataMember(Name = "numOfCombinedAWSDevices")]
	public int NumOfCombinedAWSDevices { get; set; }

	/// <summary>
	/// Number of Azure resources monitored with a local Collector
	/// </summary>
	[SantabaReadOnly]
	[DataMember(Name = "numOfCombinedAzureDevices")]
	public int NumOfCombinedAzureDevices { get; set; }

	/// <summary>
	/// Number of GCP resources monitored with a local Collector
	/// </summary>
	[SantabaReadOnly]
	[DataMember(Name = "numOfCombinedGcpDevices")]
	public int NumOfCombinedGcpDevices { get; set; }

	/// <summary>
	/// Number of devices with active ConfigSources
	/// </summary>
	[SantabaReadOnly]
	[DataMember(Name = "numOfConfigSourceDevices")]
	public int NumOfConfigSourceDevices { get; set; }

	/// <summary>
	/// Number of GCP resources
	/// </summary>
	[SantabaReadOnly]
	[DataMember(Name = "numOfGcpDevices")]
	public int NumOfGcpDevices { get; set; }

	/// <summary>
	/// The num of saved maps
	/// </summary>
	[DataMember(Name = "numOfSavedMaps")]
	public int NumOfSavedMaps { get; set; }

	/// <summary>
	/// Number of services (created via LM Service Insight)
	/// </summary>
	[SantabaReadOnly]
	[DataMember(Name = "numOfServices")]
	public int NumOfServices { get; set; }

	/// <summary>
	/// Number of stopped AWS resources
	/// </summary>
	[SantabaReadOnly]
	[DataMember(Name = "numOfStoppedAWSDevices")]
	public int NumOfStoppedAWSDevices { get; set; }

	/// <summary>
	/// Number of stopped Azure resources
	/// </summary>
	[SantabaReadOnly]
	[DataMember(Name = "numOfStoppedAzureDevices")]
	public int NumOfStoppedAzureDevices { get; set; }

	/// <summary>
	/// Number of stopped GCP resources not monitored with a local Collector
	/// </summary>
	[SantabaReadOnly]
	[DataMember(Name = "numOfStoppedGcpDevices")]
	public int NumOfStoppedGcpDevices { get; set; }

	/// <summary>
	/// Number of terminated AWS resources
	/// </summary>
	[SantabaReadOnly]
	[DataMember(Name = "numOfTerminatedAWSDevices")]
	public int NumOfTerminatedAWSDevices { get; set; }

	/// <summary>
	/// Number of terminated Azure resources
	/// </summary>
	[SantabaReadOnly]
	[DataMember(Name = "numOfTerminatedAzureDevices")]
	public int NumOfTerminatedAzureDevices { get; set; }

	/// <summary>
	/// Number of terminated GCP resources
	/// </summary>
	[SantabaReadOnly]
	[DataMember(Name = "numOfTerminatedGcpCloudDevices")]
	public int NumOfTerminatedGcpCloudDevices { get; set; }

	/// <summary>
	/// Number of websites
	/// </summary>
	[SantabaReadOnly]
	[DataMember(Name = "numOfWebsites")]
	public int NumOfWebsites { get; set; }

	/// <summary>
	/// The parent billing
	/// </summary>
	[DataMember(Name = "parentBilling")]
	public string ParentBilling { get; set; } = string.Empty;

	/// <summary>
	/// The timestamp window of api users in sec
	/// </summary>
	[DataMember(Name = "timestampWindowOfApiUsersInSec")]
	public int TimestampWindowOfApiUsersInSec { get; set; }

	/// <summary>
	/// The zuora
	/// </summary>
	[DataMember(Name = "zuora")]
	public JObject? Zuora { get; set; }

	/// <summary>
	/// The zuora invoice details
	/// </summary>
	[DataMember(Name = "zuoraInvoiceDetails")]
	public JObject? ZuoraInvoiceDetails { get; set; }
}
