namespace LogicMonitor.Api.LogicModules;

/// <summary>
/// IntegrationMetadata
/// </summary>
[DataContract]
public class IntegrationMetadata
{
	/// <summary>
	/// Specifies the target last published Id
	/// </summary>
	[DataMember(Name = "targetLastPublishedId")]
	public string? TargetLastPublishedId { get; set; }

	/// <summary>
	/// The metadata checksum for the target last published LMModule content
	/// </summary>
	[DataMember(Name = "targetLastPublishedChecksum")]
	public string? TargetLastPublishedChecksum { get; set; }

	/// <summary>
	/// Specifies the target last published version
	/// </summary>
	[DataMember(Name = "targetLastPublishedVersion")]
	public string? TargetLastPublishedVersion { get; set; }

	/// <summary>
	/// The metadata checksum for the LMModule content
	/// </summary>
	[DataMember(Name = "originChecksum")]
	public string? OriginChecksum { get; set; }

	/// <summary>
	/// Specifies the origin Author companies namespace
	/// </summary>
	[DataMember(Name = "originAuthorNamespace")]
	public string? OriginAuthorNamespace { get; set; }

	/// <summary>
	/// Specifies if the Applies To function is changed from origin or not
	/// </summary>
	[DataMember(Name = "isChangedFromOrigin")]
	public bool IsChangedFromOrigin { get; set; }

	/// <summary>
	/// Specifies the audited registry Id
	/// </summary>
	[DataMember(Name = "auditedRegistryId")]
	public string? AuditedRegistryId { get; set; }

	/// <summary>
	/// Specifies the target lineage Id
	/// </summary>
	[DataMember(Name = "targetLineageId")]
	public string? TargetLineageId { get; set; }

	/// <summary>
	/// DataSources | EventSources | PropertySources | ConfigSources | LogSources | TopologySources | Jobmonitors | AppliesTo Functions | SNMP SysOID Maps\nThe type of LogicModule
	/// </summary>
	[DataMember(Name = "logicModuleType")]
	public string? LogicModuleType { get; set; }

	/// <summary>
	/// Specifies if the Applies To function is changed from target last published or not
	/// </summary>
	[DataMember(Name = "isChangedFromTargetLastPublished")]
	public bool IsChangedFromTargetLastPublished { get; set; }

	/// <summary>
	/// The origin lineage Id of the LMmodule
	/// </summary>
	[DataMember(Name = "originLineageId")]
	public string? OriginLineageId { get; set; }

	/// <summary>
	/// Specifies the origin Author companies unique Id
	/// </summary>
	[DataMember(Name = "originAuthorCompanyUUID")]
	public string? OriginAuthorCompanyUUID { get; set; }

	/// <summary>
	/// The LocalModule Id
	/// </summary>
	[DataMember(Name = "localModuleId")]
	public int LocalModuleId { get; set; }

	/// <summary>
	/// The Registry ID of the Exchange Integration this module is based from
	/// </summary>
	[DataMember(Name = "originRegistryId")]
	public string? OriginRegistryId { get; set; }

	/// <summary>
	/// Specifies the origin version
	/// </summary>
	[DataMember(Name = "originVersion")]
	public string? OriginVersion { get; set; }

	/// <summary>
	/// Specifies the audited registry version
	/// </summary>
	[DataMember(Name = "auditedVersion")]
	public string? AuditedVersion { get;}
}