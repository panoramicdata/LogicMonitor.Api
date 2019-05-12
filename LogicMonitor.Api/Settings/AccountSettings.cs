using System.Collections.Generic;
using System.Runtime.Serialization;

namespace LogicMonitor.Api.Settings
{
	/// <summary>
	///     Account settings
	/// </summary>
	[DataContract]
	public class AccountSettings : IHasSingletonEndpoint
	{
		/// <summary>
		///     Whether the alertTotal including any in Ack
		/// </summary>
		[DataMember(Name = "alertTotalIncludeInAck")]
		public bool AlertTotalIncludingAnyInAck { get; set; }

		/// <summary>
		///     Alert total count, including those in SDT
		/// </summary>
		[DataMember(Name = "alertTotalIncludeInSdt")]
		public bool AlertTotalIncludingAnyInSdt { get; set; }

		/// <summary>
		///     The current AWS device count
		/// </summary>
		[DataMember(Name = "numOfAWSDevices")]
		public int AwsDeviceCount { get; set; }

		/// <summary>
		///     The current Azure device count
		/// </summary>
		[DataMember(Name = "numOfAzureDevices")]
		public int AzureDeviceCount { get; set; }

		/// <summary>
		///     Combined AWS Device count
		/// </summary>
		[DataMember(Name = "numOfCombinedAWSDevices")]
		public int CombinedAwsDeviceCount { get; set; }

		/// <summary>
		///     Combined Azure Device count
		/// </summary>
		[DataMember(Name = "numOfCombinedAzureDevices")]
		public int CombinedAzureDeviceCount { get; set; }

		/// <summary>
		///     Combined GCP Device count
		/// </summary>
		[DataMember(Name = "numOfCombinedGcpDevices")]
		public int CombinedGcpDeviceCount { get; set; }

		/// <summary>
		///     Company display name
		/// </summary>
		[DataMember(Name = "companyDisplayName")]
		public string CompanyDisplayName { get; set; }

		/// <summary>
		///     The current ConfigSource device count
		/// </summary>
		[DataMember(Name = "numOfConfigSourceDevices")]
		public int ConfigSourceDeviceCount { get; set; }

		/// <summary>
		///     Contacts
		/// </summary>
		[DataMember(Name = "contacts")]
		public List<Contact> Contacts { get; set; }

		/// <summary>
		///     Whether to destroy the account
		/// </summary>
		[DataMember(Name = "destroyAccount")]
		public bool DestroyAccount { get; set; }

		/// <summary>
		///     When to destroy the account
		/// </summary>
		[DataMember(Name = "destroyOnLocal")]
		public string DestroyOnLocal { get; set; }

		/// <summary>
		///     The current device count
		/// </summary>
		[DataMember(Name = "numberOfDevices")]
		public int DeviceCount { get; set; }

		/// <summary>
		///     Whether remote sessions are enabled
		/// </summary>
		[DataMember(Name = "enableRemoteSession")]
		public bool EnableRemoteSession { get; set; }

		/// <summary>GCP Device count
		/// </summary>
		[DataMember(Name = "numOfGcpDevices")]
		public int GcpDeviceCount { get; set; }

		/// <summary>
		/// The account balance in USD
		/// </summary>
		[DataMember(Name = "zuoraInvoiceDetails")]
		public InvoiceDetails InvoiceDetails { get; set; }

		/// <summary>
		///     The Kubernetes device count
		/// </summary>
		[DataMember(Name = "numberOfKubernetesDevices")]
		public int KubernetesDeviceCount { get; set; }

		/// <summary>
		///     Light Device count
		/// </summary>
		[DataMember(Name = "numberOfLightDevices")]
		public int LightDeviceCount { get; set; }

		/// <summary>
		///     The parent billing account
		/// </summary>
		[DataMember(Name = "parentBilling")]
		public string ParentBillingAccount { get; set; }

		/// <summary>
		/// The account number
		/// </summary>
		[DataMember(Name = "zuora")]
		public PaymentInformation PaymentInformation { get; set; }

		/// <summary>
		///     Primary contact e-mail address
		/// </summary>
		[DataMember(Name = "email")]
		public string PrimaryContactEmailAddress { get; set; }

		/// <summary>
		///     Primary contact name
		/// </summary>
		[DataMember(Name = "name")]
		public string PrimaryContactName { get; set; }

		/// <summary>
		///     Primary contact phone number
		/// </summary>
		[DataMember(Name = "phone")]
		public string PrimaryContactPhoneNumber { get; set; }

		/// <summary>
		///     Whether remote sessions are enabled
		/// </summary>
		[DataMember(Name = "requireTwoFA")]
		public bool RequireTwoFactorAuthentication { get; set; }

		/// <summary>
		///     The service count
		/// </summary>
		[DataMember(Name = "numOfServices")]
		public int ServiceCount { get; set; }

		/// <summary>
		///     The session timeout in seconds
		/// </summary>
		[DataMember(Name = "sessionTimeoutInSeconds")]
		public int SessionTimeoutSeconds { get; set; }

		/// <summary>
		///     Standard Device count
		/// </summary>
		[DataMember(Name = "numberOfStandardDevices")]
		public int StandardDeviceCount { get; set; }

		/// <summary>
		///     Stopped AWS Device count
		/// </summary>
		[DataMember(Name = "numOfStoppedAWSDevices")]
		public int StoppedAwsDeviceCount { get; set; }

		/// <summary>
		///     Stopped Azure Device count
		/// </summary>
		[DataMember(Name = "numOfStoppedAzureDevices")]
		public int StoppedAzureDeviceCount { get; set; }

		/// <summary>
		///     Stopped GCP Device count
		/// </summary>
		[DataMember(Name = "numOfStoppedGcpDevices")]
		public int StoppedGcpDeviceCount { get; set; }

		/// <summary>
		///     Terminated AWS Device count
		/// </summary>
		[DataMember(Name = "numOfTerminatedAWSDevices")]
		public int TerminatedAwsDeviceCount { get; set; }

		/// <summary>
		///     Terminated Azure Device count
		/// </summary>
		[DataMember(Name = "numOfTerminatedAzureDevices")]
		public int TerminatedAzureDeviceCount { get; set; }

		/// <summary>
		///     Terminated GCP Device count
		/// </summary>
		[DataMember(Name = "numOfTerminatedGcpCloudDevices")]
		public int TerminatedGcpDeviceCount { get; set; }

		/// <summary>
		///     Timezone as text
		/// </summary>
		[DataMember(Name = "timezone")]
		public string TimeZone { get; set; }

		/// <summary>
		///     The web site count
		/// </summary>
		[DataMember(Name = "numOfWebsites")]
		public int WebsiteCount { get; set; }

		/// <summary>
		///     IP Whitelist
		/// </summary>
		[DataMember(Name = "whiteList")]
		public string WhiteList { get; set; }

		/// <summary>
		///     The endpoint
		/// </summary>
		/// <returns></returns>
		public string Endpoint() => "setting/companySetting";
	}
}