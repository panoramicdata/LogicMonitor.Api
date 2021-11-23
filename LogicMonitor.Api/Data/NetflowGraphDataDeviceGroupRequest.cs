namespace LogicMonitor.Api.Data;

/// <summary>
///    A request for netflow graph data, for a Device Group
/// </summary>
public class NetflowDeviceGroupGraphDataRequest : GraphDataRequest
{
	/// <summary>
	///    The netflow Device Group ID for use with GraphType.NetflowBandwidth
	/// </summary>
	public int DeviceGroupId { get; set; }

	internal override string SubUrl => $"device/groups/{DeviceGroupId}/netflow/graphs/bandwidth_octets_both/data?{TimePart}&qosType=all&rowFilters={HttpUtility.UrlEncode("[]")}";

	/// <inheritdoc />
	public override void Validate()
	{
		if (DeviceGroupId == 0)
		{
			throw new ArgumentException("NetflowDeviceId must be specified");
		}

		ValidateInternal();
	}
}
