namespace LogicMonitor.Api.Alerts;

/// <summary>
/// A notification stage
/// </summary>
[DataContract]
public class NotificationStage
{
	/// <summary>
	/// The destination type
	/// </summary>
	[DataMember(Name = "type")]
	[JsonConverter(typeof(StringEnumConverter))]
	public NotificationStageType NotificationStageType { get; set; }

	/// <summary>
	/// The destination address
	/// </summary>
	[DataMember(Name = "addr")]
	public string Address { get; set; } = string.Empty;

	/// <summary>
	/// The comment
	/// </summary>
	[DataMember(Name = "comment")]
	public string Comment { get; set; } = string.Empty;

	/// <summary>
	/// The method
	/// </summary>
	[DataMember(Name = "method")]
	public string Method { get; set; } = string.Empty;

	/// <summary>
	/// The contact details
	/// </summary>
	[DataMember(Name = "contact")]
	public string Contact { get; set; } = string.Empty;
}
