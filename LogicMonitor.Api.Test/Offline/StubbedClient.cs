namespace LogicMonitor.Api.Test.Offline;

/// <summary>
/// Builds <see cref="LogicMonitorClient"/> instances that talk to a <see cref="StubHttpMessageHandler"/>.
/// </summary>
internal static class StubbedClient
{
	public const string Account = "acme";

	public static LogicMonitorClient Create(
		StubHttpMessageHandler handler,
		Action<LogicMonitorClientOptions>? configure = null)
	{
		var options = new LogicMonitorClientOptions
		{
			Account = Account,
			AccessId = "id",
			AccessKey = "key"
		};
		configure?.Invoke(options);
		return new LogicMonitorClient(options, handler)
		{
			AttemptCount = 2,
			WaitDuringLogicMonitorUpgrades = false
		};
	}

	public static string PageJson(string itemsJson, int total)
		=> $$$"""{"status":200,"errmsg":"OK","data":{"total":{{{total}}},"items":{{{itemsJson}}}}}""";
}
