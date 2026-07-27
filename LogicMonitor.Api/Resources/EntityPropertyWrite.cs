namespace LogicMonitor.Api.Resources;

/// <summary>
///    A single directive to write one custom property onto a <see cref="Resource" /> or
///    <see cref="ResourceGroup" />, identified by type and id.
/// </summary>
/// <remarks>
///    <para>
///    This is the config-as-code shape for <b>setting or rotating</b> hidden property fields (e.g.
///    <c>snmp.community</c>, <c>*.pass</c>, <c>*.key</c>) to a new value. It writes one named property
///    at a time, so a masked <c>********</c> value is never sent - you always supply the real value.
///    </para>
///    <para>
///    Note on the masked-secret concern: LogicMonitor masks these values as <c>********</c> on GET, but
///    for a <see cref="Resource" /> / <see cref="ResourceGroup" /> the server treats that mask as
///    "leave unchanged" on write, so a full-object round-trip (GET then
///    <see cref="LogicMonitorClient.PutAsync{T}(T, System.Threading.CancellationToken)" />, e.g. for a
///    rename) does <b>not</b> clobber the real secret. This was verified empirically. Use
///    <see cref="EntityPropertyWrite" /> when you actually want to change the stored value, not merely
///    preserve it across an unrelated update.
///    </para>
///    <para>
///    Applying these writes requires LogicMonitor administrator rights. A list of these objects
///    deserialises directly from JSON such as:
///    <code>
///    [ { "type": "resourceGroup", "id": 1234, "name": "snmp.community", "value": "public" } ]
///    </code>
///    </para>
/// </remarks>
[DataContract]
public class EntityPropertyWrite
{
	/// <summary>
	///    The type of entity to write the property onto.
	/// </summary>
	[DataMember(Name = "type")]
	public EntityPropertyWriteTargetType Type { get; set; }

	/// <summary>
	///    The id of the target <see cref="Resource" /> or <see cref="ResourceGroup" />.
	/// </summary>
	[DataMember(Name = "id")]
	public int Id { get; set; }

	/// <summary>
	///    The custom property name to write (e.g. <c>snmp.community</c>).
	/// </summary>
	[DataMember(Name = "name")]
	public string Name { get; set; } = string.Empty;

	/// <summary>
	///    The custom property value to write. If null and the property exists, it is removed.
	/// </summary>
	[DataMember(Name = "value")]
	public string? Value { get; set; }

	/// <summary>
	///    The properties sub-URL that this write targets, e.g.
	///    <c>device/groups/1234/properties</c> or <c>device/devices/1234/properties</c>.
	/// </summary>
	/// <exception cref="NotSupportedException">Thrown when <see cref="Type" /> is not a supported target.</exception>
	public string PropertiesSubUrl() => Type switch
	{
		EntityPropertyWriteTargetType.Resource => $"device/devices/{Id}/properties",
		EntityPropertyWriteTargetType.ResourceGroup => $"device/groups/{Id}/properties",
		_ => throw new NotSupportedException($"Unsupported {nameof(EntityPropertyWriteTargetType)}: {Type}")
	};

	/// <inheritdoc />
	public override string ToString() => $"{Type} {Id}: {Name}={Value}";
}
