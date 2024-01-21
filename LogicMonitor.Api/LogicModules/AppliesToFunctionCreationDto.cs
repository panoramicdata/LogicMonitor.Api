namespace LogicMonitor.Api.LogicModules;

/// <summary>
/// An AppliesTo Function creation DTO
/// </summary>
[DataContract]
public class AppliesToFunctionCreationDto : CreationDto<AppliesToFunction>, IHasName, IHasDescription
{
	/// <summary>
	/// The name
	/// </summary>
	[DataMember(Name = "name")]
	public string Name { get; set; } = string.Empty;

	/// <summary>
	/// The description
	/// </summary>
	[DataMember(Name = "description")]
	public string Description { get; set; } = string.Empty;

	/// <summary>
	/// The code
	/// </summary>
	[DataMember(Name = "code")]
	public string Code { get; set; } = string.Empty;
}
