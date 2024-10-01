namespace LogicMonitor.Api.Data;

/// <summary>
///    A request for netflow graph data
/// </summary>
[DataContract]
public class NetflowGraphDataRequest : GraphDataRequest
{
	/// <summary>
	///    The netflow Resource id for use with GraphType.NetflowBandwidth
	/// </summary>
	[DataMember(Name = "deviceId")]
	public int ResourceId { get; set; }

	/// <summary>
	/// Obsolete
	/// </summary>
	[JsonIgnore, IgnoreDataMember]
	[Obsolete("Use ResourceId", true)]
	public int DeviceId => ResourceId;

	/// <summary>
	/// The Netflow Filter
	/// </summary>
	public NetflowFilters NetflowFilter { get; set; } = new NetflowFilters();

	internal override string SubUrl => $"device/devices/{ResourceId}/topTalkersGraph?netflowFilter={NetflowFilter.AsUrlEncodedString()}&{TimePart}&maxSamples=458";

	/// <inheritdoc />
	public override void Validate()
	{
		if (ResourceId == 0)
		{
			throw new ArgumentException("NetflowDeviceId must be specified");
		}

		if (NetflowFilter is null)
		{
			throw new ArgumentException("NetflowFilter must not be null.");
		}

		ValidateInternal();
	}
}
