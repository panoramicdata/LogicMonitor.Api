namespace LogicMonitor.Api.Data;

/// <summary>
///    A request for device graph data for a devicedatasource
/// </summary>
public class DeviceDataSourceGraphDataRequest : GraphDataRequest
{
	/// <summary>
	///    The DataSourceInstanceGroup Id
	/// </summary>
	public int? DataSourceInstanceGroupId { get; set; }

	/// <summary>
	///    The OverviewGraph Id
	/// </summary>
	public int? OverviewGraphId { get; set; }

	internal override string SubUrl => $"device/devicedatasourceinstancegroups/{DataSourceInstanceGroupId}/graphs/{OverviewGraphId}/data?{TimePart}";

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
