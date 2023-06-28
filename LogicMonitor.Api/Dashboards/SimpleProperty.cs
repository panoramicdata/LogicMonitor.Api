namespace LogicMonitor.Api.Dashboards;

/// <summary>
/// A simple property
/// </summary>
[DataContract]
public class SimpleProperty
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
	/// The type of property among Inherit|Owned
	/// </summary>
	[DataMember(Name = "type")]
	[JsonConverter(typeof(StringEnumConverter))]
	public SimplePropertyType? Type { get; set; } = SimplePropertyType.Owned;

	/// <summary>
	/// The inherit list of the property
	/// </summary>
	[DataMember(Name = "inheritList")]
	public List<InheritedItem> InheritList { get; set; } = new();

	/// <inheritdoc />
	public override string ToString() => $"{Type}: {Name}={Value}";
}
