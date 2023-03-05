namespace LogicMonitor.Api.Data;

/// <summary>
/// Forecast GraphData
/// </summary>
public class ForecastGraphData
{
	/// <summary>
	/// The forecasted GraphData
	/// </summary>
	public GraphData ForecastedGraphData { get; set; } = new();

	/// <summary>
	/// The training graph data
	/// </summary>
	public GraphData TrainingGraphData { get; set; } = new();

	/// <summary>
	/// Returns a string that represents the current object.
	/// </summary>
	public override string ToString() => $"{ForecastedGraphData.TitleString} forecast data";
}
