using System.Diagnostics;
using System.Runtime.Serialization;

namespace LogicMonitor.Api.Dashboards
{
	/// <summary>
	///     An Html Widget HtmlWidgetResource
	/// </summary>
	[DebuggerDisplay("{Type}:{Url}")]
	[DataContract]
	public class HtmlWidgetResource
	{
		/// <summary>
		///     The resource type
		/// </summary>
		[DataMember(Name = "type")]
		public string Type { get; set; }

		/// <summary>
		///     The URL. If type = html this should be a url. If type = iframe this should be an iframe.
		/// </summary>
		[DataMember(Name = "URL")]
		public string Url { get; set; }
	}
}