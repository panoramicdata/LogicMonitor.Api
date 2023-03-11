namespace LogicMonitor.Api.Settings;

/// <summary>
/// Payment information
/// </summary>
[DataContract]
public class PaymentInformation
{
	/// <summary>
	/// The AccountPaymentTerm
	/// </summary>
	[DataMember(Name = "accountPaymentTerm")]
	public string AccountPaymentTerm { get; set; } = string.Empty;

	/// <summary>
	/// The CardExpiryMonth
	/// </summary>
	[DataMember(Name = "cardExpMonth")]
	public int CardExpiryMonth { get; set; }

	/// <summary>
	/// The CardMaskNumber
	/// </summary>
	[DataMember(Name = "cardMaskNumber")]
	public string CardMaskNumber { get; set; } = string.Empty;

	/// <summary>
	/// The CardHolderName
	/// </summary>
	[DataMember(Name = "cardHolderName")]
	public string CardHolderName { get; set; } = string.Empty;

	/// <summary>
	/// The AccountBdc
	/// </summary>
	[DataMember(Name = "accountBDC")]
	public int AccountBdc { get; set; }

	/// <summary>
	/// The CreditCardType
	/// </summary>
	[DataMember(Name = "creditCardType")]
	public string CreditCardType { get; set; } = string.Empty;

	/// <summary>
	/// The UpdatedDate
	/// </summary>
	[DataMember(Name = "updatedDate")]
	public int UpdatedDate { get; set; }

	/// <summary>
	/// The AccountCrmId
	/// </summary>
	[DataMember(Name = "accountCrmId")]
	public string AccountCrmId { get; set; } = string.Empty;

	/// <summary>
	/// The CreditCardCountry
	/// </summary>
	[DataMember(Name = "creditCardCountry")]
	public string CreditCardCountry { get; set; } = string.Empty;

	/// <summary>
	/// The PaymentType
	/// </summary>
	[DataMember(Name = "paymentType")]
	public string PaymentType { get; set; } = string.Empty;

	/// <summary>
	/// The AccountStatus
	/// </summary>
	[DataMember(Name = "accountStatus")]
	public string AccountStatus { get; set; } = string.Empty;

	/// <summary>
	/// The AccountId
	/// </summary>
	[DataMember(Name = "accountId")]
	public string AccountId { get; set; } = string.Empty;

	/// <summary>
	/// The CardExpiryYear
	/// </summary>
	[DataMember(Name = "cardExpYear")]
	public int CardExpiryYear { get; set; }

	/// <summary>
	/// The AccountBillingEmail
	/// </summary>
	[DataMember(Name = "accountBillingEmail")]
	public string AccountBillingEmail { get; set; } = string.Empty;

	/// <summary>
	/// The CreditCardAddress1
	/// </summary>
	[DataMember(Name = "creditCardAddress1")]
	public string CreditCardAddress1 { get; set; } = string.Empty;

	/// <summary>
	/// The CreditCardAddress2
	/// </summary>
	[DataMember(Name = "creditCardAddress2")]
	public string CreditCardAddress2 { get; set; } = string.Empty;

	/// <summary>
	/// The CreditCardPostalCode
	/// </summary>
	[DataMember(Name = "creditCardPostalCode")]
	public string CreditCardPostalCode { get; set; } = string.Empty;

	/// <summary>
	/// The DefaultPaymentMethodId
	/// </summary>
	[DataMember(Name = "defaultPaymentMethodId")]
	public string DefaultPaymentMethodId { get; set; } = string.Empty;

	/// <summary>
	/// The CreditCardCity
	/// </summary>
	[DataMember(Name = "creditCardCity")]
	public string CreditCardCity { get; set; } = string.Empty;

	/// <summary>
	/// The Phone
	/// </summary>
	[DataMember(Name = "phone")]
	public string Phone { get; set; } = string.Empty;

	/// <summary>
	/// The AccountBalance
	/// </summary>
	[DataMember(Name = "accountBalance")]
	public decimal AccountBalance { get; set; }

	/// <summary>
	/// The CreditCardState
	/// </summary>
	[DataMember(Name = "creditCardState")]
	public string CreditCardState { get; set; } = string.Empty;

	/// <summary>
	/// The Email		/// </summary>
	[DataMember(Name = "email")]
	public string Email { get; set; } = string.Empty;
}
