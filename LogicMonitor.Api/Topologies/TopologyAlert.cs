namespace LogicMonitor.Api.Topologies;

/// <summary>
/// Topology alert
/// </summary>
[DataContract]
public class TopologyAlert : StringIdentifiedItem
{
	/// <summary>
	/// type
	/// </summary>
	[DataMember(Name = "type")]
	public string Type { get; set; } = string.Empty;

	/// <summary>
	/// internalId
	/// </summary>
	[DataMember(Name = "internalId")]
	public string InternalId { get; set; } = string.Empty;

	/// <summary>
	/// startEpoch
	/// </summary>
	[DataMember(Name = "startEpoch")]
	public int StartEpoch { get; set; }

	/// <summary>
	/// acked
	/// </summary>
	[DataMember(Name = "acked")]
	public bool Acked { get; set; }

	/// <summary>
	/// severity
	/// </summary>
	[DataMember(Name = "severity")]
	public int Severity { get; set; }

	/// <summary>
	/// sdted
	/// </summary>
	[DataMember(Name = "sdted")]
	public bool Sdted { get; set; }

	/// <summary>
	/// monitorObjectId
	/// </summary>
	[DataMember(Name = "monitorObjectId")]
	public int MonitorObjectId { get; set; }

	/// <summary>
	/// monitorObjectType
	/// </summary>
	[DataMember(Name = "monitorObjectType")]
	public string MonitorObjectType { get; set; } = string.Empty;

	/// <summary>
	/// monitorObjectName
	/// </summary>
	[DataMember(Name = "monitorObjectName")]
	public string MonitorObjectName { get; set; } = string.Empty;

	/// <summary>
	/// resourceId
	/// </summary>
	[DataMember(Name = "resourceId")]
	public int ResourceId { get; set; }

	/// <summary>
	/// resourceTemplateId
	/// </summary>
	[DataMember(Name = "resourceTemplateId")]
	public int ResourceTemplateId { get; set; }

	/// <summary>
	/// resourceTemplateType
	/// </summary>
	[DataMember(Name = "resourceTemplateType")]
	public string ResourceTemplateType { get; set; } = string.Empty;

	/// <summary>
	/// resourceTemplateName
	/// </summary>
	[DataMember(Name = "resourceTemplateName")]
	public string ResourceTemplateName { get; set; } = string.Empty;

	/// <summary>
	/// instanceId
	/// </summary>
	[DataMember(Name = "instanceId")]
	public int InstanceId { get; set; }

	/// <summary>
	/// instanceName
	/// </summary>
	[DataMember(Name = "instanceName")]
	public string InstanceName { get; set; } = string.Empty;

	/// <summary>
	/// dataPointName
	/// </summary>
	[DataMember(Name = "dataPointName")]
	public string DataPointName { get; set; } = string.Empty;

	/// <summary>
	/// dataPointId
	/// </summary>
	[DataMember(Name = "dataPointId")]
	public int DataPointId { get; set; }

	/// <summary>
	/// suppressor
	/// </summary>
	[DataMember(Name = "suppressor")]
	public string Suppressor { get; set; } = string.Empty;

	/// <summary>
	/// dependencyRoutingState
	/// </summary>
	[DataMember(Name = "dependencyRoutingState")]
	public string DependencyRoutingState { get; set; } = string.Empty;
}