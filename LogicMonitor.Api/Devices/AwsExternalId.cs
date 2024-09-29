namespace LogicMonitor.Api.Devices;

/// <summary>
/// AwsExternalId
/// </summary>
[DataContract]

public class AwsExternalId
{
	/// <summary>
	/// Created at
	/// </summary>
	[DataMember(Name = "createdAt")]
	public string CreatedAt { get; set; } = string.Empty;

	/// <summary>
	/// External id
	/// </summary>
	[DataMember(Name = "externalId")]
	public string ExternalId { get; set; } = string.Empty;
}
