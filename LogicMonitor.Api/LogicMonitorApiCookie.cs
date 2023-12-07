namespace LogicMonitor.Api;

internal class LogicMonitorApiCookie(string text)
{
	public string Text { get; } = text;
	public DateTime CreateDateTimeUtc { get; } = DateTime.UtcNow;
}
