namespace LogicMonitor.Api.Alerts;

/// <summary>
/// A column specification
/// </summary>
[DataContract]
public class ColumnSpecification
{
	/// <summary>
	/// Whether visible
	/// </summary>
	[DataMember(Name = "visible")]
	public bool IsVisible { get; set; }

	/// <summary>
	/// The label
	/// </summary>
	[DataMember(Name = "columnLabel")]
	public string Label { get; set; }

	/// <summary>
	/// The key
	/// </summary>
	[DataMember(Name = "columnKey")]
	public string Key { get; set; }
}
