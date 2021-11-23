using System.Runtime.Serialization;

namespace LogicMonitor.Api;

/// <summary>
/// Item identified by a string
/// </summary>
[DataContract]
public abstract class NamedStringIdentifiedItem : StringIdentifiedItem
{
	/// <summary>
	/// The name
	/// </summary>
	[DataMember(Name = "name")]
	public string Name { get; set; }

	/// <summary>
	/// The description
	/// </summary>
	[DataMember(Name = "description")]
	public string Description { get; set; }

	/// <inheritdoc />
	public override string ToString() => $"{Name} ({Id})";
}
