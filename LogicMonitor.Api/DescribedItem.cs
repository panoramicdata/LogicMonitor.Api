using System.Runtime.Serialization;

namespace LogicMonitor.Api;

/// <summary>
///    A described item
/// </summary>
public abstract class DescribedItem : IdentifiedItem
{
	/// <summary>
	///    The LogicMonitor Description
	/// </summary>
	[DataMember(Name = "description")]
	public string Description { get; set; }

	/// <summary>
	///    ToString override
	/// </summary>
	/// <returns>'Name (Id)'</returns>
	public override string ToString() => $"{Description} ({Id})";
}
