namespace LogicMonitor.Api;

/// <summary>
/// A simple property
/// </summary>
[DataContract]
public class Property
{
	/// <summary>
	///    The property name
	/// </summary>
	[DataMember(Name = "name")]
	public string Name { get; set; } = string.Empty;

	/// <summary>
	///    The property value
	/// </summary>
	[DataMember(Name = "value")]
	public string Value { get; set; } = string.Empty;

	/// <summary>
	///    The property type
	/// </summary>
	[DataMember(Name = "type")]
	[JsonConverter(typeof(StringEnumConverter))]
	public PropertyType? Type { get; set; }

	/// <summary>
	///    The list of inherited items
	/// </summary>
	[DataMember(Name = "inheritList")]
	public List<InheritedItem> InheritList { get; set; } = new();

	/// <inheritdoc />
	public override string ToString() => $"{Type}: {Name}={Value}";
}
