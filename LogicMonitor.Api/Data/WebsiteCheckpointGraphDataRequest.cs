namespace LogicMonitor.Api.Data;

/// <summary>
///    A Website graph data request
/// </summary>
public class WebsiteCheckpointGraphDataRequest : WebsiteOverviewGraphDataRequest
{
	internal override string SubUrl
	{
		get
		{
			var websiteGraphName = WebsiteGraphType switch
			{
				WebsiteGraphType.OverallStatus => "status",
				WebsiteGraphType.ResponseTime => "responseTime",
				WebsiteGraphType.Statistics => "statistics",
				_ => throw new ArgumentOutOfRangeException(),
			};
			return $"website/websites/{WebsiteId}/checkpoints/{WebsiteCheckPointId}/graphs/{websiteGraphName}/data?{TimePart}";
		}
	}

	/// <summary>
	///    The Website checkpoint id
	/// </summary>
	public int WebsiteCheckPointId { get; set; }

	/// <inheritdoc />
	public override void Validate()
	{
		base.Validate();
		if (WebsiteCheckPointId <= 0)
		{
			throw new ArgumentException("WebsiteCheckPointId must be specified.");
		}
	}
}
