namespace LogicMonitor.Api.Dashboards;

/// <summary>
///     Text Widget
/// </summary>
public class TextWidgetCreationDto : WidgetCreationDto<TextWidget>
{
	/// <summary>
	///     The text HTML content
	/// </summary>
	[DataMember(Name = "content")]
	public string Content { get; set; } = string.Empty;

	/// <inheritdoc />
	public override string Type { get; } = "text";
}
