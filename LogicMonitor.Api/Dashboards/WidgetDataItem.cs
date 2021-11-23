using System.Runtime.Serialization;

namespace LogicMonitor.Api.Dashboards;

/// <summary>
///     Widget data item
/// </summary>
[DataContract]
public class WidgetDataItem
{
	/// <summary>
	///     position
	/// </summary>
	[DataMember(Name = "position")]
	public int? Position { get; set; }

	/// <summary>
	///     rounding
	/// </summary>
	[DataMember(Name = "rounding")]
	public int? Rounding { get; set; }

	/// <summary>
	///     bottomLabel
	/// </summary>
	[DataMember(Name = "bottomLabel")]
	public string BottomLabel { get; set; }

	/// <summary>
	///     value
	/// </summary>
	[DataMember(Name = "value")]
	public string Value { get; set; }

	/// <summary>
	///     rightLabel
	/// </summary>
	[DataMember(Name = "rightLabel")]
	public string RightLabel { get; set; }

	/// <summary>
	///     colorLevel
	/// </summary>
	[DataMember(Name = "colorLevel")]
	public int? ColorLevel { get; set; }

	/// <summary>
	///     errorMessage
	/// </summary>
	[DataMember(Name = "errorMessage")]
	public string ErrorMessage { get; set; }

	/// <summary>
	///     useCommaSeparators
	/// </summary>
	[DataMember(Name = "useCommaSeparators")]
	public string UseCommaSeparators { get; set; }
}
