namespace LogicMonitor.Api.LogicModules;

/// <summary>
///     A virtual data point
/// </summary>
[DataContract]
public class VirtualDataPoint : UndescribedNamedItem
{
	/// <summary>
	///     The calculation
	/// </summary>
	[DataMember(Name = "rpn")]
	public string Rpn { get; set; } = string.Empty;
}
