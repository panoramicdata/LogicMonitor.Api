using System.Collections.Generic;
using System.Runtime.Serialization;

namespace LogicMonitor.Api.Settings
{
	/// <summary>
	/// Invoice details
	/// </summary>
	[DataContract]
	public class InvoiceDetails
	{
		/// <summary>
		/// The account number
		/// </summary>
		[DataMember(Name = "accountNumber")]
		public string AccountNumber { get; set; }

		/// <summary>
		/// The account balance in USD
		/// </summary>
		[DataMember(Name = "accountBalance")]
		public decimal AccountBalanceUsd { get; set; }

		/// <summary>
		/// The list of previous invoices
		/// </summary>
		[DataMember(Name = "previousInvoices")]
		public List<Invoice> PreviousInvoices { get; set; }

		/// <summary>
		/// The product name
		/// </summary>
		[DataMember(Name = "productName")]
		public string ProductName { get; set; }

		/// <summary>
		/// The rate plan name
		/// </summary>
		[DataMember(Name = "ratePlanName")]
		public string RatePlanName { get; set; }
	}
}