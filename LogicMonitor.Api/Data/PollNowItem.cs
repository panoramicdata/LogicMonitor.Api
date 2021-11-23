namespace LogicMonitor.Api.Data;

/// <summary>
/// A poll now item
/// </summary>
[DataContract]
public class PollNowItem
{
	/// <summary>
	/// The name
	/// </summary>
	[DataMember(Name = "name")]
	public string Name { get; set; }

	/// <summary>
	/// The value
	/// </summary>
	[DataMember(Name = "value")]
	public string Value { get; set; }

	/// <inheritdoc />
	public override string ToString() => $"{Name}: {Value}";
}
