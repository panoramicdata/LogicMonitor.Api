namespace LogicMonitor.Api.Extensions;

public static partial class LogItemExtensions
{
	private static T? GetGroupValueAsStructOrNull<T>(Match match, string groupName) where T : struct
	{
		if (!match.Groups[groupName].Success)
		{
			return default;
		}

		var stringValue = match.Groups[groupName].Value;

		if (stringValue == "NA")
		{
			return default;
		}

		if (typeof(T) == typeof(int))
		{
			var result = int.Parse(stringValue, CultureInfo.InvariantCulture);
			return result is T t1 ? t1 : default;
		}

		if (typeof(T) == typeof(long))
		{
			var result = long.Parse(stringValue, CultureInfo.InvariantCulture);
			return result is T t2 ? t2 : default;
		}

		return typeof(T) == typeof(bool)
			? bool.TryParse(stringValue, out var boolValue) ? boolValue is T t3 ? t3 : default : (T?)default
			: throw new NotSupportedException($"Type {typeof(T)} is not supported");
	}

	private static T? GetGroupValueAsTypeOrNull<T>(Match match, string groupName) where T : class
	{
		if (!match.Groups[groupName].Success)
		{
			return default;
		}

		var stringValue = match.Groups[groupName].Value;

		return typeof(T) == typeof(string)
			? stringValue is T t ? t : default
			: typeof(T) == typeof(List<string>)
			? stringValue.Split(',').ToList() is T t2 ? t2 : default
			: typeof(T) == typeof(List<int>)
			? stringValue.Split(',').Select(int.Parse).ToList() is T t3 ? t3 : default
			: throw new NotSupportedException($"Type {typeof(T)} is not supported");
	}

	private static (LogItemRegex? LogItemRegex, Match? Match) GetMatchFromDescription(string description)
	{
		if (TryGetGroupUpdateWithGetExtraMatch(description, out var getExtraMatch))
		{
			return (_groupActionLogItemRegex, getExtraMatch);
		}

		return _regexs
		  .Select(entry => (LogItemRegex: entry, Match: SafeMatch(entry.Regex, description)))
			.FirstOrDefault(entry => entry.Match.Success);
	}

	private static bool TryGetGroupUpdateWithGetExtraMatch(string description, out Match match)
	{
		match = Match.Empty;

		if (!description.Contains("getExtra: update value=", StringComparison.Ordinal))
		{
			return false;
		}

		var actionStartIndex = description.IndexOf("\"Action=", StringComparison.Ordinal);
		if (actionStartIndex < 0)
		{
			return false;
		}

		var actionSegment = description.Substring(actionStartIndex);
		match = SafeMatch(_groupActionRegex, actionSegment);
		return match.Success;
	}
}
