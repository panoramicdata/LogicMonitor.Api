using System.Runtime.Serialization;

namespace LogicMonitor.Api.Dashboards
{
	/// <summary>
	///     Text Widget
	/// </summary>
	public class TextWidgetCreationDto : WidgetCreationDto<TextWidget>
	{
		/// <summary>
		///     The text HTML content
		/// </summary>
		[DataMember(Name = "content")]
		public string Content { get; set; }

		/// <inheritdoc />
		public override string Type { get; } = "text";
	}
}