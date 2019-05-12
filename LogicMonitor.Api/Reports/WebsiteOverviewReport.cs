using System.Collections.Generic;
using System.Runtime.Serialization;

namespace LogicMonitor.Api.Reports
{
	/// <summary>
	/// An Website Service Overview report
	/// </summary>
	[DataContract]
	public class WebsiteOverviewReport : DateRangeReport
	{
		/// <summary>
		/// The items
		/// </summary>
		[DataMember(Name = "items")]
		public string Items { get; set; }

		/// <summary>
		/// The items type
		/// </summary>
		[DataMember(Name = "itemsType")]
		public string ItemsType { get; set; }

		/// <summary>
		/// The displayType
		/// </summary>
		[DataMember(Name = "displayType")]
		public int DisplayType { get; set; }

		/// <summary>
		/// The filter
		/// </summary>
		[DataMember(Name = "includeTypes")]
		public List<object> IncludeTypes { get; set; }

		/// <summary>
		/// Whether to exclude SDT
		/// </summary>
		[DataMember(Name = "excludeSDT")]
		public bool ExcludeSDT { get; set; }

		/// <summary>
		/// Whether to exclude services with 100% availability
		/// </summary>
		[DataMember(Name = "exclude100Availability")]
		public bool Exclude100Availability { get; set; }
	}
}