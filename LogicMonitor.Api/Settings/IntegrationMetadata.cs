namespace LogicMonitor.Api.Settings;

/// <summary>
/// IntegrationMetadata
/// </summary>
[DataContract]
public class IntegrationMetadata
{
	/// <summary>
	/// Specifies the target last published Id
	/// </summary>
	[DataMember(Name = "targetLastPublishedId", IsRequired = false)]
	public string? TargetLastPublishedId { get; set; }

	/// <summary>
	/// The metadata checksum for the target last published LMModule content
	/// </summary>
	[DataMember(Name = "targetLastPublishedChecksum", IsRequired = false)]
	public string? TargetLastPublishedChecksum { get; set; }

	/// <summary>
	/// Specifies the target last published version
	/// </summary>
	[DataMember(Name = "targetLastPublishedVersion", IsRequired = false)]
	public string? TargetLastPublishedVersion { get; set; }

	/// <summary>
	/// The metadata checksum for the LMModule content
	/// </summary>
	[DataMember(Name = "originChecksum", IsRequired = false)]
	public string? OriginChecksum { get; set; }

	/// <summary>
	/// Specifies the origin Author companies namespace
	/// </summary>
	[DataMember(Name = "originAuthorNamespace", IsRequired = false)]
	public string? OriginAuthorNamespace { get; set; }

	/// <summary>
	/// Specifies if the Applies To function is changed from origin or not
	/// </summary>
	[DataMember(Name = "isChangedFromOrigin", IsRequired = false)]
	public bool IsChangedFromOrigin { get; set; }

	/// <summary>
	/// Specifies the audited registry Id
	/// </summary>
	[DataMember(Name = "auditedRegistryId", IsRequired = false)]
	public string? AuditedRegistryId { get; set; }

	/// <summary>
	/// Specifies the target lineage Id
	/// </summary>
	[DataMember(Name = "targetLineageId", IsRequired = false)]
	public string? TargetLineageId { get; set; }

	/// <summary>
	/// DataSources | EventSources | PropertySources | ConfigSources | LogSources | TopologySources | Jobmonitors | AppliesTo Functions | SNMP SysOID Maps\nThe type of LogicModule
	/// </summary>
	[DataMember(Name = "logicModuleType")]
	[JsonConverter(typeof(StringEnumConverter))]
	public LogicModuleType LogicModuleType { get; set; }

	/// <summary>
	/// Specifies if the Applies To function is changed from target last published or not
	/// </summary>
	[DataMember(Name = "isChangedFromTargetLastPublished", IsRequired = false)]
	public bool IsChangedFromTargetLastPublished { get; set; }

	/// <summary>
	/// The origin lineage Id of the LMmodule
	/// </summary>
	[DataMember(Name = "originLineageId", IsRequired = false)]
	public string? OriginLineageId { get; set; }

	/// <summary>
	/// Specifies the origin Author companies unique Id
	/// </summary>
	[DataMember(Name = "originAuthorCompanyUUID", IsRequired = false)]
	public string? OriginAuthorCompanyUUID { get; set; }

	/// <summary>
	/// The LocalModule Id
	/// </summary>
	[DataMember(Name = "localModuleId")]
	public int LocalModuleId { get; set; }

	/// <summary>
	/// The Registry ID of the Exchange Integration this module is based from
	/// </summary>
	[DataMember(Name = "originRegistryId", IsRequired = false)]
	public string? OriginRegistryId { get; set; }

	/// <summary>
	/// Specifies the origin version
	/// </summary>
	[DataMember(Name = "originVersion", IsRequired = false)]
	public string? OriginVersion { get; set; }

	/// <summary>
	/// Specifies the audited registry version
	/// </summary>
	[DataMember(Name = "auditedVersion", IsRequired = false)]
	public string? AuditedVersion { get; }
}