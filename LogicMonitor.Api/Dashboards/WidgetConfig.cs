namespace LogicMonitor.Api.Dashboards;

/// <summary>
///     Widget Config
/// </summary>
[DataContract]
public class WidgetConfig
{
	/// <summary>
	/// Create a WidgetConfig
	/// </summary>
	public WidgetConfig()
	{
	}

	/// <summary>
	///     Column
	/// </summary>
	[DataMember(Name = "col")]
	public int Col { get; set; }

	/// <summary>
	///     Row
	/// </summary>
	[DataMember(Name = "row")]
	public int Row { get; set; }

	/// <summary>
	///     Size X
	/// </summary>
	[DataMember(Name = "sizex")]
	public int SizeX { get; set; }

	/// <summary>
	///     Size Y
	/// </summary>
	[DataMember(Name = "sizey")]
	public int SizeY { get; set; }
}
