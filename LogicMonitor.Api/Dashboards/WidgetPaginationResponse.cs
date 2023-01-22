namespace LogicMonitor.Api.Dashboards;

/// <summary>
///     A Dashboard widget
/// </summary>
[DataContract]
public class WidgetPaginationResponse : IHasEndpoint
{
	/// <summary>
	/// total
	/// </summary>
	[DataMember(Name = "total")]
	public int Total { get; set; }

	/// <summary>
	/// search id
	/// </summary>
	[DataMember(Name = "searchId")]
	public string SearchId { get; set; }

	/// <summary>
	/// items
	/// </summary>
	[DataMember(Name = "items")]
	public Widget[] Items { get; set; }

	/// <summary>
	///     The endpoint
	/// </summary>
	public string Endpoint() => "dashboard/widgets";
}
