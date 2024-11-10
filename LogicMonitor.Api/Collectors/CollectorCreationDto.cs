namespace LogicMonitor.Api.Collectors;

/// <summary>
///     A LogicMonitor Collector
/// </summary>
[DataContract]
public class CollectorCreationDto : CreationDto<Collector>, IHasDescription
{
	/// <summary>
	///     The Collector's description
	/// </summary>
	[DataMember(Name = "description")]
	public string Description { get; set; } = string.Empty;

	/// <summary>
	///     The Id of the failover Collector configured for this Collector
	/// </summary>
	[DataMember(Name = "backupAgentId")]
	public int BackupCollectorId { get; set; }

	/// <summary>
	///     Whether or not automatic fail back is enabled for the Collector
	/// </summary>
	[DataMember(Name = "enableFailBack")]
	public bool? EnableFailBack { get; set; }

	/// <summary>
	///     The interval, in minutes, after which alert notifications for the Collector will be resent
	/// </summary>
	[DataMember(Name = "resendIval")]
	public int? ResentIntervalMinutes { get; set; } = 15;

	/// <summary>
	///     Whether alert clear notifications are suppressed for the Collector
	/// </summary>
	[DataMember(Name = "suppressAlertClear")]
	public bool SuppressAlertClear { get; set; }

	/// <summary>
	///     The Id of the escalation chain associated with this Collector
	/// </summary>
	[DataMember(Name = "escalatingChainId")]
	public int EscalationChainId { get; set; }

	/// <summary>
	///     The Id of the group the Collector is in
	/// </summary>
	[DataMember(Name = "collectorGroupId")]
	public int CollectorGroupId { get; set; } = 1;

	/// <summary>
	///     Whether or not the Resource the Collector is installed on is enabled for fail over
	/// </summary>
	[DataMember(Name = "enableFailOverOnCollectorDevice")]
	public bool? EnableFailOverOnCollectorResource { get; set; } // TODO - use the AutomaticUpgradeInfo class

	/// <summary>
	///     Whether or not the Resource the Collector is installed on should be automatically added into monitoring
	/// </summary>
	[DataMember(Name = "needAutoCreateCollectorDevice")]
	public bool? NeedAutoCreateCollectorResource { get; set; } = true;

	/// <summary>
	///     The next recipient (leave as 0)
	/// </summary>
	[DataMember(Name = "nextRecipient")]
	public int NextRecipient { get; set; }

	/// <summary>
	///     The specified Collector ResourceGroup Id
	/// </summary>
	[DataMember(Name = "specifiedCollectorDeviceGroupId")]
	public int SpecifiedCollectorResourceGroupId { get; set; }
}
