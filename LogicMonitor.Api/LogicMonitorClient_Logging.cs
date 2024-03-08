namespace LogicMonitor.Api;

/// <summary>
///     LogItems
/// </summary>
public partial class LogicMonitorClient
{
	/// <summary>
	///     Logs multiple items
	/// </summary>
	/// <param name="writeLogRequests">A list of log write requests</param>
	/// <param name="cancellationToken"></param>
	/// <returns>The response</returns>
	public Task<WriteLogResponse> WriteLogAsync(
		IEnumerable<WriteLogRequest> writeLogRequests,
		CancellationToken cancellationToken)
		=> PostAsync<IEnumerable<WriteLogRequest>, WriteLogResponse>(writeLogRequests, "log/ingest", cancellationToken);

	/// <summary>
	///     Logs a single writeLogRequest
	/// </summary>
	/// <param name="writeLogRequest">A single log write requests</param>
	/// <param name="cancellationToken"></param>
	/// <returns>The response</returns>
	public Task<WriteLogResponse> WriteLogAsync(
		WriteLogRequest writeLogRequest,
		CancellationToken cancellationToken)
		=> WriteLogAsync(new[] { writeLogRequest }, cancellationToken);

	/// <summary>
	///     Logs a single writeLogRequest
	/// </summary>
	/// <param name="writeLogLevel"></param>
	/// <param name="deviceId">The device id</param>
	/// <param name="message">The message</param>
	/// <param name="cancellationToken"></param>
	/// <returns>The response</returns>
	public Task<WriteLogResponse> WriteLogAsync(
		WriteLogLevel writeLogLevel,
		int deviceId,
		string message,
		CancellationToken cancellationToken)
		=> WriteLogAsync(new[] { new WriteLogRequest(writeLogLevel, deviceId, message) }, cancellationToken);

	/// <summary>
	///     Logs a single writeLogRequest
	/// </summary>
	/// <param name="writeLogLevel"></param>
	/// <param name="deviceDisplayName">The device display name</param>
	/// <param name="message">The message</param>
	/// <param name="cancellationToken"></param>
	/// <returns>The response</returns>
	public Task<WriteLogResponse> WriteLogAsync(
		WriteLogLevel writeLogLevel,
		string deviceDisplayName,
		string message,
		CancellationToken cancellationToken)
		=> WriteLogAsync(new[] { new WriteLogRequest(writeLogLevel, deviceDisplayName, message) }, cancellationToken);

	/// <summary>
	///     Logs a single writeLogRequest
	/// </summary>
	/// <param name="writeLogLevel"></param>
	/// <param name="customPropertyName">The custom property name</param>
	/// <param name="customPropertyValue">
	///		The custom property value.
	///		This must be unique across for the specified custom property value for the entire LM portal.
	/// </param>
	/// <param name="message">The message</param>
	/// <param name="cancellationToken"></param>
	/// <returns>The response</returns>
	public Task<WriteLogResponse> WriteLogAsync(
		WriteLogLevel writeLogLevel,
		string customPropertyName,
		string customPropertyValue,
		string message,
		CancellationToken cancellationToken)
		=> WriteLogAsync(new[] { new WriteLogRequest(writeLogLevel, customPropertyName, customPropertyValue, message) }, cancellationToken);

	/// <summary>
	///     Logs a single writeLogRequest
	/// </summary>
	/// <param name="writeLogLevel"></param>
	/// <param name="propertyDictionary">
	///		The property dictionary, the combination of which will be used to identify the resource.
	///		This must be unique across the specified custom property values for the entire LM portal.
	/// </param>
	/// <param name="message">The message</param>
	/// <param name="cancellationToken"></param>
	/// <returns>The response</returns>
	public Task<WriteLogResponse> WriteLogAsync(
		WriteLogLevel writeLogLevel,
		Dictionary<string, string> propertyDictionary,
		string message,
		CancellationToken cancellationToken)
		=> WriteLogAsync(new[] { new WriteLogRequest(writeLogLevel, propertyDictionary, message) }, cancellationToken);
}
