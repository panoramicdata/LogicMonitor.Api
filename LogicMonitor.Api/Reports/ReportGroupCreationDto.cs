namespace LogicMonitor.Api.Reports;

/// <summary>
///    A ReportGroup creation DTO
/// </summary>
[DataContract]
public class ReportGroupCreationDto : CreationDto<ReportGroup>, IHasName, IHasDescription
{
	/// <summary>
	///    The name
	/// </summary>
	[DataMember(Name = "name")]
	public string Name { get; set; } = string.Empty;

	/// <summary>
	///    The description
	/// </summary>
	[DataMember(Name = "description")]
	public string Description { get; set; } = string.Empty;

	/// <summary>
	///    ToString override
	/// </summary>
	/// <returns>Name</returns>
	public override string ToString() => Name;
}
