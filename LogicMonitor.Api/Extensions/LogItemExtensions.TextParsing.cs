namespace LogicMonitor.Api.Extensions;

public static partial class LogItemExtensions
{
	private static int SkipIdListCharacters(ReadOnlySpan<char> text, int position)
	{
		while (position < text.Length && (char.IsAsciiDigit(text[position]) || text[position] == ','))
		{
			position++;
		}

		return position;
	}

	/// <summary>
	/// Parses a non-empty comma-separated list of ids. An empty entry fails, where the regex path's
	/// int.Parse would throw.
	/// </summary>
	private static bool TryParseIdList(ReadOnlySpan<char> text, [NotNullWhen(true)] out List<int>? ids)
	{
		ids = null;
		if (text.IsEmpty)
		{
			return false;
		}

		var result = new List<int>();
		foreach (var range in text.Split(','))
		{
			if (!int.TryParse(text[range], NumberStyles.None, CultureInfo.InvariantCulture, out var id))
			{
				return false;
			}

			result.Add(id);
		}

		ids = result;
		return true;
	}

	private static bool TryReadNumber(ReadOnlySpan<char> text, ref int position, out int value)
	{
		var end = position;
		while (end < text.Length && char.IsAsciiDigit(text[end]))
		{
			end++;
		}

		if (!int.TryParse(text[position..end], NumberStyles.None, CultureInfo.InvariantCulture, out value))
		{
			return false;
		}

		position = end;
		return true;
	}

	private static bool TryReadLiteral(ReadOnlySpan<char> text, ref int position, string literal)
	{
		if (!text[position..].StartsWith(literal, StringComparison.Ordinal))
		{
			return false;
		}

		position += literal.Length;
		return true;
	}

	/// <summary>
	/// Finds <paramref name="value"/> starting at or after <paramref name="start"/> and ending at or
	/// before <paramref name="limit"/>, returning its index in <paramref name="text"/> or -1.
	/// </summary>
	private static int IndexOf(ReadOnlySpan<char> text, string value, int start, int limit)
	{
		if (start > limit)
		{
			return -1;
		}

		var index = text[start..limit].IndexOf(value, StringComparison.Ordinal);
		return index < 0 ? -1 : start + index;
	}

	/// <summary>
	/// Splits description[start..end] on commas, as the regex path's string.Split(',') does, without
	/// first copying the (potentially very large) list out of the description.
	/// </summary>
	private static List<string> SplitOnCommas(string description, int start, int end)
	{
		var result = new List<string>();
		while (true)
		{
			var comma = description.IndexOf(',', start, end - start);
			if (comma < 0)
			{
				result.Add(description[start..end]);
				return result;
			}

			result.Add(description[start..comma]);
			start = comma + 1;
		}
	}
}
