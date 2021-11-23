namespace LogicMonitor.Api.Dashboards;

/// <summary>
///     An HTML widget
/// </summary>
[DataContract]
public class HtmlWidget : Widget
{
	/// <summary>
	///     Constructor with default parameters
	/// </summary>
	public HtmlWidget()
	{
		Type = "html";
	}

	/// <summary>
	///     The HTML widget resources
	/// </summary>
	[DataMember(Name = "resources")]
	public List<HtmlWidgetResource> HtmlWidgetResources { get; set; }

	/// <summary>
	///     Whether this is a custom Html widget
	/// </summary>
	[DataMember(Name = "isCustom")]
	public bool IsCustom { get; set; }

	/// <summary>
	///     The display settings
	/// </summary>
	[DataMember(Name = "displaySettings")]
	public object DisplaySettings { get; set; }
}
