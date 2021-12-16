namespace LogicMonitor.Api.Data;

/// <summary>
///    A Website graph data request
/// </summary>
public class WebsiteOverviewGraphDataRequest : GraphDataRequest
{
	/// <summary>
	///    The Website Id
	/// </summary>
	public int WebsiteId { get; set; }

	/// <summary>
	///    The Website graph type
	/// </summary>
	public WebsiteGraphType WebsiteGraphType { get; set; }

	internal override string SubUrl
	{
		get
		{
			var websiteGraphName = WebsiteGraphType switch
			{
				WebsiteGraphType.OverallStatus => "overallStatus",
				WebsiteGraphType.ResponseTime => "pingAvgRTT",
				_ => throw new ArgumentOutOfRangeException(),
			};
			return $"website/websites/{WebsiteId}/graphs/{websiteGraphName}/data?{TimePart}";
		}
	}

	/// <inheritdoc />
	public override void Validate()
	{
		if (WebsiteId <= 0)
		{
			throw new ArgumentException("WebsiteId must be specified.");
		}

		if (WebsiteGraphType == WebsiteGraphType.Unknown)
		{
			throw new ArgumentException("WebsiteGraphType must be specified.");
		}

		ValidateInternal();
	}
}
