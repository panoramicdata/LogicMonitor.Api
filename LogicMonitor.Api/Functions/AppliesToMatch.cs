namespace LogicMonitor.Api.Functions;

/// <summary>
///     An applies to match
/// </summary>
[DataContract]
public class AppliesToMatch
{
	/// <summary>
	///     The type (e.g. Device / Website)
	/// </summary>
	[DataMember(Name = "type")]
	public MonitoredObjectType Type { get; set; }

	/// <summary>
	///     The id
	/// </summary>
	[DataMember(Name = "id")]
	public int Id { get; set; }

	/// <summary>
	///     The name
	/// </summary>
	[DataMember(Name = "name")]
	public string Name { get; set; } = string.Empty;

	/// <inheritdoc />
	public override string ToString() => $"{Type} {Id}: {Name}";
}
