namespace LogicMonitor.Api.Alerts;

/// <summary>
/// An Escalation chain creation DTO
/// </summary>
[DataContract]
public class EscalationChainCreationDto
	: CreationDto<EscalationChain>, IHasName, IHasDescription
{
	/// <summary>
	///    The LogicMonitor Name
	/// </summary>
	[DataMember(Name = "name")]
	public string Name { get; set; } = string.Empty;

	/// <summary>
	///    The LogicMonitor Description
	/// </summary>
	[DataMember(Name = "description")]
	public string Description { get; set; } = string.Empty;

	/// <summary>
	/// Whether throttling is enabled
	/// </summary>
	[DataMember(Name = "enableThrottling")]
	public bool EnableThrottling { get; set; }

	/// <summary>
	/// The throttling period in seconds
	/// </summary>
	[DataMember(Name = "throttlingPeriod")]
	public int ThrottlingPeriodMinutes { get; set; }

	/// <summary>
	/// The alert count for throttling
	/// </summary>
	[DataMember(Name = "throttlingAlerts")]
	public int ThrottlingAlertCount { get; set; }

	/// <summary>
	/// Whether in alerting
	/// </summary>
	[DataMember(Name = "inAlerting")]
	public bool InAlerting { get; set; }

	/// <summary>
	/// The CC destinations. This is the field the API honours on write.
	/// </summary>
	[DataMember(Name = "ccDestinations")]
	public List<Destination> CcDestinations { get; set; } = [];

	/// <summary>
	/// The destinations. This is the field the API honours on write.
	/// </summary>
	[DataMember(Name = "destinations")]
	public List<Destination> Destinations { get; set; } = [];

	/// <summary>
	/// The CC destinations, under the name the READ model returns.
	/// </summary>
	/// <remarks>
	/// An alias for <see cref="CcDestinations"/>, kept so code written against the read shape still
	/// compiles. It is deliberately NOT serialised. When it was, an unset alias went out as an empty
	/// array alongside the populated canonical field, the API took the empty one, and the call
	/// returned HTTP 200 with a fully-formed chain and no CC recipients - silent data loss, visible
	/// only on a re-read. See issue #28.
	/// </remarks>
	[Obsolete("Use CcDestinations. The API honours ccDestinations on write; ccdestination is the read shape.")]
	public List<Destination> CcDestination
	{
		get => CcDestinations;
		set => CcDestinations = value;
	}

	/// <summary>
	/// The destinations, under the name the READ model returns.
	/// </summary>
	/// <remarks>
	/// An alias for <see cref="Destinations"/>. Serialising this instead produced HTTP 400
	/// "Missing subchain. Add at least one subchain." - an error that does not name the field at
	/// fault. See issue #28.
	/// </remarks>
	[Obsolete("Use Destinations. The API honours destinations on write; destination is the read shape.")]
	public List<Destination> Destination
	{
		get => Destinations;
		set => Destinations = value;
	}
}
