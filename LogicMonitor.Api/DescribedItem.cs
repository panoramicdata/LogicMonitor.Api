namespace LogicMonitor.Api;

/// <summary>
///    A described item
/// </summary>
public abstract class DescribedItem : IdentifiedItem, IHasDescription
{
	/// <summary>
	///    The LogicMonitor Description
	/// </summary>
	[DataMember(Name = "description")]
	public string Description { get; set; } = string.Empty;

	/// <summary>
	///    ToString override
	/// </summary>
	/// <returns>'Name (Id)'</returns>
	public override string ToString() => $"{Description} ({Id})";
}
