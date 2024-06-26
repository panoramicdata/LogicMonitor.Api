namespace LogicMonitor.Api.Functions;

/// <summary>
/// AppliesTo response
/// </summary>
[DataContract]
public class AppliesToResponse
{
	/// <summary>
	/// The requested original applies to
	/// </summary>
	[DataMember(Name = "originalAppliesTo")]
	public string OriginalAppliesTo { get; set; } = string.Empty;

	/// <summary>
	/// The requested original applies to
	/// </summary>
	[DataMember(Name = "currentAppliesTo")]
	public string CurrentAppliesTo { get; set; } = string.Empty;

	/// <summary>
	/// The original matches
	/// </summary>
	[DataMember(Name = "originalMatches")]
	public List<AppliesToMatch> OriginalMatches { get; set; } = [];

	/// <summary>
	/// The current matches
	/// </summary>
	[DataMember(Name = "currentMatches")]
	public List<AppliesToMatch> CurrentMatches { get; set; } = [];

	/// <summary>
	/// The warn message
	/// </summary>
	[DataMember(Name = "warnMessage")]
	public string? WarnMessage { get; set; }
}
