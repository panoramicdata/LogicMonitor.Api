using System.Runtime.Serialization;

namespace LogicMonitor.Api.Dashboards;

/// <summary>
/// A flash widget
/// </summary>
[DataContract]
public class FlashWidget : Widget
{
	/// <summary>
	/// The height
	/// </summary>
	[DataMember(Name = "height")]
	public int Height { get; set; }

	/// <summary>
	/// The URL
	/// </summary>
	[DataMember(Name = "url")]
	public string Url { get; set; }

	/// <summary>
	/// The display settings
	/// </summary>
	[DataMember(Name = "displaySettings")]
	public object DisplaySettings { get; set; }
}
