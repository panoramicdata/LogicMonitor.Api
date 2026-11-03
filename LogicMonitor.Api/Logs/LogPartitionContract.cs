namespace LogicMonitor.Api.Logs;

/// <summary>
/// A log partition usage contract
/// </summary>
[DataContract]
public class LogPartitionContract
{
	/// <summary>
	/// Contract Id
	/// </summary>
	[SantabaReadOnly]
	[DataMember(Name = "id")]
	public int Id { get; set; }

	/// <summary>
	/// Auto restart ingestion on renewal
	/// </summary>
	[DataMember(Name = "autoRestartOnRenewal")]
	public bool AutoRestartOnRenewal { get; set; }

	/// <summary>
	/// Contract interval in hours: 0 = monthly (1st of next month), 24 = daily, 168 = weekly (next Monday), or custom hours
	/// </summary>
	[DataMember(Name = "contractIntervalHours")]
	public int ContractIntervalHours { get; set; }

	/// <summary>
	/// Contract start time in epoch seconds
	/// </summary>
	[SantabaReadOnly]
	[DataMember(Name = "fromEpoch")]
	public long FromEpoch { get; set; }

	/// <summary>
	/// Timestamp when usage limit was reached
	/// </summary>
	[SantabaReadOnly]
	[DataMember(Name = "limitReachedAt")]
	public long LimitReachedAt { get; set; }

	/// <summary>
	/// Retention period in days
	/// </summary>
	[DataMember(Name = "retention")]
	public int Retention { get; set; }

	/// <summary>
	/// Subscription SKU
	/// </summary>
	[DataMember(Name = "sku")]
	public string Sku { get; set; } = string.Empty;

	/// <summary>
	/// Contract state (active, future, expired)
	/// </summary>
	[SantabaReadOnly]
	[DataMember(Name = "state")]
	public string State { get; set; } = string.Empty;

	/// <summary>
	/// Stop ingestion when limit exceeded
	/// </summary>
	[DataMember(Name = "stopIngestionOnLimit")]
	public bool StopIngestionOnLimit { get; set; }

	/// <summary>
	/// Contract end time in epoch seconds
	/// </summary>
	[SantabaReadOnly]
	[DataMember(Name = "toEpoch")]
	public long ToEpoch { get; set; }

	/// <summary>
	/// Usage limit
	/// </summary>
	[DataMember(Name = "usageLimit")]
	public string UsageLimit { get; set; } = string.Empty;
}
