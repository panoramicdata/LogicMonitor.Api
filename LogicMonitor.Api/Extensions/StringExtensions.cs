using System.Text.RegularExpressions;

namespace LogicMonitor.Api.Extensions
{
	internal static class StringExtensions
	{
		public static string LowerCaseFirst(this string value)
			=> value.Length == 0
				? value
				: value.Substring(0, 1).ToLowerInvariant() + value.Substring(1);

		public static string UpperCaseFirst(this string value)
			=> value.Length == 0
				? value
				: value.Substring(0, 1).ToUpperInvariant() + value.Substring(1);

		public static string DeCamelCase(this string value)
			=> Regex.Replace(value, "(?<a>(?<!^)((?:[A-Z][a-z])|(?:(?<!^[A-Z]+)[A-Z0-9]+(?:(?=[A-Z][a-z])|$))|(?:[0-9]+)))", " ${a}");

		public static string EscapeProblematicCharacters(this string value)
			=> value.Replace("\\", "\\\\").Replace("(", @"\(").Replace(")", @"\)").Replace('[', '*').Replace(']', '*');

		public static string EscapeSlashes(this string value)
			=> value.Replace("\\", "\\\\");

		/// <summary>
		/// Convert the + character into %2B. This is required to get device display names
		/// that include "+". There may be other characters that require this also!
		/// </summary>
		/// <param name="value"></param>
		/// <returns></returns>
		public static string EscapePlusCharacter(this string value)
			=> value.Replace("+", "%2B");
	}
}