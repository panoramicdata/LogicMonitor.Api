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
	public int Position { get; set; }

	/// <summary>
	///     rounding
	/// </summary>
	[DataMember(Name = "rounding")]
	public int Rounding { get; set; }

	/// <summary>
	///     bottomLabel
	/// </summary>
	[DataMember(Name = "bottomLabel")]
	public string BottomLabel { get; set; } = string.Empty;

	/// <summary>
	///     value
	/// </summary>
	[DataMember(Name = "value")]
	public string Value { get; set; } = string.Empty;

	/// <summary>
	///     rightLabel
	/// </summary>
	[DataMember(Name = "rightLabel")]
	public string RightLabel { get; set; } = string.Empty;

	/// <summary>
	///     colorLevel
	/// </summary>
	[DataMember(Name = "colorLevel")]
	public int ColorLevel { get; set; }

	/// <summary>
	///     errorMessage
	/// </summary>
	[DataMember(Name = "errorMessage")]
	public string ErrorMessage { get; set; } = string.Empty;

	/// <summary>
	///     useCommaSeparators
	/// </summary>
	[DataMember(Name = "useCommaSeparators")]
	public string UseCommaSeparators { get; set; } = string.Empty;
}
