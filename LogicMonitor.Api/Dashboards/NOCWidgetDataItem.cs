namespace LogicMonitor.Api.Dashboards;

/// <summary>
/// Widget data for the NOC widget type
/// </summary>
[DataContract]
public class NOCWidgetDataItem
{
	/// <summary>
	/// Whether this is in scheduled downtime
	/// </summary>
	[DataMember(Name = "inSDT")]
	public bool InSdt { get; set; }

	/// <summary>
	/// The type of the widget data item
	/// </summary>
	/// <remarks>
	/// This could be used for discrimination during deserialization
	/// </remarks>
	[DataMember(Name = "type")]
	public string Type { get; set; } = string.Empty;

	/// <summary>
	/// The name of the data item
	/// </summary>
	[DataMember(Name = "subType")]
	public string SubType { get; set; } = string.Empty;

	/// <summary>
	/// The name of the data item
	/// </summary>
	[DataMember(Name = "name")]
	public string Name { get; set; } = string.Empty;

	/// <summary>
	/// The Id of the entity to which this refers
	/// </summary>
	[DataMember(Name = "entityId")]
	public int EntityId { get; set; }

	/// <summary>
	/// The Id of the parent of the entity to which this refers
	/// </summary>
	[DataMember(Name = "parentId")]
	public int ParentId { get; set; }

	/// <summary>
	/// Number of confirmed warn alert
	/// </summary>
	[DataMember(Name = "confirmedWarnAlert")]
	public int ConfirmedWarnAlert { get; set; }

	/// <summary>
	/// Number in scheduled downtime warn alert
	/// </summary>
	[DataMember(Name = "inSDTWarnAlert")]
	public int InSDTWarnAlert { get; set; }

	/// <summary>
	/// Number in scheduled downtime and confirmed warn alert
	/// </summary>
	[DataMember(Name = "inSDTAndConfirmedWarnAlert")]
	public int InSDTAndConfirmedWarnAlert { get; set; }

	/// <summary>
	/// Number in warn alert
	/// </summary>
	[DataMember(Name = "warnAlert")]
	public int WarnAlert { get; set; }

	/// <summary>
	/// Number in confirmed error alert
	/// </summary>
	[DataMember(Name = "confirmedErrorAlert")]
	public int ConfirmedErrorAlert { get; set; }

	/// <summary>
	/// Number in scheduled downtime error alerts
	/// </summary>
	[DataMember(Name = "inSDTErrorAlert")]
	public int InSdtErrorAlert { get; set; }

	/// <summary>
	/// Number of in scheduled downtime and confirmed error alerts
	/// </summary>
	[DataMember(Name = "inSDTAndConfirmedErrorAlert")]
	public int InSdtAndConfirmedErrorAlert { get; set; }

	/// <summary>
	/// Number of error alerts
	/// </summary>
	[DataMember(Name = "errorAlert")]
	public int ErrorAlert { get; set; }

	/// <summary>
	/// Number of confirmed critical alerts
	/// </summary>
	[DataMember(Name = "confirmedCriticalAlert")]
	public int ConfirmedCriticalAlert { get; set; }

	/// <summary>
	/// Number in scheduled downtime and critical alerts
	/// </summary>
	[DataMember(Name = "inSDTCriticalAlert")]
	public int InSdtCriticalAlert { get; set; }

	/// <summary>
	/// Number in scheduled downtime and confirmed critical alerts
	/// </summary>
	[DataMember(Name = "inSDTAndConfirmedCriticalAlert")]
	public int InSdtAndConfirmedCriticalAlert { get; set; }

	/// <summary>
	/// Number of critical alerts
	/// </summary>
	[DataMember(Name = "criticalAlert")]
	public int CriticalAlert { get; set; }

	/// <summary>
	/// The resource template type
	/// </summary>
	[DataMember(Name = "resourceTemplateType")]
	public string ResourceTemplateType { get; set; } = string.Empty;
}