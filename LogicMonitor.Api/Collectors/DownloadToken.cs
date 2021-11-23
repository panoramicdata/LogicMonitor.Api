namespace LogicMonitor.Api.Collectors;

/// <summary>
/// Download token
/// </summary>
[DataContract]
public class DownloadToken
{
	/// <summary>
	/// The token string
	/// </summary>
	[DataMember(Name = "token")]
	public string Token { get; set; }
}
