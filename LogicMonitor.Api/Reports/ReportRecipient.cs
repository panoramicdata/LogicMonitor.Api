namespace LogicMonitor.Api.Reports;

/// <summary>
/// A report recipient
/// </summary>
[DataContract]
public class ReportRecipient
{
	/// <summary>
	/// Where admin refers to a user in the account and arbitrary refers to an email address not associated with a user account.Acceptable values are: admin, arbitrary, group
	/// </summary>
	[DataMember(Name = "type")]
	public string Type { get; set; } = null!;

	/// <summary>
	/// This should always be email
	/// </summary>
	[DataMember(Name = "method")]
	public string? Method { get; set; }

	/// <summary>
	/// This should be a username if type\u003dadmin, or an email address if type\u003darbitrary
	/// </summary>
	[DataMember(Name = "addr")]
	public string Address { get; set; } = null!;

	/// <summary>
	/// If the type is admin and the method is email, the field should indicate the actual email address of the admin
	/// </summary>
	[DataMember(Name = "additionInfo")]
	public string? AdditionInfo { get; set; }
}
