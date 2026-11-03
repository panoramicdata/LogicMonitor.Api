namespace LogicMonitor.Api.Logs;

/// <summary>
/// An identifier for a saved log query
/// </summary>
[DataContract]
public class LogQueryObjectIdentifier
{
	/// <summary>
	/// The id
	/// </summary>
	[DataMember(Name = "id")]
	public string Id { get; set; } = string.Empty;

	/// <summary>
	/// The model
	/// </summary>
	[DataMember(Name = "model")]
	public string Model { get; set; } = string.Empty;
}
