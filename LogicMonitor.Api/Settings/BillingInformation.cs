namespace LogicMonitor.Api.Settings;

/// <summary>
/// Billing information
/// </summary>
[DataContract]
public class BillingInformation : IHasSingletonEndpoint
{
	/// <summary>
	/// The account number
	/// </summary>
	[DataMember(Name = "zuora")]
	public PaymentInformation PaymentInformation { get; set; } = new();

	/// <summary>
	/// The account balance in USD
	/// </summary>
	[DataMember(Name = "zuoraInvoiceDetails")]
	public InvoiceDetails InvoiceDetails { get; set; } = new();

	/// <summary>
	///     The endpoint
	/// </summary>
	public string Endpoint() => "setting/zuora/info";
}
