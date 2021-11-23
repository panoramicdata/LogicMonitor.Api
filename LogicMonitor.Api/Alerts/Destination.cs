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
	public string Address { get; set; }

	/// <summary>
	/// A comment
	/// </summary>
	[DataMember(Name = "comment")]
	public string Comment { get; set; }

	/// <summary>
	/// The contact
	/// </summary>
	[DataMember(Name = "contact")]
	public string Contact { get; set; }

	/// <summary>
	/// The method
	/// </summary>
	[DataMember(Name = "method")]
	public string Method { get; set; }

	/// <summary>
	/// The period
	/// </summary>
	[DataMember(Name = "period")]
	public EscalationChainPeriod Period { get; set; }

	/// <summary>
	/// The stages
	/// </summary>
	[DataMember(Name = "stages")]
	public List<List<NotificationStage>> Stages { get; set; }

	/// <summary>
	/// The destination type
	/// </summary>
	[DataMember(Name = "type")]
	[JsonConverter(typeof(StringEnumConverter))]
	public DestinationType DestinationType { get; set; }
}
