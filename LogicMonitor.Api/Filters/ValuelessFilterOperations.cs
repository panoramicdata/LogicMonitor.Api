namespace LogicMonitor.Api.Filters;

internal static class ValuelessFilterOperations
{
	private static readonly HashSet<string> _operations =
		new(StringComparer.Ordinal) { ":::empty", ":::null", "!::empty", "!::null" };

	public static bool Contains(string operation) => _operations.Contains(operation);
}
