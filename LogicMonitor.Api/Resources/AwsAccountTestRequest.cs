namespace LogicMonitor.Api.Resources;

/// <summary>
/// A request to test AWS account credentials
/// </summary>
[DataContract]
public class AwsAccountTestRequest
{
	/// <summary>
	/// The access id
	/// </summary>
	[DataMember(Name = "accessId")]
	public string AccessId { get; set; } = string.Empty;

	/// <summary>
	/// The access key
	/// </summary>
	[DataMember(Name = "accessKey")]
	public string AccessKey { get; set; } = string.Empty;

	/// <summary>
	/// The account id
	/// </summary>
	[DataMember(Name = "accountId")]
	public string AccountId { get; set; } = string.Empty;

	/// <summary>
	/// The assumed role arn
	/// </summary>
	[DataMember(Name = "assumedRoleArn")]
	public string AssumedRoleArn { get; set; } = string.Empty;

	/// <summary>
	/// The bucket name for billing files
	/// </summary>
	[DataMember(Name = "bucketNameForBillingFiles")]
	public string BucketNameForBillingFiles { get; set; } = string.Empty;

	/// <summary>
	/// The checked services
	/// </summary>
	[DataMember(Name = "checkedServices")]
	public string CheckedServices { get; set; } = string.Empty;

	/// <summary>
	/// The external id
	/// </summary>
	[DataMember(Name = "externalId")]
	public string ExternalId { get; set; } = string.Empty;

	/// <summary>
	/// The group id
	/// </summary>
	[DataMember(Name = "groupId")]
	public int GroupId { get; set; }

	/// <summary>
	/// The is billing accept opt in warnings
	/// </summary>
	[DataMember(Name = "isBillingAcceptOptInWarnings")]
	public bool IsBillingAcceptOptInWarnings { get; set; }

	/// <summary>
	/// The new billing bucket
	/// </summary>
	[DataMember(Name = "newBillingBucket")]
	public string NewBillingBucket { get; set; } = string.Empty;

	/// <summary>
	/// The new billing path
	/// </summary>
	[DataMember(Name = "newBillingPath")]
	public string NewBillingPath { get; set; } = string.Empty;
}
