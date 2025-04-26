namespace LogicMonitor.Api.Alerts;

/// <summary>
/// The total alert stats for the logged-in user
/// </summary>
[DataContract]
public class AlertStat : IHasSingletonEndpoint
{
	/// <summary>
	/// The warning count
	/// </summary>
	[DataMember(Name = "warns")]
	public int Warnings { get; set; }

	/// <summary>
	/// The error count
	/// </summary>
	[DataMember(Name = "errors")]
	public int Errors { get; set; }

	/// <summary>
	/// The critical count
	/// </summary>
	[DataMember(Name = "criticals")]
	public int Criticals { get; set; }

	/// <summary>
	/// The Website warning count
	/// </summary>
	[DataMember(Name = "websiteWarns")]
	public int WebsiteWarnings { get; set; }

	/// <summary>
	/// The Website error count
	/// </summary>
	[DataMember(Name = "websiteErrors")]
	public int WebsiteErrors { get; set; }

	/// <summary>
	/// The Website critical count
	/// </summary>
	[DataMember(Name = "websiteCriticals")]
	public int WebsiteCriticals { get; set; }

	/// <summary>
	/// The dead host count
	/// </summary>
	[DataMember(Name = "deadhosts")]
	public int DeadHosts { get; set; }

	/// <summary>
	/// The count of warning alerts in scheduled down time
	/// </summary>
	[DataMember(Name = "warnSdtAlerts")]
	public int WarningSdtAlerts { get; set; }

	/// <summary>
	/// The count of error alerts in scheduled down time
	/// </summary>
	[DataMember(Name = "errorSdtAlerts")]
	public int ErrorSdtAlerts { get; set; }

	/// <summary>
	/// The count of critical alerts in scheduled down time
	/// </summary>
	[DataMember(Name = "criticalSdtAlerts")]
	public int CriticalSdtAlerts { get; set; }

	/// <summary>
	/// The count of alerts in scheduled down time
	/// </summary>
	[DataMember(Name = "sdtAlerts")]
	public int SdtAlerts { get; set; }

	/// <summary>
	/// The total count of alerts
	/// </summary>
	[DataMember(Name = "totalAlerts")]
	public int TotalAlerts { get; set; }

	/// <summary>
	/// The count of warning alerts that have been acknowledged
	/// </summary>
	[DataMember(Name = "warnAckAlerts")]
	public int WarningAckAlerts { get; set; }

	/// <summary>
	/// The count of error alerts that have been acknowledged
	/// </summary>
	[DataMember(Name = "errorAckAlerts")]
	public int ErrorAckAlerts { get; set; }

	/// <summary>
	/// The count of critical alerts that have been acknowledged
	/// </summary>
	[DataMember(Name = "criticalAckAlerts")]
	public int CriticalAckAlerts { get; set; }


	/// <summary>
	/// The total count of alerts
	/// </summary>
	[DataMember(Name = "ackAlerts")]
	public int AckAlerts { get; set; }

	/// <summary>
	/// Whether the alert total includes acknowledges alerts
	/// </summary>
	[DataMember(Name = "alertTotalIncludeInAck")]
	public bool AlertTotalIncludeInAck { get; set; }

	/// <summary>
	/// Whether the alert total includes those in SDT
	/// </summary>
	[DataMember(Name = "alertTotalIncludeInSdt")]
	public bool AlertTotalIncludeInSdt { get; set; }

	/// <summary>
	///    The endpoint
	/// </summary>
	public string Endpoint() => "alert/stat";
}
