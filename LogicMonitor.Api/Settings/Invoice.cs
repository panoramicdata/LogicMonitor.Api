namespace LogicMonitor.Api.Settings;

/// <summary>
/// An invoice
/// </summary>
[DataContract]
public class Invoice
{
	/// <summary>
	/// The account id
	/// </summary>
	[DataMember(Name = "accountId")]
	public string AccountId { get; set; } = string.Empty;

	/// <summary>
	/// The amount in USD
	/// </summary>
	[DataMember(Name = "amount")]
	public decimal AmountUsd { get; set; }

	/// <summary>
	/// The balance in USD
	/// </summary>
	[DataMember(Name = "balance")]
	public decimal BalanceUsd { get; set; }

	/// <summary>
	/// The created date
	/// </summary>
	[DataMember(Name = "createdDate")]
	public string CreatedDate { get; set; } = string.Empty;

	/// <summary>
	/// The due date
	/// </summary>
	[DataMember(Name = "dueDate")]
	public string DueDate { get; set; } = string.Empty;

	/// <summary>
	/// The invoice date
	/// </summary>
	[DataMember(Name = "invoiceDate")]
	public string InvoiceDate { get; set; } = string.Empty;

	/// <summary>
	/// The invoice number
	/// </summary>
	[DataMember(Name = "invoiceId")]
	public string InvoiceId { get; set; } = string.Empty;

	/// <summary>
	/// The invoice status
	/// </summary>
	[DataMember(Name = "invoiceStatus")]
	public string InvoiceStatus { get; set; } = string.Empty;

	/// <summary>
	/// The invoice number
	/// </summary>
	[DataMember(Name = "invoiceNumber")]
	public string InvoiceNumber { get; set; } = string.Empty;

	/// <summary>
	/// The payment amount
	/// </summary>
	[DataMember(Name = "paymentAmount")]
	public decimal PaymentAmountUsd { get; set; }

	/// <summary>
	/// The posted date
	/// </summary>
	[DataMember(Name = "postedDate")]
	public string PostedDate { get; set; } = string.Empty;

	/// <summary>
	/// The target date
	/// </summary>
	[DataMember(Name = "targetDate")]
	public string TargetDate { get; set; } = string.Empty;

	/// <summary>
	/// The updated date
	/// </summary>
	[DataMember(Name = "updatedDate")]
	public string UpdatedDate { get; set; } = string.Empty;
}
