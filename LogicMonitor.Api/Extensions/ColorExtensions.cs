namespace LogicMonitor.Api.Extensions;

/// <summary>
/// Color Extensions
/// </summary>
internal static class ColorExtensions
{
	/// <summary>
	/// Convert a color to the HTML equivalent e.g. #FF0000
	/// </summary>
	/// <param name="color"></param>
	public static string ToHtml(this Color color) => "#" +
			color.R.ToString("X2", CultureInfo.InvariantCulture) +
			color.G.ToString("X2", CultureInfo.InvariantCulture) +
			color.B.ToString("X2", CultureInfo.InvariantCulture);
}
