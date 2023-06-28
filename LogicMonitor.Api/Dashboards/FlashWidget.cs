namespace LogicMonitor.Api.Dashboards;

/// <summary>
/// A flash widget
/// </summary>
[DataContract]
public class FlashWidget : Widget, IWidget
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
	public string Url { get; set; } = string.Empty;

	/// <summary>
	/// The display settings
	/// </summary>
	[DataMember(Name = "displaySettings")]
	public DisplaySettings DisplaySettings { get; set; } = new();
}
