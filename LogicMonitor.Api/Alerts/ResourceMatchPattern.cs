namespace LogicMonitor.Api.Alerts;

/// <summary>
///     A resource selector within an <see cref="AlertDependencyRule"/>: a resource group name and a
///     resource (instance) pattern the rule's routing behaviour applies to.
/// </summary>
[DataContract]
public class ResourceMatchPattern
{
	/// <summary>
	///     The resource group name (e.g. "PDL - Panoramic Data").
	/// </summary>
	[DataMember(Name = "group")]
	public string Group { get; set; } = string.Empty;

	/// <summary>
	///     The resource / instance match pattern (e.g. "AU-E1:networkinterface:pdl-col-au-01893_z1").
	/// </summary>
	[DataMember(Name = "resource")]
	public string Resource { get; set; } = string.Empty;

	/// <inheritdoc />
	public override string ToString() => $"{Group} / {Resource}";
}
