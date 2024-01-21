namespace LogicMonitor.Api.Settings;

/// <summary>
/// A base class for Integration creation DTOs
/// </summary>
/// <typeparam name="T"></typeparam>
/// <remarks>
///   Constructor
/// </remarks>
/// <param name="type">The integration type</param>
public abstract class IntegrationCreationDto<T>(string type)
	: CreationDto<Integration>, IHasName where T : Integration
{
	/// <summary>
	///    The LogicMonitor Name
	/// </summary>
	[DataMember(Name = "name")]
	public string Name { get; set; } = string.Empty;

	/// <summary>
	///     The integration type
	/// </summary>
	[DataMember(Name = "type")]
	public string Type { get; } = type;

	/// <summary>
	///     Extra configuration
	/// </summary>
	[DataMember(Name = "extra")]
	public string Extra { get; set; } = string.Empty;
}