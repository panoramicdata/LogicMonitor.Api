namespace LogicMonitor.Api.OpsNotes;

/// <summary>
/// A Website Ops Note Scope creation DTO
/// </summary>
[DataContract]
public class WebsiteGroupOpsNoteScopeCreationDto : OpsNoteScopeCreationDto
{
	/// <summary>
	/// Constructor
	/// </summary>
	public WebsiteGroupOpsNoteScopeCreationDto()
	{
		Type = "websiteGroup";
	}

	/// <summary>
	/// The website group Id
	/// </summary>
	[DataMember(Name = "websiteGroupId")]
	public int WebsiteGroupId { get; set; }
}
