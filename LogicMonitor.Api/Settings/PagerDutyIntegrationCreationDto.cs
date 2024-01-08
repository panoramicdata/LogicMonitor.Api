namespace LogicMonitor.Api.Settings;

/// <summary>
/// PagerDuty Integration Creation Dto
/// </summary>
[DataContract]
public class PagerDutyIntegrationCreationDto : HttpIntegrationCreationDto
{
	/// <summary>
	/// Constructor
	/// </summary>
	public PagerDutyIntegrationCreationDto() : base("pagerduty")
	{
	}

	/// <summary>
	/// Service Key
	/// </summary>
	[DataMember(Name = "serviceKey")]
	public string ServiceKey { get; set; } = string.Empty;
}
