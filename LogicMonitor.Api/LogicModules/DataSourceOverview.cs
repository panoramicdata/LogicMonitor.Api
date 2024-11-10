namespace LogicMonitor.Api.LogicModules;

/// <summary>
/// DataSourceOverview
/// </summary>
[DataContract]
public class DataSourceOverview : NamedItem
{
	/// <summary>
	/// The group Id
	/// </summary>
	[DataMember(Name = "groupId")]
	public int GroupId { get; set; }

	/// <summary>
	/// The DisplayName
	/// </summary>
	[DataMember(Name = "displayName")]
	public string DisplayName { get; set; } = string.Empty;

	/// <summary>
	/// The Alias
	/// </summary>
	[DataMember(Name = "alias")]
	public string Alias { get; set; } = string.Empty;

	/// <summary>
	/// The Info
	/// </summary>
	[DataMember(Name = "info")]
	public string Info { get; set; } = string.Empty;

	/// <summary>
	/// The LockDescription
	/// </summary>
	[DataMember(Name = "lockDescription")]
	public bool LockDescription { get; set; }

	/// <summary>
	/// The Resource Id
	/// </summary>
	[DataMember(Name = "deviceId")]
	public int ResourceId { get; set; }

	/// <summary>
	/// The Resource description
	/// </summary>
	[DataMember(Name = "deviceDescription")]
	public string ResourceDescription { get; set; } = string.Empty;

	/// <summary>
	/// The DataSourceId
	/// </summary>
	[DataMember(Name = "dataSourceId")]
	public int DataSourceId { get; set; }

	/// <summary>
	/// The ResourceDataSourceId
	/// </summary>
	[DataMember(Name = "deviceDataSourceId")]
	public int ResourceDataSourceId { get; set; }

	/// <summary>
	/// The WildValue
	/// </summary>
	[DataMember(Name = "wildValue")]
	public string WildValue { get; set; } = string.Empty;

	/// <summary>
	/// The WildValue2
	/// </summary>
	[DataMember(Name = "wildValue2")]
	public string WildValue2 { get; set; } = string.Empty;

	/// <summary>
	/// The DisableAlerting
	/// </summary>
	[DataMember(Name = "disableAlerting")]
	public bool DisableAlerting { get; set; }

	/// <summary>
	/// The HasAlertDisabledDataPoints
	/// </summary>
	[DataMember(Name = "hasAlertDisabledDataPoints")]
	public bool HasAlertDisabledDataPoints { get; set; }

	/// <summary>
	/// The StopMonitoring
	/// </summary>
	[DataMember(Name = "stopMonitoring")]
	public bool StopMonitoring { get; set; }

	/// <summary>
	/// The AlertStatus
	/// </summary>
	[DataMember(Name = "alertStatus")]
	public AlertStatus AlertStatus { get; set; }

	/// <summary>
	/// The SdtStatus
	/// </summary>
	[DataMember(Name = "sdtStatus")]
	public SdtStatus SdtStatus { get; set; }

	/// <summary>
	/// The AlertDisableStatus
	/// </summary>
	[DataMember(Name = "alertDisableStatus")]
	public AlertDisableStatus AlertDisableStatus { get; set; }

	/// <summary>
	/// The SdtAt
	/// </summary>
	[DataMember(Name = "sdtAt")]
	public string SdtAt { get; set; } = string.Empty;

	/// <summary>
	/// ToString override
	/// </summary>
	/// <returns>'{Description ?? Name} ({Id})'</returns>
	public override string ToString() => $"{Description ?? Name} ({Id})";
}
