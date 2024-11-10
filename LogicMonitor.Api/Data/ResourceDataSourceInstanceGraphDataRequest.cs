namespace LogicMonitor.Api.Data;

/// <summary>
///    A request for device graph data for a ResourceDataSource
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
	public int ResourceDataSourceInstanceId { get; set; }

	internal override string SubUrl => $"device/devicedatasourceinstances/{ResourceDataSourceInstanceId}/graphs/{DataSourceGraphId}/data?{TimePart}";

	/// <inheritdoc />
	public override void Validate()
	{
		if (DataSourceGraphId is <= 0 and not (-1))
		{
			throw new ArgumentException("DataSourceGraphId must be specified.");
		}

		if (ResourceDataSourceInstanceId <= 0)
		{
			throw new ArgumentException("ResourceDataSourceInstanceId must be specified.");
		}

		ValidateInternal();
	}
}
