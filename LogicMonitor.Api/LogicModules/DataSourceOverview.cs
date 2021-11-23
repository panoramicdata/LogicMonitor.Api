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
	/// The group Id
	/// </summary>
	[DataMember(Name = "displayName")]
	public string DisplayName { get; set; }

	/// <summary>
	/// The group Id
	/// </summary>
	[DataMember(Name = "alias")]
	public string Alias { get; set; }

	/// <summary>
	/// The group Id
	/// </summary>
	[DataMember(Name = "info")]
	public string Info { get; set; }

	/// <summary>
	/// The group Id
	/// </summary>
	[DataMember(Name = "lockDescription")]
	public bool LockDescription { get; set; }

	/// <summary>
	/// The group Id
	/// </summary>
	[DataMember(Name = "deviceId")]
	public int DeviceId { get; set; }

	/// <summary>
	/// The group Id
	/// </summary>
	[DataMember(Name = "deviceDescription")]
	public string DeviceDescription { get; set; }

	/// <summary>
	/// The group Id
	/// </summary>
	[DataMember(Name = "dataSourceId")]
	public int DataSourceId { get; set; }

	/// <summary>
	/// The group Id
	/// </summary>
	[DataMember(Name = "deviceDataSourceId")]
	public int DeviceDataSourceId { get; set; }

	/// <summary>
	/// The group Id
	/// </summary>
	[DataMember(Name = "wildValue")]
	public string WildValue { get; set; }

	/// <summary>
	/// The group Id
	/// </summary>
	[DataMember(Name = "wildValue2")]
	public string WildValue2 { get; set; }

	/// <summary>
	/// The group Id
	/// </summary>
	[DataMember(Name = "disableAlerting")]
	public bool DisableAlerting { get; set; }

	/// <summary>
	/// The group Id
	/// </summary>
	[DataMember(Name = "hasAlertDisabledDataPoints")]
	public bool HasAlertDisabledDataPoints { get; set; }

	/// <summary>
	/// The group Id
	/// </summary>
	[DataMember(Name = "stopMonitoring")]
	public bool StopMonitoring { get; set; }

	/// <summary>
	/// The group Id
	/// </summary>
	[DataMember(Name = "alertStatus")]
	public AlertStatus AlertStatus { get; set; }

	/// <summary>
	/// The group Id
	/// </summary>
	[DataMember(Name = "sdtStatus")]
	public SdtStatus SdtStatus { get; set; }

	/// <summary>
	/// The group Id
	/// </summary>
	[DataMember(Name = "alertDisableStatus")]
	public AlertDisableStatus AlertDisableStatus { get; set; }

	/// <summary>
	/// The group Id
	/// </summary>
	[DataMember(Name = "sdtAt")]
	public string SdtAt { get; set; }

	/// <summary>
	/// ToString override
	/// </summary>
	/// <returns>'{Description ?? Name} ({Id})'</returns>
	public override string ToString() => $"{Description ?? Name} ({Id})";
}
