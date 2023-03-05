namespace LogicMonitor.Api.Dashboards;

/// <summary>
/// A TableWidgetRowDataSourceInstance
/// </summary>
[DataContract]
public class TableWidgetRowDataSourceInstance
{
	/// <summary>
	///     The DataSourceInstance id
	/// </summary>
	[DataMember(Name = "instanceId")]
	public int DataSourceInstanceId { get; set; }

	/// <summary>
	///     The DataSourceInstance name
	/// </summary>
	[DataMember(Name = "instanceName")]
	public string DataSourceInstanceName { get; set; } = string.Empty;

	/// <summary>
	///     The confidence
	/// </summary>
	[DataMember(Name = "confidence")]
	public int Confidence { get; set; }

	/// <summary>
	///     The instanceName
	/// </summary>
	[DataMember(Name = "dataPointName")]
	public string DataPointName { get; set; } = string.Empty;

	/// <summary>
	///     The confidence
	/// </summary>
	[DataMember(Name = "dataPointId")]
	public int DataPointId { get; set; }

	/// <summary>
	///     The validation status code
	/// </summary>
	[DataMember(Name = "validationStatusCode")]
	public int ValidationStatusCode { get; set; }
}
