namespace LogicMonitor.Api.Dashboards;

/// <summary>
/// An AlertCreationDtoColumn
/// </summary>
[DataContract]
public class AlertCreationDtoColumn
{
	/// <summary>
	/// The column key
	/// </summary>
	[DataMember(Name = "columnKey")]
	public string ColumnKey { get; set; } = string.Empty;

	/// <summary>
	/// The column label
	/// </summary>
	[DataMember(Name = "columnLabel")]
	public string ColumnLabel { get; set; } = string.Empty;

	/// <summary>
	/// Visibility
	/// </summary>
	[DataMember(Name = "visible")]
	public bool IsVisible { get; set; }

	/// <summary>
	/// The column width in pixels
	/// </summary>
	[DataMember(Name = "columnSize")]
	public int? ColumnWidthPixels { get; set; }

	/// <summary>
	/// Whether the column is a custom column
	/// </summary>
	[DataMember(Name = "isDynamic")]
	public bool? IsCustom { get; set; }
}
