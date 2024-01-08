namespace LogicMonitor.Api.Settings;

/// <summary>
/// HTTP Creation Dto
/// </summary>
[DataContract]
public class ServiceNowIntegrationCreationDto : HttpIntegrationCreationDto
{
	/// <summary>
	/// Constructor
	/// </summary>
	public ServiceNowIntegrationCreationDto() : base("servicenow")
	{
	}

	/// <summary>
	/// The ServiceNow subdomain
	/// </summary>
	[DataMember(Name = "subDomain")]
	public string SubDomain { get; set; } = string.Empty;

	/// <summary>
	/// The ServiceNow username
	/// </summary>
	[DataMember(Name = "servicenowUsername")]
	public string ServiceNowUsername { get; set; } = string.Empty;

	/// <summary>
	/// The ServiceNow password
	/// </summary>
	[DataMember(Name = "servicenowPassword")]
	public string ServiceNowPassword { get; set; } = string.Empty;

	/// <summary>
	/// The ServiceNow company
	/// </summary>
	[DataMember(Name = "servicenowCompany")]
	public string ServiceNowCompany { get; set; } = string.Empty;

	/// <summary>
	/// The ServiceNow warn severity
	/// </summary>
	[DataMember(Name = "servicenowWarnSeverity")]
	public string ServiceNowWarnSeverity { get; set; } = string.Empty;

	/// <summary>
	/// The ServiceNow error severity
	/// </summary>
	[DataMember(Name = "servicenowErrorSeverity")]
	public string ServiceNowErrorSeverity { get; set; } = string.Empty;

	/// <summary>
	/// The ServiceNow critical severity
	/// </summary>
	[DataMember(Name = "servicenowCriticalSeverity")]
	public string ServiceNowCriticalSeverity { get; set; } = string.Empty;

	/// <summary>
	/// The ServiceNow create incident state
	/// </summary>
	[DataMember(Name = "servicenowCreateIncidentState")]
	public string ServiceNowCreateIncidentState { get; set; } = string.Empty;

	/// <summary>
	/// The ServiceNow update incident state
	/// </summary>
	[DataMember(Name = "servicenowUpdateIncidentState")]
	public string ServiceNowUpdateIncidentState { get; set; } = string.Empty;

	/// <summary>
	/// The ServiceNow clear incident state
	/// </summary>
	[DataMember(Name = "servicenowClearIncidentState")]
	public string ServiceNowClearIncidentState { get; set; } = string.Empty;

	/// <summary>
	/// The ServiceNow acknowledgement incident state
	/// </summary>
	[DataMember(Name = "servicenowAckIncidentState")]
	public string ServiceNowAckIncidentState { get; set; } = string.Empty;

	/// <summary>
	/// The ServiceNow due Date/time
	/// </summary>
	[DataMember(Name = "servicenowDueDateTime")]
	public string ServiceNowDueDateTime { get; set; } = string.Empty;
}
