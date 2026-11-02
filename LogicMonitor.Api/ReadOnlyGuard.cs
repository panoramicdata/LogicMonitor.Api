namespace LogicMonitor.Api;

internal static class ReadOnlyGuard
{
	internal static void ThrowIfWrite(LogicMonitorClientOptions options, HttpMethod method)
	{
		if (options.IsReadOnly
			&& method != HttpMethod.Get
			&& method != HttpMethod.Head
			&& method != HttpMethod.Options)
		{
			throw new InvalidOperationException($"The client is read-only (IsReadOnly is true): {method} requests are not allowed.");
		}
	}
}
