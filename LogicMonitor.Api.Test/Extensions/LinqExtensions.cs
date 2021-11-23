using System;
using System.Collections.Generic;
using System.Linq;

namespace LogicMonitor.Api.Test.Extensions;

internal static class LinqExtensions
{
	internal static IEnumerable<TSource> DistinctBy<TSource, TKey>(
		this IEnumerable<TSource> source,
		Func<TSource, TKey> keySelector
		)
	{
		var seenKeys = new HashSet<TKey>();
		return source.Where(e => seenKeys.Add(keySelector(e)));
	}
}
