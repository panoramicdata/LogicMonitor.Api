namespace LogicMonitor.Api.Dashboards;

/// <summary>
/// Creation DTO for an <see cref="AdvancedMetricsWidget"/>.
/// </summary>
[DataContract]
public class AdvancedMetricsWidgetCreationDto : WidgetCreationDto<AdvancedMetricsWidget>
{
	/// <inheritdoc />
	public override string Type => "advancedMetrics";

	/// <summary>
	/// The LMQL query that drives the widget's data.
	/// </summary>
	[DataMember(Name = "lmql")]
	public string LmqlQuery { get; set; } = string.Empty;
}
