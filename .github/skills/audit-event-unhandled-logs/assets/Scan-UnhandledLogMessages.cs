#:project ../../../../LogicMonitor.Api/LogicMonitor.Api.csproj

using System.Diagnostics;
using System.Globalization;
using System.Text.RegularExpressions;
using LogicMonitor.Api.Extensions;
using LogicMonitor.Api.Logs;

var options = ParseArgs(args);
var scanStopwatch = Stopwatch.StartNew();

AppDomain.CurrentDomain.SetData("REGEX_DEFAULT_MATCH_TIMEOUT", TimeSpan.FromSeconds(options.RegexTimeoutSeconds));

if (!Directory.Exists(options.RootPath))
{
	Console.Error.WriteLine($"[{TimestampUtc()}] Path not found: {options.RootPath}");
	return 1;
}

Console.WriteLine($"[{TimestampUtc()}] Scanning unhandled logs from: {options.RootPath}");
Console.WriteLine($"[{TimestampUtc()}] Will print progress every {options.ProgressEvery} messages.");
Console.WriteLine($"[{TimestampUtc()}] Regex timeout per message: {options.RegexTimeoutSeconds}s.");

var files = Directory
	.EnumerateFiles(options.RootPath)
	.OrderBy(Path.GetFileName, StringComparer.OrdinalIgnoreCase)
	.ToList();

Console.WriteLine($"[{TimestampUtc()}] Found {files.Count} file(s). Starting scan...");

var aggregates = new Dictionary<string, Aggregate>(StringComparer.Ordinal);
var totalMessages = 0;
var handledMessages = 0;
var unhandledMessages = 0;
var erroredMessages = 0;

var fileIndex = 0;

foreach (var filePath in files)
{
	fileIndex++;
	Console.WriteLine($"[{TimestampUtc()}] [{fileIndex}/{files.Count}] Reading {Path.GetFileName(filePath)}...");

	var lines = File.ReadAllLines(filePath);
	var currentBlock = new List<string>();

	foreach (var line in lines)
	{
		if (line.StartsWith("-----", StringComparison.Ordinal))
		{
			ProcessBlock(currentBlock, filePath);
			currentBlock.Clear();
			continue;
		}

		currentBlock.Add(line);
	}

	ProcessBlock(currentBlock, filePath);
}

var unmatched = aggregates.Values
	.OrderByDescending(a => a.Count)
	.ThenBy(a => a.Message, StringComparer.Ordinal)
	.ToList();

if (options.First > 0)
{
	unmatched = unmatched.Take(options.First).ToList();
}

for (var i = 0; i < unmatched.Count; i++)
{
	var item = unmatched[i];
	Console.WriteLine($"[{i + 1}] count={item.Count}; file={Path.GetFileName(item.SourceFile)}; id={item.SourceId}");
	if (!string.IsNullOrWhiteSpace(item.Error))
	{
		Console.WriteLine($"[{TimestampUtc()}] ERROR: {item.Error}");
	}
	Console.WriteLine(item.Message);
	Console.WriteLine(new string('-', 80));
}

Console.WriteLine($"[{TimestampUtc()}] Total messages: {totalMessages}");
Console.WriteLine($"[{TimestampUtc()}] Handled: {handledMessages}");
Console.WriteLine($"[{TimestampUtc()}] Unhandled: {unhandledMessages}");
Console.WriteLine($"[{TimestampUtc()}] Errored: {erroredMessages}");
Console.WriteLine($"[{TimestampUtc()}] Unique unmatched patterns: {aggregates.Count}");
Console.WriteLine($"[{TimestampUtc()}] Scan complete in {scanStopwatch.Elapsed:c}");

return 0;

void ProcessBlock(List<string> block, string filePath)
{
	if (block.Count < 2)
	{
		return;
	}

	var message = string.Join(Environment.NewLine, block.Skip(1)).Trim();
	if (string.IsNullOrWhiteSpace(message))
	{
		return;
	}

	totalMessages++;
	if (totalMessages % options.ProgressEvery == 0)
	{
		Console.WriteLine(
			$"[{TimestampUtc()}] Progress: processed={totalMessages}, handled={handledMessages}, unhandled={unhandledMessages}, errored={erroredMessages}, elapsed={scanStopwatch.Elapsed:c}");
	}

	var sourceId = ExtractSourceId(block[0]);
	if (options.LogEachMessage)
	{
		Console.WriteLine($"[{TimestampUtc()}] Parsing message #{totalMessages}, id={sourceId}, file={Path.GetFileName(filePath)}, length={message.Length}...");
	}

	ClassifyMessage(message, filePath, sourceId);
}

void ClassifyMessage(string message, string filePath, string sourceId)
{
	try
	{
		var logItem = new LogItem
		{
			Id = sourceId,
			Description = message,
			PerformedByUsername = string.Empty,
			SessionId = string.Empty,
			IpAddress = string.Empty,
			HappenedOnTimeStampUtc = 0
		};

		var auditEvent = logItem.ToAuditEvent();
		if (auditEvent.MatchedRegExId == -1)
		{
			unhandledMessages++;
			AddAggregate(message, filePath, sourceId, null);
		}
		else
		{
			handledMessages++;
		}
	}
	catch (RegexMatchTimeoutException ex)
	{
		erroredMessages++;
		var timeoutError = $"Regex timeout after {options.RegexTimeoutSeconds}s while parsing message id={sourceId}: {ex.Message}";
		AddAggregate(message, filePath, sourceId, timeoutError);
		Console.WriteLine($"[{TimestampUtc()}] TIMEOUT: {timeoutError}");
	}
	catch (Exception ex)
	{
		erroredMessages++;
		AddAggregate(message, filePath, sourceId, ex.Message);
		Console.WriteLine($"[{TimestampUtc()}] ERROR parsing id={sourceId}: {ex.Message}");
	}
}

void AddAggregate(string message, string sourceFile, string sourceId, string? error)
{
	if (aggregates.TryGetValue(message, out var existing))
	{
		existing.Count++;
		return;
	}

	aggregates[message] = new Aggregate
	{
		Message = message,
		SourceFile = sourceFile,
		SourceId = sourceId,
		Error = error,
		Count = 1
	};
}

static string ExtractSourceId(string header)
{
	const string marker = " - LogItem ";
	var markerIndex = header.IndexOf(marker, StringComparison.Ordinal);
	if (markerIndex < 0)
	{
		return "unknown";
	}

	var start = markerIndex + marker.Length;
	var end = header.IndexOf(' ', start);
	if (end < 0)
	{
		return header[start..].Trim();
	}

	return header[start..end].Trim();
}

static Options ParseArgs(string[] args)
{
	string[] valueOptionNames = ["--root", "--first", "--progress-every", "--regex-timeout-seconds"];

	var values = new Dictionary<string, string>(StringComparer.Ordinal);
	var logEachMessage = false;

	var i = 0;
	while (i < args.Length)
	{
		var arg = args[i];

		if (Array.IndexOf(valueOptionNames, arg) >= 0 && i + 1 < args.Length)
		{
			values[arg] = args[i + 1];
			i += 2;
			continue;
		}

		if (arg == "--log-each-message")
		{
			logEachMessage = true;
		}

		i++;
	}

	return new Options(
		Path.GetFullPath(GetStringOption(values, "--root", "./LogicMonitor.Api.Test/EventLogs/UnhandledLogs")),
		GetInt32Option(values, "--first", 20, int.MinValue),
		GetInt32Option(values, "--progress-every", 200, 1),
		GetInt32Option(values, "--regex-timeout-seconds", 5, 1),
		logEachMessage);
}

static string GetStringOption(Dictionary<string, string> values, string name, string fallback)
	=> values.TryGetValue(name, out var value) ? value : fallback;

// Reads an integer option, falling back to the supplied default if it is absent,
// unparseable, or below the supplied minimum.
static int GetInt32Option(Dictionary<string, string> values, string name, int fallback, int minimum)
	=> values.TryGetValue(name, out var raw)
		&& int.TryParse(raw, NumberStyles.Integer, CultureInfo.InvariantCulture, out var parsed)
		&& parsed >= minimum
			? parsed
			: fallback;

static string TimestampUtc() => DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss.fff 'UTC'", CultureInfo.InvariantCulture);

file sealed class Aggregate
{
	public required string Message { get; init; }
	public required string SourceFile { get; init; }
	public required string SourceId { get; init; }
	public string? Error { get; init; }
	public int Count { get; set; }
}

file sealed record Options(string RootPath, int First, int ProgressEvery, int RegexTimeoutSeconds, bool LogEachMessage);
