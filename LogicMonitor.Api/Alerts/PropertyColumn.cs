namespace LogicMonitor.Api.Alerts;

/// <summary>
/// Property column
/// </summary>
[DataContract]
public class PropertyColumn
{
	/// <summary>
	/// Column index
	/// </summary>
	[DataMember(Name = "index")]
	public int Index { get; set; }

	/// <summary>
	/// Column name
	/// </summary>
	[DataMember(Name = "columnName")]
	public string ColumnName { get; set; } = string.Empty;
}

