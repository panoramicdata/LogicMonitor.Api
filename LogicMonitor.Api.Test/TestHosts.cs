namespace LogicMonitor.Api.Test;

/// <summary>
/// Well-known hosts used by the integration tests.
/// </summary>
/// <remarks>
/// These tests create real Uptime ping/web checks in a live portal, so the target must be a
/// genuinely reachable, stable, publicly routable address. Documentation addresses (RFC 5737)
/// cannot be used here - the portal would never see the check succeed. The address is defined
/// once, here, so that there is a single place to change it.
/// </remarks>
internal static class TestHosts
{
	/// <summary>
	/// Google Public DNS. Chosen because it is globally reachable, anycast, and answers ICMP.
	/// </summary>
	public static string PingTarget => "8.8.8.8";
}
