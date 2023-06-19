namespace LogicMonitor.Api.LogicModules;

/// <summary>
/// A collector attribute counter
/// </summary>
[DataContract]
public class CollectorAttributeCounter
{
	/// <summary>
	/// comment
	/// </summary>
	[DataMember(Name = "comment")]
	public object Comment { get; set; } = new();

	/// <summary>
	/// name
	/// </summary>
	[DataMember(Name = "name")]
	public string Name { get; set; } = string.Empty;

	/// <summary>
	/// value
	/// </summary>
	[DataMember(Name = "value")]
	public object Value { get; set; } = new();
}
