using System.Runtime.Serialization;

namespace LogicMonitor.Api.Settings
{
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
		public PaymentInformation PaymentInformation { get; set; }

		/// <summary>
		/// The account balance in USD
		/// </summary>
		[DataMember(Name = "zuoraInvoiceDetails")]
		public InvoiceDetails InvoiceDetails { get; set; }

		/// <summary>
		///     The endpoint
		/// </summary>
		/// <returns></returns>
		public string Endpoint() => "setting/zuora/info";
	}
}