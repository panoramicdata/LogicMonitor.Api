namespace LogicMonitor.Api.Data;

/// <summary>
///    A request for Resource graph data for a ResourceDataSource
/// </summary>
public class ResourceDataSourceGraphDataRequest : GraphDataRequest
{
	/// <summary>
	///    The DataSourceInstanceGroup Id
	/// </summary>
	public int? DataSourceInstanceGroupId { get; set; }

	/// <summary>
	///    The OverviewGraph Id
	/// </summary>
	public int? OverviewGraphId { get; set; }

	/// <summary>
	///  The Resource id
	/// </summary>
	public int? DeviceId { get; set; }

	/// <summary>
	/// The Datasource Id
	/// </summary>
	public int DeviceDataSourceId { get; set; }

	internal override string SubUrl => $"device/devices/{DeviceId}/devicedatasources/{DeviceDataSourceId}/groups/{DataSourceInstanceGroupId}/graphs/{OverviewGraphId}/data?{TimePart}";

	/// <inheritdoc />
	public override void Validate()
	{
		if (DataSourceInstanceGroupId <= 0)
		{
			throw new ArgumentException("DataSourceInstanceGroupId must be specified.");
		}

		if (OverviewGraphId <= 0)
		{
			throw new ArgumentException("OverviewGraphId must be specified.");
		}

		ValidateInternal();
	}
}
