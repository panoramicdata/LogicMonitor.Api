namespace LogicMonitor.Api.Extensions;

/// <summary>
/// Log HTTP request or response headers
/// </summary>
public static class LoggerExtensions
{
	/// <summary>
	/// Log the HTTTP request or response details
	/// </summary>
	/// <param name="logger">An ILogger</param>
	/// <param name="isRequest">Whether it's HTTP request headers / HTTP response headers</param>
	/// <param name="prefix">A GUID prefix</param>
	/// <param name="headers">HTTP headers</param>
	public static void LogHttpHeaders(this ILogger logger, bool isRequest, string? prefix, HttpHeaders? headers)
	{
		var guidPrefix = prefix != null ? (prefix + " ") : string.Empty;

		// Log headers
		if (headers != null)
		{
			try
			{
				var headerType = (isRequest ? "REQUEST HEADERS:" : "RESPONSE HEADERS:") + "\r\n\r\n";

				var output = string.Empty;
				foreach (var header in headers.Where(x => x.Key != "Authorization"))
				{
					output += $"HEADER-NAME: {header.Key} | HEADER-VALUE(S): {string.Join(";", header.Value)} " + "\r\n";
				}

				logger.LogDebug("{Guid}{HeaderType}{Headers}", guidPrefix, headerType, output);
			}
			catch (Exception e)
			{
				logger.LogDebug("{Guid}Unable to get headers for debug logging: {Message}", guidPrefix, e.Message);
			}
		}
	}
}
