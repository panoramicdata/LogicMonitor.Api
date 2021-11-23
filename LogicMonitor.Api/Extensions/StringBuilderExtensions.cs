namespace LogicMonitor.Api.Extensions;

internal static class StringBuilderExtensions
{
	public static void AppendIfNotNull(this StringBuilder stringBuilder, string parameter, object value, string @operator = "=") => AppendIfNotNullInternal(stringBuilder, parameter, value, @operator, "&", false);

	private static void AppendIfNotNullInternal(StringBuilder stringBuilder, string parameter, object value, string @operator, string delimiter, bool encodeResult)
	{
		// Null function if the object is null
		if (value == null)
		{
			return;
		}
		// Object is not null so we're going to do something with it

		// If the StringBuilder already contains an entry, use delimiter to concatenate
		stringBuilder.Append($"{(stringBuilder.Length == 0 ? string.Empty : delimiter)}{parameter.LowerCaseFirst()}{@operator}{(encodeResult ? value.UriEscape() : value)}");
	}

	internal static string UriEscape(this object value)
	{
		// If it's a list, return each item UrlEscaped, separated by "|"
		if (value is IEnumerable<string> list)
		{
			return string.Join("|", list.Select(item => item.UriEscape()));
		}

		return Uri
				.EscapeUriString(value.ToString())
				.Replace("&", "%26")
				.Replace(":", "%3A")
				.Replace("/", "%2F")
				.Replace(",", "%2C")
			;
	}
}
