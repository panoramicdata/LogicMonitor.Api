namespace LogicMonitor.Api.Data;

/// <summary>
/// Widget graph data request
/// </summary>
public class WidgetGraphDataRequest : GraphDataRequest
{
	/// <summary>
	///    The Widget Id.
	/// </summary>
	public int WidgetId { get; set; }

	internal override string SubUrl => $"dashboard/widgets/{WidgetId}/data?{TimePart}";

	/// <inheritdoc />
	public override void Validate()
	{
		if (WidgetId <= 0)
		{
			throw new ArgumentException("WidgetId must be specified.");
		}
		ValidateInternal();
	}
}
