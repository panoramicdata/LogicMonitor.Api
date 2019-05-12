using LogicMonitor.Api.Converters;
using Newtonsoft.Json;

namespace LogicMonitor.Api.Dashboards
{
	/// <summary>
	///    Map Point
	/// </summary>
	[JsonConverter(typeof(MapPointConverter))]
	public class MapPoint
	{
		/// <summary>
		///    The type
		/// </summary>
		public string Type { get; set; }
	}
}