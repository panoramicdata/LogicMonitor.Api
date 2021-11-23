namespace LogicMonitor.Api;

/// <summary>
/// A named entity
/// </summary>
[DataContract]
public class NamedEntity
{
	/// <summary>
	/// The entity's name
	/// </summary>
	[DataMember(Name = "name")]
	public string Name { get; set; }

	/// <summary>
	/// The entity's description
	/// </summary>
	public string Description { get; set; }

	/// <summary>
	/// Returns a string that represents the current object.
	/// </summary>
	public override string ToString() => Name;
}
