namespace LogicMonitor.Api;

internal class LogicMonitorApiCookie
{
	public LogicMonitorApiCookie(string text)
	{
		Text = text;
		CreateDateTimeUtc = DateTime.UtcNow;
	}

	public string Text { get; }
	public DateTime CreateDateTimeUtc { get; }
}
