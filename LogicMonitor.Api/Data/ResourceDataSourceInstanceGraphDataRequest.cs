namespace LogicMonitor.Api.Data;

/// <summary>
///    A request for device graph data for a DeviceDataSource
/// </summary>
public class ResourceDataSourceInstanceGraphDataRequest : GraphDataRequest
{
	/// <summary>
	///    The DataSourceGraph Id.
	///    If null, DataSourceGraphName must be non-null
	/// </summary>
	public int DataSourceGraphId { get; set; }

	/// <summary>
	///    The DataSourceInstance Id
	/// </summary>
	public int DeviceDataSourceInstanceId { get; set; }

	internal override string SubUrl => $"device/devicedatasourceinstances/{DeviceDataSourceInstanceId}/graphs/{DataSourceGraphId}/data?{TimePart}";

	/// <inheritdoc />
	public override void Validate()
	{
		if (DataSourceGraphId is <= 0 and not (-1))
		{
			throw new ArgumentException("DataSourceGraphId must be specified.");
		}

		if (DeviceDataSourceInstanceId <= 0)
		{
			throw new ArgumentException("DeviceDataSourceInstanceId must be specified.");
		}

		ValidateInternal();
	}
}
