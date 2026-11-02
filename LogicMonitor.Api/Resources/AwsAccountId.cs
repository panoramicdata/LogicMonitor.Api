namespace LogicMonitor.Api.Resources;

/// <summary>
/// The LogicMonitor AWS account ID
/// </summary>
[DataContract]
public class AwsAccountId
{
	/// <summary>
	/// The account id
	/// </summary>
	[DataMember(Name = "accountId")]
	public string AccountId { get; set; } = string.Empty;
}
