namespace LogicMonitor.Api.Dashboards;

/// <summary>
/// A saved map widget
/// </summary>
[DataContract]
public class SavedMapWidget : Widget, IWidget
{
	/// <summary>
	/// The display settings
	/// </summary>
	[DataMember(Name = "displaySettings")]
	public DisplaySettings DisplaySettings { get; set; } = new();

	/// <summary>
	/// The interval
	/// </summary>
	[DataMember(Name = "interval")]
	public int Interval { get; set; }

	/// <summary>
	/// The saved map's ID
	/// </summary>
	[DataMember(Name = "savedMapId")]
	public int SavedMapId { get; set; }

	/// <summary>
	/// The scale
	/// </summary>
	[DataMember(Name = "scale")]
	public float Scale { get; set; }

	/// <summary>
	/// The saved map's name
	/// </summary>
	[DataMember(Name = "savedMapName")]
	public string SavedMapName { get; set; } = string.Empty;

	/// <summary>
	/// The saved map's group's name
	/// </summary>
	[DataMember(Name = "savedMapGroupName")]
	public string SavedMapGroupName { get; set; } = string.Empty;
}