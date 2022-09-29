using System.Linq;

namespace LogicMonitor.Api.Extensions;

/// <summary>
/// Log a HTTP request or response as a debug mode output for a logger
/// </summary>
public static class LoggerExtensions
{
	/// <summary>
	/// Log the HTTTP request or response details
	/// </summary>
	/// <param name="logger">An ILogger</param>
	/// <param name="prefix">A GUID prefix</param>
	/// <param name="headers">HTTP headers</param>
	/// <param name="body">HTTP body</param>
	public static void LogHttpDetails(this ILogger logger, bool isRequest, string? prefix, HttpHeaders? headers, string? body)
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

		// Log body
		if (body != null)
		{
			try
			{
				var bodyType = (isRequest ? "REQUEST BODY:" : "RESPONSE BODY:") + "\r\n\r\n";

				logger.LogDebug("{Guid}{BodyType}{Body}", guidPrefix, bodyType, body);
			}
			catch (Exception e)
			{
				logger.LogDebug("{Guid}Unable to get body for debug logging: {Message}", guidPrefix, e.Message);
			}
		}
	}
}
