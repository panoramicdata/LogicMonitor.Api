namespace LogicMonitor.Api.Dashboards;

/// <summary>
/// A Text widget
/// </summary>
[DataContract]
public class TextWidget : Widget, IWidget
{
	/// <summary>
	/// The Html.  Maximum length 409,600 characters.
	/// </summary>
	[DataMember(Name = "content")]
	public string Html { get; set; } = string.Empty;

	/// <summary>
	///     The display settings
	/// </summary>
	[DataMember(Name = "displaySettings")]
	public DisplaySettings DisplaySettings { get; set; } = new();
}
