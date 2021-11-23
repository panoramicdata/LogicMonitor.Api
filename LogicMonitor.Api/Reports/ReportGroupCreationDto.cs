namespace LogicMonitor.Api.Reports;

/// <summary>
///    A ReportGroup creation DTO
/// </summary>
[DataContract]
public class ReportGroupCreationDto : CreationDto<ReportGroup>
{
	/// <summary>
	///    The name
	/// </summary>
	[DataMember(Name = "name")]
	public string Name { get; set; }

	/// <summary>
	///    The description
	/// </summary>
	[DataMember(Name = "description")]
	public string Description { get; set; }

	/// <summary>
	///    ToString override
	/// </summary>
	/// <returns>Name</returns>
	public override string ToString() => Name;
}
