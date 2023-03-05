namespace LogicMonitor.Api.Dashboards;

/// <summary>
/// DashboardData
/// </summary>
[DataContract]
public class DashboardData
{
	/// <summary>
	/// The permission of the user that made the API call
	/// </summary>
	[DataMember(Name = "userPermission")]
	public string UserPermission { get; set; } = string.Empty;

	/// <summary>
	/// The name of the dashboard
	/// </summary>
	[DataMember(Name = "name")]
	public string Name { get; set; } = string.Empty;

	/// <summary>
	/// Whether or not the dashboard is sharable. This value will always be true unless the dashboard is a private dashboard
	/// </summary>
	[DataMember(Name = "sharable")]
	public string Sharable { get; set; } = string.Empty;

	/// <summary>
	/// The Id of the dashboard
	/// </summary>
	[DataMember(Name = "id")]
	public int Id { get; set; }
}
