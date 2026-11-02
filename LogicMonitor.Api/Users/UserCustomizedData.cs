namespace LogicMonitor.Api.Users;

/// <summary>
/// A user customized data item, such as the default dashboard
/// </summary>
[DataContract]
public class UserCustomizedData
{
	/// <summary>
	/// The id
	/// </summary>
	[DataMember(Name = "id")]
	public string Id { get; set; } = string.Empty;

	/// <summary>
	/// The value
	/// </summary>
	[DataMember(Name = "value")]
	public string Value { get; set; } = string.Empty;
}
