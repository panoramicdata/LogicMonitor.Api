namespace LogicMonitor.Api;

internal static class ReadOnlyGuard
{
	// POST endpoints that only read: they take a request body but change nothing on the portal.
	private static readonly string[] _readOnlyPostPaths =
	[
		"functions",
		"device/instances/datafetch",
		"hierarchy/dataSourceInstances",
		"log/search"
	];

	internal static void ThrowIfWrite(LogicMonitorClientOptions options, HttpMethod method, string subUrl)
	{
		if (!options.IsReadOnly
			|| method == HttpMethod.Get
			|| method == HttpMethod.Head
			|| method == HttpMethod.Options
			|| (method == HttpMethod.Post && IsReadOnlyPostPath(subUrl)))
		{
			return;
		}

		throw new InvalidOperationException($"The client is read-only (IsReadOnly is true): {method} {subUrl} is not allowed.");
	}

	private static bool IsReadOnlyPostPath(string subUrl)
	{
		var path = subUrl.Split('?')[0].Trim('/');
		const string restPrefix = "santaba/rest/";
		if (path.StartsWith(restPrefix, StringComparison.Ordinal))
		{
			path = path[restPrefix.Length..];
		}

		return _readOnlyPostPaths.Contains(path, StringComparer.Ordinal)
			|| path.StartsWith("log/search/", StringComparison.Ordinal);
	}
}
