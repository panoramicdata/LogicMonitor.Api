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
	public string Name { get; set; }

	/// <summary>
	/// The note
	/// </summary>
	[DataMember(Name = "note")]
	public string Note { get; set; }

	/// <summary>
	/// The Value
	/// </summary>
	[DataMember(Name = "value")]
	public string Value { get; set; }

	/// <summary>
	/// The Error message
	/// </summary>
	[DataMember(Name = "errMsg")]
	public string ErrMsg { get; set; }

	/// <inheritdoc />
	public override string ToString() => $"{Name}: {Value} ({ErrMsg})";
}
