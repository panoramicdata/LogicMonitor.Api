﻿namespace LogicMonitor.Api.Data;

/// <summary>
///    A request for netflow graph data, for a ResourceGroup
/// </summary>
public class NetflowResourceGroupGraphDataRequest : GraphDataRequest
{
	/// <summary>
	/// The Netflow ResourceGroup  ID for use with GraphType.NetflowBandwidthid
	/// </summary>
	[DataMember(Name = "hostGroupId")]
	public int ResourceGroupId { get; set; }

	/// <summary>
	/// Obsolete
	/// </summary>
	[JsonIgnore, IgnoreDataMember]
	[Obsolete("Use ResourceGroupId", true)]
	public int DeviceGroupId => ResourceGroupId;

	internal override string SubUrl => $"device/groups/{ResourceGroupId}/netflow/graphs/bandwidth_octets_both/data?{TimePart}&qosType=all&rowFilters={HttpUtility.UrlEncode("[]")}";

	/// <inheritdoc />
	public override void Validate()
	{
		if (ResourceGroupId == 0)
		{
			throw new ArgumentException("NetflowDeviceId must be specified");
		}

		ValidateInternal();
	}
}
