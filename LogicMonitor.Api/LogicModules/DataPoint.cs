namespace LogicMonitor.Api.LogicModules;

/// <summary>
/// A DataSource DataPoint
/// </summary>
[DataContract]
public class DataPoint : NamedItem
{
	/// <summary>
	/// alert expression note
	/// </summary>
	[DataMember(Name = "alertExprNote")]
	public string AlertExpressionNote { get; set; } = string.Empty;

	/// <summary>
	/// The ID of the LMModule
	/// </summary>
	[DataMember(Name = "id")]
	public int DataPointId { get; set; }

	/// <summary>
	/// The datasource id
	/// </summary>
	[DataMember(Name = "dataSourceId")]
	public int DataSourceId { get; set; }

	/// <summary>
	/// The count that the alert must exist for this many poll cycles before it will be triggered
	/// </summary>
	[DataMember(Name = "alertTransitionInterval")]
	public int AlertTransitionInterval { get; set; }

	/// <summary>
	/// The count that the alert must exist for this many poll cycles before the alert will be cleared
	/// </summary>
	[DataMember(Name = "alertClearTransitionInterval")]
	public int AlertClearTransitionInterval { get; set; }

	/// <summary>
	/// The consolidation function
	/// </summary>
	[DataMember(Name = "consolidateFunc")]
	public string ConsolidationFunction { get; set; } = string.Empty;

	/// <summary>
	/// The data metric type, values can be 0-7 (0:unknown, 1:counter, 2:gauge, 3:derive, 5:status, 6:compute, 7:counter32, 8:counter64)
	/// </summary>
	[DataMember(Name = "type")]
	public int Type { get; set; }

	/// <summary>
	/// The data value type, values can be 1-8 (1:boolean, 2:byte, 3:short, 4:int, 5:long, 6:float, 7:double, 8:ulong)
	/// </summary>
	[DataMember(Name = "dataType")]
	public int DataType { get; set; }

	/// <summary>
	/// The max digits of the data value
	/// </summary>
	[DataMember(Name = "maxDigits")]
	public int MaxDigits { get; set; }

	/// <summary>
	/// The post processor method for the data value. Currently support complex expression and groovy.
	/// </summary>
	[DataMember(Name = "postProcessorMethod")]
	public string PostProcessorMethod { get; set; } = string.Empty;

	/// <summary>
	/// The post processor parameter, e.g. dataPoint1*2
	/// </summary>
	[DataMember(Name = "postProcessorParam")]
	public string PostProcessorParam { get; set; } = string.Empty;

	/// <summary>
	/// The name of the raw data field name used to fetch value, e.g. avgrtt, output
	/// </summary>
	[DataMember(Name = "rawDataFieldName")]
	public string RawDataFieldName { get; set; } = string.Empty;

	/// <summary>
	/// The max value of the datapoint value range
	/// </summary>
	[DataMember(Name = "maxValue")]
	public string MaxValue { get; set; } = string.Empty;

	/// <summary>
	/// The minimum value of the datapoint value range
	/// </summary>
	[DataMember(Name = "minValue")]
	public string MinValue { get; set; } = string.Empty;

	/// <summary>
	/// The first user parameter will be used to fetch the datapoint value. e.g. snmp oid
	/// </summary>
	[DataMember(Name = "userParam1")]
	public string UserParam1 { get; set; } = string.Empty;

	/// <summary>
	/// The second user parameter will be used to fetch the datapoint value. e.g. jmx attribute name
	/// </summary>
	[DataMember(Name = "userParam2")]
	public string UserParam2 { get; set; } = string.Empty;

	/// <summary>
	/// The third user parameter will be used to fetch the datapoint value.
	/// </summary>
	[DataMember(Name = "userParam3")]
	public string UserParam3 { get; set; } = string.Empty;

	/// <summary>
	/// The triggered alert level if we cannot collect data for this datapoint, value can be 1-4 (0:unused alert, 1:alert ok, 2:warn alert, 2:error alert, 4:critical alert)
	/// </summary>
	[DataMember(Name = "alertForNoData")]
	public int AlertForNoData { get; set; }

	/// <summary>
	/// The alert threshold define for the datapoint. e.g. \u0027\u003e 60 80 90\u0027 mean it will: \ntrigger warn alert if value \u003e 60\ntrigger error alert if value \u003e 80\ntrigger critical alert if value \u003e 90
	/// </summary>
	[DataMember(Name = "alertExpr")]
	public string AlertExpression { get; set; } = string.Empty;

	/// <summary>
	/// The customized alert message subject define, empty string mean we will use the define in default template
	/// </summary>
	[DataMember(Name = "alertSubject")]
	public string AlertSubject { get; set; } = string.Empty;

	/// <summary>
	/// The customized alert message body define,  empty string mean we will use the define in default template
	/// </summary>
	[DataMember(Name = "alertBody")]
	public string AlertBody { get; set; } = string.Empty;

	/// <summary>
	/// Expression of anomaly detection setting, split by comma\n0 means off,  1 means on, -1 means invalid\n1,0,1 \u003d   warn : ON     error: OFF   critical: ON\nEmpty value on this parameter means : 0,0,0
	/// </summary>
	[DataMember(Name = "enableAnomalyAlertSuppression")]
	public string SuppressAnomalyAlerts { get; set; } = string.Empty;   // This is a string NOT a bool e.g. "enableAnomalyAlertSuppression": "0,0,0"

	/// <summary>
	/// the AD advance setting enable flag
	/// </summary>
	[DataMember(Name = "adAdvSettingEnabled")]
	public bool? IsAdAdvSettingEnabled { get; set; }

	/// <summary>
	/// enable anomaly detection advance setting for WARN severity
	/// </summary>
	[DataMember(Name = "warnAdAdvSetting")]
	public string WarnAdAdvSetting { get; set; } = string.Empty;

	/// <summary>
	/// enable anomaly detection advance setting for ERROR severity
	/// </summary>
	[DataMember(Name = "errorAdAdvSetting")]
	public string ErrorAdAdvSetting { get; set; } = string.Empty;

	/// <summary>
	/// enable anomaly detection advance setting for CRITICAL severity
	/// </summary>
	[DataMember(Name = "criticalAdAdvSetting")]
	public string CriticialAdAdvSetting { get; set; } = string.Empty;

	/// <summary>
	/// portable id for origin tracking
	/// </summary>
	[DataMember(Name = "originId")]
	public string OriginId { get; set; } = string.Empty;
}
