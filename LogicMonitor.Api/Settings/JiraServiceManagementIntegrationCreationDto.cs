namespace LogicMonitor.Api.Settings;

/// <summary>
/// Jira Service Management Creation Dto
/// </summary>
[DataContract]
public class JiraServiceManagementIntegrationCreationDto : IntegrationCreationDto<JiraServiceManagementIntegration>
{
	/// <summary>
	/// Constructor
	/// </summary>
	public JiraServiceManagementIntegrationCreationDto() : base("jsm")
	{
	}
}
