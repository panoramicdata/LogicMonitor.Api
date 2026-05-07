namespace LogicMonitor.Api.Resources;

/// <summary>
/// A cluster alert configuration for a ResourceGroup.
/// Triggers a group-level alert when a threshold number (or percentage) of devices/instances
/// in the group are in alert for a given DataSource data point.
/// REST: /device/groups/{deviceGroupId}/clusterAlertConf[/{id}]
/// </summary>
[DataContract]
public class ResourceGroupClusterAlertConfig : IdentifiedItem, IHasEndpoint, IPatchable
{
	/// <summary>
	/// The ResourceGroup Id this configuration belongs to.
	/// Not returned by the API; set by client methods so that Endpoint() is correct.
	/// </summary>
	public int ResourceGroupId { get; set; }

	/// <summary>
	/// Whether the cluster alert counts across devices ("host") or instances ("instance")
	/// </summary>
	[DataMember(Name = "countBy")]
	public string CountBy { get; set; } = "host";

	/// <summary>
	/// The name of the DataPoint this configuration is based on (read-only, filled by LM)
	/// </summary>
	[SantabaReadOnly]
	[DataMember(Name = "dataPointName")]
	public string DataPointName { get; set; } = string.Empty;

	/// <summary>
	/// The Id of the DataSource to base the cluster alert on
	/// </summary>
	[DataMember(Name = "dataSourceId")]
	public int DataSourceId { get; set; }

	/// <summary>
	/// The Id of the DataPoint to base the cluster alert on (read-only, filled by LM)
	/// </summary>
	[SantabaReadOnly]
	[DataMember(Name = "dataPointId")]
	public int DataPointId { get; set; }

	/// <summary>
	/// The alert level that must be present on a device/instance for it to be counted.
	/// Acceptable values: 2 (Warning), 3 (Error), 4 (Critical)
	/// </summary>
	[DataMember(Name = "minAlertLevel")]
	public int MinAlertLevel { get; set; }

	/// <summary>
	/// The display name of the DataSource (read-only, filled by LM)
	/// </summary>
	[SantabaReadOnly]
	[DataMember(Name = "dataSourceDisplayName")]
	public string DataSourceDisplayName { get; set; } = string.Empty;

	/// <summary>
	/// Whether alerting is disabled for this cluster alert configuration
	/// </summary>
	[DataMember(Name = "disableAlerting")]
	public bool DisableAlerting { get; set; }

	/// <summary>
	/// Whether individual device/instance alerts are suppressed when the cluster alert fires.
	/// Defaults to true.
	/// </summary>
	[DataMember(Name = "suppressIndAlert")]
	public bool SuppressIndividualAlerts { get; set; } = true;

	/// <summary>
	/// Whether the alert expression is evaluated as an absolute count or a percentage.
	/// Acceptable values: "absolute", "percentage"
	/// </summary>
	[DataMember(Name = "thresholdType")]
	public string ThresholdType { get; set; } = "absolute";

	/// <summary>
	/// The description of the DataPoint (read-only, filled by LM)
	/// </summary>
	[SantabaReadOnly]
	[DataMember(Name = "dataPointDescription")]
	public string DataPointDescription { get; set; } = string.Empty;

	/// <summary>
	/// The expression that indicates how many devices/instances must be in alert to trigger the
	/// cluster alert, e.g. "&gt;= 2" for absolute or "&gt;= 50" for percentage
	/// </summary>
	[DataMember(Name = "alertExpr")]
	public string AlertExpression { get; set; } = string.Empty;

	/// <summary>
	/// The endpoint for this resource, scoped to the owning ResourceGroup
	/// </summary>
	public string Endpoint() => $"device/groups/{ResourceGroupId}/clusterAlertConf";
}
