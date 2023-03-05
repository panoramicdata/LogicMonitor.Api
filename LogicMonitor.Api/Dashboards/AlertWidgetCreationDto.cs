namespace LogicMonitor.Api.Dashboards;

/// <summary>
///     An alert widget creation DTO
/// </summary>
[DataContract]
public class AlertWidgetCreationDto : WidgetCreationDto<AlertWidget>
{
	/// <summary>
	///     The name
	/// </summary>
	public override string Type { get; } = "alert";

	/// <summary>
	/// Widget Tokens
	/// </summary>
	[DataMember(Name = "widgetTokens")]
	public List<EntityProperty> CustomProperties { get; set; } = new();

	/// <summary>
	/// Additional specification, including column ordering
	/// </summary>
	[DataMember(Name = "extra")]
	public string ExtraString { get; set; } = string.Empty;

	/// <summary>
	/// The filters
	/// </summary>
	[DataMember(Name = "filters")]
	public AlertCreationDtoFilter Filters { get; set; } = new();

	/// <summary>
	/// Additional specification, including column ordering
	/// </summary>
	[IgnoreDataMember]
	public AlertCreationDtoAlertExtra Extra
	{
		set => ExtraString = JsonConvert.SerializeObject(value);
	}
}
