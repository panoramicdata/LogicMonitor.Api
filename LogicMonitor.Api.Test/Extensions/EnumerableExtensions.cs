namespace LogicMonitor.Api.Test.Extensions;

public static class EnumerableExtensions
{
	public static bool HasDuplicates<T>(this IEnumerable<T> subjects) => HasDuplicates(subjects, EqualityComparer<T>.Default);

	private static bool HasDuplicates<T>(this IEnumerable<T> subjects, IEqualityComparer<T> comparer)
	{
		if (subjects is null)
		{
			throw new ArgumentNullException(nameof(subjects));
		}

		if (comparer is null)
		{
			throw new ArgumentNullException(nameof(comparer));
		}

		var set = new HashSet<T>(comparer);
		return subjects.Any(s => !set.Add(s));
	}
}
