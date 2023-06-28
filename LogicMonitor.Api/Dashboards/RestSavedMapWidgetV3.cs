namespace LogicMonitor.Api.Dashboards;

/// <summary>
/// RestSavedMapWidgetV3
/// </summary>
[DataContract]
public class RestSavedMapWidgetV3 : Widget, IWidget
{
	/// <summary>
	/// Scale
	/// </summary>
	[DataMember(Name = "scale")]
	public int Scale { get; set; }

	/// <summary>
	/// SavedMapName
	/// </summary>
	[DataMember(Name = "SavedMapName")]
	public string SavedMapName { get; set; } = string.Empty;

	/// <summary>
	/// SavedMapId
	/// </summary>
	[DataMember(Name = "savedMapId")]
	public int SavedMapId { get; set; }

	/// <summary>
	/// SavedMapGroupName
	/// </summary>
	[DataMember(Name = "savedMapGroupName")]
	public string SavedMapGroupName { get; set; } = string.Empty;
}
