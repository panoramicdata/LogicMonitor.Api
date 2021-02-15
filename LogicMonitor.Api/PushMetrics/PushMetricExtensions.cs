using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace LogicMonitor.Api.PushMetrics
{
	/// <summary>
	/// Push Metric extensions
	/// </summary>
	public static class PushMetricExtensions
	{
		/// <summary>
		/// Convert a well-typed dictionary into whatever it is that LogicMonitor wants
		/// </summary>
		/// <param name="keyValuePairs"></param>
		public static Dictionary<string, string> ToLogicMonitorDictionary(
			this Dictionary<DateTimeOffset, double> keyValuePairs)
			=> keyValuePairs.ToDictionary(
				kvp => kvp.Key.ToUnixTimeSeconds().ToString(CultureInfo.InvariantCulture),
				kvp => kvp.Value.ToString(CultureInfo.InvariantCulture)
				);

		/// <summary>
		/// Convert a well-typed dictionary into whatever it is that LogicMonitor wants
		/// </summary>
		/// <param name="keyValuePairs"></param>
		public static Dictionary<string, string> ToLogicMonitorDictionary(
			this Dictionary<DateTimeOffset, float> keyValuePairs)
			=> keyValuePairs.ToDictionary(
				kvp => kvp.Key.ToUnixTimeSeconds().ToString(CultureInfo.InvariantCulture),
				kvp => kvp.Value.ToString(CultureInfo.InvariantCulture)
				);

		/// <summary>
		/// Convert a well-typed dictionary into whatever it is that LogicMonitor wants
		/// </summary>
		/// <param name="keyValuePairs"></param>
		public static Dictionary<string, string> ToLogicMonitorDictionary(
			this Dictionary<DateTimeOffset, int> keyValuePairs)
			=> keyValuePairs.ToDictionary(
				kvp => kvp.Key.ToUnixTimeSeconds().ToString(CultureInfo.InvariantCulture),
				kvp => kvp.Value.ToString(CultureInfo.InvariantCulture)
				);
	}
}
