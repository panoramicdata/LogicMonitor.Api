namespace LogicMonitor.Api.Logs;

/// <summary>
/// A field panel item of a saved log query
/// </summary>
[DataContract]
public class LogQueryFieldPanelItem
{
	/// <summary>
	/// The column size
	/// </summary>
	[DataMember(Name = "columnSize")]
	public int ColumnSize { get; set; }

	/// <summary>
	/// The field
	/// </summary>
	[DataMember(Name = "field")]
	public string Field { get; set; } = string.Empty;

	/// <summary>
	/// The is visible
	/// </summary>
	[DataMember(Name = "isVisible")]
	public bool IsVisible { get; set; }
}
