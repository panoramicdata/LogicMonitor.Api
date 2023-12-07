namespace LogicMonitor.Api.Websites;

/// <summary>
/// A Website folder
/// </summary>
[DataContract]
public class WebsiteFolder : NamedItem
{
	/// <summary>
	/// Whether alerting is disabled
	/// </summary>
	[DataMember(Name = "disableAlerting")]
	public bool DisableAlerting { get; set; }

	/// <summary>
	/// Whether there are alerts within this folder
	/// </summary>
	[DataMember(Name = "hasAlerts")]
	public bool HasAlerts { get; set; }

	/// <summary>
	/// Whether websites alert is disabled
	/// </summary>
	[DataMember(Name = "hasWebsitesAlertDisabled")]
	public bool HasWebsitesAlertDisabled { get; set; }

	/// <summary>
	/// Whether websites are disabled
	/// </summary>
	[DataMember(Name = "hasWebsitesDisabled")]
	public bool HasWebsitesDisabled { get; set; }

	/// <summary>
	/// Whether has websites SDT
	/// </summary>
	[DataMember(Name = "hasWebsitesSdt")]
	public bool HasWebsitesSdt { get; set; }

	/// <summary>
	/// Whether there are unacknowledged alerts within this folder
	/// </summary>
	[DataMember(Name = "hasUnackedAlerts")]
	public bool HasUnackedAlerts { get; set; }

	/// <summary>
	/// Whether is in SDT
	/// </summary>
	[DataMember(Name = "inSDT")]
	public bool InSdt { get; set; }

	/// <summary>
	/// Whether monitoring is stopped
	/// </summary>
	[DataMember(Name = "stopMonitoring")]
	public bool StopMonitoring { get; set; }

	/// <summary>
	/// Child groups
	/// </summary>
	[DataMember(Name = "subGroups")]
	public List<WebsiteFolder> SubGroups { get; set; } = [];

	/// <summary>
	/// Child Websites
	/// </summary>
	[DataMember(Name = "websites")]
	public List<Website> Websites { get; set; } = [];
}
