namespace LogicMonitor.Api;

/// <summary>
/// Supplements <see cref="IHasEndpoint"/> for types whose creation URL differs from
/// the base endpoint. For example, Uptime ping checks must be POSTed to
/// <c>device/devices?type=uptimepingcheck</c> to trigger server-side initialisation of
/// the internal ping-check data sources, while GET/PUT/DELETE continue to use the plain
/// <c>device/devices/{id}</c> path.
/// </summary>
public interface IHasCreationEndpoint
{
	/// <summary>
	/// Returns the sub-URL to use when creating (POSTing) a new instance of this type.
	/// </summary>
	string CreationEndpoint();
}
