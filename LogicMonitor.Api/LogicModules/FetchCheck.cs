namespace LogicMonitor.Api.LogicModules;

/// <summary>
/// A fetch check
/// </summary>
[DataContract]
public class FetchCheck
{
	/// <summary>
	/// Fetch
	/// </summary>
	[DataMember(Name = "fetch")]
	public object? Fetch { get; set; }
}
