using System.Security.Cryptography;

namespace LogicMonitor.Api;

/// <summary>
///     The main client for querying the portal.
/// </summary>
public partial class LogicMonitorClient
{

	/// <summary>
	/// Get the result of a Collector debug command using sessionId
	/// </summary>
	/// <param name="collectorId">The ID of the collector on which the command was executed</param>
	/// <param name="sessionId">The request ID from the ExecuteDebugCommandResponse</param>
	/// <param name="cancellationToken">The optional cancellation token</param>
	public Task<ExecuteDebugCommandResponse> GetDebugCommandResultAsync(int collectorId, int sessionId, CancellationToken cancellationToken)
	=> GetBySubUrlAsync<ExecuteDebugCommandResponse>($"debug/{sessionId}?collectorId={collectorId}&_={DateTime.UtcNow.Ticks}", cancellationToken);

	/// <summary>
	///     Waits for a Debug command response
	/// </summary>
	/// <param name="collectorId">The ID of the collector on which the command was executed</param>
	/// <param name="commandText">The command to execute</param>
	/// <param name="timeoutMs">The maximum amount of time to wait (default 10000 ms)</param>
	/// <param name="sleepIntervalMs">The sleep interval between attempts to retrieve the response (default 500ms)</param>
	/// <param name="cancellationToken">The cancellation token</param>
	public async Task<ExecuteDebugCommandResponse?> ExecuteDebugCommandAndWaitForResultAsync(
		int collectorId,
		string commandText,
		int timeoutMs,
		int sleepIntervalMs,
		CancellationToken cancellationToken)
	{
		var executeDebugCommandResponse = await ExecuteDebugCommandAsync(collectorId, commandText, cancellationToken).ConfigureAwait(false);

		var stopwatch = Stopwatch.StartNew();
		ExecuteDebugCommandResponse? debugCommandResult = null;
		while (stopwatch.ElapsedMilliseconds < timeoutMs)
		{
			try
			{
				debugCommandResult = await GetDebugCommandResultAsync(collectorId, executeDebugCommandResponse.SessionId, cancellationToken).ConfigureAwait(false);
			}
			catch
			{
				// ignored
			}

			// Do we have a response?
			if (!string.IsNullOrWhiteSpace(debugCommandResult?.Output))
			{
				break;
			}

			await Task.Delay(sleepIntervalMs, cancellationToken).ConfigureAwait(false);
		}

		return debugCommandResult;
	}

	private async Task SetCustomPropertyAsync(
		int id,
		string name,
		string? value,
		SetPropertyMode mode,
		string subUrl,
		CancellationToken cancellationToken)
	{
		var propertiesSubUrl = $"{subUrl}/{id}/properties";
		var task = mode switch
		{
			SetPropertyMode.Automatic => SetCustomPropertyAutomaticAsync(propertiesSubUrl, name, value, cancellationToken),
			SetPropertyMode.Create => CreateCustomPropertyAsync(propertiesSubUrl, name, value, cancellationToken),
			SetPropertyMode.Update => UpdateCustomPropertyAsync(propertiesSubUrl, name, value, cancellationToken),
			SetPropertyMode.Delete => DeleteCustomPropertyAsync(propertiesSubUrl, name, value, cancellationToken),
			_ => throw new ArgumentOutOfRangeException(nameof(mode), mode, null)
		};
		await task.ConfigureAwait(false);
	}

	private async Task SetCustomPropertyAutomaticAsync(string propertiesSubUrl, string name, string? value, CancellationToken cancellationToken)
	{
		// Determine whether there is an existing property
		try
		{
			_ = await GetBySubUrlAsync<EntityProperty>($"{propertiesSubUrl}/{name}", cancellationToken).ConfigureAwait(false);

			// No exception thrown? It exists, so delete it when the new value is null, otherwise PUT the replacement
			if (value is null)
			{
				await DeleteAsync($"{propertiesSubUrl}/{name}", cancellationToken).ConfigureAwait(false);
			}
			else
			{
				await PutAsync($"{propertiesSubUrl}/{name}", new { value }, cancellationToken).ConfigureAwait(false);
			}
		}
		catch (Exception)
		{
			// It doesn't exist, so POST a new one (unless it's null, in which case nothing to do)
			if (value is not null)
			{
				_ = await PostAsync<EntityProperty, EntityProperty>(
					new EntityProperty { Name = name, Value = value },
					propertiesSubUrl,
					cancellationToken).ConfigureAwait(false);
			}
		}
	}

	private async Task CreateCustomPropertyAsync(string propertiesSubUrl, string name, string? value, CancellationToken cancellationToken)
	{
		if (value is null)
		{
			throw new InvalidOperationException("Value must not be set to null when creating the property.");
		}

		await PostAsync<EntityProperty, EntityProperty>(new EntityProperty { Name = name, Value = value }, propertiesSubUrl, cancellationToken).ConfigureAwait(false);
	}

	private async Task UpdateCustomPropertyAsync(string propertiesSubUrl, string name, string? value, CancellationToken cancellationToken)
	{
		if (value is null)
		{
			throw new InvalidOperationException("Value must not be set to null when updating the property.");
		}

		await PutAsync($"{propertiesSubUrl}/{name}", new { value }, cancellationToken).ConfigureAwait(false);
	}

	private async Task DeleteCustomPropertyAsync(string propertiesSubUrl, string name, string? value, CancellationToken cancellationToken)
	{
		if (value is not null)
		{
			throw new InvalidOperationException("Value must be set to null when deleting the value.");
		}

		await DeleteAsync($"{propertiesSubUrl}/{name}", cancellationToken).ConfigureAwait(false);
	}

	/// <summary>
	/// Returns true if the specified property on the class has the SantabaReadOnly attribute defined
	/// </summary>
	/// <param name="name">The name to match</param>
	/// <param name="logicMonitorClassType">The type of class to query</param>
	/// <returns>True if the property is read only</returns>
	public static bool IsPropertyReadOnly(string name, Type logicMonitorClassType)
		=> IsPropertyReadOnly(name, logicMonitorClassType, false);

	/// <summary>
	/// Returns true if the specified property on the class has the SantabaReadOnly attribute defined
	/// </summary>
	/// <param name="name">The name to match</param>
	/// <param name="logicMonitorClassType">The type of class to query</param>
	/// <param name="tryJsonNameFirst">If true, will use the DataMember/JSON property name to match before using property name</param>
	/// <returns>True if the property is read only</returns>
	public static bool IsPropertyReadOnly(string name, Type logicMonitorClassType, bool tryJsonNameFirst)
	{
		PropertyInfo? propertyInfo;
		if (tryJsonNameFirst)
		{
			// Try and find a property which matches the DataMemberAttributes name if it's set
			propertyInfo = logicMonitorClassType.GetProperties()
				.SingleOrDefault(p =>
					p.GetCustomAttributes(typeof(DataMemberAttribute), false)
					.Cast<DataMemberAttribute>()
					.SingleOrDefault(entity => entity.IsNameSetExplicitly)?
					.Name == name)
				// Also try and get the property by it's normal name
				?? logicMonitorClassType.GetProperty(name);
		}
		else
		{
			// Just a simple GetProperty
			propertyInfo = logicMonitorClassType.GetProperty(name);
		}

		return propertyInfo is null
			? throw new PropertyNotFoundException($"Could not find property on {logicMonitorClassType.Name} with name {name}.")
			: Attribute.IsDefined(propertyInfo, typeof(SantabaReadOnly));
	}

	/// <summary>
	/// Clone an ICloneableItem
	/// </summary>
	/// <typeparam name="T">The type of the item being cloned</typeparam>
	/// <param name="id">The id of the item being cloned</param>
	/// <param name="cloneRequest">The clone request</param>
	/// <param name="cancellationToken">An optional CancellationToken</param>
	public Task<T> CloneAsync<T>(int id, CloneRequest<T> cloneRequest, CancellationToken cancellationToken) where T : IHasEndpoint, ICloneableItem, new()
		=> PostAsync<CloneRequest<T>, T>(cloneRequest, $"{new T().Endpoint()}/{id}/clone", cancellationToken);

	/// <summary>
	/// Waits out a rate-limit backoff, reporting it at a level a caller can actually see.
	///
	/// Issue #38: the aggregate wait here is unbounded - the 429 loop retries until the portal relents -
	/// and it is silent by every other measure: no CPU, no open socket, no exception. Logged at Debug, a
	/// consumer running at Information could not distinguish a polite backoff from a hung call. The
	/// cumulative attempt and total are included because the tenth five-minute wait reads very
	/// differently from the first, and only this client is in a position to know which it is.
	/// </summary>
	/// <returns>The new cumulative wait for this request, in milliseconds.</returns>
	private async Task<long> WaitForRateLimitAsync(
		string prefix,
		string rateLimitInformation,
		int delayMs,
		int attempt,
		long previousTotalMs,
		CancellationToken cancellationToken)
	{
		var totalMs = previousTotalMs + delayMs;

		// Once more than a single maximum backoff has been spent waiting, this has stopped being routine.
		if (totalMs > 1000L * MaximumBackOffSeconds)
		{
			_logger.LogWarning(
				"{Prefix} Rate limiting hit (attempt {Attempt}, {TotalWaitedMs:N0}ms waited so far): {RateLimitInformation}, waiting {DelayMs:N0}ms",
				prefix,
				attempt,
				totalMs,
				rateLimitInformation,
				delayMs);
		}
		else
		{
			_logger.LogInformation(
				"{Prefix} Rate limiting hit (attempt {Attempt}, {TotalWaitedMs:N0}ms waited so far): {RateLimitInformation}, waiting {DelayMs:N0}ms",
				prefix,
				attempt,
				totalMs,
				rateLimitInformation,
				delayMs);
		}

		await Task.Delay(delayMs, cancellationToken).ConfigureAwait(false);

		return totalMs;
	}
}
