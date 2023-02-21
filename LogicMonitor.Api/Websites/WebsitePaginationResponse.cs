namespace LogicMonitor.Api.Websites;

/// <summary>
/// WebsitePaginationResponse
/// </summary>

[DataContract]
public class WebsitePaginationResponse
{
	/// <summary>
	/// total
	/// </summary>
	[DataMember(Name = "total", IsRequired = false)]
	public int Total { get; set; }

	/// <summary>
	/// searchId
	/// </summary>
	[DataMember(Name = "searchId", IsRequired = false)]
	public string? SearchId { get; set; }

	/// <summary>
	/// items
	/// </summary>
	[DataMember(Name = "items", IsRequired = false)]
	public Website[]? Items { get; set; }
}
