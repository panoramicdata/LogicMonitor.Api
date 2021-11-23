namespace LogicMonitor.Api.LogicModules;

/// <summary>
/// Value check must
/// </summary>
[DataContract]
public class ValueCheckMust
{
	/// <summary>
	///    The variable
	/// </summary>
	[DataMember(Name = "range")]

	public ValueCheckRange Range { get; set; }
}
