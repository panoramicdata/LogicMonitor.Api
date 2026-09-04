using System.Runtime.CompilerServices;

namespace LogicMonitor.Api.Test.Conventions;

/// <summary>
/// Cover for issue #38.
///
/// The rate-limit backoff wait is unbounded in aggregate and silent by every other measure - no CPU,
/// no open socket, no exception. Logged at Debug, a consumer running at Information cannot tell a
/// polite backoff from a hung call; that cost an hour of investigation once.
///
/// This is a source-level guard rather than a runtime test because asserting on log output would
/// need an HTTP test handler, a mocking framework and a capturing logger, none of which this test
/// project has. The failure it protects against is a sixth backoff site being added at Debug, which
/// a source check catches precisely.
/// </summary>
public class RateLimitLoggingTests
{
	private static string ClientSource([CallerFilePath] string thisFile = "")
	{
		// <repo>/LogicMonitor.Api.Test/Conventions/<this file>  ->  <repo>/LogicMonitor.Api/LogicMonitorClient.cs
		var repoRoot = Path.GetDirectoryName(Path.GetDirectoryName(Path.GetDirectoryName(thisFile)!)!)!;
		var path = Path.Combine(repoRoot, "LogicMonitor.Api", "LogicMonitorClient.cs");
		File.Exists(path).Should().BeTrue($"the client source should be at {path}");
		return File.ReadAllText(path);
	}

	[Fact]
	public void EveryRateLimitBackoffGoesThroughTheSharedHelper()
	{
		var source = ClientSource();

		// The helper is what decides the level and carries the cumulative attempt/total.
		var helperReferences = source.Split("WaitForRateLimitAsync(").Length - 1;

		// One declaration plus one call per backoff site.
		helperReferences.Should().BeGreaterThan(1,
			"every rate-limit backoff must route through WaitForRateLimitAsync so it is logged consistently");
	}

	[Fact]
	public void NoRateLimitBackoffIsLoggedAtDebug()
	{
		var source = ClientSource();

		var offenders = source
			.Split('\n')
			.Select((line, index) => (line, number: index + 1))
			.Where(x => x.line.Contains("Rate limiting hit", StringComparison.Ordinal)
					 && x.line.Contains("LogDebug", StringComparison.Ordinal))
			.Select(x => $"line {x.number}")
			.ToList();

		offenders.Should().BeEmpty(
			"a rate-limit wait is unbounded and otherwise invisible, so it must be logged at Information or above");
	}

	[Fact]
	public void TheHelperEscalatesToWarningOnALongCumulativeWait()
	{
		var source = ClientSource();

		source.Should().Contain("LogWarning",
			"a wait that has exceeded a full maximum backoff has stopped being routine");
		source.Should().Contain("TotalWaitedMs",
			"the log must carry the cumulative wait, not just the current delay - the tenth wait reads differently from the first");
	}
}
