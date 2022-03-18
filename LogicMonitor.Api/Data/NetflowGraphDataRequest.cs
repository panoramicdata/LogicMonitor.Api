namespace LogicMonitor.Api.Data;

/// <summary>
///    A request for netflow graph data
/// </summary>
public class NetflowGraphDataRequest : GraphDataRequest
{
	/// <summary>
	///    The netflow device id for use with GraphType.NetflowBandwidth
	/// </summary>
	public int DeviceId { get; set; }

	/// <summary>
	/// The Netflow Filter
	/// </summary>
	public NetflowFilter NetflowFilter { get; set; } = new NetflowFilter();

	internal override string SubUrl => $"device/devices/{DeviceId}/topTalkersGraph?netflowFilter={NetflowFilter.AsUrlEncodedString()}&{TimePart}&maxSamples=458";

	/// <inheritdoc />
	public override void Validate()
	{
		if (DeviceId == 0)
		{
			throw new ArgumentException("NetflowDeviceId must be specified");
		}

		if (NetflowFilter is null)
		{
			throw new ArgumentException("NetflowFilter must not be null.");
		}

		NetflowFilter.Validate();
		ValidateInternal();
	}
}
