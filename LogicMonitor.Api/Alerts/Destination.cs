namespace LogicMonitor.Api.Alerts;

/// <summary>
/// An escalation chain destination
/// </summary>
[DataContract]
public class Destination
{
	/// <summary>
	/// The address
	/// </summary>
	[DataMember(Name = "addr")]
	public string Address { get; set; } = string.Empty;

	/// <summary>
	/// A comment
	/// </summary>
	[DataMember(Name = "comment")]
	public string Comment { get; set; } = string.Empty;

	/// <summary>
	/// The contact
	/// </summary>
	[DataMember(Name = "contact")]
	public string Contact { get; set; } = string.Empty;

	/// <summary>
	/// The method
	/// </summary>
	[DataMember(Name = "method")]
	public string Method { get; set; } = string.Empty;

	/// <summary>
	/// The period
	/// </summary>
	[DataMember(Name = "period")]
	public Period Period { get; set; } = new();

	/// <summary>
	/// The stages
	/// </summary>
	[DataMember(Name = "stages")]
	public List<List<NotificationStage>> Stages { get; set; } = new();

	/// <summary>
	/// The destination type
	/// </summary>
	[DataMember(Name = "type")]
	[JsonConverter(typeof(StringEnumConverter))]
	public DestinationType DestinationType { get; set; }
}
