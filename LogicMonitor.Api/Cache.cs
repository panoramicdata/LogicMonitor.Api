using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using System;
using System.Collections.Concurrent;
using System.Linq;

namespace LogicMonitor.Api;

internal class Cache<TIndex, TValue>
{
	private readonly ConcurrentDictionary<TIndex, CachedItem<TValue>> _cache = new();
	private TimeSpan _maxAge;
	private readonly object _ageLock = new();
	private DateTime _lastAged;
	private readonly ILogger _logger;

	internal TimeSpan MaxAge
	{
		get => _maxAge;
		set
		{
			_logger.LogTrace("CACHE MAX AGE SET");
			if (value < TimeSpan.Zero)
			{
				throw new ArgumentOutOfRangeException("Value must be greater than zero");
			}
			_maxAge = value;

			// Force age
			Age(true);
		}
	}

	public Cache(
		TimeSpan maxAge,
		ILogger logger)
	{
		_logger = logger ?? new NullLogger<Cache<TIndex, TValue>>();
		MaxAge = maxAge;
	}

	public void Clear()
		=> _cache.Clear();

	/// <summary>
	/// Try to get a value from the cache
	/// </summary>
	/// <param name="index"></param>
	/// <param name="value"></param>
	/// <returns>True if successful</returns>
	internal bool TryGetValue(TIndex index, out TValue value)
	{
		// If we have an item and it has not expired, return it
		if (_cache.TryGetValue(index, out var cachedItem) && cachedItem.ExpiryTimeUtc > DateTime.UtcNow)
		{
			_logger.LogTrace("CACHE HIT");
			value = cachedItem.Item;
			return true;
		}

		_logger.LogTrace("CACHE MISS");
		value = default;
		return false;
	}

	internal void AddOrUpdate(TIndex index, TValue value)
	{
		_logger.LogTrace("CACHE ADD/UPDATE");
		var cachedItem = new CachedItem<TValue>
		{
			ExpiryTimeUtc = DateTime.UtcNow + MaxAge,
			Item = value
		};
		_cache.AddOrUpdate(index, cachedItem, (_, __) => cachedItem);
	}

	public void Age(bool force = false)
	{
		// Have we aged recently?
		if (!force && DateTime.UtcNow - _lastAged < TimeSpan.FromMinutes(1))
		{
			// Yes.  Don't do again for a bit.
			return;
		}

		_logger.LogTrace("CACHE AGE START");
		lock (_ageLock)
		{
			// Double lock
			if (DateTime.UtcNow - _lastAged < TimeSpan.FromMinutes(1))
			{
				return;
			}

			// Delete older items
			var now = DateTime.UtcNow;
			foreach (var deletableItemKey in _cache.Keys.Where(k => _cache.TryGetValue(k, out var value) && value.ExpiryTimeUtc < now).ToList())
			{
				_cache.TryRemove(deletableItemKey, out var _);
			}

			_lastAged = DateTime.UtcNow;
		}
		_logger.LogTrace("CACHE AGE COMPLETE");
	}
}
