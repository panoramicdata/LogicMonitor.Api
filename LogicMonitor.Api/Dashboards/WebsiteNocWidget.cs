using System.Collections.Generic;
using System.Runtime.Serialization;

namespace LogicMonitor.Api.Dashboards
{
	/// <summary>
	/// A Website NOC widget
	/// </summary>
	[DataContract]
	public class WebsiteNocWidget : Widget
	{
		/// <summary>
		/// Items
		/// </summary>
		[DataMember(Name = "items")]
		public List<WebsiteNocWidgetItem> Items { get; set; }
	}
}