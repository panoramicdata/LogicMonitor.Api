using System;

namespace LogicMonitor.Api;

internal class CachedItem<T>
{
	public T Item { get; set; }

	public DateTime ExpiryTimeUtc { get; set; }
}
