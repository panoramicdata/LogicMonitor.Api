namespace LogicMonitor.Api;

/// <summary>
/// A simple property
/// </summary>
[DataContract]
public class EntityProperty
{
	/// <summary>
	/// The name of the property
	/// </summary>
	[DataMember(Name = "name")]
	public string Name { get; set; } = string.Empty;

	/// <summary>
	/// The value of the property
	/// </summary>
	[DataMember(Name = "value")]
	public string Value { get; set; } = string.Empty;

	/// <summary>
	/// The type of property among Inherit|System|Custom
	/// </summary>
	[DataMember(Name = "type")]
	[JsonConverter(typeof(StringEnumConverter))]
	public PropertyType Type { get; set; }

	/// <summary>
	/// The inherit list of the property
	/// </summary>
	[DataMember(Name = "inheritList")]
	public List<InheritedItem> InheritList { get; set; } = new();

	/// <inheritdoc />
	public override string ToString() => $"{Type}: {Name}={Value}";
}
