namespace LogicMonitor.Api.LogicModules;

/// <summary>
/// A config check script
/// </summary>
[DataContract]
public class ConfigCheckScript
{
	/// <summary>
	/// The format
	/// </summary>
	[DataMember(Name = "format")]
	public string Format { get; set; }

	/// <summary>
	/// Groovy script
	/// </summary>
	[DataMember(Name = "groovy")]
	public string Groovy { get; set; }

	/// <summary>
	/// The diff check
	/// </summary>
	[DataMember(Name = "diff_check")]
	public DiffCheck DiffCheck { get; set; }

	/// <summary>
	/// The fetch check
	/// </summary>
	[DataMember(Name = "fetch_check")]
	public FetchCheck FetchCheck { get; set; }

	/// <summary>
	/// The value check
	/// </summary>
	[DataMember(Name = "value_check")]
	public ValueCheck ValueCheck { get; set; }
}
