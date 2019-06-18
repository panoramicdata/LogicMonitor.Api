using System.Collections.Generic;
using System.Runtime.Serialization;

namespace LogicMonitor.Api.Dashboards
{
	/// <summary>
	/// Widget data
	/// </summary>
	[DataContract]
	public class WidgetData
	{
		/// <summary>
		///     Type
		/// </summary>
		[DataMember(Name = "type")]
		public string Type { get; set; }

		/// <summary>
		///     Title
		/// </summary>
		[DataMember(Name = "title")]
		public string Title { get; set; }

		/// <summary>
		///     Availability
		/// </summary>
		[DataMember(Name = "availability")]
		public double? Availability { get; set; }

		/// <summary>
		///     Color level
		/// </summary>
		[DataMember(Name = "colorLevel")]
		public int? ColorLevel { get; set; }

		/// <summary>
		///     Data
		/// </summary>
		[DataMember(Name = "data")]
		public List<WidgetDataItem> Data { get; set; }

		/// <summary>
		///     Result list (used by SLA Multi widget)
		/// </summary>
		[DataMember(Name = "resultList")]
		public List<WidgetDataItem> ResultList { get; set; }
	}
}