namespace LogicMonitor.Api.Data;

/// <summary>
/// A poll now inner result
/// </summary>
[DataContract]
public class PollNowInnerResult
{
	/// <summary>
	/// The Id
	/// </summary>
	[DataMember(Name = "id")]
	public int Id { get; set; }

	/// <summary>
	/// The Name
	/// </summary>
	[DataMember(Name = "name")]
	public string Name { get; set; } = string.Empty;

	/// <summary>
	/// The note
	/// </summary>
	[DataMember(Name = "note")]
	public string Note { get; set; } = string.Empty;

	/// <summary>
	/// The Value
	/// </summary>
	[DataMember(Name = "value")]
	public string Value { get; set; } = string.Empty;

	/// <summary>
	/// The Error message
	/// </summary>
	[DataMember(Name = "errMsg")]
	public string ErrMsg { get; set; } = string.Empty;

	/// <inheritdoc />
	public override string ToString() => $"{Name}: {Value} ({ErrMsg})";
}
