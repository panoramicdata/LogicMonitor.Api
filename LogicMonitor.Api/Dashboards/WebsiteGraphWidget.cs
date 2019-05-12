using System.Runtime.Serialization;

namespace LogicMonitor.Api.Dashboards
{
	/// <summary>
	/// An Website Graph widget
	/// </summary>
	[DataContract]
	public class WebsiteGraphWidget : GraphWidget
	{
		/// <summary>
		/// The Website checkpoint id
		/// </summary>
		[DataMember(Name = "checkpointId")]
		public int WebsiteCheckPointId { get; set; }

		/// <summary>
		/// The graph name
		/// </summary>
		[DataMember(Name = "graph")]
		public string GraphName { get; set; }

		/// <summary>
		/// The website name
		/// </summary>
		[DataMember(Name = "websiteName")]
		public string WebsiteName { get; set; }

		/// <summary>
		/// The geographic information
		/// </summary>
		[DataMember(Name = "geoInfo")]
		public string GeographicInformation { get; set; }

		/// <summary>
		///     The display settings
		/// </summary>
		[DataMember(Name = "displaySettings")]
		public object DisplaySettings { get; set; }
	}
}