namespace LogicMonitor.Api.LogicModules;

/// <summary>
/// A value check
/// </summary>
[DataContract]
public class ValueCheck
{
	/// <summary>
	///    The variable
	/// </summary>
	[DataMember(Name = "variable")]
	public string Variable { get; set; } = string.Empty;

	/// <summary>
	/// A value check must
	/// </summary>
	[DataMember(Name = "must")]
	public List<ValueCheckMust> ValueCheckMust { get; set; } = new();
}
