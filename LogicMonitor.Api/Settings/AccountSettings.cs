namespace LogicMonitor.Api.Settings;

/// <summary>
///     Account settings
/// </summary>
[DataContract]
public class AccountSettings : IHasSingletonEndpoint
{
	/// <summary>
	///     Whether keep me signed in is enabled
	/// </summary>
	[DataMember(Name = "enableKeepMeSignedIn")]
	public bool KeepMeSignedInIsEnabled { get; set; }

	/// <summary>
	///     The number of days to keep the user signed in
	/// </summary>
	[DataMember(Name = "keepMeSignedInConfigurableDays")]
	public bool KeepMeSignedInConfigurableDays { get; set; }

	/// <summary>
	///     The number of committed containers
	/// </summary>
	[DataMember(Name = "numberOfCommittedContainer")]
	public int CommittedContainerCount { get; set; }

	/// <summary>
	///     The number of active alerts
	/// </summary>
	[DataMember(Name = "numberOfOpenAlerts")]
	public int ActiveAlertCount { get; set; }

	/// <summary>
	///     The number of PaaS resources
	/// </summary>
	[DataMember(Name = "numOfPaaSResources")]
	public int PaasResourceCount { get; set; }

	/// <summary>
	///     The number of IaaS resources
	/// </summary>
	[DataMember(Name = "numOfIaaSResources")]
	public int IaasResourceCount { get; set; }

	/// <summary>
	///     The number of IaaS resources
	/// </summary>
	[DataMember(Name = "numOfNonComputeResources")]
	public int NonComputeResourceCount { get; set; }

	/// <summary>
	///     The number of serverless resources
	/// </summary>
	[DataMember(Name = "numOfServerlessResources")]
	public int ServerlessResourceCount { get; set; }

	/// <summary>
	///     The number of active users
	/// </summary>
	[DataMember(Name = "numberOfSessionUsers")]
	public int ActiveUserCount { get; set; }

	/// <summary>
	///     The Alert Rule count
	/// </summary>
	[DataMember(Name = "numberOfAlertRules")]
	public int AlertRuleCount { get; set; }

	/// <summary>
	///     Whether the alertTotal including any in Ack
	/// </summary>
	[DataMember(Name = "alertTotalIncludeInAck")]
	public bool AlertTotalIncludingAnyInAck { get; set; }

	/// <summary>
	///     Alert total count, including those in SDT
	/// </summary>
	[DataMember(Name = "alertTotalIncludeInSdt")]
	public bool AlertTotalIncludingAnyInSdt { get; set; }

	/// <summary>
	/// Allow shared reports
	/// </summary>
	[DataMember(Name = "allowSharedReports")]
	public bool AllowSharedReports { get; set; }

	/// <summary>
	///     The API user count
	/// </summary>
	[DataMember(Name = "numberOfApiUsers")]
	public int ApiUserCount { get; set; }

	/// <summary>
	///     The API user timestamp window in seconds
	/// </summary>
	[DataMember(Name = "timestampWindowOfApiUsersInSec")]
	public int ApiUserTimestampWindowSeconds { get; set; }

	/// <summary>
	///     The number of alerts from yesterday
	/// </summary>
	[DataMember(Name = "numberOfPreviousDayAlerts")]
	public int PreviousDayAlertCount { get; set; }

	/// <summary>
	///     The number of committed devices
	/// </summary>
	[DataMember(Name = "numberOfCommittedDevices")]
	public int CommittedResourceCount { get; set; }

	/// <summary>
	///     The number of committed config devices
	/// </summary>
	[DataMember(Name = "numberOfCommittedConfigDevices")]
	public int CommittedConfigResourceCount { get; set; }

	/// <summary>
	///     The number of committed services
	/// </summary>
	[DataMember(Name = "numberOfCommittedServices")]
	public int CommittedServiceCount { get; set; }

	/// <summary>
	///     The number of committed websites
	/// </summary>
	[DataMember(Name = "numberOfCommittedWebsites")]
	public int CommittedWebsiteCount { get; set; }

	/// <summary>
	///     The current AWS devResourceice count
	/// </summary>
	[DataMember(Name = "numOfAWSDevices")]
	public int AwsResourceCount { get; set; }

	/// <summary>
	///     The current Azure deResourcevice count
	/// </summary>
	[DataMember(Name = "numOfAzureDevices")]
	public int AzureResourceCount { get; set; }

	/// <summary>
	///     Combined AWS Resource count
	/// </summary>
	[DataMember(Name = "numOfCombinedAWSDevices")]
	public int CombinedAwsResourceCount { get; set; }

	/// <summary>
	///     Combined Azure Resource count
	/// </summary>
	[DataMember(Name = "numOfCombinedAzureDevices")]
	public int CombinedAzureResourceCount { get; set; }

	/// <summary>
	///     Combined GCP Resource count
	/// </summary>
	[DataMember(Name = "numOfCombinedGcpDevices")]
	public int CombinedGcpResourceCount { get; set; }

	/// <summary>
	///     Company display name
	/// </summary>
	[DataMember(Name = "companyDisplayName")]
	public string CompanyDisplayName { get; set; } = string.Empty;

	/// <summary>
	///     The current ConfigSource Resource count
	/// </summary>
	[DataMember(Name = "numOfConfigSourceDevices")]
	public int ConfigSourceResourceCount { get; set; }

	/// <summary>
	///     The data point count
	/// </summary>
	[DataMember(Name = "numberOfComplexDataPoints")]
	public int ComplexDataPointCount { get; set; }

	/// <summary>
	///     Contacts
	/// </summary>
	[DataMember(Name = "contacts")]
	public List<Contact> Contacts { get; set; } = [];

	/// <summary>
	///     The data point count
	/// </summary>
	[DataMember(Name = "numberOfDataPoints")]
	public int DataPointCount { get; set; }

	/// <summary>
	///     The dashboard count
	/// </summary>
	[DataMember(Name = "numberOfDashboards")]
	public int DashboardCount { get; set; }

	/// <summary>
	///     The DataSource Instance count
	/// </summary>
	[DataMember(Name = "numberOfDatasourceInstances")]
	public int DataSourceInstanceCount { get; set; }

	/// <summary>
	///     Whether to destroy the account
	/// </summary>
	[DataMember(Name = "destroyAccount")]
	public bool DestroyAccount { get; set; }

	/// <summary>
	///     When to destroy the account
	/// </summary>
	[DataMember(Name = "destroyOnLocal")]
	public string DestroyOnLocal { get; set; } = string.Empty;

	/// <summary>
	///     The current device count
	/// </summary>
	[DataMember(Name = "numberOfDevices")]
	public int ResourceCount { get; set; }

	/// <summary>
	///     The Service count that triggers a warning
	/// </summary>
	[DataMember(Name = "numOfServicesWarningLimit")]
	public int ServiceWarningLimit { get; set; }

	/// <summary>
	///     The Service count that should not be exceeded
	/// </summary>
	[DataMember(Name = "numOfServicesErrorLimit")]
	public int ServiceErrorLimit { get; set; }

	/// <summary>
	///     The AlertRule count that triggers a warning
	/// </summary>
	[DataMember(Name = "numOfAlertRulesWarningLimit")]
	public int AlertRuleWarningLimit { get; set; }

	/// <summary>
	///     The AlertRule count that should not be exceeded
	/// </summary>
	[DataMember(Name = "numOfAlertRulesErrorLimit")]
	public int AlertRuleErrorLimit { get; set; }

	/// <summary>
	///     The ResourceGroup count that should not be exceeded
	/// </summary>
	[DataMember(Name = "hostGroupWarningLimit")]
	public int ResourceGroupWarningLimit { get; set; }

	/// <summary>
	///     The ResourceGroup count that must should not be exceeded
	/// </summary>
	[DataMember(Name = "hostGroupErrorLimit")]
	public int ResourceGroupErrorLimit { get; set; }

	/// <summary>
	///     The ResourceGroups info
	/// </summary>
	[DataMember(Name = "hostGroupsInfo")]
	public ResourceGroupsInfo ResourceGroupsInfo { get; set; } = new();

	/// <summary>
	///     The dynamic threshold count
	/// </summary>
	[DataMember(Name = "numberOfDynamicThresholds")]
	public int DynamicThresholdCount { get; set; }

	/// <summary>
	///     The dynamic threshold limit
	/// </summary>
	[DataMember(Name = "numberOfAllowedDTConfigs")]
	public int DynamicThresholdLimit { get; set; }

	/// <summary>
	///     Whether remote sessions are enabled
	/// </summary>
	[DataMember(Name = "enableRemoteSession")]
	public bool EnableRemoteSession { get; set; }

	/// <summary>GCP Resource count
	/// </summary>
	[DataMember(Name = "numOfGcpDevices")]
	public int GcpResourceCount { get; set; }

	/// <summary>
	/// numOfMongoDBAtlas Resources
	/// </summary>
	[DataMember(Name = "numOfMongoDBAtlasDevices")]
	public int MongoDbAtlasResourceCount { get; set; }

	/// <summary>
	/// numberOfCommittedCloud Resource
	/// </summary>
	[DataMember(Name = "numberOfCommittedCloudDevices")]
	public int CommittedCloudResourceCount { get; set; }

	/// <summary>
	/// The account balance in USD
	/// </summary>
	[DataMember(Name = "zuoraInvoiceDetails")]
	public InvoiceDetails InvoiceDetails { get; set; } = new();

	/// <summary>
	/// Kubernetes Resource stats
	/// </summary>
	[DataMember(Name = "kubernetesDevices")]
	public KubernetesResourceStats KubernetesResourceStats { get; set; } = new();

	/// <summary>
	///     The Kubernetes Resource count
	/// </summary>
	[DataMember(Name = "numberOfKubernetesDevices")]
	public int KubernetesResourceCount { get; set; }

	/// <summary>
	///     Light Resource count
	/// </summary>
	[DataMember(Name = "numberOfLightDevices")]
	public int LightResourceCount { get; set; }

	/// <summary>
	///     monthlyPerResourceMetrics
	/// </summary>
	[DataMember(Name = "monthlyPerDeviceMetrics")]
	public long MonthlyPerResourceMetrics { get; set; }                       // Yes a LONG as some customers have e.g. 387,471,825,283 which is TOO BIG FOR AN INT!

	/// <summary>
	///     monthlyPerResourceMetricsQuotaErrorLimit
	/// </summary>
	[DataMember(Name = "monthlyPerDeviceMetricsQuotaErrorLimit")]
	public long MonthlyPerResourceMetricsQuotaErrorLimit { get; set; }        // Yes a LONG as some customers have e.g. 387,471,825,283 which is TOO BIG FOR AN INT!

	/// <summary>
	///     monthlyPerResourceMetricsQuotaWarningLimit
	/// </summary>
	[DataMember(Name = "monthlyPerDeviceMetricsQuotaWarningLimit")]
	public long MonthlyPerResourceMetricsQuotaWarningLimit { get; set; }      // Yes a LONG as some customers have e.g. 387,471,825,283 which is TOO BIG FOR AN INT!

	/// <summary>
	///     The PaaS Resource count
	/// </summary>
	[DataMember(Name = "numOfPaasDevices")]
	public int PaasDeviceCount { get; set; }

	/// <summary>
	///     The parent billing account
	/// </summary>
	[DataMember(Name = "parentBilling")]
	public string ParentBillingAccount { get; set; } = string.Empty;

	/// <summary>
	/// The account number
	/// </summary>
	[DataMember(Name = "zuora")]
	public PaymentInformation PaymentInformation { get; set; } = new();

	/// <summary>
	///     The per-DataSource instance count
	/// </summary>
	[DataMember(Name = "numberOfInstancesPerDS")]
	public Dictionary<string, int> PerDataSourceInstanceCount { get; set; } = [];

	/// <summary>
	///     Primary contact e-mail address
	/// </summary>
	[DataMember(Name = "email")]
	public string PrimaryContactEmailAddress { get; set; } = string.Empty;

	/// <summary>
	///     Primary contact name
	/// </summary>
	[DataMember(Name = "name")]
	public string PrimaryContactName { get; set; } = string.Empty;

	/// <summary>
	///     Primary contact phone number
	/// </summary>
	[DataMember(Name = "phone")]
	public string PrimaryContactPhoneNumber { get; set; } = string.Empty;

	/// <summary>
	///     The Root Cause Analysis rule count
	/// </summary>
	[DataMember(Name = "numberOfRootCauseAnalysisRules")]
	public bool RootCauseAnalysisRuleCount { get; set; }

	/// <summary>
	///     The number of reports executed in the last 24 hours
	/// </summary>
	[DataMember(Name = "numberOfReportsInLast24Hrs")]
	public bool ReportsInLast24HoursCount { get; set; }

	/// <summary>
	///     Whether to require two-factor authentication
	/// </summary>
	[DataMember(Name = "requireTwoFA")]
	public bool RequireTwoFactorAuthentication { get; set; }

	/// <summary>
	///     Whether to require two-factor authentication for remote sessions
	/// </summary>
	[DataMember(Name = "requireTwoFAForRemoteSession")]
	public bool RequireTwoFactorAuthenticationForRemoteSessions { get; set; }

	/// <summary>
	///     The SaaS Resource count
	/// </summary>
	[DataMember(Name = "numOfSaasDevices")]
	public int SaasResourceCount { get; set; }

	/// <summary>
	///     The SaaS lite Resource count
	/// </summary>
	[DataMember(Name = "numOfSaasLiteDevices")]
	public int SaasLiteResourceCount { get; set; }

	/// <summary>
	///     The saved map count
	/// </summary>
	[DataMember(Name = "numOfSavedMaps")]
	public int SavedMapCount { get; set; }

	/// <summary>
	///     The service count
	/// </summary>
	[DataMember(Name = "numOfServices")]
	public int ServiceCount { get; set; }

	/// <summary>
	///     The session timeout in seconds
	/// </summary>
	[DataMember(Name = "sessionTimeoutInSeconds")]
	public int SessionTimeoutSeconds { get; set; }

	/// <summary>
	///     Standard Resource count
	/// </summary>
	[DataMember(Name = "numberOfStandardDevices")]
	public int StandardResourceCount { get; set; }

	/// <summary>
	///     Stopped AWS Resource count
	/// </summary>
	[DataMember(Name = "numOfStoppedAWSDevices")]
	public int StoppedAwsResourceCount { get; set; }

	/// <summary>
	///     Stopped Azure Resource count
	/// </summary>
	[DataMember(Name = "numOfStoppedAzureDevices")]
	public int StoppedAzureResourceCount { get; set; }

	/// <summary>
	///     Stopped GCP Resource count
	/// </summary>
	[DataMember(Name = "numOfStoppedGcpDevices")]
	public int StoppedGcpResourceCount { get; set; }

	/// <summary>
	///     The stopped PaaS Resource count
	/// </summary>
	[DataMember(Name = "numOfStoppedPaasDevices")]
	public int StoppedPaasResourceCount { get; set; }

	/// <summary>
	///     The tenant identifier property name
	/// </summary>
	[DataMember(Name = "tenantIdentifierPropertyName")]
	public string TenantIdentifierPropertyName { get; set; } = string.Empty;

	/// <summary>
	///     Terminated AWS Resource count
	/// </summary>
	[DataMember(Name = "numOfTerminatedAWSDevices")]
	public int TerminatedAwsResourceCount { get; set; }

	/// <summary>
	///     Terminated Azure Resource count
	/// </summary>
	[DataMember(Name = "numOfTerminatedAzureDevices")]
	public int TerminatedAzureResourceCount { get; set; }

	/// <summary>
	///     Terminated GCP Resource count
	/// </summary>
	[DataMember(Name = "numOfTerminatedGcpCloudDevices")]
	public int TerminatedGcpResourceCount { get; set; }

	/// <summary>
	///     The terminated PaaS Resource count
	/// </summary>
	[DataMember(Name = "numOfTerminatedPaasDevices")]
	public int TerminatedPaasResourceCount { get; set; }

	/// <summary>
	///     Time Zone as text
	/// </summary>
	[DataMember(Name = "timezone")]
	public string TimeZone { get; set; } = string.Empty;

	/// <summary>
	///     The web site count
	/// </summary>
	[DataMember(Name = "numOfWebsites")]
	public int WebsiteCount { get; set; }

	/// <summary>
	///     The web site group count
	/// </summary>
	[DataMember(Name = "numOfWebsiteFolders")]
	public int WebsiteGroupCount { get; set; }

	/// <summary>
	///     The web site group count that should not be exceeded
	/// </summary>
	[DataMember(Name = "numOfWebsiteFoldersWarningLimit")]
	public int WebsiteGroupWarningLimit { get; set; }

	/// <summary>
	///     The web site group count that must should not be exceeded
	/// </summary>
	[DataMember(Name = "numOfWebsiteFoldersErrorLimit")]
	public int WebsiteGroupErrorLimit { get; set; }

	/// <summary>
	///     IP WhiteList
	/// </summary>
	[DataMember(Name = "whiteList")]
	public string WhiteList { get; set; } = string.Empty;

	/// <summary>
	///     AccountDomain WhiteList
	/// </summary>
	[DataMember(Name = "accountDomainWhiteList")]
	public string AccountDomainWhiteList { get; set; } = string.Empty;

	/// <summary>
	///     User Suspend Days
	/// </summary>
	[DataMember(Name = "userSuspendDays")]
	public int UserSuspendDays { get; set; }

	/// <summary>
	///     User Role count
	/// </summary>
	[DataMember(Name = "numberOfUserRoles")]
	public int UserRoleCount { get; set; }

	/// <summary>
	///     Widget count
	/// </summary>
	[DataMember(Name = "numberOfWidgets")]
	public int WidgetCount { get; set; }

	/// <summary>
	/// Whether collector debug is enabled
	/// </summary>
	[DataMember(Name = "enableCollectorDebug")]
	public bool EnableCollectorDebug { get; set; }

	/// <summary>
	/// Whether test scripts are enabled
	/// </summary>
	[DataMember(Name = "enableTestScript")]
	public bool EnableTestScript { get; set; }

	/// <summary>
	/// enableScriptsInTextWidget
	/// </summary>
	[DataMember(Name = "enableScriptsInTextWidget")]
	public bool EnableScriptsInTextWidget { get; set; }

	/// <summary>
	/// enableUpdateOfTenantIdentifierProperty
	/// </summary>
	[DataMember(Name = "enableUpdateOfTenantIdentifierProperty")]
	public bool EnableUpdateOfTenantIdentifierProperty { get; set; }

	/// <summary>
	/// remainingTimeToKeepTenantIdentifierPropertyTextBoxDisabled
	/// </summary>
	[DataMember(Name = "remainingTimeToKeepTenantIdentifierPropertyTextBoxDisabled")]
	public string RemainingTimeToKeepTenantIdentifierPropertyTextBoxDisabled { get; set; } = string.Empty;

	/// <summary>
	/// maximumTimeToKeepTenantIdentifierPropertyTextBoxDisabledInMins
	/// </summary>
	[DataMember(Name = "maximumTimeToKeepTenantIdentifierPropertyTextBoxDisabledInMins")]
	public int MaximumTimeToKeepTenantIdentifierPropertyTextBoxDisabledInMins { get; set; }

	/// <summary>
	/// TokenDisabledDays
	/// </summary>
	[DataMember(Name = "tokenDisabledDays")]
	public int TokenDisabledDays { get; set; }

	/// <summary>
	/// LogNumOfPipelines 
	/// </summary>
	[DataMember(Name = "logNumOfPipelines")]
	public int LogNumOfPipelines { get; set; }

	/// <summary>
	/// LogNumOfPartitions 
	/// </summary>
	[DataMember(Name = "logNumOfPartitions")]
	public int LogNumOfPartitions { get; set; }

	/// <summary>
	/// LogNumOfAlertConditions 
	/// </summary>
	[DataMember(Name = "logNumOfAlertConditions")]
	public int LogNumOfAlertConditions { get; set; }

	/// <summary>
	/// Widgets per dashboard 
	/// </summary>
	[DataMember(Name = "widgetsPerDashboard")]
	public object? widgetsPerDashboard { get; set; }

	/// <summary>
	///     The endpoint
	/// </summary>
	public string Endpoint() => "setting/companySetting";
}
