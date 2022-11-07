namespace LogicMonitor.Api;

internal static class Retry
{
	/// <summary>
	/// Retry
	/// </summary>
	/// <typeparam name="T">The type returned from the async method</typeparam>
	/// <param name="action">The action to perform</param>
	/// <param name="maxTryCount">The maximum number of attempts to be made (default is 10)</param>
	/// <param name="retryInterval">The period to wait between attempts (default is for immediate retry)</param>
	/// <param name="cancellationToken">The cancellation token</param>
	internal static async Task<T> Do<T>(
		Func<Task<T>> action,
		int maxTryCount,
		TimeSpan retryInterval,
		CancellationToken cancellationToken
		)
	{
		if (retryInterval < TimeSpan.Zero)
		{
			throw new ArgumentException("Retry interval must be positive.", nameof(retryInterval));
		}

		var exceptions = new List<Exception>();

		for (var tryIndex = 0; tryIndex < maxTryCount; tryIndex++)
		{
			try
			{
				if (tryIndex > 0)
				{
					await Task.Delay(retryInterval, cancellationToken).ConfigureAwait(false);
				}

				return await action().ConfigureAwait(false);
			}
			catch (Exception ex)
			{
				exceptions.Add(ex);
			}
		}

		throw new AggregateException(exceptions);
	}
}
