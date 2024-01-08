namespace LogicMonitor.Api.Settings;

/// <summary>
/// AutoTask Integration Creation Dto
/// </summary>
[DataContract]
public class AutoTaskIntegrationCreationDto : IntegrationCreationDto<AutoTaskIntegration>
{
	/// <summary>
	/// Constructor
	/// </summary>
	public AutoTaskIntegrationCreationDto() : base("autotask")
	{
	}

	/// <summary>
	/// The zone
	/// </summary>
	[DataMember(Name = "zone")]
	public int Zone { get; set; }

	/// <summary>
	/// The accountId
	/// </summary>
	[DataMember(Name = "accountId")]
	public int AccountId { get; set; }

	/// <summary>
	/// The dueDateTime
	/// </summary>
	[DataMember(Name = "dueDateTime")]
	public string DueDateTime { get; set; } = string.Empty;

	/// <summary>
	/// The queueId
	/// </summary>
	[DataMember(Name = "queueId")]
	public int QueueId { get; set; }

	/// <summary>
	/// The warnPriority
	/// </summary>
	[DataMember(Name = "warnPriority")]
	public int WarnPriority { get; set; }

	/// <summary>
	/// The errorPriority
	/// </summary>
	[DataMember(Name = "errorPriority")]
	public int ErrorPriority { get; set; }

	/// <summary>
	/// The criticalPriority
	/// </summary>
	[DataMember(Name = "criticalPriority")]
	public int CriticalPriority { get; set; }

	/// <summary>
	/// The statusNewTicket
	/// </summary>
	[DataMember(Name = "statusNewTicket")]
	public int StatusNewTicket { get; set; }

	/// <summary>
	/// The statusUpdateTicket
	/// </summary>
	[DataMember(Name = "statusUpdateTicket")]
	public int StatusUpdateTicket { get; set; }

	/// <summary>
	/// The statusCloseTicket
	/// </summary>
	[DataMember(Name = "statusCloseTicket")]
	public int StatusCloseTicket { get; set; }

	/// <summary>
	/// The statusAckTicket
	/// </summary>
	[DataMember(Name = "statusAckTicket")]
	public int StatusAckTicket { get; set; }
}