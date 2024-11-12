namespace LogicMonitor.Api.Settings;

/// <summary>
///     A Connectwise Manage integration
/// </summary>
[DataContract]
public class ConnectwiseManageIntegration : HttpIntegration
{
	/// <summary>
	/// The Server
	/// </summary>
	[DataMember(Name = "connectwiseServer")]
	public string ConnectwiseServer { get; set; } = string.Empty;

	/// <summary>
	/// The Company
	///	</summary>
	[DataMember(Name = "connectwiseCompany")]
	public string ConnectwiseCompany { get; set; } = string.Empty;

	/// <summary>
	/// The Public Key
	/// </summary>
	[DataMember(Name = "publicKey")]
	public string PublicKey { get; set; } = string.Empty;

	/// <summary>
	/// The Private Key
	/// </summary>
	[DataMember(Name = "privateKey")]
	public string PrivateKey { get; set; } = string.Empty;

	/// <summary>
	/// How the connection is authenticated
	/// </summary>
	[DataMember(Name = "connectWiseAuthenticated")]
	public bool ConnectWiseAuthenticated { get; set; }

	/// <summary>
	/// The Service Board Id
	/// </summary>
	[DataMember(Name = "serviceBoardId")]
	public int ServiceBoardId { get; set; }

	/// <summary>
	/// The Service Team Id
	/// </summary>
	[DataMember(Name = "serviceTeamId")]
	public int ServiceTeamId { get; set; }

	/// <summary>
	/// The default company ID to use if none is specified in the alert
	/// </summary>
	[DataMember(Name = "connectWiseCompanyId")]
	public int DefaultCompanyId { get; set; }

	/// <summary>
	/// The ticket type ID
	/// </summary>
	[DataMember(Name = "ticketTypeId")]
	public int TicketTypeId { get; set; }

	/// <summary>
	/// The warn priority ID
	/// </summary>
	[DataMember(Name = "warnPriorityId")]
	public int WarnPriorityId { get; set; }

	/// <summary>
	/// The error priority ID
	/// </summary>
	[DataMember(Name = "errorPriorityId")]
	public int ErrorPriorityId { get; set; }

	/// <summary>
	/// The critical priority ID
	/// </summary>
	[DataMember(Name = "criticalPriorityId")]
	public int CriticalPriorityId { get; set; }

	/// <summary>
	/// The status to use for new tickets
	/// </summary>
	[DataMember(Name = "statusNewTicket")]
	public int StatusNewTicket { get; set; }

	/// <summary>
	/// The status to use for updated tickets
	/// </summary>
	[DataMember(Name = "statusUpdateTicket")]
	public int StatusUpdateTicket { get; set; }

	/// <summary>
	/// The status to use for clear tickets
	/// </summary>
	[DataMember(Name = "statusClearTicket")]
	public int StatusClearTicket { get; set; }

	/// <summary>
	/// The status to use for acknowledged tickets
	/// </summary>
	[DataMember(Name = "statusAckTicket")]
	public int StatusAckTicket { get; set; }
}

