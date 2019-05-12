using System.Runtime.Serialization;

namespace LogicMonitor.Api.Dashboards
{
	/// <summary>
	/// A pie chart widget
	/// </summary>
	[DataContract]
	public class PieChartWidget : Widget
	{
		/// <summary>
		/// The pie chart info
		/// </summary>
		[DataMember(Name = "pieChartInfo")]
		public PieChartWidgetInfo Info { get; set; }

		/// <summary>
		///     The display settings
		/// </summary>
		[DataMember(Name = "displaySettings")]
		public object DisplaySettings { get; set; }
	}
}